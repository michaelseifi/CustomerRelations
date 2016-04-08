using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;
namespace daisybrand.forecaster.Validations
{
    public class DecimalValidationRule : ValidationRule
    {
        public DecimalValidationRule()
        {

        }
        protected DecimalValidationRule(ValidationStep validationStep, bool validatesOnTargetUpdated)
            : base(validationStep, validatesOnTargetUpdated)
        {

        }

        public override ValidationResult Validate(object value, System.Globalization.CultureInfo cultureInfo)
        {
            if (value.ToString() == "") return new ValidationResult(true, null);
            try
            {
                var val = Convert.ToDouble(value.ToString());
                return new ValidationResult(true, null);
            }
            catch (Exception ex)
            {
                return new ValidationResult(false, "Please enter a valid number");
            }
        }
    }
}
