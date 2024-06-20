using System.ComponentModel.DataAnnotations;
using System.IO;

namespace ConfigValidation
{
    public class ExistingDirectoryAttribute : ValidationAttribute
    {
        // protected override ValidationResult IsValid(object value, ValidationContext validationContext)
        // {
        //     if (IsValid(value))
        //     {
        //         return ValidationResult.Success;
        //     }
        //
        //     return new ValidationResult("BAD!");
        // }

        public override bool IsValid(object value)
        {
            if (!(value is string dirPath))
            {
                return false;
            }

            return Directory.Exists(dirPath);
        }
    }
}