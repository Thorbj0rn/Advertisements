
using System.Collections.Generic;

namespace Advertisements.Interfaces.Models
{
    /// <summary>
    /// Модель данных для фильтрации
    /// </summary>
    public class FilterRequest
    {
        /// <summary>
        /// Модель для пагинации
        /// </summary>
        public PaginationRequest Pagination { get; set; }
        /// <summary>
        /// Модель для поиска
        /// </summary>
        public List<SearchRequest> Search { get; set; }
        /// <summary>
        /// Модель для сортировки
        /// </summary>
        public SortRequest Sort { get; set; }
    }
}
