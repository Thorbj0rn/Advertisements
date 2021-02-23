using Advertisements.Interfaces.Models.AdvertisementService;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.IO;
using System.Text;

namespace Advertisements.Interfaces.ValidationAttributes
{
    public class RequiredFileExtension : ValidationAttribute
    {
        public RequiredFileExtension(string extensions)
        {
            Extensions = extensions;
        }

        public string Extensions { get; }

        public string GetErrorMessage() =>
            $"Загружаемый файл должен быть с расширением {Extensions}.";

        protected override ValidationResult IsValid(object value,
            ValidationContext validationContext)
        {
            var file = (IFormFile)value;
            if (file != null)
            {
                var extension = Path.GetExtension(file.FileName).Trim('.');

                if (!Extensions.Contains(extension))
                {
                    return new ValidationResult(GetErrorMessage());
                }
            }
            return ValidationResult.Success;
        }
    }
}
