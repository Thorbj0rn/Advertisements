using Advertisements.Data.Enums;
using System;
using System.Collections.Generic;
using System.Text;

namespace Advertisements.Interfaces.Models.AdvertisementService
{
    /// <summary>
    /// Входная модель для получения списка объявлений
    /// </summary>
    public class AdvertisementsRequest
    {
        public ImageSizes ImageSize { get; set; }
        /// <summary>
        /// Фильтры
        /// </summary>
        public List<FilterRequest> Filters { get; set; } 
        /// <summary>
        /// Сортировки
        /// </summary>
        public List<SortRequest> Sorts { get; set; }
        /// <summary>
        /// Пагинация
        /// </summary>
        public PaginationRequest Pagination { get; set; }
    }

    /// <summary>
    /// Модель для сортировки
    /// </summary>
    public class SortRequest
    {
        /// <summary>
        /// Имя поля
        /// </summary>
        public string FieldName { get; set; }
        /// <summary>
        /// Сортировать от большего к меньшему
        /// </summary>
        public bool Desc { get; set; }
    }

    /// <summary>
    /// Модель для фильтра
    /// </summary>
    public class FilterRequest
    {
        /// <summary>
        /// Имя поля
        /// </summary>
        public string FieldName { get; set; }
        /// <summary>
        /// Условие
        /// </summary>
        public FilterConditionEnum Condition { get; set; }
        /// <summary>
        /// Значение
        /// </summary>
        public string Value { get; set; }
    }

    /// <summary>
    /// Перечисление условий для фильтра
    /// </summary>
    public enum FilterConditionEnum
    {
        GreaterThan = 0,
        LesserThan = 1,
        Equal = 2,
        Contains = 3
    }

    /// <summary>
    /// Модель пагинации
    /// </summary>
    public class PaginationRequest
    {
        /// <summary>
        /// Номер страницы
        /// </summary>
        public int Page { get; set; }
        /// <summary>
        /// Количество элементов на странице
        /// </summary>
        public int PageSize { get; set; }
    }
}
