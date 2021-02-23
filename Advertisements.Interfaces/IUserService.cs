
using Advertisements.Interfaces.Models;
using Advertisements.Interfaces.Models.UserService;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Advertisements.Interfaces
{
    /// <summary>
    /// Интерфейс описывающий сервис для работы с пользователями
    /// </summary>
    public interface IUserService
    {
        /// <summary>
        /// Добавляет/изменяет пользователя
        /// </summary>
        /// <param name="req"></param>
        /// <returns></returns>
        Task<bool> Update(UpdateUserRequest request);
        /// <summary>
        /// Удаляет пользователя
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        Task<bool> Delete(Guid id);
        /// <summary>
        /// Возвращает список пользователей
        /// </summary>
        /// <returns></returns>
        Task<List<UserResponse>> Get();
    }
}
