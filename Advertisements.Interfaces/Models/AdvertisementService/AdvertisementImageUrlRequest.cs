using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
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
        [Required(ErrorMessage = "Введите идентификатор объявления.")]
        public Guid Id { get; set; }
        /// <summary>
        /// Ширина
        /// </summary>
        [Required(ErrorMessage = "Введите ширину изображения.")]
        public int Width { get; set; }
        /// <summary>
        /// Высота
        /// </summary>
        [Required(ErrorMessage = "Введите высоту изображения.")]
        public int Heigth { get; set; }
    }
}
