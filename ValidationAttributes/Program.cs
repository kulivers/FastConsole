using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace ConfigValidation
{
    public class NestedClassAttribute : ValidationAttribute
    {
        protected override ValidationResult IsValid(object value, ValidationContext validationContext)
        {
            if (value == null)
            {
                return ValidationResult.Success;
            }

            if (!(value is NestedClass kata))
            {
                throw new InvalidCastException();
            }

            return ValidationResult.Success;
        }
    }
    public class NestedClass
    {
        public string Some { get; set; }
    }
    public class Config
    {
        [NestedClass]
        public NestedClass Nullable { get; set; } = new NestedClass() { Some = "SISKAMAMd" };
    }

    
    internal class Program
    {

        public static void CW<T>(T t)
        {
            var type = t.GetType().Name;
            throw new ArgumentNullException(type);
        }

        public static void Main(string[] args)
        {
            var config = new Config();
            CW(config);
            var context = new ValidationContext(config);
            var validationResults = new List<ValidationResult>();
            var tryValidateObject = Validator.TryValidateObject(config, context, validationResults, validateAllProperties: true);
            // Validator.ValidateObject(config, context, true);
        }
    }
}