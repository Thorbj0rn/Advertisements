using System;
using System.Collections.Generic;
using System.Text;

namespace Advertisements.Interfaces.Models.AdvertisementService
{
    /// <summary>
    /// Входная модель для получения ссылки на изображение объявления
    /// </summary>
    public class AdvertisementImageUrlRequest
    {
        /// <summary>
        /// Идентификатор
        /// </summary>
        public Guid Id { get; set; }
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
