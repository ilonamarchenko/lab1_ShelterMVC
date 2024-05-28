using System;
using System.ComponentModel.DataAnnotations;

namespace ShelterDomain.Validation
{
    public class DateNotInFutureAttribute : ValidationAttribute
    {
        protected override ValidationResult IsValid(object value, ValidationContext validationContext)
        {
            if (value != null)
            {
                DateTime date = (DateTime)value;
                if (date > DateTime.Now)
                {
                    return new ValidationResult("Дата створення не може бути у майбутньому.");
                }
            }
            return ValidationResult.Success;
        }
    }
}
