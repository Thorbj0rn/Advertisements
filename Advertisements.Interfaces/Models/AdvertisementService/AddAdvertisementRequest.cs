using Advertisements.Interfaces.ValidationAttributes;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace Advertisements.Interfaces.Models.AdvertisementService
{
    /// <summary>
    /// Входная модель данных для добавления объявления
    /// </summary>
    public class AddAdvertisementRequest
    {                
        /// <summary>
        /// Текст
        /// </summary>
        [Required(ErrorMessage = "Введите текст объявления")]
        public string Text { get; set; }
        /// <summary>
        /// Изображение
        /// </summary>        
        [RequiredFileExtension("jpg,jpeg,png")]
        public IFormFile Image { get; set; }
    }
}
