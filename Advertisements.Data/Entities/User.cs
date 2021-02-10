using System;
using System.Collections.Generic;
using System.Text;

namespace Advertisements.Data.Entities
{
    /// <summary>
    /// Сущность описывающая пользователя
    /// </summary>
    public class User
    {
        /// <summary>
        /// Идентификатор
        /// </summary>
        public Guid Id { get; set; }
        /// <summary>
        /// Имя
        /// </summary>
        public string Name { get; set; }


        /// <summary>
        /// Навигационное поле - Объявления
        /// </summary>
        public List<Advertisement> Advertisements { get; set; }
    }
}
