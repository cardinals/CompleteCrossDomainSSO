using System;
using System.Configuration;

namespace Common
{
    public class ConfigHelper
    {
        public static readonly string TicketKey = GetAppSettingValue("TicketKey");   
        public static readonly string Secret = GetAppSettingValue("Secret");
        public static readonly string ClientId = GetAppSettingValue("ClientId");
        public static readonly string AuthUrl = GetAppSettingValue("AuthUrl");
        public static string GetAppSettingValue(string key)
        {
            string value = null;
            foreach (string item in ConfigurationManager.AppSettings)
            {
                if (item.Equals(key, StringComparison.CurrentCultureIgnoreCase))
                {
                    value = ConfigurationManager.AppSettings[key];
                    break;
                }
            }
            return value;
        }
    }
}
