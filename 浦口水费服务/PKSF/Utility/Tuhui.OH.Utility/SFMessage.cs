using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Tuhui.OH.Utility
{
    //浦口水费提供数据
    public partial class SFMessage
    {
        //处理结果
        public string CLJG { get; set; }

        //用户号
        public string YHH { get; set; }

        //月信息
        public List<MonthMessage> YXX { get; set; }

        //结尾
        public string JW { get; set; }
    }

    public partial class MonthMessage
    { 
        //应缴月份
        public string YJYF { get; set; }

        //本月水量
        public string BYSL { get; set; }

        //垃圾费
        public string LJF { get; set; }

        //滞纳金
        public string ZNJ { get; set; }

        //结余
        public string JY { get; set; }

        //抄表日期
        public string CBRQ { get; set; }

        //水费金额
        public string SFJE { get; set; }

        //总金额
        public string ZJE { get; set; }

        //总用量
        public string YL { get; set; }

        //缴费日期
        public string JFRQ { get; set; }

        //是否缴费(0、 已缴费 1、未缴费)
        public string SFJF { get; set; }
    }
}
