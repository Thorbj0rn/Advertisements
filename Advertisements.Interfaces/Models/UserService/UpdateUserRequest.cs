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
    }
}
