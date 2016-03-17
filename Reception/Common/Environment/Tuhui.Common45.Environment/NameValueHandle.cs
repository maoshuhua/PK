using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Tuhui.Common45.Environment
{
    public class NameValueHandle
    {
        private static Dictionary<string, string> _Dic
        {
            get
            {
                return EnvironmentHandle.NameValueSetting.NameValues;
            }
        }

        public static string GetString(string key)
        {
            if (!_Dic.ContainsKey(key))
            {
                throw new ArgumentNullException("Environment_NameValue.Config don't contains this key[" + key + "]");
            }
            return _Dic[key];
        }

        public static string GetString(string key, string defaultValue)
        {
            if (!_Dic.ContainsKey(key)) return defaultValue;
            return _Dic[key] ?? defaultValue;
        }

        public static string[] GetStringArray(string key, char separator)
        {
            return GetString(key).Split(separator);
        }

        public static string[] GetStringArray(string key, char separator, string[] defaultValue)
        {
            return GetString(key, string.Join(separator.ToString(), defaultValue)).Split(separator);
        }

        public static int GetInt32(string key)
        {
            return int.Parse(GetString(key));
        }

        public static int GetInt32(string key, int defaultValue)
        {
            int.TryParse(GetString(key, defaultValue.ToString()), out defaultValue);
            return defaultValue;
        }

        public static bool GetBoolean(string key)
        {
            return bool.Parse(GetString(key));
        }

        public static bool GetBoolean(string key, bool defaultValue)
        {
            bool.TryParse(GetString(key, defaultValue.ToString()), out defaultValue);
            return defaultValue;
        }

        public static decimal GetDecimal(string key)
        {
            return decimal.Parse(GetString(key));
        }

        public static decimal GetDecimal(string key, decimal defaultValue)
        {
            decimal.TryParse(GetString(key, defaultValue.ToString()), out defaultValue);
            return defaultValue;
        }

        public static DateTime GetDateTime(string key)
        {
            return DateTime.Parse(GetString(key));
        }

        public static DateTime GetDateTime(string key, DateTime defaultValue)
        {
            DateTime.TryParse(GetString(key, defaultValue.ToString()), out defaultValue);
            return defaultValue;
        }
    }
}
