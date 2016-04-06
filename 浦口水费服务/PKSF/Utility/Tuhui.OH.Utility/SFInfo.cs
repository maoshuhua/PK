using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Tuhui.OH.Utility
{
    //第三方需要数据
    public partial class SFInfo
    {
        //实收水量
        public string accWaterSum { get; set; }
        
        //客户账户
        public string custID { get; set; }

        //未用到
        public string feeFrac { get; set; }

        //垃圾费
        public string garbageFee { get; set; }

        //未用到
        public string meterData { get; set; }

        //未用到
        public string oughtReadDate { get; set; }

        //缴费时间
        public string payDate { get; set; }

        //缴费地点
        public string payPlace { get; set; }

        //违约金
        public string penalty { get; set; }

        //上期结余
        public string prevBalance { get; set; }

        //抄表日期
        public string readDate { get; set; }

        //缴费状态
        public string waterFee { get; set; }

        //总金额
        public string waterFeeAll { get; set; }

        //总水量
        public string waterSum { get; set; }
    }
}
