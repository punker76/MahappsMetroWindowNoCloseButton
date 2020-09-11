using System;


namespace WpfFramework.Models.Settings
{
    public class LicenseInfoSerializable
    {
        public Guid ID { get; set; }
        public string License { get; set; }

        public bool isValid { get; set; }
        public DateTime LastLicenseCheck { get; set; }

        public LicenseInfoSerializable()
        {

        }

        public LicenseInfoSerializable(Guid id, string license, bool valid, DateTime LastCheck)
        {
            ID = id;
            License = license;
            isValid = valid;
            LastLicenseCheck = LastCheck;
        }
    }
}
