using System;
using System.Collections.Generic;
using System.Text;

namespace Advertisements.Interfaces.Models.UserService
{
    /// <summary>
    /// Выходная модель данных пользователя
    /// </summary>
    public class UserResponse
    {
        /// <summary>
        /// Идентификатор
        /// </summary>
        public Guid Id { get; set; }
        /// <summary>
        /// Имя
        /// </summary>
        public string Name { get; set; }
    }
}
