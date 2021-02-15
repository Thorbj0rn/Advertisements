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

namespace Advertisements.Services
{
    /// <summary>
    /// Сервис для работы с объявлениями
    /// </summary>
    public class AdvertisementService: IAdvertisementService
    {
        private readonly DataContext _dataContext;
        private readonly ILogger<AdvertisementService> _logger;        
        private readonly AdvertisementsOptions _options;
        private readonly IFileService _fileService;
        

        public AdvertisementService(DataContext dataContext, 
                                    ILogger<AdvertisementService> logger,                                    
                                    IOptions<AdvertisementsOptions> options,
                                    IFileService fileService)
        {
            _dataContext = dataContext;
            _logger = logger;
            _options = options.Value;
            _fileService = fileService;
        }

        public Task<bool> DeleteAdvertisement(Guid id)
        {
            throw new NotImplementedException();
        }

        public async Task<bool> UpdateAdvertisement(UpdateAdvertisementRequest req)
        {
            try
            {
                var id = req.Id.HasValue ? req.Id.Value : Guid.NewGuid();
                var saveFileReq = new SaveFileRequest
                {
                    FileName = $"{id}",
                    DirectoryName = FileDirectoryEnum.Advertisements,
                    UploadedFile = req.Image
                };

                var url = req.Image != null ? await _fileService.SaveFile(saveFileReq) : "";
                
                var adv = new Advertisement
                {
                    Id = id,
                    ImageUrl = url,
                    Text = req.Text,
                    // TODO: UserId идентификатор авторизованного пользователя
                    UserId = req.UserId
                };

                _dataContext.Attach(adv);
                _dataContext.Entry(adv).State = req.Id.HasValue ? EntityState.Modified : EntityState.Added;
                await _dataContext.SaveChangesAsync();

                return true;
            }
            catch(Exception ex)
            {
                _logger.LogError(ex.Message);
                return false;
            }
        }

        public async Task<PaginationResponse<AdvertisementResponse>> GetAdvertisements(AdvertisementsRequest req)
        {
            try
            {
                // Выбираем объявления
                var advs =  _dataContext.Advertisements
                    .Select(a => new AdvertisementResponse
                    {
                        Id = a.Id,
                        Number = a.Number,
                        Text = a.Text,
                        DateCreate = a.DateCreate,
                        ImageUrl = a.ImageUrl,
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

        private string GetSort(string fieldName, bool desc)
        {
            var descString = desc ? " desc" : "";
            return fieldName + descString;
        }
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
