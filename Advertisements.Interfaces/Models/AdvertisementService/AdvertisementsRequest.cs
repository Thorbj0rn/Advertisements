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
        public List<FilterRequest> Filters { get; set; } 
        public List<SortRequest> Sorts { get; set; }
        public PaginationRequest Pagination { get; set; }
    }

    public class SortRequest
    {
        public string FieldName { get; set; }
        public bool Desc { get; set; }
    }

    public class FilterRequest
    {
        public string FieldName { get; set; }
        public FilterConditionEnum Condition { get; set; }
        public string Value { get; set; }
    }
    public enum FilterConditionEnum
    {
        GreaterThan = 0,
        LesserThan = 1,
        Equal = 2,
        Contains = 3
    }

    public class PaginationRequest
    {
        public int Page { get; set; }
        public int PageSize { get; set; }
    }
}
