using System.Collections.Generic;

namespace Advertisements.Interfaces.Models
{
    /// <summary>
    /// Модель данных для поиска
    /// </summary>
    public class SearchRequest
    {
        /// <summary>
        /// Поле для поиска
        /// </summary>
        public string FieldName { get; set; }
        /// <summary>
        /// Значение для поиска
        /// </summary>
        public string SearchValue { get; set; }
        /// <summary>
        /// Искать с, для поиска по диапазону
        /// </summary>
        public string SearchFrom { get; set; }
        /// <summary>
        /// Искать по, для поиска по диапазону
        /// </summary>
        public string SearchTo { get; set; }
    }
}
