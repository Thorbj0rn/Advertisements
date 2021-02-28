using Advertisements.Interfaces.Options;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Options;
using SixLabors.ImageSharp;
using SixLabors.ImageSharp.Formats;
using SixLabors.ImageSharp.Processing;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Advertisements.WebApi.Middlewares
{   
    public class ImageResizeMiddleware
    {
        private readonly RequestDelegate _next;
        private readonly FilesOptions _options;
        private readonly IHostingEnvironment _hostingEnvironment;
        private static readonly List<string> _extensions = new List<string> { ".jpeg", ".jpg", ".png" };


        public ImageResizeMiddleware(RequestDelegate next,
                                     IHostingEnvironment hostingEnvironment,
                                     IOptions<FilesOptions> options)
        {
            _next = next;
            _options = options.Value;
            _hostingEnvironment = hostingEnvironment;
        }
        public async Task Invoke(HttpContext context)
        {
            var path = context.Request.Path.Value;            
            if (!_extensions.Any(x => path.EndsWith(x, StringComparison.OrdinalIgnoreCase)))
            {
                await _next.Invoke(context);
                return;
            }

            var filePath = $"{_hostingEnvironment.WebRootPath}{path}";
            if (!File.Exists(filePath))
            {
                throw new Exception("Файл не найден.");                
            }

            var width = int.Parse(context.Request.Query["width"].ToString());
            width = width == 0 ? _options.SmallImageSize.Width : width;
            var height = int.Parse(context.Request.Query["height"].ToString()); 
            height = height == 0 ? _options.SmallImageSize.Heigth : height;

            IImageFormat imageFormat ; 
            var image = Image.Load(filePath, out imageFormat);
            image.Mutate(x => x.Resize(width, height));

            var ms = new MemoryStream();
            image.Save(ms, imageFormat);            

            await context.Response.Body.WriteAsync(ms.ToArray(), 0, (int)ms.Length);
                        
            await _next.Invoke(context);
        }
    }
}
