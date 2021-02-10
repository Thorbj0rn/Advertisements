using Advertisements.Interfaces.Models.AdvertisementService;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Advertisements.Interfaces
{
    /// <summary>
    /// Интерфейс описывающий сервис для работы с объявлениями
    /// </summary>
    public interface IAdvertisementService
    {
        /// <summary>
        /// Добавить/изменить объявление
        /// </summary>
        /// <param name="req"></param>
        /// <returns></returns>
        Task<bool> UpdateAdvertisement(UpdateAdvertisementRequest req);
        /// <summary>
        /// Удалить объявление
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        Task<bool> DeleteAdvertisement(Guid id);
    }
}
