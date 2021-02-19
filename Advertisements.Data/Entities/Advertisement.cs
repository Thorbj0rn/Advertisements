using System;
using System.Collections.Generic;
using System.Text;

namespace Advertisements.Data.Entities
{
    /// <summary>
    /// Сущность описывающая объявление
    /// </summary>
    public class Advertisement
    {
        /// <summary>
        /// Идентификатор
        /// </summary>
        public Guid Id { get; set; }
        /// <summary>
        /// Номер объявления
        /// </summary>
        public int Number { get; set; }
        /// <summary>
        /// Идентификатор пользователя
        /// </summary>
        public Guid UserId { get; set; }
        /// <summary>
        /// Текст
        /// </summary>
        public string Text { get; set; }
        /// <summary>
        /// Ссылка на изображение
        /// </summary>
        public string ImageUrl { get; set; }
        /// <summary>
        /// Расширение файла
        /// </summary>
        public string ImageExtension { get; set; }
        /// <summary>
        /// Путь к файлу
        /// </summary>
        public string ImagePath { get; set; }
        /// <summary>
        /// Рейтинг
        /// </summary>
        public double Rating { get; set; }
        /// <summary>
        /// Дата создания
        /// </summary>
        public DateTime Created { get; set; }


        /// <summary>
        /// Навигационное поле - Пользователь
        /// </summary>
        public User User { get; set; }
    }
}
