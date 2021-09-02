using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace Projeto.CrossCutting.Validations
{
    public class TimeGreaterThan : ValidationAttribute
    {
        private readonly string comparisonProperty;

        public TimeGreaterThan(string comparisonProperty)
        {
            this.comparisonProperty = comparisonProperty;
        }

        protected override ValidationResult IsValid(object value, ValidationContext validationContext)
        {
            string Message = string.Empty;

            if (value is TimeSpan)
            {
                var property = validationContext.ObjectType.GetProperty(comparisonProperty);

                if (property == null)
                    Message = string.Format("Propriedade {0} não encontrada.", property.Name);
                else
                {
                    var comparisonValue = (TimeSpan?)property.GetValue(validationContext.ObjectInstance);

                    if (comparisonValue == null || TimeSpan.Parse(value.ToString()) > comparisonValue)
                        return ValidationResult.Success;
                    else
                        Message = "Por favor, informe a hora superior à " + ((TimeSpan)comparisonValue).ToString("hh':'mm");
                }
            }
            else
                Message = "Tipo do objeto inválido.";

            return new ValidationResult(Message);
        }
    }
}

