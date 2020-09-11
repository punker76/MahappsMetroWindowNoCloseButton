using WpfFramework.Utilities;
using System.Globalization;
using System.Text.RegularExpressions;
using System.Windows.Controls;

namespace WpfFramework.Validators
{
    public class RepetierApiKeyValidator : ValidationRule
    {
        public override ValidationResult Validate(object value, CultureInfo cultureInfo)
        {
            if (value != null && (Regex.IsMatch((string)value, RegexHelper.RepetierServerProApiKey)))
                return ValidationResult.ValidResult;

            return new ValidationResult(false, Resources.Localization.Strings.EnterValidRepetierApiKey);
        }
    }
}
