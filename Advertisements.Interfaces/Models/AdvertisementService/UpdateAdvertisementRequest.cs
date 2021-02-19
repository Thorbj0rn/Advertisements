using Advertisements.Interfaces.ValidationAttributes;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace Advertisements.Interfaces.Models.AdvertisementService
{
    /// <summary>
    /// Входная модель для добавления/изменения объявления
    /// </summary>
    public class UpdateAdvertisementRequest
    {
        /// <summary>
        /// Идентификатор
        /// </summary>
        [Required(ErrorMessage = "Введите идентификатор объявления.")]
        public Guid Id { get; set; }
        /// <summary>
        /// Текст
        /// </summary>
        [Required(ErrorMessage = "Введите текст объявления.")]
        public string Text { get; set; }
        /// <summary>
        /// Изображение
        /// </summary>        
        [RequiredFileExtension("jpg,jpeg,png")]
        public IFormFile Image { get; set; }        
    }
}
