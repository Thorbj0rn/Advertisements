using Advertisements.Data.Enums;
using Advertisements.Interfaces;
using Advertisements.Interfaces.Models.FileService;
using Advertisements.Interfaces.Options;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using SixLabors.ImageSharp;
using SixLabors.ImageSharp.Processing;
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

        /// <summary>
        /// Сохраняет файл
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        public async Task<SaveFileResponse> Save(SaveFileRequest request)
        {
            try
            {                
                var path = $"{_options.FilesPath}/{request.DirectoryName.ToString()}/{request.FileName}/";
                var savePath = $"{_hostingEnvironment.WebRootPath}{path}";
                var directory = Path.GetDirectoryName(savePath);
                if (!Directory.Exists(directory))
                    Directory.CreateDirectory(directory);

                var image = Image.Load(request.UploadedFile.OpenReadStream());
                var ratio = image.Height / image.Width;
                var smallImage = image.Clone(x => x.Resize(_options.SmallImageSize.Width, _options.SmallImageSize.Heigth * ratio));                

                var extension = Path.GetExtension(request.UploadedFile.FileName);
                image.Save($"{savePath}{ImageSizes.Full.ToString()}{extension}");
                smallImage.Save($"{savePath}{ImageSizes.Small.ToString()}{extension}");                

                var httpContext = _httpContextAccessor.HttpContext.Request;
                var url = $"{httpContext.Scheme}://{_httpContextAccessor.HttpContext.Request.Host.Value}{path}";
                
                return new SaveFileResponse { Url = url, Extension = extension, Path = savePath};
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message);
                throw new Exception(ex.Message);
            }
        }

        /// <summary>
        /// Возвращает ссылку на изображение
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        public async Task<string> GetImageUrl(ImageUrlRequest request)
        {
            try
            {
                var fileName = $"{request.Width}x{request.Heigth}{request.Extension}";
                var path = $"{_options.FilesPath}/{request.Directory.ToString()}/{request.Id}/";
                var saveDirectory = $"{_hostingEnvironment.WebRootPath}{path}";
                var filePath = $"{saveDirectory}{fileName}";
                var httpContext = _httpContextAccessor.HttpContext.Request;

                if (File.Exists(filePath))
                    return $"{httpContext.Scheme}://{_httpContextAccessor.HttpContext.Request.Host.Value}{path}{fileName}";

                var image = Image.Load($"{saveDirectory}{ImageSizes.Full.ToString()}{request.Extension}");
                image.Mutate(x => x.Resize(request.Width, request.Heigth));
                image.Save(filePath);

                return $"{httpContext.Scheme}://{_httpContextAccessor.HttpContext.Request.Host.Value}{path}{fileName}";
            }
            catch (Exception exception)
            {
                _logger.LogError(exception.Message);
                throw exception;
            }
        }
    }
}
