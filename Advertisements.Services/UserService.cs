using Advertisements.Data;
using Advertisements.Data.Entities;
using Advertisements.Data.Enums;
using Advertisements.Interfaces;
using Advertisements.Interfaces.Models;
using Advertisements.Interfaces.Models.UserService;
using Advertisements.Services.Extensions;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
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
        public async Task<bool> Delete(Guid id)
        {
            try
            {
                var user = new User { Id = id };
                _dataContext.Remove(user);
                await _dataContext.SaveChangesAsync();

                return true;
            }
            catch(Exception exception)
            {
                _logger.LogError(exception.Message);
                throw exception; 
            }
        }

        /// <summary>
        /// Возвращает список пользователей
        /// </summary>
        /// <returns></returns>
        public async Task<List<UserResponse>> Get()
        {
            try
            {
                var users = await _dataContext.Users
                    .Select(u => new UserResponse
                    {
                        Id = u.Id,
                        Name = u.Name,
                        Login = u.Login,
                        Role = u.Role
                    })
                    .ToListAsync();
                return users;
            }
            catch(Exception exception)
            {
                _logger.LogError(exception.Message);
                throw exception;
            }
        }

        /// <summary>
        /// Добавляет/изменяет пользователя
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        public async Task<bool> Update(UpdateUserRequest request)
        {
            try
            {
                var key = Guid.NewGuid().ToString();
                var password = request.Password.GetSHA256Hash(key);

                var user = new User
                {
                    Id = request.Id.GetValueOrDefault(Guid.Empty),
                    Name = request.Name,
                    Login = request.Login,
                    Password = password,
                    PassKey = key,
                    Role = request.Role                    
                };

                _dataContext.Attach(user);
                _dataContext.Entry(user).State = request.Id.HasValue ? EntityState.Modified : EntityState.Added;

                await _dataContext.SaveChangesAsync();

                return true;
            }
            catch (Exception exception)
            {
                _logger.LogError(exception.Message);
                throw exception;
            }
        }
    }
}
