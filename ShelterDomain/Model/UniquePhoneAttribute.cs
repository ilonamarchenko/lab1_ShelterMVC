using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using ShelterDomain.Model;

namespace ShelterInfrastructure.Validation
{
    public class UniquePhoneAttribute : ValidationAttribute
    {
        protected override ValidationResult IsValid(object value, ValidationContext validationContext)
        {
            var shelter = (Shelter)validationContext.ObjectInstance;
            var shelters = (IEnumerable<Shelter>)validationContext.GetService(typeof(IEnumerable<Shelter>));

            if (shelters.Any(s => s.Contact == shelter.Contact && s.ShelterId != shelter.ShelterId))
            {
                return new ValidationResult(ErrorMessage);
            }

            return ValidationResult.Success;
        }
    }
}
