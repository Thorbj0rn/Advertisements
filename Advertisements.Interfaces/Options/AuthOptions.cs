using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.Text;

namespace Advertisements.Interfaces.Options
{
    public class AuthOptions
    {
        public static string Issuer = "AdvertisementServer"; 
        public static string Audience = "AdvertisementClient"; 
        const string Key = "2d8f62ec-a923-4842-8036-6e6cc8304e7d";   
        public  const int Lifetime = 60; 
        public static SymmetricSecurityKey GetSymmetricSecurityKey()
        {
            return new SymmetricSecurityKey(Encoding.ASCII.GetBytes(Key));
        }
    }
}
