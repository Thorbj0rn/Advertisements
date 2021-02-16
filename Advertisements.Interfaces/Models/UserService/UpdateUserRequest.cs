using Advertisements.Data.Enums;
using System;
using System.Collections.Generic;
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
        public string Name { get; set; }
        /// <summary>
        /// Логин
        /// </summary>
        public string Login { get; set; }
        /// <summary>
        /// Пароль
        /// </summary>
        public string Password { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public string PassKey { get; set; }
        /// <summary>
        /// Роль
        /// </summary>
        public UserRoleEnum Role { get; set; }
    }
}
