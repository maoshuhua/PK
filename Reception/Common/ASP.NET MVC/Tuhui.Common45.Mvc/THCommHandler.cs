using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web.Mvc;
using System.Web.Routing;
using System.Web;
using Tuhui.Common45.Environment;
using Tuhui.Common45.Utility;
using System.Management;

namespace Tuhui.Common45.Mvc
{
    public static class THCommHandler
    {
        #region Static Property
        //private static bool RegisterFlag;
        #endregion

        #region Constructor
        static THCommHandler()
        {
            //RegisterFlag = false;
        }
        #endregion

        #region Route
        public static void Register()
        {
            RegisterRoute();
        }

        private static void RegisterRoute()
        {
        }
        #endregion
    }

    public class SysProp
    {
        public SysProp()
        {

            ManagementObjectSearcher PhysicalMemory = new ManagementObjectSearcher("select * from Win32_PhysicalMemory");
            ManagementObjectSearcher Processor = new ManagementObjectSearcher("select * from Win32_Processor");
            ManagementObjectSearcher Os = new ManagementObjectSearcher("select * from Win32_OperatingSystem");
            ManagementObjectSearcher VideoController = new ManagementObjectSearcher("select * from Win32_VideoController");
            ManagementObjectSearcher CompSys = new ManagementObjectSearcher("select * from Win32_ComputerSystem");

            PhysicalMemory_Capacity = String.Format("{0} MB", Convert.ToInt64(GetValue(PhysicalMemory, "Capacity")) / 1024 / 1024);

            ProcessorName = (string)GetValue(Processor, "Name");

            OperatingSystemBit = System.Environment.Is64BitOperatingSystem ? "64位" : "32位";
            Os_Caption = (string)GetValue(Os, "Caption");
            Os_Version = System.Environment.OSVersion.Version.ToString();
            ServicePack = !String.IsNullOrEmpty(System.Environment.OSVersion.ServicePack)
                ? System.Environment.OSVersion.ServicePack
                : "无";
            SystemSpecialFolder = System.Environment.GetFolderPath(System.Environment.SpecialFolder.System);
            Video_Caption = (string)GetValue(VideoController, "Caption");
            UserName = System.Environment.UserName;
            UserDomainName = System.Environment.UserDomainName;
            CompSys_Workgroup = (string)GetValue(CompSys, "Workgroup");
        }

        private object GetValue(ManagementObjectSearcher searcher, string propName)
        {
            foreach (ManagementObject mobj in searcher.Get())
                return mobj[propName];
            throw new NotSupportedException();
        }

        /// <summary>
        /// 物理内存
        /// </summary>
        public string PhysicalMemory_Capacity { get; set; }

        /// <summary>
        /// 处理器
        /// </summary>
        public string ProcessorName { get; set; }

        /// <summary>
        /// 处理器架构
        /// </summary>
        public string OperatingSystemBit { get; set; }

        /// <summary>
        /// window名称
        /// </summary>
        public string Os_Caption { get; set; }

        /// <summary>
        /// window版本
        /// </summary>
        public string Os_Version { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public string ServicePack { get; set; }

        /// <summary>
        /// 系统目录
        /// </summary>
        public string SystemSpecialFolder { get; set; }

        /// <summary>
        /// 显卡名称
        /// </summary>
        public string Video_Caption { get; set; }

        /// <summary>
        /// 用户名
        /// </summary>
        public string UserName { get; set; }

        /// <summary>
        /// 计算机名称
        /// </summary>
        public string UserDomainName { get; set; }

        /// <summary>
        /// 工作组
        /// </summary>
        public string CompSys_Workgroup { get; set; }
    }
}
