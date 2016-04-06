using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Configuration;
using System.Data;
using System.Data.OracleClient;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.ServiceProcess;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Timers;

namespace WindowsServiceFYWrite
{
    public partial class FYWrite : ServiceBase
    {
        //interval
        private int intervalTime = Convert.ToInt32(ConfigurationManager.AppSettings["interval"].ToString().Trim());

        //时间格式
        private string dateFormat = "yyyy-mm-dd hh24:mi:ss";

        delegate void Delegate_do();

        public FYWrite()
        {
            InitializeComponent();
        }

        protected override void OnStart(string[] args)
        {
            LogInfo("服务启动");

            theout();
        }

        protected void theout()
        {
            try
            {
                Delegate_do Delegate_do = new Delegate_do(All);
                IAsyncResult result = Delegate_do.BeginInvoke(thend, Delegate_do);
            }
            catch (Exception e)
            {
                LogInfo("异步委托：" + e.Message);
            }
        }

        protected void thend(IAsyncResult asyncResult)
        {
            Thread.Sleep(intervalTime); //休眠5分钟，进行下一轮操作
            theout();
        }

        protected void All()
        {
            //小卡基本信息、一般情况
            XK();
            //分娩记录产妇
            FMJLCF();
            //分娩记录新生儿
            FMJLXSE();
            //回执单
            HZD();
            //产后访视产妇
            CHFSCF();
            //产后访视新生儿
            CHFSXSE();
        }

        //小卡基本信息、一般情况
        protected void XK() 
        {
            /* 表名：FI_PREGWOMAN_BASEINFO
             * 基本信息
             * 建卡编号(由自已先行编写) string Card_Code -- string JKBH
             * 建册单位 int CreateHospital -- string JCDW
             * 建册日期 datetime CreateTime -- datetime JCRQ
             * 身份证 string GravidaIdCard -- string SFZH
             * 姓名 string GravidalName -- string XM 
             * 年龄 int GravidalAge -- int NL 
             * 工作单位 string GravidalWorkUnit -- string GZDW
             * 电话 string GravidalPhone -- string LXDH
             * 邮编 string ZipCode -- string YB
             * 丈夫姓名 string ManName -- string ZFXM
             * 丈夫工作单位 string ManWorkUnit -- string ZFGZDW
             * 家庭住址所在区 string HomeAddress_DID --
             * 家庭住址所在街道 string HomeAddress_SID --
             * 家庭住址明细 string HomeAddress -- string XZZ
             * 家庭所在地邮编 string HomeZipCode --
             * 产后休养地所在区 string RelaxAddress_DID -- 
             * 产后休养地所在街道 string RelaxAddress_SID --
             * 产后休养地详细地址 string RelaxAddress -- string CHXYDZ
             * 休养地电话 string RelaxPhone --
             * 孕妇学校(1\2\3\4\5) int School --
             * 医保卡号 string YBCard --
             * 操作日期 datetime OperateTime -- datetime CZSJ
             * 操作人 int OperateUserId --
             * 建册单位编码 string HospitalCode -- 
             * 
             * 一般情况
             * 文化程度（1大学、2中学、3小学4、文盲） int Culture -- string WHCD
             * 职业 string Profession -- string ZY
             * 孕次 int YunCi -- int YC
             * 产次 int CanCi -- int CC
             * 末次月经 datetime LastMenstruation -- datetime MCYJ
             * 预产期 datetime Delivery -- datetime YCQ
             * 月经史 string Menstrual -- string YJS
             * (暂时去掉)孕早期情况（见手册）int YunEarly -- string YZQQK
             * 孕早期情况其他 string YunOther -- string YZQQKQT
             */
            try
            {
                //获取小卡基本情况、一般情况
                LogInfo("获取小卡基本情况、一般情况sqlserver：" + String.Format(@"select a.*,b.*,c.DicCode,c.Hos_Name from SmallCard a left join SmallCardOther b on a.Card_ID=b.CardID left join Hospital_Info c on c.Hos_Code = a.CreateHospital where a.CardState=0"));
                DataTable dt = SqlHelper.ExecuteDataSet(CommandType.Text, String.Format(@"select a.*,b.*,c.DicCode,c.Hos_Name from SmallCard a left join SmallCardOther b on a.Card_ID=b.CardID left join Hospital_Info c on c.Hos_Code = a.CreateHospital where a.CardState=0"), null).Tables[0];
                if (dt.Rows.Count > 0)
                {
                    foreach (DataRow dr in dt.Rows)
                    {
                        try
                        {
                            LogInfo("UID-----------------------------------" + dr["U_ID"].ToString());
                            LogInfo("FI_PREGWOMAN_BASEINFO插入：" + String.Format(@"insert into FI_PREGWOMAN_BASEINFO(SERIAL_NO,JKBH,JCDW,JCRQ,SFZH,XM,NL,GZDW,LXDH,YB,ZFXM,ZFGZDW,XZZ,CHXYDZ,CZSJ,ZY,WHCD,YC,CC,MCYJ,YCQ,YJS,YZQQKQT,JLBS,DQBM) values('{0}','{1}','{2}',to_date('{3}','" + dateFormat + "'),'{4}','{5}',{6},'{7}','{8}','{9}','{10}','{11}','{12}','{13}',to_date('{14}','" + dateFormat + "'),'{15}','{16}',{17},{18},to_date('{19}','" + dateFormat + "'),to_date('{20}','" + dateFormat + "'),'{21}','{22}','0','{23}')",
                                GenerateStringID(),
                                Convert.ToString(dr["Card_Code"]),
                                Convert.ToInt32(dr["Hos_Name"]),
                                Convert.ToString(dr["CreateTime"]),
                                Convert.ToString(dr["GravidaIdCard"]),
                                Convert.ToString(dr["GravidalName"]),
                                Convert.ToInt32(dr["GravidalAge"]),
                                Convert.ToString(dr["GravidalWorkUnit"]),
                                Convert.ToString(dr["GravidalPhone"]),
                                Convert.ToString(dr["ZipCode"]),
                                Convert.ToString(dr["ManName"]),
                                Convert.ToString(dr["ManWorkUnit"]),
                                Convert.ToString(dr["HomeAddress"]),
                                Convert.ToString(dr["RelaxAddress"]),
                                Convert.ToString(dr["OperateTime"]),
                                Convert.ToString(dr["Profession"]),
                                Convert.ToString(dr["Culture"]),
                                Convert.ToInt32(dr["YunCi"]),
                                Convert.ToInt32(dr["CanCi"]),
                                Convert.ToString(dr["LastMenstruation"]),
                                Convert.ToString(dr["Delivery"]),
                                Convert.ToString(dr["Menstrual"]),
                                Convert.ToString(dr["YunOther"]),
                                Convert.ToInt32(dr["DicCode"])
                                ));
                            int i = OracleHelper.ExecuteNonQuery(String.Format(@"insert into FI_PREGWOMAN_BASEINFO(SERIAL_NO,JKBH,JCDW,JCRQ,SFZH,XM,NL,GZDW,LXDH,YB,ZFXM,ZFGZDW,XZZ,CHXYDZ,CZSJ,ZY,WHCD,YC,CC,MCYJ,YCQ,YJS,YZQQKQT,JLBS,DQBM) values('{0}','{1}','{2}',to_date('{3}','" + dateFormat + "'),'{4}','{5}',{6},'{7}','{8}','{9}','{10}','{11}','{12}','{13}',to_date('{14}','" + dateFormat + "'),'{15}','{16}',{17},{18},to_date('{19}','" + dateFormat + "'),to_date('{20}','" + dateFormat + "'),'{21}','{22}','0','{23}')",
                                GenerateStringID(),
                                Convert.ToString(dr["Card_Code"]),
                                Convert.ToInt32(dr["Hos_Name"]),
                                Convert.ToString(dr["CreateTime"]),
                                Convert.ToString(dr["GravidaIdCard"]),
                                Convert.ToString(dr["GravidalName"]),
                                Convert.ToInt32(dr["GravidalAge"]),
                                Convert.ToString(dr["GravidalWorkUnit"]),
                                Convert.ToString(dr["GravidalPhone"]),
                                Convert.ToString(dr["ZipCode"]),
                                Convert.ToString(dr["ManName"]),
                                Convert.ToString(dr["ManWorkUnit"]),
                                Convert.ToString(dr["HomeAddress"]),
                                Convert.ToString(dr["RelaxAddress"]),
                                Convert.ToString(dr["OperateTime"]),
                                Convert.ToString(dr["Profession"]),
                                Convert.ToString(dr["Culture"]),
                                Convert.ToInt32(dr["YunCi"]),
                                Convert.ToInt32(dr["CanCi"]),
                                Convert.ToString(dr["LastMenstruation"]),
                                Convert.ToString(dr["Delivery"]),
                                Convert.ToString(dr["Menstrual"]),
                                Convert.ToString(dr["YunOther"]),
                                Convert.ToInt32(dr["DicCode"])
                                ));
                            if (i > 0) //插入成功
                            {
                                LogInfo("小卡基本情况、一般情况插入成功");
                                //更新
                                int gx_num = SqlHelper.ExecteNonQuery(CommandType.Text, String.Format(@"update SmallCard set CardState = 2 where Card_ID={0}", Convert.ToInt32(dr["Card_ID"])), null);
                                if (gx_num > 0) //更新成功 
                                {
                                    LogInfo("状态更新成功");
                                }
                                else //更新失败
                                {
                                    LogInfo("状态更新失败");
                                }
                            }
                            else //插入失败 
                            {
                                LogInfo("小卡基本情况、一般情况插入失败");
                            }
                        }
                        catch (Exception e) {

                            LogInfo("错误：" + e.Message);
                        }
                    }
                }

                Thread.Sleep(200);
            }
            catch (Exception e) {

                LogInfo("小卡基本信息、一般情况异常：" + e.Message);
            }
        }

        //分娩记录产妇
        protected void FMJLCF()
        {
            /* 表名：FI_PREGWOMAN_LABOUR
             * 分娩记录产妇
             * 住院号（医院）string HospitalNum -- string ZYH
             * 分娩日期 datetime DeliveryTime -- datetime FMRQ
             * 分娩孕周 string GestationalWeeks -- int YZ "+" int YZT
             * 分娩方式（0顺产 1剖宫产）int DeliveryType -- string FMFS
             * 分娩方式其它(方式为6时) string DeliveryOtherType -- string FMFSQT
             * 分娩地址 string DeliveryUrl -- string FMDD
             * 产妇出院日期 datetime LeaveHospital -- datetime CYRQ
             * 操作人 int OperateUserId -- 
             * 操作时间 datetime OperateTime -- datetime CZSJ
             */
            try
            {
                //获取分娩记录产妇
                LogInfo("获取分娩记录产妇sqlserver：" + String.Format(@"select a.*,b.Card_Code,b.HospitalCode from DeliveryRecord a inner join SmallCard b on a.U_ID = b.U_ID where a.DRState=0"));
                DataTable dt = SqlHelper.ExecuteDataSet(CommandType.Text, String.Format(@"select a.*,b.Card_Code,b.HospitalCode from DeliveryRecord a inner join SmallCard b on a.U_ID = b.U_ID where a.DRState=0"), null).Tables[0];
                if (dt.Rows.Count > 0)
                {
                    foreach (DataRow dr in dt.Rows)
                    {
                        try
                        {
                            int YZ = 0;
                            int YZT = 0;
                            string GestationalWeeks = Convert.ToString(dr["GestationalWeeks"]);
                            if (GestationalWeeks != "")
                            {
                                string[] sArray = GestationalWeeks.Split('+');
                                if (sArray.Length == 2)
                                {
                                    YZ = Convert.ToInt32(sArray[0]);
                                    YZT = Convert.ToInt32(sArray[1]);
                                }
                            }
                            LogInfo("UID-----------------------------------" + dr["U_ID"].ToString());
                            LogInfo("FI_PREGWOMAN_LABOUR插入：" + String.Format(@"insert into FI_PREGWOMAN_LABOUR(JKBH,ZYH,FMRQ,FMFS,FMFSQT,FMDD,CYRQ,CZSJ,CFBJBH,YZ,YZT,JLBS,DQBM) values('{0}','{1}',to_date('{2}','" + dateFormat + "'),'{3}','{4}','{5}',to_date('{6}','" + dateFormat + "'),to_date('{7}','" + dateFormat + "'),'{8}',{9},{10},'0','{11}')",
                                GenerateStringID(),
                                Convert.ToString(dr["HospitalNum"]),
                                Convert.ToString(dr["DeliveryTime"]),
                                Convert.ToString(dr["DeliveryType"]),
                                Convert.ToString(dr["DeliveryOtherType"]),
                                Convert.ToString(dr["DeliveryUrl"]),
                                Convert.ToString(dr["LeaveHospital"]),
                                Convert.ToString(dr["OperateTime"]),
                                Convert.ToString(dr["Card_Code"]),
                                YZ,
                                YZT,
                                Convert.ToString(dr["HospitalCode"])
                                ));
                            int i = OracleHelper.ExecuteNonQuery(String.Format(@"insert into FI_PREGWOMAN_LABOUR(JKBH,ZYH,FMRQ,FMFS,FMFSQT,FMDD,CYRQ,CZSJ,CFBJBH,YZ,YZT,JLBS,DQBM) values('{0}','{1}',to_date('{2}','" + dateFormat + "'),'{3}','{4}','{5}',to_date('{6}','" + dateFormat + "'),to_date('{7}','" + dateFormat + "'),'{8}',{9},{10},'0','{11}')",
                                GenerateStringID(),
                                Convert.ToString(dr["HospitalNum"]),
                                Convert.ToString(dr["DeliveryTime"]),
                                Convert.ToString(dr["DeliveryType"]),
                                Convert.ToString(dr["DeliveryOtherType"]),
                                Convert.ToString(dr["DeliveryUrl"]),
                                Convert.ToString(dr["LeaveHospital"]),
                                Convert.ToString(dr["OperateTime"]),
                                Convert.ToString(dr["Card_Code"]),
                                YZ,
                                YZT,
                                Convert.ToString(dr["HospitalCode"])
                                ));

                            if (i > 0) //插入成功
                            {
                                LogInfo("分娩记录产妇插入成功");
                                //更新
                                int gx_num = SqlHelper.ExecteNonQuery(CommandType.Text, String.Format(@"update DeliveryRecord set DRState = 2 where ID={0}", Convert.ToInt32(dr["ID"])), null);
                                if (gx_num > 0) //更新成功 
                                {
                                    LogInfo("状态更新成功");
                                }
                                else //更新失败
                                {
                                    LogInfo("状态更新失败");
                                }
                            }
                            else //插入失败 
                            {
                                LogInfo("分娩记录产妇插入失败");
                            }
                        }
                        catch (Exception e)
                        {

                            LogInfo("错误：" + e.Message);
                        }
                    }
                }

                Thread.Sleep(200);
            }
            catch (Exception e)
            {

                LogInfo("分娩记录产妇异常：" + e.Message);
            }
        }
        
        //分娩记录新生儿
        protected void FMJLXSE()
        {
            /* 表名：FI_NEWBABY_INFO
             * 分娩记录新生儿
             * 性别 0 女 1男 2 其它 int Sex -- string XB
             * 胎数(0单1双2多胎） int Tirenumber -- string TS
             * 出生体重(克) double Weight -- int CSTZ
             * Apgar评分 int ApgarScore -- 
             * 存活情况0活产1死胎2死产 int Survival -- string CHQK
             * 新生儿疾病筛查(0否，1是）int NeonateScreen --
             * 筛查明细项（0 1 2）string ScreenDetail --
             * 新生儿出院时间 datetime LeaveHospitalTime -- datetime CYRQ
             * 操作人 int OperateUserId --
             * 操作时间 datetime OperateTime -- datetime CZSJ
             */
            try
            {
                //获取分娩记录新生儿
                LogInfo("获取分娩记录新生儿sqlserver：" + String.Format(@"select a.*,b.Card_Code,b.HospitalCode from NeonateRecord a inner join SmallCard b on a.U_ID = b.U_ID where a.NRState=0"));
                DataTable dt = SqlHelper.ExecuteDataSet(CommandType.Text, String.Format(@"select a.*,b.Card_Code,b.HospitalCode from NeonateRecord a inner join SmallCard b on a.U_ID = b.U_ID where a.NRState=0"), null).Tables[0];
                if (dt.Rows.Count > 0)
                {
                    foreach (DataRow dr in dt.Rows)
                    {
                        try
                        {
                            LogInfo("UID-----------------------------------" + dr["U_ID"].ToString());
                            LogInfo("FI_NEWBABY_INFO插入：" + String.Format(@"insert into FI_NEWBABY_INFO(SERIAL_NO,JKBH,XB,TS,CSTZ,CHQK,CYRQ,CZSJ,JLBS,DQBM) values('{0}','{1}','{2}','{3}','{4}','{5}',to_date('{6}','" + dateFormat + "'),to_date('{7}','" + dateFormat + "'),'0','{8}')",
                                GenerateStringID(),
                                Convert.ToString(dr["Card_Code"]),
                                Convert.ToString(dr["Sex"]),
                                Convert.ToString(dr["Tirenumber"]),
                                Convert.ToInt32(dr["Weight"]),
                                Convert.ToString(dr["Survival"]),
                                Convert.ToString(dr["LeaveHospitalTime"]),
                                Convert.ToString(dr["OperateTime"]),
                                Convert.ToString(dr["HospitalCode"])
                                ));
                            int i = OracleHelper.ExecuteNonQuery(String.Format(@"insert into FI_NEWBABY_INFO(SERIAL_NO,JKBH,XB,TS,CSTZ,CHQK,CYRQ,CZSJ,JLBS,DQBM) values('{0}','{1}','{2}','{3}','{4}','{5}',to_date('{6}','" + dateFormat + "'),to_date('{7}','" + dateFormat + "'),'0','{8}')",
                                GenerateStringID(),
                                Convert.ToString(dr["Card_Code"]),
                                Convert.ToString(dr["Sex"]),
                                Convert.ToString(dr["Tirenumber"]),
                                Convert.ToInt32(dr["Weight"]),
                                Convert.ToString(dr["Survival"]),
                                Convert.ToString(dr["LeaveHospitalTime"]),
                                Convert.ToString(dr["OperateTime"]),
                                Convert.ToString(dr["HospitalCode"])
                                ));
                            if (i > 0) //插入成功
                            {
                                LogInfo("分娩记录新生儿插入成功");
                                //更新
                                int gx_num = SqlHelper.ExecteNonQuery(CommandType.Text, String.Format(@"update NeonateRecord set NRState = 2 where ID={0}", Convert.ToInt32(dr["ID"])), null);
                                if (gx_num > 0) //更新成功 
                                {
                                    LogInfo("状态更新成功");
                                }
                                else //更新失败
                                {
                                    LogInfo("状态更新失败");
                                }
                            }
                            else //插入失败 
                            {
                                LogInfo("分娩记录新生儿插入失败");
                            }
                        }
                        catch (Exception e)
                        {

                            LogInfo("错误：" + e.Message);
                        }
                    }
                }

                Thread.Sleep(200);
            }
            catch (Exception e)
            {

                LogInfo("娩记录新生儿异常：" + e.Message);
            }
        }

        //回执单
        protected void HZD()
        {
            /* 表名：FI_PREGWOM_RETURNFILE
             * 回执单
             * 产后修养地（服务中心）int RestHospital -- 
             * 预约产后访视时间 datetime FanShiTime --
             * 图片1 string Photo1 --
             * 图片2 string Photo2 --
             * 操作时间 datetime OperateTime -- datetime CZSJ
             * 建卡编号 string JKBH -- string JCQX
             * 姓名 string XM -- string XM
             * 出生日期 datetime CSRQ -- datetime CSRQ
             * 年龄 int NL -- int NL
             * 建卡单位 string JCDW -- string JCDW
             * 产后修养区域 string CHXYQX -- string CHXYQX
             * 现住址 string XZZ -- string XZZ
             * 产后修养地址 string CHXYDZ -- string CHXYDZ
             * 婴儿性别 string YEXB -- string YEXB
             * 出生体重 string CSTZ -- string CSTZ
             * 分娩日期 datetime FMRQ -- datetime FMRQ
             * 联系人姓名 string ZFXM -- string ZFXM
             * 联系电话 string LXDH -- string LXDH
             * 接产医院 string JCYY -- string JCYY
             * 出院日期 datetime CYRQ -- datetime CYRQ
             * 将卡交到 string CKJD -- string CKJD
             */
            try
            {
                LogInfo("获取修养接收sqlserver：" + String.Format(@"select a.*,b.HospitalCode from RestReceive a inner join SmallCard b on a.U_ID = b.U_ID where a.State=0"));
                DataTable dt = SqlHelper.ExecuteDataSet(CommandType.Text, String.Format(@"select a.*,b.HospitalCode from RestReceive a inner join SmallCard b on a.U_ID = b.U_ID where a.State=0"), null).Tables[0];
                if (dt.Rows.Count > 0)
                {
                    foreach (DataRow dr in dt.Rows)
                    {
                        try
                        {
                            LogInfo("UID-----------------------------------" + dr["U_ID"].ToString());
                            LogInfo("FI_PREGWOM__RETURNFILE插入：" + String.Format(@"insert into FI_PREGWOM_RETURNFILE(JKBH,CZSJ,JCQX,XM,CSRQ,NL,JCDW,CHXYQX,XZZ,CHXYDZ,YEXB,CSTZ,FMRQ,ZFXM,LXDH,JCYY,CYRQ,CKJD,JLBS,DQBM,JKRQ) values('{0}',to_date('{1}','" + dateFormat + "'),'{2}','{3}',to_date('{4}','" + dateFormat + "'),{5},'{6}','{7}','{8}','{9}','{10}','{11}',to_date('{12}','" + dateFormat + "'),'{13}','{14}','{15}',to_date('{16}','" + dateFormat + "'),'{17}','0','{18}',GETDATE())",
                                  GenerateStringID(),
                                  Convert.ToString(dr["OperateTime"]),
                                  Convert.ToString(dr["JKBH"]),
                                  Convert.ToString(dr["XM"]),
                                  Convert.ToString(dr["CSRQ"]),
                                  Convert.ToInt32(dr["NL"]),
                                  Convert.ToString(dr["JCDW"]),
                                  Convert.ToString(dr["CHXYQX"]),
                                  Convert.ToString(dr["XZZ"]),
                                  Convert.ToString(dr["CHXYDZ"]),
                                  Convert.ToString(dr["YEXB"]),
                                  Convert.ToString(dr["CSTZ"]),
                                  Convert.ToString(dr["FMRQ"]),
                                  Convert.ToString(dr["ZFXM"]),
                                  Convert.ToString(dr["LXDH"]),
                                  Convert.ToString(dr["JCYY"]),
                                  Convert.ToString(dr["CYRQ"]),
                                  Convert.ToString(dr["CKJD"]),
                                  Convert.ToString(dr["HospitalCode"])
                                ));
                            int i = OracleHelper.ExecuteNonQuery(String.Format(@"insert into FI_PREGWOM_RETURNFILE(JKBH,CZSJ,JCQX,XM,CSRQ,NL,JCDW,CHXYQX,XZZ,CHXYDZ,YEXB,CSTZ,FMRQ,ZFXM,LXDH,JCYY,CYRQ,CKJD,JLBS,DQBM) values('{0}',to_date('{1}','" + dateFormat + "'),'{2}','{3}',to_date('{4}','" + dateFormat + "'),{5},'{6}','{7}','{8}','{9}','{10}','{11}',to_date('{12}','" + dateFormat + "'),'{13}','{14}','{15}',to_date('{16}','" + dateFormat + "'),'{17}','0','{18}')",
                                  GenerateStringID(),
                                  Convert.ToString(dr["OperateTime"]),
                                  Convert.ToString(dr["JKBH"]),
                                  Convert.ToString(dr["XM"]),
                                  Convert.ToString(dr["CSRQ"]),
                                  Convert.ToInt32(dr["NL"]),
                                  Convert.ToString(dr["JCDW"]),
                                  Convert.ToString(dr["CHXYQX"]),
                                  Convert.ToString(dr["XZZ"]),
                                  Convert.ToString(dr["CHXYDZ"]),
                                  Convert.ToString(dr["YEXB"]),
                                  Convert.ToString(dr["CSTZ"]),
                                  Convert.ToString(dr["FMRQ"]),
                                  Convert.ToString(dr["ZFXM"]),
                                  Convert.ToString(dr["LXDH"]),
                                  Convert.ToString(dr["JCYY"]),
                                  Convert.ToString(dr["CYRQ"]),
                                  Convert.ToString(dr["CKJD"]),
                                  Convert.ToString(dr["HospitalCode"])
                                ));
                            if (i > 0) //插入成功
                            {
                                LogInfo("修养接收插入成功");
                                //更新
                                int gx_num = SqlHelper.ExecteNonQuery(CommandType.Text, String.Format(@"update RestReceive set State = 2 where ID={0}", Convert.ToInt32(dr["ID"])), null);
                                if (gx_num > 0) //更新成功 
                                {
                                    LogInfo("状态更新成功");
                                }
                                else //更新失败
                                {
                                    LogInfo("状态更新失败");
                                }
                            }
                            else //插入失败 
                            {
                                LogInfo("修养接收插入失败");
                            }
                        }
                        catch (Exception e)
                        {

                            LogInfo("错误：" + e.Message);
                        }
                    }
                }

                Thread.Sleep(200);
            }
            catch (Exception e)
            {

                LogInfo("修养接收异常：" + e.Message);
            }
        }

        //产后访视产妇
        protected void CHFSCF()
        {
            /* 表名：FI_PREGWOMAN_VISIT
             * 产后访视产妇
             * 随访日期 datetime VisitTime -- datetime JCSJ
             * 产后第几天 int PostpartumDay -- int DJT
             * 体温 double Temperature -- string TWC 
             * 一般健康情况 string Healthy -- string YBJKZK
             * 一般心理情况 string Psychologic -- string YBXLQK
             * 血压 string Blood -- string XY
             * 乳房(1 未见异常2异常 填入内容) int Breast --
             * 乳房其它 string BreastOther --
             * 恶露 int Lochia -- string EL1
             * 恶露其它 string LochiaOther --
             * 子宫 int Uterus -- string ZG
             * 子宫其它 string UterusOther -- string ZGMS
             * 伤口 int Wound --string SK
             * 伤口其它 string WoundOther -- string SKMS
             * 其它 string Other -- string QT
             * 分类 int Classify --string FL
             * 分类其它 string ClassifyOther -- string FLMS
             * 指导 string Guide -- string ZD
             * 指导其他 string GuideOther -- string ZDMS
             * 转诊 int Referral -- string ZZ
             * 转诊原因 string ReferralOther -- string ZZYY
             * 下次随访日期 datetime NextVisitTime -- datetime XCSFRQ
             * 随访医生签名 string Doctor -- FSZ
             * 随访医生ID int DoctorCode --
             * 操作人 int OperateUserId --
             * 操作时间 datetime OperateTime -- datetime CZSJ
             */
            try
            {
                //获取产后访视产妇
                LogInfo("获取产后访视产妇sqlserver：" + String.Format(@"select a.*,b.Card_Code,b.HospitalCode from PostpartumVisit a inner join SmallCard b on a.U_ID = b.U_ID where a.PVisitState=0"));
                DataTable dt = SqlHelper.ExecuteDataSet(CommandType.Text, String.Format(@"select a.*,b.Card_Code,b.HospitalCode from PostpartumVisit a inner join SmallCard b on a.U_ID = b.U_ID where a.PVisitState=0"), null).Tables[0];
                if (dt.Rows.Count > 0)
                {
                    foreach (DataRow dr in dt.Rows)
                    {
                        try
                        {
                            LogInfo("UID-----------------------------------" + dr["U_ID"].ToString());
                            LogInfo("FI_PREGWOMAN_VISIT插入：" + String.Format(@"insert into FI_PREGWOMAN_VISIT(FSBH,JKBH,JCSJ,DJT,YBJKZK,YBXLQK,XY,ZGMS,SKMS,QT,FLMS,ZD,ZDMS,ZZYY,XCSFRQ,CZSJ,TWC,EL1,ZG,SK,FL,ZZ,JLBS,DQBM,FSZ) values('{0}','{1}',to_date('{2}','" + dateFormat + "'),{3},'{4}','{5}','{6}','{7}','{8}','{9}','{10}','{11}','{12}','{13}',to_date('{14}','" + dateFormat + "'),to_date('{15}','" + dateFormat + "'),'{16}','{17}','{18}','{19}','{20}','{21}','0','{22}','{23}')",
                                 GenerateStringID(),
                                 Convert.ToString(dr["Card_Code"]),
                                 Convert.ToString(dr["VisitTime"]),
                                 Convert.ToInt32(dr["PostpartumDay"]),
                                 Convert.ToString(dr["Healthy"]),
                                 Convert.ToString(dr["Psychologic"]),
                                 Convert.ToString(dr["Blood"]),
                                 Convert.ToString(dr["UterusOther"]),
                                 Convert.ToString(dr["WoundOther"]),
                                 Convert.ToString(dr["Other"]),
                                 Convert.ToString(dr["ClassifyOther"]),
                                 Convert.ToString(dr["Guide"]),
                                 Convert.ToString(dr["GuideOther"]),
                                 Convert.ToString(dr["ReferralOther"]),
                                 Convert.ToString(dr["NextVisitTime"]),
                                 Convert.ToString(dr["OperateTime"]),
                                 Convert.ToString(dr["Temperature"]),
                                 Convert.ToString(dr["Lochia"]),
                                 Convert.ToString(dr["Uterus"]),
                                 Convert.ToString(dr["Wound"]),
                                 Convert.ToString(dr["Classify"]),
                                 Convert.ToString(dr["Referral"]),
                                 Convert.ToString(dr["HospitalCode"]),
                                 Convert.ToString(dr["Doctor"])
                               ));
                            int i = OracleHelper.ExecuteNonQuery(String.Format(@"insert into FI_PREGWOMAN_VISIT(FSBH,JKBH,JCSJ,DJT,YBJKZK,YBXLQK,XY,ZGMS,SKMS,QT,FLMS,ZD,ZDMS,ZZYY,XCSFRQ,CZSJ,TWC,EL1,ZG,SK,FL,ZZ,JLBS,DQBM,FSZ) values('{0}','{1}',to_date('{2}','" + dateFormat + "'),{3},'{4}','{5}','{6}','{7}','{8}','{9}','{10}','{11}','{12}','{13}',to_date('{14}','" + dateFormat + "'),to_date('{15}','" + dateFormat + "'),'{16}','{17}','{18}','{19}','{20}','{21}','0','{22}','{23}')",
                                 GenerateStringID(),
                                 Convert.ToString(dr["Card_Code"]),
                                 Convert.ToString(dr["VisitTime"]),
                                 Convert.ToInt32(dr["PostpartumDay"]),
                                 Convert.ToString(dr["Healthy"]),
                                 Convert.ToString(dr["Psychologic"]),
                                 Convert.ToString(dr["Blood"]),
                                 Convert.ToString(dr["UterusOther"]),
                                 Convert.ToString(dr["WoundOther"]),
                                 Convert.ToString(dr["Other"]),
                                 Convert.ToString(dr["ClassifyOther"]),
                                 Convert.ToString(dr["Guide"]),
                                 Convert.ToString(dr["GuideOther"]),
                                 Convert.ToString(dr["ReferralOther"]),
                                 Convert.ToString(dr["NextVisitTime"]),
                                 Convert.ToString(dr["OperateTime"]),
                                 Convert.ToString(dr["Temperature"]),
                                 Convert.ToString(dr["Lochia"]),
                                 Convert.ToString(dr["Uterus"]),
                                 Convert.ToString(dr["Wound"]),
                                 Convert.ToString(dr["Classify"]),
                                 Convert.ToString(dr["Referral"]),
                                 Convert.ToString(dr["HospitalCode"]),
                                 Convert.ToString(dr["Doctor"])
                                ));
                            if (i > 0) //插入成功
                            {
                                LogInfo("产后访视产妇插入成功");
                                //更新
                                int gx_num = SqlHelper.ExecteNonQuery(CommandType.Text, String.Format(@"update PostpartumVisit set PVisitState = 2 where ID={0}", Convert.ToInt32(dr["ID"])), null);
                                if (gx_num > 0) //更新成功 
                                {
                                    LogInfo("状态更新成功");
                                }
                                else //更新失败
                                {
                                    LogInfo("状态更新失败");
                                }
                            }
                            else //插入失败 
                            {
                                LogInfo("产后访视产妇插入失败");
                            }
                        }
                        catch (Exception e)
                        {

                            LogInfo("错误：" + e.Message);
                        }
                    }
                }

                Thread.Sleep(200);
            }
            catch (Exception e)
            {

                LogInfo("产后访视产妇异常：" + e.Message);
            }
        }

        //产后访视新生儿
        protected void CHFSXSE()
        {
            /* 表名：FI_NEWBABY_VISIT
             * 产后访视新生儿
             * 随访时间 datetime VisitTime -- datetime FSRQ
             * 体温 double Temperature -- string TWW
             * 喂养方式 string FeedingMode -- string WYFS
             * 皮肤 string Skin -- string PF
             * 脐部 string Navel --
             * 眼-正常红肿分沁物 string YanJing -- 
             * 鹅口疮-有或无 string Thrush --
             * 大便-次数 int DBNum --
             * 大便-色 string DBS --
             * 大便-形状 string DBYJ --
             * 体重 double Weight -- string TZ
             * 处理 string Handle -- string ZDSX
             * 访视医生 string VisitDoctor -- FSZ
             * 访视医生编号 int VisitDoctorCode --
             * 操作人 int OperateUserId --
             * 操作时间 datetime OperateTime -- datetime CZSJ
             */
            try
            {
                //获取产后访视新生儿
                LogInfo("获取产后访视新生儿sqlserver：" + String.Format(@"select a.*,b.Card_Code,b.HospitalCode from PostpartumVisitChild a inner join SmallCard b on a.U_ID = b.U_ID where a.PVisitChildState=0"));
                DataTable dt = SqlHelper.ExecuteDataSet(CommandType.Text, String.Format(@"select a.*,b.Card_Code,b.HospitalCode from PostpartumVisitChild a inner join SmallCard b on a.U_ID = b.U_ID where a.PVisitChildState=0"), null).Tables[0];
                if (dt.Rows.Count > 0)
                {
                    foreach (DataRow dr in dt.Rows)
                    {
                        try
                        {
                            LogInfo("UID-----------------------------------" + dr["U_ID"].ToString());
                            LogInfo("FI_NEWBABY_VISIT插入：" + String.Format(@"insert into FI_NEWBABY_VISIT(SERIAL_NO,JKBH,FSRQ,TWW,WYFS,PF,TZ,ZDSX,CZSJ,JLBS,DQBM,FSZ) values('{0}','{1}',to_date('{2}','" + dateFormat + "'),'{3}','{4}','{5}','{6}','{7}',to_date('{8}','" + dateFormat + "'),'0','{9}','{10}')",
                                 GenerateStringID(),
                                 Convert.ToString(dr["Card_Code"]),
                                 Convert.ToString(dr["VisitTime"]),
                                 Convert.ToString(dr["Temperature"]),
                                 Convert.ToString(dr["FeedingMode"]),
                                 Convert.ToString(dr["Skin"]),
                                 Convert.ToString(dr["Weight"]),
                                 Convert.ToString(dr["Handle"]),
                                 Convert.ToString(dr["OperateTime"]),
                                 Convert.ToString(dr["HospitalCode"]),
                                 Convert.ToString(dr["VisitDoctor"])
                               ));
                            int i = OracleHelper.ExecuteNonQuery(String.Format(@"insert into FI_NEWBABY_VISIT(SERIAL_NO,JKBH,FSRQ,TWW,WYFS,PF,TZ,ZDSX,CZSJ,JLBS,DQBM,FSZ) values('{0}','{1}',to_date('{2}','" + dateFormat + "'),'{3}','{4}','{5}','{6}','{7}',to_date('{8}','" + dateFormat + "'),'0','{9}','{10}')",
                                 GenerateStringID(),
                                 Convert.ToString(dr["Card_Code"]),
                                 Convert.ToString(dr["VisitTime"]),
                                 Convert.ToString(dr["Temperature"]),
                                 Convert.ToString(dr["FeedingMode"]),
                                 Convert.ToString(dr["Skin"]),
                                 Convert.ToString(dr["Weight"]),
                                 Convert.ToString(dr["Handle"]),
                                 Convert.ToString(dr["OperateTime"]),
                                 Convert.ToString(dr["HospitalCode"]),
                                 Convert.ToString(dr["VisitDoctor"])
                                ));
                            if (i > 0) //插入成功
                            {
                                LogInfo("产后访视新生儿插入成功");
                                //更新
                                int gx_num = SqlHelper.ExecteNonQuery(CommandType.Text, String.Format(@"update PostpartumVisitChild set PVisitChildState = 2 where ID={0}", Convert.ToInt32(dr["ID"])), null);
                                if (gx_num > 0) //更新成功 
                                {
                                    LogInfo("状态更新成功");
                                }
                                else //更新失败
                                {
                                    LogInfo("状态更新失败");
                                }
                            }
                            else //插入失败 
                            {
                                LogInfo("产后访视新生儿插入失败");
                            }
                        }
                        catch (Exception e)
                        {

                            LogInfo("错误：" + e.Message);
                        }
                    }
                }

                Thread.Sleep(200);
            }
            catch (Exception e)
            {

                LogInfo("产后访视新生儿异常：" + e.Message);
            }
        }
        
        //生成主键GUID16位
        private string GenerateStringID()
        {
            long i = 1;
            foreach (byte b in Guid.NewGuid().ToByteArray())
            {
                i *= ((int)b + 1);
            }

            return string.Format("{0:x}", i - DateTime.Now.Ticks);
        }

        //写入日记
        protected void LogInfo(string str)
        {
            if (!Directory.Exists("C:\\FYWRITELOG"))
            {
                Directory.CreateDirectory("C:\\FYWRITELOG");
            }

            using (System.IO.StreamWriter sw = new System.IO.StreamWriter("C:\\FYWRITELOG\\" + DateTime.Now.ToString("yyyyMMdd") + ".txt", true))
            {
                sw.WriteLine(DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss ") + str);
            }

        }

        protected override void OnStop()
        {
            LogInfo("服务关闭");
        }
    }
}
