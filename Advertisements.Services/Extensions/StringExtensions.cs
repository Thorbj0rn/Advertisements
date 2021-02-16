using System;
using System.Collections.Generic;
using System.Security.Cryptography;
using System.Text;

namespace Advertisements.Services.Extensions
{
    /// <summary>
    /// Расширения для работы со строками
    /// </summary>
    public static class StringExtensions
    {
        /// <summary>
        /// Хэширует строку
        /// </summary>
        /// <param name="input"></param>
        /// <param name="salt"></param>
        /// <returns></returns>
        public static string GetSHA256Hash(this string input, string salt)
        {
            var sha256 = new SHA256Managed();
            var bytes = Encoding.UTF8.GetBytes(salt + input);
            var result = sha256.ComputeHash(bytes);
            var stringRes = BitConverter.ToString(result);
            return stringRes;
        }
    }
}
