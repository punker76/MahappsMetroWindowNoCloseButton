using System.Globalization;
using System.Security;
using System.Text.RegularExpressions;
using System.Windows.Controls;
using WpfFramework.Models.Settings;
using WpfFramework.Utilities;

namespace WpfFramework.Validators
{
    public class LicenseCodeValidator : ValidationRule
    {
        public override ValidationResult Validate(object value, CultureInfo cultureInfo)
        {
            string license = SecureStringHelper.ConvertToString((SecureString)value);
            if (!string.IsNullOrEmpty(license) && (Regex.IsMatch((string)license, LicenseManager.FormatCheck.ToString())))
                return ValidationResult.ValidResult;

            return new ValidationResult(false, Resources.Localization.Strings.EnterValidRepetierApiKey);
        }
    }
}
