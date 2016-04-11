using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Tuhui.OH.Utility
{
   
    public static class SFSplite
    {
        //获取以空为分隔数组
        public static List<string> GetList(string str)
        {
            string[] arr = str.Split(' ');
            List<string> list = new List<string>();
            if (arr.Length > 0)
            {
                for (var i = 0; i < arr.Length; i++)
                {
                    if (!string.IsNullOrEmpty(arr[i]))
                    {
                        list.Add(arr[i]);
                    }
                }
            }

            return list;
        } 
    
        //获取水费基本信息
        public static SFMessage GetInfo(string recvStr)
        {
            SFMessage model = new SFMessage();

            //有处理结果、用户号、结尾
            if (recvStr.Length >= 20)
            {
                //结尾 4
                model.JW = "EEEE"; //固定
                recvStr = recvStr.Remove(recvStr.Length - model.JW.Length, model.JW.Length);

                //处理结果 6
                model.CLJG = recvStr.Substring(0, 6).TrimEnd();
                recvStr = recvStr.Remove(0,6);

                //用户号 10
                model.YHH = recvStr.Substring(0, 10).TrimEnd();
                recvStr = recvStr.Remove(0, 10);

                //每组为118位
                int count = recvStr.Length / 118;

                if (count > 0)
                {
                    model.YXX = new List<MonthMessage>();

                    for (var i = 0; i < count; i++)
                    {
                        //月信息
                        MonthMessage entity = new MonthMessage();
                        //应缴月份 6
                        entity.YJYF = recvStr.Substring(0, 6).TrimEnd();
                        recvStr = recvStr.Remove(0, 6);
                        //本月水量 6
                        entity.BYSL = recvStr.Substring(0, 6).TrimEnd();
                        recvStr = recvStr.Remove(0, 6);
                        //垃圾费 16
                        entity.LJF = recvStr.Substring(0, 16).TrimEnd();
                        recvStr = recvStr.Remove(0, 16);
                        //滞纳金 16
                        entity.ZNJ = recvStr.Substring(0, 16).TrimEnd();
                        recvStr = recvStr.Remove(0, 16);
                        //结余 10
                        entity.JY = recvStr.Substring(0, 10).TrimEnd();
                        recvStr = recvStr.Remove(0, 10);
                        //抄表日期 10
                        entity.CBRQ = recvStr.Substring(0, 10).TrimEnd();
                        recvStr = recvStr.Remove(0, 10);
                        //水费金额 16
                        entity.SFJE = recvStr.Substring(0, 16).TrimEnd();
                        recvStr = recvStr.Remove(0, 16);
                        //总金额 16
                        entity.ZJE = recvStr.Substring(0, 16).TrimEnd();
                        recvStr = recvStr.Remove(0, 16);
                        //用量 6
                        entity.YL = recvStr.Substring(0, 6).TrimEnd();
                        recvStr = recvStr.Remove(0, 6);
                        //缴费日期 10
                        entity.JFRQ = recvStr.Substring(0, 10).TrimEnd();
                        recvStr = recvStr.Remove(0, 10);
                        //是否缴费(0、 已缴费 1、未缴费) 6
                        entity.SFJF = recvStr.Substring(0, 6).TrimEnd();
                        recvStr = recvStr.Remove(0, 6);

                        model.YXX.Insert(0, entity);
                    }
                }
            }

            return model;
        }

        //金额处理转化为以“元”为单位
        public static string MoneyConvert(string money){
            if (money == "" || money == "0")
            {
                return money;
            }
            else {
                try
                {
                    double d_money = Convert.ToDouble(money);

                    return (d_money / 1000).ToString();
                }
                catch {

                    return money;
                }
            }
        }
    }
}
