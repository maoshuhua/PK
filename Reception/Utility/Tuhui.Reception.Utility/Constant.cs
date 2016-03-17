using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Tuhui.Common45.Environment;

namespace Tuhui.Reception.Utility
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

        public static string ProvinceVideoResourceAPI
        {
            get
            {
                return NameValueHandle.GetString("ProvinceVideoResourceAPI");
            }
        }
    }
}
