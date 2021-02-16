using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.Text;

namespace Advertisements.Interfaces.Options
{
    public class AuthOptions
    {
        public static string Issuer = "AdvertisementServer"; // издатель токена
        public static string Audience = "AdvertisementClient"; // потребитель токена
        const string Key = "2d8f62ec-a923-4842-8036-6e6cc8304e7d";   // ключ для шифрации
        public  const int Lifetime = 60; // время жизни токена - 1 минута
        public static SymmetricSecurityKey GetSymmetricSecurityKey()
        {
            return new SymmetricSecurityKey(Encoding.ASCII.GetBytes(Key));
        }
    }
}
