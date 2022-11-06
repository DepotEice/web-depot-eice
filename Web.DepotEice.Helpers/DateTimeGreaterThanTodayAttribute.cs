using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace Web.DepotEice.Helpers
{
    public class DateTimeGreaterThanTodayAttribute : ValidationAttribute
    {
        public DateTimeGreaterThanTodayAttribute()
        {

        }

        protected override ValidationResult? IsValid(object? value, ValidationContext validationContext)
        {
            string errorMessage = FormatErrorMessage(validationContext.DisplayName);

            if (value is null)
            {
                return new ValidationResult("The DateTime cannot be null");
            }

            if (!(value is DateTime minimumDateTime))
            {
                return new ValidationResult("The value is not a type of DateTime");
            }

            if (DateTime.Now > minimumDateTime)
            {
                return new ValidationResult(errorMessage);
            }

            return ValidationResult.Success;
        }
    }
}
