using Advertisements.Data;
using Advertisements.Data.Entities;
using Advertisements.Interfaces;
using Advertisements.Interfaces.Models.UserService;
using Microsoft.EntityFrameworkCore;
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
        public async Task<bool> DeleteUser(Guid id)
        {
            try
            {
                var user = new User { Id = id };
                _dataContext.Remove(user);
                await _dataContext.SaveChangesAsync();

                return true;
            }
            catch(Exception ex)
            {
                _logger.LogError(ex.Message);
                return false; 
            }
        }

        /// <summary>
        /// Добавляет/изменяет пользователя
        /// </summary>
        /// <param name="req"></param>
        /// <returns></returns>
        public async Task<bool> UpdateUser(UpdateUserRequest req)
        {
            try
            {
                var user = new User
                {
                    Id = req.Id.GetValueOrDefault(Guid.Empty),
                    Name = req.Name
                };

                _dataContext.Attach(user);
                _dataContext.Entry(user).State = req.Id.HasValue ? EntityState.Modified : EntityState.Added;

                await _dataContext.SaveChangesAsync();

                return true;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message);
                return false;
            }
        }
    }
}
