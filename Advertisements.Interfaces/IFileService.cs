using Advertisements.Interfaces.Models.FileService;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Advertisements.Interfaces
{
    /// <summary>
    /// Интерфейс описывающий сервис для работы с файлами
    /// </summary>
    public interface IFileService
    {
        /// <summary>
        /// Сохраняет файл, возвращает url
        /// </summary>
        /// <param name="req"></param>
        /// <returns></returns>
        Task<string> SaveFile(SaveFileRequest req);
    }
}
