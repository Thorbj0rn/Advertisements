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
        /// Возвращает ссылку на изображение заданного размера заданного объявления
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        public async Task<string> GetImageUrl(AdvertisementImageUrlRequest request)
        {
            try
            {
                var advertisement = await _dataContext.Advertisements.FirstOrDefaultAsync(a => a.Id == request.Id);
                if (advertisement == null)
                    throw new Exception("Объявление не найдено.");

                var imageRequest = new ImageUrlRequest
                {
                    Id = request.Id,
                    Directory = FileDirectories.Advertisements,
                    Extension = advertisement.ImageExtension,
                    Heigth = request.Heigth,
                    Width = request.Width
                };
                var imageUrl = await _fileService.GetImageUrl(imageRequest);

                return imageUrl;
            }
            catch (Exception exception)
            {
                _logger.LogError(exception.Message);
                throw exception;
            }
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
                var advertisement = await _dataContext.Advertisements.FirstOrDefaultAsync(a => a.Id == id);
                if (advertisement == null)
                    throw new Exception("Объявление не найдено.");
                if (_user.Role != UserRoles.Admin && _user.Id != advertisement.UserId)
                    throw new Exception("У вас нет прав удалять данное объявление.");

                if (Directory.Exists(advertisement.ImagePath))
                    Directory.Delete(advertisement.ImagePath, true);

                _dataContext.Remove(advertisement);
                await _dataContext.SaveChangesAsync();

                return true;
            }
            catch(Exception exception)
            {
                _logger.LogError(exception.Message);
                throw exception;
            }
        }

        /// <summary>
        /// Добавляет объявление
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        public async Task<int> Add(AddAdvertisementRequest request)
        {
            using (var transaction = await _dataContext.Database.BeginTransactionAsync(System.Data.IsolationLevel.Serializable))
            {
                try
                {
                    var currentCount = await _dataContext.Advertisements.CountAsync(a => a.UserId == _user.Id);
                    if (currentCount >= _options.MaxAdsPerUser)
                        throw new Exception("Вы исчерпали лимит объявлений.");

                    var id = Guid.NewGuid();
                    var saveFileRequest = new SaveFileRequest
                    {
                        FileName = $"{id}",
                        DirectoryName = FileDirectories.Advertisements,
                        UploadedFile = request.Image
                    };

                    var filesInfo = request.Image != null ? await _fileService.Save(saveFileRequest) : new SaveFileResponse { Url = "", Extension = ""} ;

                    var advertisement = new Data.Entities.Advertisement
                    {
                        Id = id,
                        ImageUrl = filesInfo.Url,
                        ImageExtension = filesInfo.Extension,
                        ImagePath = filesInfo.Path,
                        Text = request.Text,
                        UserId = _user.Id
                    };

                    await _dataContext.AddAsync(advertisement);
                    await _dataContext.SaveChangesAsync();

                    await transaction.CommitAsync();
                    return advertisement.Number;
                }
                catch (Exception exception)
                {
                    await transaction.RollbackAsync();
                    _logger.LogError(exception.Message);
                    throw exception;
                }
            }
        }

        /// <summary>
        /// Обновляет объявление
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        public async Task<bool> Update(UpdateAdvertisementRequest request)
        {
            using (var transaction = await _dataContext.Database.BeginTransactionAsync(System.Data.IsolationLevel.RepeatableRead))
            {
                try
                {
                    var advertisement = await _dataContext.Advertisements.FirstOrDefaultAsync(a => a.Id == request.Id);
                    if (advertisement == null)
                        throw new Exception("Объявление не найдено.");
                    if (_user.Role != UserRoles.Admin && _user.Id != advertisement.UserId)
                        throw new Exception("У вас нет прав редактировать данное объявление.");

                    var filesInfo = new SaveFileResponse { Url = "", Extension = "" };
                    if (request.Image != null)
                    {
                        if(Directory.Exists(advertisement.ImagePath))
                            Directory.Delete(advertisement.ImagePath, true);

                        var saveFileReq = new SaveFileRequest
                        {
                            FileName = $"{advertisement.Id}",
                            DirectoryName = FileDirectories.Advertisements,
                            UploadedFile = request.Image
                        };
                        filesInfo = await _fileService.Save(saveFileReq);
                    }

                    advertisement.ImageUrl = filesInfo.Url;
                    advertisement.ImageExtension = filesInfo.Extension;
                    advertisement.ImagePath = filesInfo.Path;
                    advertisement.Text = request.Text;

                    await _dataContext.SaveChangesAsync();

                    await transaction.CommitAsync();
                    return true;
                }
                catch (Exception exception)
                {
                    await transaction.RollbackAsync();
                    _logger.LogError(exception.Message);
                    throw exception;
                }
            }
        }

        /// <summary>
        /// Возвращает список объявлений
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        public async Task<PaginationResponse<AdvertisementResponse>> Get(AdvertisementsRequest request)
        {
            try
            {                
                // Выбираем объявления
                var advertisements =  _dataContext.Advertisements
                    .Select(a => new AdvertisementResponse
                    {
                        Id = a.Id,
                        Number = a.Number,
                        Text = a.Text,
                        Created = a.Created,
                        ImageUrl = $"{a.ImageUrl}{ImageSizes.Small.ToString()}{a.ImageExtension}",
                        Rating = a.Rating,
                        UserId = a.UserId,
                        UserName = a.User.Name
                    });
                // Применяем фильтры
                if (request?.Filters != null && request.Filters.Count > 0)
                {
                    request.Filters.ForEach(f =>
                    {
                        var expression = GetConditionTemplate(f.Condition, f.FieldName);
                        advertisements = advertisements.Where(expression, f.Value);
                    });
                }
                // Применяем сортировки
                if (request?.Sorts != null && request.Sorts.Count > 0)
                {
                    var sort = string.Join(',', request.Sorts.Select(s => GetSort(s.FieldName, s.Desc)).ToList());
                    advertisements = advertisements.OrderBy(sort);
                }
                // Применяем пагинацию
                var page = request?.Pagination?.Page != null ? request.Pagination.Page : 1;
                var pageSize = request?.Pagination?.PageSize != null ? request.Pagination.PageSize : 10;
                var pageRes = advertisements.PageResult(page, pageSize);
                
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
