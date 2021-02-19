using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
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
        [Required(ErrorMessage = "Введите логин.")]
        public string Login { get; set; }
        /// <summary>
        /// Пароль
        /// </summary>
        [Required(ErrorMessage = "Введите пароль.")]
        public string Password { get; set; }
    }
}
