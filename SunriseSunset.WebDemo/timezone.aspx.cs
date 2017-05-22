using System;
using DotNetStarter.Abstractions;
using SunriseSunset.Abstractions;
using SunriseSunset.Models;
using System.Configuration;
using Newtonsoft.Json;

namespace SunriseSunset.WebDemo
{
    public partial class TimeZone : System.Web.UI.Page
    {
        Import<ISunriseSunsetService> _SunriseSunsetService;

        protected void Page_Load(object sender, EventArgs e)
        {
            GetTimeZoneInfo();
        }

        private void GetTimeZoneInfo()
        {
            // Google api requires the number of seconds from midnight on January 1, 1970 to get a timezone
            int seconds = (int)(DateTime.Now - new DateTime(1970, 1, 1)).TotalSeconds;

            // Google api requires a developer key for this api
            string googleKey = ConfigurationManager.AppSettings.Get("GoogleKey");

            string url = string.Format("https://maps.googleapis.com/maps/api/timezone/json?location={0}&timestamp={1}&key={2}", "28.38523,-81.56387", seconds, googleKey);
           // var tz = GetAsyncResult<TimeZoneData>(url);

            //if (tz != null)
            //{
               
            //    litoutput.Text += WSOL.Library.Messaging.AddMessage("ID: " + tz.timeZoneId, System.Diagnostics.EventLogEntryType.Information);
            //    litoutput.Text += WSOL.Library.Messaging.AddMessage("Name: " + tz.timeZoneName, System.Diagnostics.EventLogEntryType.Warning);
            //    litoutput.Text += WSOL.Library.Messaging.AddMessage("Raw offset: " + tz.rawOffset, System.Diagnostics.EventLogEntryType.Error);
            //    litoutput.Text += WSOL.Library.Messaging.AddMessage("Daylight Savings offsett: " + tz.dstOffset, System.Diagnostics.EventLogEntryType.Error);
            //    litoutput.Text += WSOL.Library.Messaging.AddMessage("UT Now: " + DateTime.UtcNow, System.Diagnostics.EventLogEntryType.Information);

            //    DateTime localTime = DateTime.UtcNow.AddSeconds(tz.rawOffset).AddSeconds(tz.dstOffset);

            //    litoutput.Text += WSOL.Library.Messaging.AddMessage("Local Time: " + localTime, System.Diagnostics.EventLogEntryType.SuccessAudit);


            //}
        }

        private T GetAsyncResult<T>(string url)
        {
            string json = string.Empty;

            
           // HttpClient client = new HttpClient();
            //client.Timeout = new TimeSpan(0, 0, 0, 5, 0); /* Set timeout to 5 seconds in case the remote system doesn't respond in a timely manner */

            //using (client)
            //{
            //    var result = client.GetAsync(url).ContinueWith((taskwithmsg) =>
            //    {
            //        json = taskwithmsg.Result.Content.ReadAsStringAsync().Result;
            //    });
            //    result.Wait();
            //}

            return (string.IsNullOrEmpty(json)) ? default(T) : JsonConvert.DeserializeObject<T>(json);
        }
    }
}

namespace WSOL.Library
{

    using System;
    using System.Configuration;
    using System.Diagnostics;
    using System.IO;
    using System.Security.Cryptography;
    using System.Text;
    using System.Web;

    /// <summary>
    /// Helper class to display messages
    /// </summary>
    public class Messaging
    {
        #region Static Constructor

        /// <summary>
        /// Static Constructor
        /// </summary>
        static Messaging()
        {
            // Check if image has been created, if not create it
            string ImageNameAndPath = System.Web.Hosting.HostingEnvironment.MapPath(_imagePath + _imageName);

            if (!System.IO.File.Exists(ImageNameAndPath))
            {
                string directory = System.Web.Hosting.HostingEnvironment.MapPath(_imagePath);

                if (!System.IO.Directory.Exists(directory))
                    System.IO.Directory.CreateDirectory(directory);

                System.Drawing.Image image = Encryption.Base64ToImage(_interactionCuesPng);

                lock (_lock)
                    image.Save(ImageNameAndPath);
            }
        }

        /// <summary>
        ///
        #endregion

        #region Constants

        /// <summary>
        /// Base64 Encoded PNG
        /// </summary>
        private const string _interactionCuesPng = "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";

        /// <summary>
        /// Temp file directory, default is "~/temp/", can be overridden by app setting key "wsol_MessagingTempFolder"
        /// </summary>
        private readonly static string _imagePath = (String.IsNullOrEmpty(ConfigurationManager.AppSettings["wsol_MessagingTempFolder"])) ? "~/temp/" : ConfigurationManager.AppSettings["wsol_MessagingTempFolder"];

        /// <summary>
        /// Filename default is "interactionCues.png", can be overridden by app setting key "wsol_MessagingTempImage"
        /// </summary>
        private readonly static string _imageName = (String.IsNullOrEmpty(ConfigurationManager.AppSettings["wsol_MessagingTempImage"])) ? "interactionCues.png" : ConfigurationManager.AppSettings["wsol_MessagingTempImage"];

        /// <summary>
        /// Lock for multithreading write/save/read issues
        /// </summary>
        private static object _lock = new object();

        #endregion

        /// <summary>
        /// Function to output an HTML string, with specific formatting for each message type.
        /// </summary>
        /// <param name="messageText">message to display</param>
        /// <param name="messageType">type of message</param>
        /// <returns>HTML Formatted String</returns>
        public static string AddMessage(string messageText, EventLogEntryType messageType, bool enableBorderBackgroundColor = true)
        {
            string borderTextColor = String.Empty;
            string backgroundColor = String.Empty;
            string iconURL = MapPathReverse(_imagePath + _imageName);
            string iconPosition = String.Empty;

            switch (messageType)
            {
                case EventLogEntryType.Error:
                case EventLogEntryType.FailureAudit:
                    borderTextColor = "cd0a0a";
                    backgroundColor = "ffece6";
                    iconPosition = "background-position: 0 -35px";
                    break;

                case EventLogEntryType.SuccessAudit:
                    borderTextColor = "006600";
                    backgroundColor = "ccffcc";
                    iconPosition = "background-position: 0 0";
                    break;

                case EventLogEntryType.Warning:
                    borderTextColor = "CF8300";
                    backgroundColor = "FFF4C0"; //"FFF1AF";
                    iconPosition = "background-position: 0 -17px";
                    break;

                case EventLogEntryType.Information:
                default:
                    borderTextColor = "006699";
                    backgroundColor = "eff9ff";

                    iconPosition = "background-position: 0 -72px";
                    break;
            }

            string border = enableBorderBackgroundColor ? String.Format(" border: 1px solid #{0}; ", borderTextColor) : string.Empty;
            string background = enableBorderBackgroundColor ? String.Format(" background-color: #{0}; ", backgroundColor) : string.Empty;

            return String.Format(
                    "<div class=\"{0}-message\" style=\"padding: 10px; margin: 10px 0; {1}; color: #{6}; {2};\"><div style=\"float: left; width: 16px; height: 16px; background-image: url({3}); {4};\"></div><div style=\"margin-left: 25px;\">{5}</div></div>",
                    messageType.ToString().ToLower(),
                    border,
                    background,
                    iconURL,
                    iconPosition,
                    messageText, borderTextColor);
        }

        /// <summary>Takes full system path and returns relative URL path to site root.
        /// </summary>
        /// <param name="fullServerPath">Full system path to file</param>
        /// <returns>Relative URL path to Site root</returns>
        public static string MapPathReverse(string fullServerPath)
        {
            if (!String.IsNullOrEmpty(fullServerPath))
            {
                if (fullServerPath.StartsWith("~") || !fullServerPath.Contains(":\\"))
                    fullServerPath = System.Web.Hosting.HostingEnvironment.MapPath(fullServerPath);

                return "/" + fullServerPath.Replace(System.Web.Hosting.HostingEnvironment.ApplicationPhysicalPath, String.Empty).Replace("\\", "/");
            }

            return String.Empty;
        }



        //Encryption 
        public class Encryption
        {
            /// <summary>
            /// Types of Hashing Algorithms
            /// </summary>
            public enum HashingAlgorithm
            {
                /// <summary>
                /// Represents MD5 Algroithm
                /// </summary>
                MD5,

                /// <summary>
                /// Represents SHA1 Algroithm
                /// </summary>
                SHA1,

                /// <summary>
                /// Represents SHA256 Algroithm
                /// </summary>
                SHA256,

                /// <summary>
                /// Represents SHA384 Algroithm
                /// </summary>
                SHA384,

                /// <summary>
                /// Represents SHA512 Algroithm
                /// </summary>
                SHA512,

                /// <summary>
                /// Represents RIPEMD160 Algroithm
                /// </summary>
                RIPEMD160
            }

            /// <summary>
            /// Converts an image to a Base64 string
            /// </summary>
            /// <param name="image">System.Drawing.Image</param>
            /// <param name="format">System.Drawing.Imaging.ImageFormat</param>
            /// <returns>Base64 string</returns>
            public static string ImageToBase64(System.Drawing.Image image, System.Drawing.Imaging.ImageFormat format)
            {
                using (System.IO.MemoryStream ms = new System.IO.MemoryStream())
                {
                    // Convert Image to byte[]
                    image.Save(ms, format);
                    byte[] imageBytes = ms.ToArray();

                    // Convert byte[] to Base64 String
                    string base64String = Convert.ToBase64String(imageBytes);

                    return base64String;
                }
            }

            /// <summary>
            /// Converts Base64 string to an image file
            /// </summary>
            /// <param name="base64String">Base64 string</param>
            /// <returns>System.Drawing.Image</returns>
            public static System.Drawing.Image Base64ToImage(string base64String)
            {
                // Convert Base64 String to byte[]
                byte[] imageBytes = Convert.FromBase64String(base64String);

                using (System.IO.MemoryStream ms = new System.IO.MemoryStream(imageBytes, 0, imageBytes.Length))
                {
                    // Convert byte[] to Image
                    ms.Write(imageBytes, 0, imageBytes.Length);
                    System.Drawing.Image image = System.Drawing.Image.FromStream(ms, true);

                    return image;
                }
            }

            /// <summary>
            /// Generates an encrypted string, from unencrypted text
            /// </summary>
            /// <param name="unencrypted">unencrypted text</param>
            /// <returns>encrypted text</returns>
            public static string GetEncryptedString(string unencrypted)
            {
                System.Security.Cryptography.MD5CryptoServiceProvider mD5CryptoServiceProvider = new System.Security.Cryptography.MD5CryptoServiceProvider();
                byte[] numArray = mD5CryptoServiceProvider.ComputeHash(System.Text.Encoding.UTF8.GetBytes(unencrypted));
                return BitConverter.ToUInt64(numArray, 0).ToString();
            }

            /// <summary>
            /// Hashes input string using provided algorithm
            /// </summary>
            /// <param name="Input">Value to hash</param>
            /// <param name="Algorithm">Algorithm to use</param>
            /// <returns>Input value hashed</returns>
            public static string GetHash(string Input, HashingAlgorithm Algorithm)
            {
                HashAlgorithm hasher = null;

                // Initialize appropriate hashing algorithm class.
                switch (Algorithm)
                {
                    case HashingAlgorithm.SHA1:
                        hasher = new SHA1Managed();
                        break;
                    case HashingAlgorithm.SHA256:
                        hasher = new SHA256Managed();
                        break;
                    case HashingAlgorithm.SHA384:
                        hasher = new SHA384Managed();
                        break;
                    case HashingAlgorithm.RIPEMD160:
                        hasher = new RIPEMD160Managed();
                        break;
                    case HashingAlgorithm.SHA512:
                        hasher = new SHA512Managed();
                        break;
                    case HashingAlgorithm.MD5:
                        hasher = new MD5CryptoServiceProvider();
                        break;
                }

                byte[] InputBytes = Encoding.Default.GetBytes(Input);
                byte[] EncryptedBytes = hasher.ComputeHash(InputBytes);

                return BitConverter.ToString(EncryptedBytes).Replace("-", string.Empty).ToLower();
            }

            /// <summary>
            /// Hashes input file using provided algorithm
            /// </summary>
            /// <param name="File">Path of file to hash</param>
            /// <param name="Algorithm">Algorithm to use</param>
            /// <returns>Input File Hashed</returns>
            public static string GetFileHash(string File, HashingAlgorithm Algorithm)
            {
                HashAlgorithm hasher = null;
                FileStream fs = new FileStream(File, FileMode.Open);

                // Initialize appropriate hashing algorithm class.
                switch (Algorithm)
                {
                    case HashingAlgorithm.SHA1:
                        hasher = new SHA1Managed();
                        break;
                    case HashingAlgorithm.SHA256:
                        hasher = new SHA256Managed();
                        break;
                    case HashingAlgorithm.SHA384:
                        hasher = new SHA384Managed();
                        break;
                    case HashingAlgorithm.RIPEMD160:
                        hasher = new RIPEMD160Managed();
                        break;
                    case HashingAlgorithm.SHA512:
                        hasher = new SHA512Managed();
                        break;
                    case HashingAlgorithm.MD5:
                        hasher = new MD5CryptoServiceProvider();
                        break;
                }

                byte[] EncryptedBytes = hasher.ComputeHash(fs);
                fs.Close();

                return BitConverter.ToString(EncryptedBytes).Replace("-", string.Empty).ToLower();
            }

            /// <summary>
            /// Encrypts email address for Gravatar URL parameter
            /// </summary>
            /// <param name="Email"></param>
            /// <returns></returns>
            public static string GetMD5EmailEncrypt(string Email)
            {
                System.Security.Cryptography.MD5CryptoServiceProvider x = new System.Security.Cryptography.MD5CryptoServiceProvider();
                byte[] bs = System.Text.Encoding.UTF8.GetBytes(Email);
                bs = x.ComputeHash(bs);
                System.Text.StringBuilder s = new System.Text.StringBuilder(50);
                foreach (byte b in bs)
                {
                    s.Append(b.ToString("x2").ToLower());
                }
                return s.ToString();
            }

            /// <summary>
            /// Proper implemetion of URl Encode that conforms to the RFC3986 Spec
            /// </summary>
            /// <param name="value">The value to Url encode</param>
            /// <returns>Returns a Url encoded string</returns>
            public static string RFC3986(string value)
            {
                value = HttpUtility.UrlDecode(value); // trying to fix %2520 issue with values with spaces

                string unreservedChars = "abcdefghijklmnopqrstuvwxyzABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789-_.~";
                StringBuilder result = new StringBuilder();

                foreach (char symbol in value)
                {
                    if (unreservedChars.IndexOf(symbol) != -1)
                    {
                        result.Append(symbol);
                    }
                    else
                    {
                        result.Append('%' + String.Format("{0:X2}", (int)symbol));
                    }
                }

                return result.ToString();
            }
        }
    }
}