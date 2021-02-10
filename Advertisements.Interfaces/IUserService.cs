﻿using Advertisements.Interfaces.Models.UserService;
using System;
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
        Task<bool> UpdateUser(UpdateUserRequest req);
        /// <summary>
        /// Удаляет пользователя
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        Task<bool> DeleteUser(Guid id);
    }
}
