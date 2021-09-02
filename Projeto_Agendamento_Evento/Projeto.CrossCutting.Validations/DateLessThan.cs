using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace Projeto.CrossCutting.Validations
{
    public class DateLessThan : ValidationAttribute
    {
        private readonly string comparisonProperty;

        public DateLessThan(string comparisonProperty)
        {
            this.comparisonProperty = comparisonProperty;
        }

        protected override ValidationResult IsValid(object value, ValidationContext validationContext)
        {
            string Message = string.Empty;

            if (value is DateTime)
            {
                var property = validationContext.ObjectType.GetProperty(comparisonProperty);

                if (property == null)
                    Message = string.Format("Propriedade {0} não encontrada.", property.Name);
                else
                {
                    var comparisonValue = (DateTime?)property.GetValue(validationContext.ObjectInstance);

                    if (comparisonValue == null || Convert.ToDateTime(value) < comparisonValue)
                        return ValidationResult.Success;
                    else
                        Message = string.Format("Por favor, informe uma data inferior à {0:dd/MM/yyyy}", comparisonValue);

                }
            }
            else
                Message = "Tipo do objeto inválido.";

            return new ValidationResult(Message);
        }
    }
}
