using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Tuhui.Common45.Environment
{
    public class EnvironmentSetting
    {
        public CacheSetting Setting_Cache { get; set; }

        public GlobalSetting Setting_Global { get; set; }

        public NameValueSetting Setting_NameValue { get; set; }

    }
}
