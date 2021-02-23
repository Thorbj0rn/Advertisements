using Advertisements.Data;
using Advertisements.Interfaces;
using Advertisements.Interfaces.Models.AuthService;
using Advertisements.Services.Extensions;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace Advertisements.Services
{
    /// <summary>
    /// Сервис для работы с авторизацией
    /// </summary>
    public class AuthService: IAuthService
    {
        private readonly DataContext _dataContext;
        private readonly ILogger<AuthService> _logger;

        public AuthService(DataContext dataContext, ILogger<AuthService> logger)
        {
            _dataContext = dataContext;
            _logger = logger;
        }

        public async Task<ClaimsIdentity> GetIdentity(IdentityRequest request)
        {
            try
            {
                var user = await _dataContext.Users.FirstOrDefaultAsync(x => x.Login == request.Login);
                if (user == null)
                    throw new Exception("Пользователь не найден.");

                var password = request.Password.GetSHA256Hash(user.PassKey);

                if (user.Password != password)
                    throw new Exception("Неверный пароль.");

                var claims = new List<Claim>
                {
                    new Claim(ClaimTypes.NameIdentifier, user.Id.ToString()),
                    new Claim(ClaimsIdentity.DefaultNameClaimType, user.Login),
                    new Claim(ClaimsIdentity.DefaultRoleClaimType, user.Role.ToString())
                };
                ClaimsIdentity claimsIdentity =
                new ClaimsIdentity(claims, "Token", ClaimsIdentity.DefaultNameClaimType,
                    ClaimsIdentity.DefaultRoleClaimType);
                return claimsIdentity;

            }
            catch (Exception exception)
            {
                _logger.LogError(exception.Message);
                throw exception;
            }
        }
    }
}
