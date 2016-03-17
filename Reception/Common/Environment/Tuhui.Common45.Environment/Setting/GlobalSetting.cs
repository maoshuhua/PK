using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ComponentModel;

namespace Tuhui.Common45.Environment
{
    /// <summary>
    /// 全局配置设置
    /// </summary>
    public class GlobalSetting
    {
        /// <summary>
        /// 网站ID
        /// </summary>
        public string SiteID { get; set; }

        /// <summary>
        /// 网站地址
        /// </summary>
        public string SiteUrl { get; set; }

        /// <summary>
        /// 网站名称
        /// </summary>
        public string SiteName { get; set; }

        /// <summary>
        /// 是否启用多语言
        /// </summary>
        public bool Mutil { get; set; }

        [DefaultValue("~/home/error")]
        public string ErrorUrl { get; set; }

        public bool Debugger { get; set; }

    }
}
