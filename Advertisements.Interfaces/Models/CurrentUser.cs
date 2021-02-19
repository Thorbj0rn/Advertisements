using Advertisements.Data.Enums;
using System;
using System.Collections.Generic;
using System.Text;

namespace Advertisements.Interfaces.Models
{
    /// <summary>
    /// Модель текущего пользователя
    /// </summary>
    public class CurrentUser
    {
        /// <summary>
        /// Идентификатор
        /// </summary>
        public Guid Id { get; set; }
        /// <summary>
        /// Роль
        /// </summary>
        public UserRoles Role { get; set; }
    }
}
