using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
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
        public string Text { get; set; }
        /// <summary>
        /// Изображение
        /// </summary>        
        public IFormFile Image { get; set; }
    }
}
