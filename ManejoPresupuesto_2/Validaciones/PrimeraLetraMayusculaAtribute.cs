using System.ComponentModel.DataAnnotations;

namespace ManejoPresupuesto_2.Validaciones
{
    public class PrimeraLetraMayusculaAtribute : ValidationAttribute
    {
        protected override ValidationResult IsValid(object value, ValidationContext validationContext)
        {
            if (value == null || string.IsNullOrEmpty(value.ToString()))
            {
                return ValidationResult.Success;
            }

            var primerLetra = value.ToString()[0].ToString();

            if(primerLetra != primerLetra.ToUpper())
            {
                return new ValidationResult("La primer letra debe ser mayúscula");
            }
            return ValidationResult.Success;
        }
    }
}
