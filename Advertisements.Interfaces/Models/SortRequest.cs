namespace Advertisements.Interfaces.Models
{    
    /// <summary>
    /// Модель данных для сортировки
    /// </summary>
    public class SortRequest
    {
        /// <summary>
        /// Поле для сортировки
        /// </summary>
        public string FieldName { get; set; }
        /// <summary>
        /// Направление сортировки
        /// </summary>
        public bool Desc { get; set; }
    }
}
