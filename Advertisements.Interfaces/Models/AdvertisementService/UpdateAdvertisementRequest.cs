using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
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
        public Guid? Id { get; set; }        
        /// <summary>
        /// Идентификатор пользователя
        /// </summary>
        public Guid UserId { get; set; }
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
