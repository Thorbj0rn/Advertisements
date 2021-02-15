using Advertisements.Data.Enums;
using Advertisements.Interfaces;
using Advertisements.Interfaces.Models.FileService;
using Advertisements.Interfaces.Options;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Threading.Tasks;

namespace Advertisements.Services
{
    /// <summary>
    /// Сервис для работы с файлами
    /// </summary>
    public class FileService: IFileService
    {
        private readonly FilesOptions _options;
        private readonly ILogger<FileService> _logger;
        private readonly IHostingEnvironment _hostingEnvironment;        
        private readonly IHttpContextAccessor _httpContextAccessor;

        public FileService(IOptions<FilesOptions> options,
                           ILogger<FileService> logger,
                           IHostingEnvironment hostingEnvironment,
                           IHttpContextAccessor httpContextAccessor)
        {
            _options = options.Value;
            _logger = logger;
            _hostingEnvironment = hostingEnvironment;
            _httpContextAccessor = httpContextAccessor;
        }

        public async Task<string> SaveFile(SaveFileRequest req)
        {
            try
            {
                // Относительный путь к файлу (относительно wwwroot)
                var path = $"{_options.FilesPath}/{req.DirectoryName.ToString()}/{req.FileName}{Path.GetExtension(req.UploadedFile.FileName)}";
                // Путь для сохранения
                var savePath = _hostingEnvironment.WebRootPath + path;
                var dir = Path.GetDirectoryName(savePath);
                if (!Directory.Exists(dir))
                    Directory.CreateDirectory(dir);

                // сохраняем файл
                using (var fileStream = new FileStream(savePath, FileMode.OpenOrCreate))
                {
                    await req.UploadedFile.CopyToAsync(fileStream);
                }

                // Ссылка на файл
                var httpContext = _httpContextAccessor.HttpContext.Request;
                var url = $"{httpContext.Scheme}://{_httpContextAccessor.HttpContext.Request.Host.Value}{path}";

                return url;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message);
                throw new Exception(ex.Message);
            }
        }
    }
}
