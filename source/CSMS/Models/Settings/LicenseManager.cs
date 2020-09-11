// Contains code from: https://stackoverflow.com/questions/10168240/encrypting-decrypting-a-string-in-c-sharp 

using System;
using WpfFramework.Utilities;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using System.Security;
using System.Security.Cryptography;
using System.Text;
using System.Xml.Serialization;
using System.Net;
using System.Web;
using System.Runtime.Serialization.Json;
using System.Management;
using System.Text.RegularExpressions;
using log4net;
using WpfFramework.Resources.Localization;

namespace WpfFramework.Models.Settings
{
    public static class LicenseManager
    {
        #region Variables
        public const string LicensesFileName = "Licenes";
        public const string LicensesExtension = "xml";

        public static Regex FormatCheck = new Regex("^AR-((\\w{8})-){2}(\\w{8})$");
        public static Regex FormatCheckEnvato = new Regex("^(\\w{8})-((\\w{4})-){3}(\\w{12})$", RegexOptions.IgnoreCase);

        private static readonly ILog logger = LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

        // Collection with ID, Name, Username, Password
        public static ObservableCollection<LicenseInfo> Licenses = new ObservableCollection<LicenseInfo>();
        #endregion
        
        // List with ID and Name
        public static IEnumerable<LicenseInfo> LicenseInfoList
        {
            get { return Licenses.Select(x => new LicenseInfo(x.ID)); }
        }

        public static LicenseInfo GetLicenseByID(Guid id)
        {
            return Licenses.FirstOrDefault(x => x.ID == id);
        }

        public static bool LicensesChanged { get; set; }

        public static bool IsLoaded { get; private set; }

        public static bool hasValidLicense
        {
            get
            {
                if (!IsLoaded || Licenses.Count == 0)
                    return false;
                else if (Licenses.Count == 1 && Licenses[0].isValid)
                    return true;
                else
                    return false;
            }
        }

        public static bool IsOnline { get => LicenseServerIsAvailable(); }
        private static bool LicenseServerIsAvailable()
        {
            try
            {
                using (var client = new WebClient())
                using (client.OpenRead(GlobalStaticConfiguration.licenseUri))
                    return true;
            }
            catch
            {
                return false;
            }
        }

        private static SecureString _masterPassword;

        public static bool VerifyMasterPasword(SecureString license)
        {
            return SecureStringHelper.ConvertToString(_masterPassword).Equals(SecureStringHelper.ConvertToString(license));
        }

        public static string GetLicensesFileName()
        {
            return $"{LicensesFileName}.{LicensesExtension}";
        }

        public static string GetLicensesFilePath()
        {
            try
            {
                string licensePath = Path.Combine(SettingsManager.GetSettingsLocation(), GetLicensesFileName());
                logger.InfoFormat(Strings.EventFetchLicensePathFormated, licensePath);
                return licensePath;
            }
            catch (Exception exc)
            {
                logger.ErrorFormat(Strings.EventExceptionOccurredFormated, exc.TargetSite, exc.Message);
                return string.Empty;
            }
        }

        public static bool Load(SecureString pasword)
        {
            try
            {
                byte[] xml = null;

                // Decrypt file
                if (File.Exists(GetLicensesFilePath()))
                {
                    var cipherWithSaltAndIv = File.ReadAllBytes(GetLicensesFilePath());

                    xml = Decrypt(cipherWithSaltAndIv, SecureStringHelper.ConvertToString(pasword));
                }

                // Save master pw for encryption
                SetMasterPassword(pasword);

                // Check if array is empty...
                if (xml != null && xml.Length > 0)
                    DeserializeFromByteArray(xml);

                Licenses.CollectionChanged += Licenses_CollectionChanged;

                IsLoaded = true;

                return true;
            }
            catch (CryptographicException exc)
            {
                logger.ErrorFormat(Strings.EventExceptionOccurredFormated, exc.TargetSite, exc.Message);
                return false;
            }
        }

        private static void DeserializeFromByteArray(byte[] xml)
        {
            var xmlSerializer = new XmlSerializer(typeof(List<LicenseInfoSerializable>));

            using (var memoryStream = new MemoryStream(xml))
            {
                ((List<LicenseInfoSerializable>)(xmlSerializer.Deserialize(memoryStream))).ForEach(license => 
                AddLicense(new LicenseInfo(license.ID, SecureStringHelper.ConvertToSecureString(license.License), license.isValid, license.LastLicenseCheck)));
            }
        }

        public static void Save()
        {
            try
            {
                // Create the directory if it does not exist
                Directory.CreateDirectory(SettingsManager.GetSettingsLocation());
                // Serialize as xml (utf-8)
                byte[] credentials = SerializeToByteArray();

                // Encrypt with master pw and save file
                byte[] encrypted = Encrypt(credentials, SecureStringHelper.ConvertToString(_masterPassword));

                // Check if the path exists, create if not
                string path = GetLicensesFilePath();
                File.WriteAllBytes(path, encrypted);
                logger.InfoFormat(Strings.EventLicenseSavedSuccessfullyAtFormated, path);

                LicensesChanged = false;
            }
            catch (Exception exc)
            {
                logger.ErrorFormat(Strings.EventExceptionOccurredFormated, exc.TargetSite, exc.Message);
            }
        }

        private static byte[] SerializeToByteArray()
        {
            // Convert CredentialInfo to CredentialInfoSerializable
            var list = new List<LicenseInfoSerializable>();

            foreach (var info in Licenses)
            {
                list.Add(new LicenseInfoSerializable(info.ID, SecureStringHelper.ConvertToString(info.License), info.isValid, info.LastLicenseCheck));
            }

            var xmlSerializer = new XmlSerializer(typeof(List<LicenseInfoSerializable>));

            using (var memoryStream = new MemoryStream())
            {
                using (var streamWriter = new StreamWriter(memoryStream, Encoding.UTF8))
                {
                    xmlSerializer.Serialize(streamWriter, list);
                    return memoryStream.ToArray();
                }
            }
        }

        private static void Licenses_CollectionChanged(object sender, System.Collections.Specialized.NotifyCollectionChangedEventArgs e)
        {
            LicensesChanged = true;
            //OnPropertyChanged(nameof(hasValidLicense));
        }

        public static void SetMasterPassword(SecureString masterPasword)
        {
            _masterPassword = masterPasword;
        }

        public static void AddLicense(LicenseInfo license)
        {
            try
            {
                Licenses.Add(license);
                logger.InfoFormat(Strings.EventLicenseAddFormated, license.ToString());
            }
            catch (Exception exc)
            {
                logger.ErrorFormat(Strings.EventExceptionOccurredFormated, exc.TargetSite, exc.Message);
            }
        }

        public static void RemoveLicense(LicenseInfo license)
        {
            try
            {
                Licenses.Remove(license);
                logger.InfoFormat(Strings.EventLicenseRemovedFormated, license.ToString());
            }
            catch (Exception exc)
            {
                logger.ErrorFormat(Strings.EventExceptionOccurredFormated, exc.TargetSite, exc.Message);
            }
        }

        #region Encryption / Decryption
        private const int KeySize = 256;
        private const int Iterations = 100000;

        private static byte[] Encrypt(byte[] text, string password)
        {
            var salt = Generate256BitsOfRandomEntropy();
            var iv = Generate256BitsOfRandomEntropy();

            using (var rfc2898DeriveBytes = new Rfc2898DeriveBytes(password, salt, Iterations))
            {
                var key = rfc2898DeriveBytes.GetBytes(KeySize / 8); // 256 Bits / 8 Bits = 32 Bytes

                using (var rijndaelManaged = new RijndaelManaged())
                {
                    rijndaelManaged.BlockSize = 256;
                    rijndaelManaged.Mode = CipherMode.CBC;
                    rijndaelManaged.Padding = PaddingMode.PKCS7;

                    using (var encryptor = rijndaelManaged.CreateEncryptor(key, iv))
                    {
                        using (var memoryStream = new MemoryStream())
                        {
                            using (var cryptoStream = new CryptoStream(memoryStream, encryptor, CryptoStreamMode.Write))
                            {
                                cryptoStream.Write(text, 0, text.Length);
                                cryptoStream.FlushFinalBlock();

                                var cipher = salt;
                                cipher = cipher.Concat(iv).ToArray();
                                cipher = cipher.Concat(memoryStream.ToArray()).ToArray();

                                memoryStream.Close();
                                cryptoStream.Close();

                                return cipher;
                            }
                        }
                    }
                }
            }
        }

        private static byte[] Decrypt(byte[] cipherWithSaltAndIv, string password)
        {
            var salt = cipherWithSaltAndIv.Take(KeySize / 8).ToArray(); // 256 bits / 8 bits = 32 bytes
            var iv = cipherWithSaltAndIv.Skip(KeySize / 8).Take(KeySize / 8).ToArray(); // Skip 32 bytes, take 32 Bytes iv
            var cipher = cipherWithSaltAndIv.Skip((KeySize / 8) * 2).Take(cipherWithSaltAndIv.Length - ((KeySize / 8) * 2)).ToArray(); // Skip 64 bytes, take cipher bytes (length - 64)

            using (var rfc2898DeriveBytes = new Rfc2898DeriveBytes(password, salt, Iterations))
            {
                var key = rfc2898DeriveBytes.GetBytes(KeySize / 8); // 256 Bits / 8 Bits = 32 Bytes

                using (var rijndaelManaged = new RijndaelManaged())
                {
                    rijndaelManaged.BlockSize = 256;
                    rijndaelManaged.Mode = CipherMode.CBC;
                    rijndaelManaged.Padding = PaddingMode.PKCS7;

                    using (var decryptor = rijndaelManaged.CreateDecryptor(key, iv))
                    {
                        using (var memoryStream = new MemoryStream(cipher))
                        {
                            using (var cryptoStream = new CryptoStream(memoryStream, decryptor, CryptoStreamMode.Read))
                            {
                                var text = new byte[cipher.Length];
                                cryptoStream.Read(text, 0, text.Length);

                                memoryStream.Close();
                                cryptoStream.Close();

                                return text;
                            }
                        }
                    }
                }
            }
        }
        private static byte[] Generate256BitsOfRandomEntropy()
        {
            var randomBytes = new byte[32]; // 32 * 8 = 256 Bits.

            using (var rngCryptoServiceProvider = new RNGCryptoServiceProvider())
            {
                rngCryptoServiceProvider.GetBytes(randomBytes);
            }

            return randomBytes;
        }
        #endregion

        #region Verify / Check
        public static bool verifyLicenseFormat(string PurchaseCode)
        {
            if (PurchaseCode.Length == 0)
                return false;
            return Regex.IsMatch(PurchaseCode, "^AR-((\\w{8})-){2}(\\w{8})$", RegexOptions.IgnoreCase);
        }

        private static bool statusLicenseEnvato(out string resultMessage, LicenseInfo License)
        {
            var builder = new UriBuilder(GlobalStaticConfiguration.author_sales_enpoint_uri);          
            var syncClient = new WebClient();
            syncClient.Headers.Add("Authorization", $"Bearer {GlobalStaticConfiguration.accesstoken}");
            syncClient.Headers.Add("User-Agent", "A software tool to calculate prices for 3D Prints.");

            ServicePointManager.Expect100Continue = true;
            ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls12;
            try
            {
                //GET DATA FROM PURCHASE CODE
                string data = queryString(builder, SecureStringHelper.ConvertToString(License.License));
                var responseStream = syncClient.DownloadString(data);

                DataContractJsonSerializer js = new DataContractJsonSerializer(typeof(VerifyPurchaseCodeRespone));
                var ms = new MemoryStream(System.Text.ASCIIEncoding.ASCII.GetBytes(responseStream));

                VerifyPurchaseCodeRespone ar = (VerifyPurchaseCodeRespone)js.ReadObject(ms);
                //CHECK IF TE ITEM ID IS THE SAME AS SET FOR THIS TOOL
                if (ar.Item.Id == GlobalStaticConfiguration.ItemId)
                {
                    License.isValid = true;
                    resultMessage = "License is valid!";
                        return true;
                    }
                    else
                    {
                    resultMessage = "License is valid, but for a diffrent item";
                        return false;
                    }
                }
            catch (Exception exc)
            {
                resultMessage = "The purchase code you've provided is not valid for this application";
                logger.ErrorFormat(Strings.EventExceptionOccurredFormated, exc.TargetSite, exc.Message);
                return false;
            }
            
        }

        public static bool activateLicenseWooSl(out string resultMessage, LicenseInfo lic)
        {
            if (!verifyLicenseFormat(SecureStringHelper.ConvertToString(lic.License)))
            {
                throw new Exception("The license key format of \"" + SecureStringHelper.ConvertToString(lic.License) + "\" is invalid!");
            }
            ActivationResponseWooSl[] ar = responseWooSl(Woo_SL_Action.Activate, lic);
            try
            {
                if (ar == null)
                {
                    resultMessage = "No internet connection available...";
                    return false;
                }
                if (ar[0].Status == "success" && (ar[0].ErrorCode == "s100" || ar[0].ErrorCode == "s101"))
                {
                    //license is activated.
                    resultMessage = ar[0].ErrorMessage; // = ar.Sig;
                    return true;
                }
                else
                {
                    //could not activate the license.
                    resultMessage = ar[0].ErrorMessage; // = ar.Sig;
                    return false;
                }
            }
            catch (Exception ex)
            {
                resultMessage = ex.Message; // = ar.Sig;
                return false;
            }

        }
        /// <summary>
        /// Deactivates the license woo sl.
        /// </summary>
        /// <param name="resultMessage">The result message.</param>
        /// <returns></returns>
        public static bool deactivateLicenseWooSl(out string resultMessage, LicenseInfo lic)
        {
            ActivationResponseWooSl[] ar = responseWooSl(Woo_SL_Action.Deactivate, lic);
            try
            {
                if (ar == null)
                {
                    resultMessage = "No internet connection available...";
                    return false;
                }
                if (ar[0].Status == "success" && (ar[0].ErrorCode == "s201"))
                {
                    //license is activated.
                    resultMessage = ar[0].ErrorMessage; // = ar.Sig;
                    return true;
                }
                else
                {
                    //could not activate the license.
                    resultMessage = ar[0].ErrorMessage; // = ar.Sig;
                    return false;
                }
            }
            catch (Exception ex)
            {
                resultMessage = ex.Message; // = ar.Sig;
                return false;
            }

        }
        /// <summary>
        /// Statuses the license woo sl.
        /// </summary>
        /// <param name="resultMessage">The result message.</param>
        /// <returns></returns>
        public static bool statusLicenseWooSl(out string resultMessage, LicenseInfo lic)
        {
            ActivationResponseWooSl[] ar = responseWooSl(Woo_SL_Action.StatusCheck, lic);
            try
            {
                if (ar == null)
                {
                    DateTime dt = DateTime.Now.AddDays(-5);
                    if (lic.LastLicenseCheck != null && lic.LastLicenseCheck > dt)
                    {
                        resultMessage = "No internet connection available, however the date of the last check is valid";
                        return true;
                    }
                    else
                    {
                        resultMessage = "No internet connection available and the date of the last check is invalid. Connect to the internet to recheck your licence!";
                        return false;
                    }
                }
                if (ar[0].Status == "success" && (ar[0].ErrorCode == "s205"))
                {
                    //var curLic = GetLicenseByID(lic.ID).LastLicenseCheck = DateTime.Now;
                    //Save();
                    resultMessage = ar[0].ErrorMessage; // = ar.Sig;
                    return true;
                }
                else
                {
                    //could not activate the license.
                    resultMessage = ar[0].ErrorMessage; // = ar.Sig;
                    return false;
                }
            }
            catch (Exception exc)
            {
                resultMessage = exc.Message; // = ar.Sig;
                logger.ErrorFormat(Strings.EventExceptionOccurredFormated, exc.TargetSite, exc.Message);
                return false;
            }

        }
        /// <summary>
        /// Checks the plugin update license woo sl.
        /// </summary>
        /// <param name="resultMessage">The result message.</param>
        /// <returns></returns>
        public static bool checkPluginUpdateLicenseWooSl(out string resultMessage, LicenseInfo lic)
        {
            ActivationResponseWooSl[] ar = responseWooSl(Woo_SL_Action.PluginUpdate, lic);
            try
            {
                if (ar == null)
                {
                    resultMessage = "No internet connection available...";
                    return false;
                }
                if (ar[0].Status == "success" && (ar[0].ErrorCode == "s205"))
                {
                    //license is activated.
                    resultMessage = ar[0].ErrorMessage; // = ar.Sig;
                    return true;
                }
                else
                {
                    //could not activate the license.
                    resultMessage = ar[0].ErrorMessage; // = ar.Sig;
                    return false;
                }
            }
            catch (Exception ex)
            {
                resultMessage = ex.Message; // = ar.Sig;
                return false;
            }

        }
        /// <summary>
        /// Checks the theme update license woo sl.
        /// </summary>
        /// <param name="resultMessage">The result message.</param>
        /// <returns></returns>
        public static bool checkThemeUpdateLicenseWooSl(out string resultMessage, LicenseInfo lic)
        {
            ActivationResponseWooSl[] ar = responseWooSl(Woo_SL_Action.ThemeUpdate, lic);
            try
            {
                if (ar == null)
                {
                    resultMessage = "No internet connection available...";
                    return false;
                }
                if (ar[0].Status == "success" && (ar[0].ErrorCode == "s205"))
                {
                    //license is activated.
                    resultMessage = ar[0].ErrorMessage; // = ar.Sig;
                    return true;
                }
                else
                {
                    //could not activate the license.
                    resultMessage = ar[0].ErrorMessage; // = ar.Sig;
                    return false;
                }
            }
            catch (Exception ex)
            {
                resultMessage = ex.Message; // = ar.Sig;
                return false;
            }

        }
        /// <summary>
        /// Gets the code version license woo sl.
        /// </summary>
        /// <param name="resultMessage">The result message.</param>
        /// <returns></returns>
        public static string getCodeVersionLicenseWooSl(out string resultMessage, LicenseInfo lic)
        {
            CodeVersionResponseWooSl[] ar = codeVersionWooSl(Woo_SL_Action.CodeVersion, lic);
            try
            {
                if (ar == null)
                {
                    resultMessage = "No internet connection available...";
                    return "NO_CONNECTION";
                }
                if (ar[0].Status == "success" && (ar[0].ErrorCode == "s403"))
                {
                    //license is activated.
                    resultMessage = "Version fetched successfully!";
                    return ar[0].versionMessage.Version;
                }
                else
                {
                    //could not activate the license.
                    resultMessage = "Error while getting the version";// ar[0].versionMessage[0]; // = ar.Sig;
                    return resultMessage;
                }
            }
            catch (Exception ex)
            {
                resultMessage = ex.Message; // = ar.Sig;
                return resultMessage;
            }

        }
        public static string getCodeVersionWooSl(out string resultMessage)
        {
            CodeVersionResponseWooSl[] ar = codeVersionWooSl(Woo_SL_Action.CodeVersion);
            try
            {
                if (ar == null)
                {
                    resultMessage = "No internet connection available...";
                    return "NO_CONNECTION";
                }
                if (ar[0].Status == "success" && (ar[0].ErrorCode == "s403"))
                {
                    //license is activated.
                    resultMessage = "Version fetched successfully!";
                    return ar[0].versionMessage.Version;
                }
                else
                {
                    //could not activate the license.
                    resultMessage = "Error while getting the version";// ar[0].versionMessage[0]; // = ar.Sig;
                    return resultMessage;
                }
            }
            catch (Exception ex)
            {
                resultMessage = ex.Message; // = ar.Sig;
                return resultMessage;
            }

        }
        /// <summary>
        /// Deletes the key license woo sl.
        /// </summary>
        /// <param name="resultMessage">The result message.</param>
        /// <returns></returns>
        public static bool deleteKeyLicenseWooSl(out string resultMessage, LicenseInfo lic)
        {
            ActivationResponseWooSl[] ar = responseWooSl(Woo_SL_Action.DeleteKey, lic);
            try
            {
                if (ar[0].Status == "success" && (ar[0].ErrorCode == "s205"))
                {
                    //license is activated.
                    resultMessage = ar[0].ErrorMessage; // = ar.Sig;
                    RemoveLicense(lic);
                    Save();
                    return true;
                }
                else
                {
                    //could not activate the license.
                    resultMessage = ar[0].ErrorMessage; // = ar.Sig;
                    return false;
                }
            }
            catch (Exception ex)
            {
                resultMessage = ex.Message; // = ar.Sig;
                return false;
            }

        }
        #endregion

        #region Private
        private static string queryStringWooCommerceSl(UriBuilder builder, Woo_SL_Action action, LicenseInfo lic = null, string version = "")
        {
            var query = HttpUtility.ParseQueryString(builder.Query);
            query["woo_sl_action"] = action.Value;
            if(lic != null)
                query["licence_key"] = SecureStringHelper.ConvertToString(lic.License);
            query["product_unique_id"] = GlobalStaticConfiguration.licenseProductName;
            if (action == Woo_SL_Action.PluginUpdate || action == Woo_SL_Action.ThemeUpdate)
                query["version"] = version;
            if (lic != null && action != Woo_SL_Action.DeleteKey)
                query["domain"] = lic.ID.ToString();
            builder.Query = query.ToString();
            return builder.ToString();
        }
        private static string queryString(UriBuilder builder, string PurchaseCode)
        {
            //build the query string.
            var query = HttpUtility.ParseQueryString(builder.Query);
            query["code"] = PurchaseCode;
            builder.Query = query.ToString();
            return builder.ToString();
        }
        private static ActivationResponseWooSl[] responseWooSl(Woo_SL_Action action, LicenseInfo lic)
            {
                var builder = new UriBuilder(GlobalStaticConfiguration.licenseUri);
                var syncClient = new WebClient();
                ServicePointManager.Expect100Continue = true;
                ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls12;
                string query;
                try
                {
                    string responseStream = syncClient.DownloadString(query = queryStringWooCommerceSl(builder, action, lic));
                    using (var ms = new MemoryStream(Encoding.Unicode.GetBytes(responseStream)))
                    {
                        DataContractJsonSerializer js = new DataContractJsonSerializer(typeof(ActivationResponseWooSl[]));
                        return (ActivationResponseWooSl[])js.ReadObject(ms);
                    }
                }
                catch (Exception ex)
                {
                    return null;
                }
            }
        /// <summary>
        /// Codes the version woo sl.
        /// </summary>
        /// <param name="action">The action.</param>
        /// <returns></returns>
        private static CodeVersionResponseWooSl[] codeVersionWooSl(Woo_SL_Action action, LicenseInfo lic = null)
        {
            var builder = new UriBuilder(GlobalStaticConfiguration.licenseUri);
            var syncClient = new WebClient();
            ServicePointManager.Expect100Continue = true;
            ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls12;
            try
            {
                string query;
                string responseStream = syncClient.DownloadString(query = queryStringWooCommerceSl(builder, action, lic));
                using (var ms = new MemoryStream(Encoding.Unicode.GetBytes(responseStream)))
                {
                    DataContractJsonSerializer js = new DataContractJsonSerializer(typeof(CodeVersionResponseWooSl[]));
                    return (CodeVersionResponseWooSl[])js.ReadObject(ms);
                }
            }
            catch (Exception ex)
            {
                return null;
            }
        }
        #endregion

        #region Static
        public static Guid getCurrentGUID()
        {
            Guid uuid = Guid.NewGuid();

            ManagementClass mc = new ManagementClass("Win32_ComputerSystemProduct");
            ManagementObjectCollection moc = mc.GetInstances();

            foreach (ManagementObject mo in moc)
            {
                uuid = Guid.Parse(mo.Properties["UUID"].Value.ToString());
                break;
            }
            return uuid;
        }

        public static bool IsLicenseValid(LicenseInfo License)
        {
            try
            {
                // Our own license
                if (FormatCheck.IsMatch(SecureStringHelper.ConvertToString(License.License)))
                {
                    // Check if already a check has been performed today
                    if (License.isValid && License.LastLicenseCheck.Date == DateTime.Now.Date)
                    {
                        return true;
                    }
                    // If not, do the check
                    else
                    {
                        string mes;
                        License.isValid = statusLicenseWooSl(out mes, License);
                        logger.Info(string.Format(Strings.EventLicenseCheckMessageFormated, mes));
                        License.LastLicenseCheck = DateTime.Now;
                        return License.isValid;
                    }
                }
                // Envato license
                else if (FormatCheckEnvato.IsMatch(SecureStringHelper.ConvertToString(License.License)))
                {
                    string mes;
                    License.isValid = statusLicenseEnvato(out mes, License);
                    License.LastLicenseCheck = DateTime.Now;
                    return License.isValid;
                }
                // Format is invalid
                else
                {
                    return false;
                }
            }
            catch (Exception exc)
            {
                logger.ErrorFormat(Strings.EventExceptionOccurredFormated, exc.TargetSite, exc.Message);
                return false;
            }
        }
        #endregion
    }
    public class Woo_SL_Action
    {
        public string Value { get; set; }

        private Woo_SL_Action(string value) { Value = value; }

        public static Woo_SL_Action Activate { get { return new Woo_SL_Action("activate"); } }
        public static Woo_SL_Action Deactivate { get { return new Woo_SL_Action("deactivate"); } }
        public static Woo_SL_Action StatusCheck { get { return new Woo_SL_Action("status-check"); } }
        public static Woo_SL_Action PluginUpdate { get { return new Woo_SL_Action("plugin_update"); } }
        public static Woo_SL_Action PluginInformation { get { return new Woo_SL_Action("plugin_information"); } }
        public static Woo_SL_Action ThemeUpdate { get { return new Woo_SL_Action("theme_update"); } }
        public static Woo_SL_Action CodeVersion { get { return new Woo_SL_Action("code_version"); } }
        public static Woo_SL_Action DeleteKey { get { return new Woo_SL_Action("key_delete"); } }
    }

}
