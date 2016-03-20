using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Tuhui.Common45.Utility
{
    public static class CommonFun
    {
        //生成GUID
        public static string GenerGuid()
        {
            return Guid.NewGuid().ToString();
        }
    }
}
