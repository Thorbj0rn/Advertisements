using System;
using System.Collections.Generic;
using System.Text;

namespace Advertisements.Interfaces.Models.FileService
{
    /// <summary>
    /// Выходная модель с информацией о сохранённых файлах
    /// </summary>
    public class SaveFileResponse
    {
        public string Url { get; set; }
        public string Path { get; set; }
        public string Extension { get; set; }
    }
}
