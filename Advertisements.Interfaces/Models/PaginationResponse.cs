using System;
using System.Collections.Generic;
using System.Text;

namespace Advertisements.Interfaces.Models
{
    /// <summary>
    /// Выходная модель для пагинации
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public class PaginationResponse<T>
    {
        /// <summary>
        /// Элементы текущей страницы
        /// </summary>
        public List<T> Items { get; set; }
        /// <summary>
        /// Номер текущей страницы
        /// </summary>
        public int CurrentPage { get; set; }
        /// <summary>
        /// Всего страниц
        /// </summary>
        public int PageCount { get; set; }
        /// <summary>
        /// Элементов на странице
        /// </summary>
        public int PageSize { get; set; }
        /// <summary>
        /// Количество строк
        /// </summary>
        public int RowCount { get; set; }
    }
}