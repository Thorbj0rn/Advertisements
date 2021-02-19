using Advertisements.Data;
using Advertisements.Data.Entities;
using Advertisements.Interfaces;
using Advertisements.Interfaces.Models.AdvertisementService;
using Advertisements.Interfaces.Options;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using Advertisements.Data.Enums;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using Advertisements.Interfaces.Models.FileService;
using System.Linq;
using System.Linq.Dynamic.Core;
using Advertisements.Interfaces.Models;
using System.Security.Claims;

namespace Advertisements.Services
{
    /// <summary>
    /// Сервис для работы с объявлениями
    /// </summary>
    public class AdvertisementService: IAdvertisementService
    {
        private readonly DataContext _dataContext;
        private readonly ILogger<Advertisement> _logger;        
        private readonly AdvertisementsOptions _options;
        private readonly IFileService _fileService;
        private readonly CurrentUser _user;

        public AdvertisementService(DataContext dataContext, 
                                    ILogger<Advertisement> logger,                                    
                                    IOptions<AdvertisementsOptions> options,
                                    IFileService fileService,
                                    IHttpContextAccessor httpContextAccessor)
        {
            _dataContext = dataContext;
            _logger = logger;
            _options = options.Value;
            _fileService = fileService;
            _user = new CurrentUser
            {
                Id = Guid.Parse(httpContextAccessor.HttpContext.User.FindFirst(ClaimTypes.NameIdentifier).Value),
                Role = (UserRoles)Enum.Parse(typeof(UserRoles), httpContextAccessor.HttpContext.User.FindFirst(ClaimTypes.Role).Value)
            };
        }

        /// <summary>
        /// Удаляет объявление
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public async Task<bool> Delete(Guid id)
        {
            try
            {
                var adv = await _dataContext.Advertisements.FirstOrDefaultAsync(a => a.Id == id);
                if (adv == null)
                    throw new Exception("Объявление не найдено.");
                if (_user.Role != UserRoles.Admin && _user.Id != adv.UserId)
                    throw new Exception("У вас нет прав удалять данное объявление.");

                if (Directory.Exists(adv.ImagePath))
                    Directory.Delete(adv.ImagePath, true);

                _dataContext.Remove(adv);
                await _dataContext.SaveChangesAsync();

                return true;
            }
            catch(Exception ex)
            {
                _logger.LogError(ex.Message);
                throw ex;
            }
        }

        /// <summary>
        /// Добавляет объявление
        /// </summary>
        /// <param name="req"></param>
        /// <returns></returns>
        public async Task<int> Add(AddAdvertisementRequest req)
        {
            using (var transaction = await _dataContext.Database.BeginTransactionAsync())
            {
                try
                {
                    var currentCount = await _dataContext.Advertisements.CountAsync(a => a.UserId == _user.Id);
                    if (currentCount >= _options.MaxAdsPerUser)
                        throw new Exception("Вы исчерпали лимит объявлений.");

                    var id = Guid.NewGuid();
                    var saveFileReq = new SaveFileRequest
                    {
                        FileName = $"{id}",
                        DirectoryName = FileDirectories.Advertisements,
                        UploadedFile = req.Image
                    };

                    var filesInfo = req.Image != null ? await _fileService.Save(saveFileReq) : new SaveFileResponse { Url = "", Extension = ""} ;

                    var adv = new Data.Entities.Advertisement
                    {
                        Id = id,
                        ImageUrl = filesInfo.Url,
                        ImageExtension = filesInfo.Extension,
                        ImagePath = filesInfo.Path,
                        Text = req.Text,
                        UserId = _user.Id
                    };

                    await _dataContext.AddAsync(adv);
                    await _dataContext.SaveChangesAsync();

                    await transaction.CommitAsync();
                    return adv.Number;
                }
                catch (Exception ex)
                {
                    await transaction.RollbackAsync();
                    _logger.LogError(ex.Message);
                    throw ex;
                }
            }
        }

        /// <summary>
        /// Обновляет объявление
        /// </summary>
        /// <param name="req"></param>
        /// <returns></returns>
        public async Task<bool> Update(UpdateAdvertisementRequest req)
        {
            using (var transaction = await _dataContext.Database.BeginTransactionAsync())
            {
                try
                {
                    var adv = await _dataContext.Advertisements.FirstOrDefaultAsync(a => a.Id == req.Id);
                    if (adv == null)
                        throw new Exception("Объявление не найдено.");
                    if (_user.Role != UserRoles.Admin && _user.Id != adv.UserId)
                        throw new Exception("У вас нет прав редактировать данное объявление.");

                    var filesInfo = new SaveFileResponse { Url = "", Extension = "" };
                    if (req.Image != null)
                    {
                        if(Directory.Exists(adv.ImagePath))
                            Directory.Delete(adv.ImagePath, true);

                        var saveFileReq = new SaveFileRequest
                        {
                            FileName = $"{adv.Id}",
                            DirectoryName = FileDirectories.Advertisements,
                            UploadedFile = req.Image
                        };
                        filesInfo = await _fileService.Save(saveFileReq);
                    }

                    adv.ImageUrl = filesInfo.Url;
                    adv.ImageExtension = filesInfo.Extension;
                    adv.ImagePath = filesInfo.Path;
                    adv.Text = req.Text;

                    await _dataContext.SaveChangesAsync();

                    await transaction.CommitAsync();
                    return true;
                }
                catch (Exception ex)
                {
                    await transaction.RollbackAsync();
                    _logger.LogError(ex.Message);
                    throw ex;
                }
            }
        }

        /// <summary>
        /// Возвращает список объявлений
        /// </summary>
        /// <param name="req"></param>
        /// <returns></returns>
        public async Task<PaginationResponse<AdvertisementResponse>> Get(AdvertisementsRequest req)
        {
            try
            {
                var size = req?.ImageSize ?? ImageSizes.Full;
                // Выбираем объявления
                var advs =  _dataContext.Advertisements
                    .Select(a => new AdvertisementResponse
                    {
                        Id = a.Id,
                        Number = a.Number,
                        Text = a.Text,
                        Created = a.Created,
                        ImageUrl = $"{a.ImageUrl}{size.ToString()}{a.ImageExtension}",
                        Rating = a.Rating,
                        UserId = a.UserId,
                        UserName = a.User.Name
                    });
                // Применяем фильтры
                if (req?.Filters != null && req.Filters.Count > 0)
                {
                    req.Filters.ForEach(f =>
                    {
                        var expression = GetConditionTemplate(f.Condition, f.FieldName);
                        advs = advs.Where(expression, f.Value);
                    });
                }
                // Применяем сортировки
                if (req?.Sorts != null && req.Sorts.Count > 0)
                {
                    var sort = string.Join(',', req.Sorts.Select(s => GetSort(s.FieldName, s.Desc)).ToList());
                    advs = advs.OrderBy(sort);
                }
                // Применяем пагинацию
                var page = req?.Pagination?.Page != null ? req.Pagination.Page : 1;
                var pageSize = req?.Pagination?.PageSize != null ? req.Pagination.PageSize : 10;
                var pageRes = advs.PageResult(page, pageSize);
                
                var result = new PaginationResponse<AdvertisementResponse>
                {
                    Items = await pageRes.Queryable.ToListAsync(),
                    CurrentPage = pageRes.CurrentPage,
                    PageCount = pageRes.PageCount,
                    PageSize = pageRes.PageSize,
                    RowCount = pageRes.RowCount
                };

                return result;
            }
            catch(Exception ex)
            {
                _logger.LogError(ex.Message);
                throw ex;
            }
        }

        /// <summary>
        /// Возвращает строку с параметрами сортировки
        /// </summary>
        /// <param name="fieldName"></param>
        /// <param name="desc"></param>
        /// <returns></returns>
        private string GetSort(string fieldName, bool desc)
        {
            var descString = desc ? " desc" : "";
            return fieldName + descString;
        }
        /// <summary>
        /// Возвращает шаблон условия
        /// </summary>
        /// <param name="type"></param>
        /// <param name="fieldName"></param>
        /// <returns></returns>
        private string GetConditionTemplate(FilterConditionEnum type, string fieldName)
        {
            var res = "";
            switch (type)
            {
                case FilterConditionEnum.Contains: 
                    res = $"{fieldName}.Contains(@0)"; 
                    break;
                case FilterConditionEnum.Equal: 
                    res = $"{fieldName} == @0"; 
                    break;
                case FilterConditionEnum.GreaterThan: 
                    res = $"{fieldName} > @0"; 
                    break;
                case FilterConditionEnum.LesserThan: 
                    res = $"{fieldName} < @0"; 
                    break;
                default: 
                    throw new Exception("Неизвестное условие.");
            }

            return res;
        }

        
    }
}
