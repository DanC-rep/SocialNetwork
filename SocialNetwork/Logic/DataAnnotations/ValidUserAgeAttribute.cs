using System.ComponentModel.DataAnnotations;

namespace Logic.DataAnnotations
{
    public class ValidUserAgeAttribute : ValidationAttribute
    {
        private readonly int minAge;
        private readonly int maxAge;

        public ValidUserAgeAttribute(int _minAge = 0, int _maxAge = 100)
        {
            minAge = _minAge;
            maxAge = _maxAge;

            ErrorMessage = $"Вам должно быть от {minAge} до {maxAge} лет";
        }

        protected override ValidationResult? IsValid(object? value, ValidationContext validationContext)
        {
            if (value is DateTime)
            {
                DateTime birthDate = (DateTime)value;

                if (birthDate > DateTime.Now)
                {
                    return new ValidationResult("Ввведите корректную дату");
                }

                var age = DateTime.Now.Year - birthDate.Year;

                if (birthDate.Month > DateTime.Now.Month || (birthDate.Month == DateTime.Now.Month
                    && birthDate.Day > DateTime.Now.Day))
                {
                    age--;
                }

                if (age < minAge || age > maxAge)
                {
                    return new ValidationResult(ErrorMessage);
                }
            }
            else
            {
                return new ValidationResult("Неправильный формат даты");
            }

            return ValidationResult.Success;
        }
    }
}
