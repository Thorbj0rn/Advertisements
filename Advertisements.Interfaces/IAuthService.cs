using Advertisements.Interfaces.Models.AuthService;
using System;
using System.Collections.Generic;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace Advertisements.Interfaces
{
    /// <summary>
    /// Интерфейс описывающий сервис авторизации
    /// </summary>
    public interface IAuthService
    {
        /// <summary>
        /// Идентифицирует пользователя
        /// </summary>
        /// <param name="req"></param>
        /// <returns></returns>
        Task<ClaimsIdentity> GetIdentity(IdentityRequest req);
    }
}
