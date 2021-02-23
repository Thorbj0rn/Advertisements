using System;
using System.Collections.Generic;
using System.Text;

namespace Advertisements.Interfaces.Options
{
    /// <summary>
    /// Опции файлов
    /// </summary>
    public class FilesOptions
    {
        /// <summary>
        /// Путь к папке с файлами
        /// </summary>
        public string FilesPath { get; set; }
        /// <summary>
        /// Размеры для маленького изображения
        /// </summary>
        public ImageSize SmallImageSize { get;set; }        
    }

    public class ImageSize
    {
        /// <summary>
        /// Ширина
        /// </summary>
        public int Width { get; set; }
        /// <summary>
        /// Высота
        /// </summary>
        public int Heigth { get; set; }
    }
}
