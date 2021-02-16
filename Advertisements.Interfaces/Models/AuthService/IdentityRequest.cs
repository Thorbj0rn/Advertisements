using System;
using System.Collections.Generic;
using System.Text;

namespace Advertisements.Interfaces.Models.AuthService
{
    /// <summary>
    /// Входная модель для идентификации пользователя
    /// </summary>
    public class IdentityRequest
    {
        /// <summary>
        /// Логин
        /// </summary>
        public string Login { get; set; }
        /// <summary>
        /// Пароль
        /// </summary>
        public string Password { get; set; }
    }
}
