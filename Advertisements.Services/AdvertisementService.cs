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

namespace Advertisements.Services
{
    /// <summary>
    /// Сервис для работы с объявлениями
    /// </summary>
    public class AdvertisementService: IAdvertisementService
    {
        private readonly DataContext _dataContext;
        private readonly ILogger<AdvertisementService> _logger;
        private readonly IHostingEnvironment _hostingEnvironment;
        private readonly AdvertisementsOptions _options;
        private readonly IHttpContextAccessor _httpContextAccessor;

        public AdvertisementService(DataContext dataContext, 
                                    ILogger<AdvertisementService> logger,
                                    IHostingEnvironment hostingEnvironment,
                                    IOptions<AdvertisementsOptions> options,
                                    IHttpContextAccessor httpContextAccessor)
        {
            _dataContext = dataContext;
            _logger = logger;
            _hostingEnvironment = hostingEnvironment;
            _options = options.Value;
            _httpContextAccessor = httpContextAccessor;
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
                var url = "";
                if (req.Image != null)
                {
                    // Относительный путь к файлу (относительно wwwroot)
                    var path = $"{_options.FilePath}/{FileDirectoryEnum.Advertisements.ToString()}/{id}{Path.GetExtension(req.Image.FileName)}";
                    // Путь для сохранения
                    var savePath = _hostingEnvironment.WebRootPath + path;
                    var dir = Path.GetDirectoryName(savePath);
                    if (!Directory.Exists(dir))
                        Directory.CreateDirectory(dir);

                    // сохраняем файл
                    using (var fileStream = new FileStream(savePath, FileMode.OpenOrCreate))
                    {
                        await req.Image.CopyToAsync(fileStream);
                    }

                    // Ссылка на файл
                    var httpContext = _httpContextAccessor.HttpContext.Request;
                    url = $"{httpContext.Scheme}://{_httpContextAccessor.HttpContext.Request.Host.Value}{path}";
                }
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
    }
}
