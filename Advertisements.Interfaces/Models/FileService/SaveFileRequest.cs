using Advertisements.Data.Enums;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Text;

namespace Advertisements.Interfaces.Models.FileService
{
    /// <summary>
    /// Входная модель для сохранения файла
    /// </summary>
    public class SaveFileRequest
    {
        /// <summary>
        /// Имя файла
        /// </summary>
        public string FileName { get; set; }
        /// <summary>
        /// Имя папки
        /// </summary>
        public FileDirectories DirectoryName { get; set; }
        /// <summary>
        /// Загруженный файл
        /// </summary>
        public IFormFile UploadedFile { get; set; }
    }
}
