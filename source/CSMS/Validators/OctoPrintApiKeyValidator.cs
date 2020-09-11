using System.Globalization;
using System.Text.RegularExpressions;
using System.Windows.Controls;
using WpfFramework.Utilities;

namespace WpfFramework.Validators
{
    public class OctoPrintApiKeyValidator : ValidationRule
    {
        public override ValidationResult Validate(object value, CultureInfo cultureInfo)
        {
            if (value != null && (Regex.IsMatch((string)value, RegexHelper.OctoPrintApiKey)))
                return ValidationResult.ValidResult;

            return new ValidationResult(false, Resources.Localization.Strings.EnterValidRepetierApiKey);
        }
    }
}
