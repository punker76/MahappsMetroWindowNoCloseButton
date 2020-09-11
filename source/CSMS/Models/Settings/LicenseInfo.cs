using System;
using System.Security;


namespace WpfFramework.Models.Settings
{
    public class LicenseInfo
    {
        public Guid ID { get; set; }

        public bool isValid { get; set; }
        public DateTime LastLicenseCheck { get; set; }
        public SecureString License { get; set; }

        public LicenseInfo()
        {
            ID = Guid.NewGuid();
        }

        public LicenseInfo(Guid id)
        {
            ID = id;
        }

        public LicenseInfo(Guid id,  SecureString license)
        {
            ID = id;
            License = license;
        }

        public LicenseInfo(Guid id, SecureString license, bool valid, DateTime lastCheck)
        {
            ID = id;
            License = license;
            isValid = valid;
            LastLicenseCheck = lastCheck;
        }
    }
}
