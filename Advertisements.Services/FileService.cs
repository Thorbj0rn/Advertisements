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

        public async Task<SaveFileResponse> Save(SaveFileRequest req)
        {
            try
            {                
                var path = $"{_options.FilesPath}/{req.DirectoryName.ToString()}/{req.FileName}/";
                var savePath = $"{_hostingEnvironment.WebRootPath}{path}";
                var dir = Path.GetDirectoryName(savePath);
                if (!Directory.Exists(dir))
                    Directory.CreateDirectory(dir);

                var image = Image.Load(req.UploadedFile.OpenReadStream());
                var ratio = image.Height / image.Width;
                var smallImage = image.Clone(x => x.Resize(_options.SmallImageSize.Width, _options.SmallImageSize.Heigth * ratio));
                var mediumImage = image.Clone(x => x.Resize(_options.MediumImageSize.Width, _options.MediumImageSize.Heigth * ratio));

                var extension = Path.GetExtension(req.UploadedFile.FileName);
                image.Save($"{savePath}{ImageSizes.Full.ToString()}{extension}");
                smallImage.Save($"{savePath}{ImageSizes.Small.ToString()}{extension}");
                mediumImage.Save($"{savePath}{ImageSizes.Medium.ToString()}{extension}");

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
    }
}
