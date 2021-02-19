using Advertisements.Data.Enums;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace Advertisements.Interfaces.Models.UserService
{
    /// <summary>
    /// Входная модель для добавления/редактирования пользователя
    /// </summary>
    public class UpdateUserRequest
    {
        /// <summary>
        /// Идентификатор
        /// </summary>
        public Guid? Id { get; set; }
        /// <summary>
        /// Имя
        /// </summary>
        [Required(ErrorMessage = "Введите имя.")]
        public string Name { get; set; }
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
        /// <summary>
        /// Роль
        /// </summary>
        [Required(ErrorMessage = "Укажите роль.")]
        public UserRoles Role { get; set; }
    }
}
