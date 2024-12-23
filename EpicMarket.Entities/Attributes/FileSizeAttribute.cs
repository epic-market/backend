using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Http;

namespace EpicMarket.Entities.Attributes
{
    public class FileSizeAttribute : ValidationAttribute
    {
        private readonly int _maxSize;

        public FileSizeAttribute(int maxSize)
        {
            _maxSize = maxSize;
        }

        protected override ValidationResult IsValid(object value, ValidationContext validationContext)
        {
            if (value is IFormFile file)
            {
                if (file.Length > _maxSize)
                {
                    return new ValidationResult($"File size cannot exceed {_maxSize / (1024 * 1024)} MB");
                }
            }
            else if (value is IFormFile[] files)
            {
                foreach (var f in files)
                {
                    if (f.Length > _maxSize)
                    {
                        return new ValidationResult($"File size cannot exceed {_maxSize / (1024 * 1024)} MB");
                    }
                }
            }
            return ValidationResult.Success;
        }
    }
} 