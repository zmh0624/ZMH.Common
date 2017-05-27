using Meehealth.License;
using System;
using System.IO;

namespace ZMH.Common
{
    public static class LicenseHelper
    {
        //private static LogRepository Logger = new LogRepository();
        /// <summary>
        /// License授权逻辑
        /// </summary>
        /// <returns></returns>
        public static bool IsAuthorized()
        {
#if DEBUG
            return true;
#else
            var flag = true;

            try
            {
                var filePath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "License.lic");
                if (!File.Exists(filePath))
                {
                    LogRepository.Log.Info("Cannot find license file. Please contact with administrator.");
                    return (flag = false);
                }

                var licensetxt = File.ReadAllText(filePath);
                var config = licensetxt.Substring(licensetxt.IndexOf("[config]") + "[config]".Length,
                    licensetxt.IndexOf("[/config]") -
                    (licensetxt.IndexOf("[config]") + "[config]".Length));

                var security = licensetxt.Substring(licensetxt.IndexOf("[security]") + "[security]".Length,
                    licensetxt.IndexOf("[/security]") -
                    (licensetxt.IndexOf("[security]") + "[security]".Length));
                var siteCode = HardwareInfoUtility.GetHardwareInfo();
                var result = Cryptography.VerifyLicense(siteCode, config, security);

                if (!result) LogRepository.Log.Info("Failed to verify license file. Please contact with administrator.");

                return (flag = result);
            }
            catch (Exception ex)
            {
                LogRepository.Log.Info("Failed to parse license file. Please contact with administrator." + ex);
                return (flag = false);
            }
#endif
        }
    }
}
