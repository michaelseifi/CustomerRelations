using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;
namespace daisybrand.forecaster.Validations
{
    public class ForecastValidationRule: ValidationRule
    {
        public ForecastValidationRule()
        {
            
        }
        protected ForecastValidationRule(ValidationStep validationStep, bool validatesOnTargetUpdated)
            : base(validationStep, validatesOnTargetUpdated)
        {
            
        }

        public override ValidationResult Validate(object value, System.Globalization.CultureInfo cultureInfo)
        {
            if (value.ToString() == "") return new ValidationResult(true, null); 
            try
            {
                int forecast = int.Parse(value.ToString());
                return new ValidationResult(true, null);
            }
            catch (Exception ex)
            {
                return new ValidationResult(false, "Please enter a valid number");
            }
        }
    }
}
