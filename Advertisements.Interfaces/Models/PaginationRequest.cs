using System;
using System.Collections.Generic;
using System.Text;

namespace Advertisements.Interfaces.Models
{
    /// <summary>
    /// Входная модель для пагинации
    /// </summary>
    public class PaginationRequest
    {
        /// <summary>
        /// Пропустить столько элементов
        /// </summary>
        public int Skip { get; set; }
        /// <summary>
        /// Взять столько элементов
        /// </summary>
        public int Take { get; set; }
    }
}
