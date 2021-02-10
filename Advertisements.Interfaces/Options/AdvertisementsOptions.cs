using System;
using System.Collections.Generic;
using System.Text;

namespace Advertisements.Interfaces.Options
{
    /// <summary>
    /// Опции объявлений
    /// </summary>
    public class AdvertisementsOptions
    {
        /// <summary>
        /// Максимальное количество объявлений для одного пользователя
        /// </summary>
        public int MaxAdsPerUser { get; set; }
    }
}
