using System.Globalization;
using System.Text.RegularExpressions;
using System.Windows.Controls;
using WpfFramework.Utilities;

namespace WpfFramework.Validators
{
    public class RepetierServerConnectionStringValidator : ValidationRule
    {
        public override ValidationResult Validate(object value, CultureInfo cultureInfo)
        {
            if (value != null && (Regex.IsMatch((string)value, RegexHelper.IPv4AddressRegex) || 
                Regex.IsMatch((string)value, RegexHelper.IPv6AddressRegex) || 
                Regex.IsMatch((string)value, RegexHelper.Fqdn)
                ))
                return ValidationResult.ValidResult;

            return new ValidationResult(false, Resources.Localization.Strings.EnterValidIPAddress);
        }
    }
}
