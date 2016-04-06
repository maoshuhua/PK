using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Tuhui.Common.Environment;

namespace Tuhui.OH.Utility
{
    public class Constant
    {
        public static int PageSize
        {
            get
            {
                return NameValueHandle.GetInt32("PageSize", 10);
            }
        }

        public static string OH_Qmap_Server_Url
        {
            get
            {
                return NameValueHandle.GetString("OH_Qmap_Server_Url");
            }
        }
        public static string OH_Qmap_Server_UrlLj
        {
            get
            {
                return NameValueHandle.GetString("OH_Qmap_Server_UrlLj");
            }
        }

        public static string OH_Qmap_Server_SubVenen_Url
        {
            get
            {
                return NameValueHandle.GetString("OH_Qmap_Server_SubVenen_Url");
            }
        }

        public static string OH_isShowAlarmInfo
        {
            get
            {
                return NameValueHandle.GetString("OH_isShowAlarmInfo");
            }
        }
    }
}
