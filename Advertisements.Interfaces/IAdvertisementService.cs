using Advertisements.Interfaces.Models;
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
        /// Добавить объявление
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        Task<int> Add(AddAdvertisementRequest request);
        /// <summary>
        /// Изменить объявление
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        Task<bool> Update(UpdateAdvertisementRequest request);
        /// <summary>
        /// Удалить объявление
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        Task<bool> Delete(Guid id);
        /// <summary>
        /// Возвращает список объявлений
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        Task<PaginationResponse<AdvertisementResponse>> Get(AdvertisementsRequest request);
        /// <summary>
        /// Возвращает ссылку на изображение заданного размера заданного объявления
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        Task<string> GetImageUrl(AdvertisementImageUrlRequest request);
    }
}
