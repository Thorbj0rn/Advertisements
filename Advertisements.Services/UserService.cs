using Advertisements.Data;
using Advertisements.Interfaces;
using Advertisements.Interfaces.Models.UserService;
using Microsoft.Extensions.Logging;
using System;
using System.Threading.Tasks;

namespace Advertisements.Services
{
    /// <summary>
    /// Сервис для работы с пользователями
    /// </summary>
    public class UserService: IUserService
    {
        private readonly DataContext _dataContext;
        private readonly ILogger<UserService> _logger;

        public UserService(DataContext dataContext, ILogger<UserService> logger)
        {
            _dataContext = dataContext;
            _logger = logger;
        }

        /// <summary>
        /// Удаляет пользователя
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public Task<bool> DeleteUser(Guid id)
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// Добавляет/изменяет пользователя
        /// </summary>
        /// <param name="req"></param>
        /// <returns></returns>
        public Task<bool> UpdateUser(UpdateUserRequest req)
        {
            throw new NotImplementedException();
        }
    }
}
