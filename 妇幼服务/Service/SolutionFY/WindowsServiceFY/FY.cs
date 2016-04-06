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

namespace WindowsServiceFY
{
    public partial class FY : ServiceBase
    {
        //定时interval
        private int intervalTime_t = Convert.ToInt32(ConfigurationManager.AppSettings["interval_t"].ToString().Trim());
        //定时interval
        private int intervalTime_f = Convert.ToInt32(ConfigurationManager.AppSettings["interval_f"].ToString().Trim());
        //oracel mcid
        private string mcid = string.Empty;
        //lock
        private static object locker = new object();

        delegate void Delegate_do();

        public FY()
        {
            InitializeComponent();
        }

        protected override void OnStart(string[] args)
        {
            LogInfo("服务启动");

            //首次执行
            theout_f();

            //定时执行
            theout_t();
        }

        //首次执行异步委托
        protected void theout_f()
        {
            try
            {
                Delegate_do Delegate_do_f = DoWork_F;
                IAsyncResult result = Delegate_do_f.BeginInvoke(thend_f, Delegate_do_f);
            }
            catch (Exception e)
            {
                LogInfo("异步委托：" + e.Message);
            }
        }

        //定时执行异步委托
        protected void theout_t()
        {
            try
            {
                Delegate_do Delegate_do_t = DoWork_T;
                IAsyncResult result = Delegate_do_t.BeginInvoke(thend_t, Delegate_do_t);
            }
            catch (Exception e)
            {
                LogInfo("异步委托：" + e.Message);
            }
        }

        //定时执行回调函数
        protected void thend_t(IAsyncResult asyncResult)
        {
            Thread.Sleep(intervalTime_t); 
            theout_t();
        }

        //首次执行回调函数
        protected void thend_f(IAsyncResult asyncResult)
        {
            Thread.Sleep(intervalTime_f);
            theout_f();
        }

        //首次执行
        protected void DoWork_F()
        { 
            //获取未同步的
            LogInfo("获取所有未同步的sqlserver：" + String.Format(@"select a.*,b.*,c.* from UserData_Get a inner join User_Info b on a.U_ID = b.U_ID inner join ProcessState c on a.U_ID = c.U_ID  where a.GetStatues = 0"));
            DataTable dt = SqlHelper.ExecuteDataSet(CommandType.Text, String.Format(@"select a.*,b.*,c.* from UserData_Get a inner join User_Info b on a.U_ID = b.U_ID inner join ProcessState c on a.U_ID = c.U_ID  where a.GetStatues = 0"), null).Tables[0];
            if (dt.Rows.Count > 0)
            {
                for (int i = 0; i < dt.Rows.Count; i++)
                {
                    //获取
                    Once(dt.Rows[i]);
                    int m_num = SqlHelper.ExecteNonQuery(CommandType.Text, String.Format(@"update UserData_Get set GetStatues=1,GetEndTime=GETDATE() where U_ID={0}",
                        Convert.ToInt32(dt.Rows[i]["U_ID"])
                        ), null);
                    if (m_num == -1)//更新失败
                    {
                        LogInfo("用户同步更新失败！");
                    }
                    else
                    { //更新成功
                        LogInfo("用户同步更新成功！");
                    }
                }
            }
        }

        //定时执行
        protected void DoWork_T()
        {
            //获取未完成的（排除访视42天已完成的）
            LogInfo("获取所有未完成的sqlserver：" + String.Format(@"select a.*,b.* from ProcessState a inner join User_Info b on a.U_ID = b.U_ID where a.BindState = 1 and len(b.U_CardID)=18 and a.PostpartumCheckState <> 1 order by b.U_ID desc"));
            DataTable dt = SqlHelper.ExecuteDataSet(CommandType.Text, String.Format(@"select a.*,b.* from ProcessState a inner join User_Info b on a.U_ID = b.U_ID where a.BindState = 1 and len(b.U_CardID)=18 and a.PostpartumCheckState <> 1 order by b.U_ID desc"), null).Tables[0];
            if (dt.Rows.Count > 0)
            {
                for (int i = 0; i < dt.Rows.Count; i++)
                {
                    //获取
                    Once(dt.Rows[i]);
                }
            }
            //保健所医生
            YSXX();
        }

        //执行一遍
        protected void Once(DataRow dr)
        {
            lock (locker)
            {
                LogInfo("身份证：-------------------------------------------" + dr["U_CardID"].ToString());

                //主键编号(oracle 最新编号)
                mcid = GETNEWESTMCID(dr["U_CardID"].ToString(), Convert.ToInt32(dr["U_ID"]));

                if (!string.IsNullOrEmpty(mcid))
                {
                    //比对是不是一致，不是一致就初始化,更新最新mcid,如果mcid为空,就插入
                    if (dr["mcid"] == DBNull.Value || string.IsNullOrEmpty(dr["mcid"].ToString()))
                    {
                        LogInfo("流程表mcid插入sqlserver：" + String.Format(@"update ProcessState set mcid='{0}',SmallCardState=0,CheckCardState1=0,CheckCardState2=0,BigCardCheckState=0,DeliveryState=0,PostpartumVisitState1=0,PostpartumVisitState2=0,PostpartumCheckState=0 where U_ID={1}",
                            mcid,
                            Convert.ToInt32(dr["U_ID"])
                           ));
                        //插入mcid
                        int m_num = SqlHelper.ExecteNonQuery(CommandType.Text, String.Format(@"update ProcessState set mcid='{0}',SmallCardState=0,CheckCardState1=0,CheckCardState2=0,BigCardCheckState=0,DeliveryState=0,PostpartumVisitState1=0,PostpartumVisitState2=0,PostpartumCheckState=0 where U_ID={1}",
                            mcid,
                            Convert.ToInt32(dr["U_ID"])
                            ), null);
                        if (m_num == -1)//更新失败
                        {
                            LogInfo("流程表mcid插入更新失败！");
                        }
                        else
                        { //更新成功
                            LogInfo("流程表mcid插入更新成功！");
                        }
                    }
                    else if (dr["mcid"].ToString() != mcid) //新增建册（2胎等情况）
                    {
                        //更新mcid
                        LogInfo("流程表mcid更新sqlserver：" + String.Format(@"update ProcessState set mcid='{0}' where U_ID={1}",
                            mcid,
                            Convert.ToInt32(dr["U_ID"])
                           ));
                        //插入mcid
                        int row_num = SqlHelper.ExecteNonQuery(CommandType.Text, String.Format(@"update ProcessState set mcid='{0}' where U_ID={1}",
                            mcid,
                            Convert.ToInt32(dr["U_ID"])
                            ), null);
                        if (row_num == -1)//更新失败
                        {
                            LogInfo("流程表mcid更新更新失败！");
                        }
                        else //更新成功
                        {
                            LogInfo("流程表mcid更新更新成功！");

                            //用户初始化
                            if (dr["U_State"].ToString() != "0") //说明在APP进行预约的，预约时接口已进行初始化
                            {
                                //实名用户初始化
                                int row_1 = SqlHelper.ExecteNonQuery(CommandType.Text, String.Format(@"update User_Info set U_State=0,U_LastMenstruation='1970-01-01',U_Delivery='1970-01-01' where U_ID={0}",
                                    Convert.ToInt32(dr["U_ID"])
                                ), null);
                                if (row_1 == -1) //更新失败
                                {
                                    LogInfo("实名用户初始化失败！");
                                }
                                else //更新成功 
                                {
                                    LogInfo("实名用户初始化成功！");
                                }
                                //业务流程状态初始化
                                int row_2 = SqlHelper.ExecteNonQuery(CommandType.Text, String.Format(@"update ProcessState set SmallCardState=0,CheckCardState1=0,CheckCardState2=0,BigCardCheckState=0,DeliveryState=0,PostpartumVisitState1=0,PostpartumVisitState2=0,PostpartumCheckState=0 where ID={0}",
                                    Convert.ToInt32(dr["ID"])
                                ), null);
                                if (row_2 == -1) //更新失败
                                {
                                    LogInfo("业务流程状态初始化失败！");
                                }
                                else //更新成功 
                                {
                                    LogInfo("业务流程状态初始化成功！");
                                }
                                //建卡资料（小卡）初始化
                                SqlHelper.ExecteNonQuery(CommandType.Text, String.Format(@"delete from SmallCard where U_ID={0} and CardState = 1",
                                    Convert.ToInt32(dr["U_ID"])
                                ), null);
                                LogInfo("建卡资料（小卡）初始化成功！");
                                //建小卡（一般情况）初始化
                                SqlHelper.ExecteNonQuery(CommandType.Text, String.Format(@"delete from SmallCardOther where U_ID={0} and  exists(select 1 from SmallCard where SmallCard.U_ID = SmallCardOther.U_ID and SmallCard.CardState = 1)",
                                   Convert.ToInt32(dr["U_ID"])
                                ), null);
                                LogInfo("建小卡（一般情况）初始化成功！");
                                //初次检查初始化
                                SqlHelper.ExecteNonQuery(CommandType.Text, String.Format(@"delete from FirstCheck where exists(select 1 from SmallCard where SmallCard.Card_Code = FirstCheck.SmCard and SmallCard.U_ID={0} and SmallCard.CardState = 1)",
                                   Convert.ToInt32(dr["U_ID"])
                                ), null);
                                LogInfo("初次检查初始化成功！");
                                //复诊记录初始化
                                SqlHelper.ExecteNonQuery(CommandType.Text, String.Format(@"delete from SubsequentCheck where exists(select 1 from SmallCard where SmallCard.Card_Code = SubsequentCheck.SmCard and SmallCard.U_ID={0} and SmallCard.CardState = 1)",
                                   Convert.ToInt32(dr["U_ID"])
                                ), null);
                                LogInfo("复诊记录初始化成功！");
                                //大卡产检初始化
                                SqlHelper.ExecteNonQuery(CommandType.Text, String.Format(@"delete from BigCardCheckInfo where U_ID={0}",
                                    Convert.ToInt32(dr["U_ID"])
                                ), null);
                                LogInfo("大卡产检初始化成功！");
                                //分娩记录初始化
                                SqlHelper.ExecteNonQuery(CommandType.Text, String.Format(@"delete from DeliveryRecord where U_ID={0} and DRState = 1",
                                    Convert.ToInt32(dr["U_ID"])
                                ), null);
                                LogInfo("分娩记录初始化成功！");
                                //新生儿(记录)初始化
                                SqlHelper.ExecteNonQuery(CommandType.Text, String.Format(@"delete from NeonateRecord where U_ID={0} and NRState = 1",
                                    Convert.ToInt32(dr["U_ID"])
                                ), null);
                                LogInfo("新生儿(记录)初始化成功！");
                                //产后访视（产妇）初始化
                                SqlHelper.ExecteNonQuery(CommandType.Text, String.Format(@"delete from PostpartumVisit where U_ID={0} and PVisitState = 1",
                                    Convert.ToInt32(dr["U_ID"])
                                ), null);
                                LogInfo("产后访视（产妇）初始化成功！");
                                //产后访视（新生儿）初始化
                                SqlHelper.ExecteNonQuery(CommandType.Text, String.Format(@"delete from PostpartumVisitChild where U_ID={0} and PVisitChildState = 1",
                                    Convert.ToInt32(dr["U_ID"])
                                ), null);
                                LogInfo("产后访视（新生儿）初始化成功！");
                                //产后42天检查初始化
                                SqlHelper.ExecteNonQuery(CommandType.Text, String.Format(@"delete from PostpartumCheck where U_ID={0}",
                                   Convert.ToInt32(dr["U_ID"])
                                ), null);
                                LogInfo("产后42天检查初始化成功！");
                            }
                        }
                    }

                    if (dr["U_State"].ToString() != "1") //产妇基本信息
                    {
                        CFJBXX(dr);
                    }
                    if (dr["SmallCardState"].ToString() != "1") //小卡对接
                    {
                        XK(dr);
                    }
                    if (dr["CheckCardState1"].ToString() != "1" || dr["CheckCardState2"].ToString() != "1" || dr["BigCardCheckState"].ToString() != "1") //小卡、大卡产检对接
                    {
                        CJ(dr);
                    }
                    if (dr["DeliveryState"].ToString() != "1") //分娩记录（母亲）（新生儿）
                    {
                        FMJL(dr);
                    }
                    if (dr["RestReceiveState"].ToString() != "1") //修养接收
                    {
                        XYJS(dr);
                    }
                    if (dr["PostpartumVisitState1"].ToString() != "1") //产后访视第一次
                    {
                        CHFS_1(dr);
                    }
                    if (dr["PostpartumVisitState2"].ToString() != "1") //产后访视第二次
                    {
                        CHFS_2(dr);
                    }
                    if (dr["PostpartumCheckState"].ToString() != "1") //42天检查
                    {
                        JC_42(dr);
                    }
                }

                //休眠
                Thread.Sleep(200);
            }
        }

        //产妇基本信息
        protected void CFJBXX(DataRow _dr)
        {
            /* 根据身份证号更新
             * 末次月经日期 U_LastMenstruation--lmperiod
             * 预产期 U_Delivery--expecteddate
             */
            try
            {
                LogInfo("获取产妇基本信息未更新oracle：" + String.Format(@"select a.idnumber,b.lmperiod,b.expecteddate from V_DMC_REGISTRATIONCARDS a inner join v_dmc_firstrecord b on a.MCID= b.PCMCID where a.mcid='{0}'",
                    mcid
                    ));
                //查询oracle数据库数据
                DataTable dt = OracleHelper.GetDataTable(String.Format(@"select a.idnumber,b.lmperiod,b.expecteddate from V_DMC_REGISTRATIONCARDS a inner join v_dmc_firstrecord b on a.MCID= b.PCMCID where a.mcid='{0}'",
                    mcid
                    ));

                if (dt.Rows.Count > 0)
                {
                    DataRow odr = dt.Rows[0];
                    if (odr != null)
                    {
                        int i_num = SqlHelper.ExecteNonQuery(CommandType.Text, String.Format(@"update User_Info set U_LastMenstruation='{0}',U_Delivery='{1}' where U_ID={2}",
                            ConvertTime(odr["lmperiod"] == DBNull.Value ? "" : Convert.ToString(odr["lmperiod"])),
                            ConvertTime(odr["expecteddate"] == DBNull.Value ? "" : Convert.ToString(odr["expecteddate"])),
                            Convert.ToInt32(_dr["U_ID"])
                            ), null);
                        if (i_num == -1) //更新失败
                        {
                            LogInfo("产妇基本信息更新失败！");
                        }
                        else
                        {
                            //更新怀孕状态(0备孕1怀孕2产后)
                            if (Convert.ToString(odr["expecteddate"]).Length > 0 && DateTime.Compare(Convert.ToDateTime(ConvertTime(Convert.ToString(odr["expecteddate"]))), DateTime.Now) < 0) //产后
                            {
                                LogInfo("产妇基本信息怀孕状态更新sqlserver：" + String.Format(@"update User_Info set U_State=2 where U_ID={0}",
                                    Convert.ToInt32(_dr["U_ID"])
                                ));
                                int x_num = SqlHelper.ExecteNonQuery(CommandType.Text, String.Format(@"update User_Info set U_State=2 where U_ID={0}",
                                    Convert.ToInt32(_dr["U_ID"])
                                ), null);
                                if (x_num == -1)//更新失败
                                {
                                    LogInfo("产妇基本信息怀孕状态更新失败！");
                                }
                                else
                                { //更新成功
                                    LogInfo("产妇基本信息怀孕状态更新成功！");
                                }
                            }
                            else //怀孕
                            {
                                LogInfo("产妇基本信息怀孕状态更新sqlserver：" + String.Format(@"update User_Info set U_State=1 where U_ID={0}",
                                        Convert.ToInt32(_dr["U_ID"])
                                ));
                                int y_num = SqlHelper.ExecteNonQuery(CommandType.Text, String.Format(@"update User_Info set U_State=1 where U_ID={0}",
                                        Convert.ToInt32(_dr["U_ID"])
                                    ), null);
                                if (y_num == -1)//更新失败
                                {
                                    LogInfo("产妇基本信息怀孕状态更新失败！");
                                }
                                else
                                { //更新成功
                                    LogInfo("产妇基本信息怀孕状态更新成功！");
                                }
                            }
                            LogInfo("产妇基本信息更新成功！");
                        }
                    }
                }
            }
            catch (Exception e)
            {
                LogInfo("产妇基本信息错误：" + e.Message);
            }
        }

        //小卡对接
        protected void XK(DataRow _dr)
        {
            /* 插入建小卡（一般情况）（基本信息）、更新业务流程状态
             * 一般情况：
             * 文化程度 Culture--meducation
             * 职业 Profession--professional
             * 孕次 YunCi--pregnancies
             * 末次月经 LastMenstruation--lmperiod
             * 预产期 Delivery--expecteddate
             * 孕早期其它情况 YunOther--npregnantearlyoth
             * 
             * 基本信息：
             * 建册日期(DateTime) CreateTime--bbdate
             * 手册编码(string) Card_Code--healthnumber
             * 身份证号(string) GravidaIdCard--idnumber
             * 姓名(string) GravidalName--mothername
             * 工作单位(string) GravidalWorkUnit--workunit
             * 电话(string) GravidalPhone--contactphone
             * 丈夫姓名(string) ManName-husbandname
             * 电话(string) RelaxPhone--husbandcontactphone
             * 工作单位(string) ManWorkUnit--husbandworkunit
             * 
             * 产后休养地详细地址(string) RelaxAddress--paprovince
             * 年龄(int) GravidalAge--age
             * 邮编(string) ZipCode--workunitzipcode
             * 家庭住址明细(string) HomeAddress--cabuilding
             * 家庭所在地邮编(string) HomeZipCode--cazipcode
             * 建册单位编码(string) HospitalCode--bbunitcode
             */

            try
            {
                #region 小卡一般情况
                int i_num = 0;
                int j_num = 0;
                LogInfo("获取小卡一般情况oracle：" + String.Format(@"select a.meducation,a.professional,b.pregnancies,b.lmperiod,b.expecteddate,b.npregnantearlyoth from V_DMC_REGISTRATIONCARDS a inner join v_dmc_firstrecord b on a.MCID= b.PCMCID where a.mcid='{0}'",
                    mcid
                    ));
                //根据身份证号查询小卡情况（一般情况）
                DataTable dt = OracleHelper.GetDataTable(String.Format(@"select a.meducation,a.professional,b.pregnancies,b.lmperiod,b.expecteddate,b.npregnantearlyoth from V_DMC_REGISTRATIONCARDS a inner join v_dmc_firstrecord b on a.MCID= b.PCMCID where a.mcid='{0}'",
                    mcid
                    ));
                if (dt.Rows.Count > 0)
                {
                    DataRow odr = dt.Rows[0];
                    //插入小卡(一般情况)
                    if (odr != null)
                    {
                        LogInfo("插入小卡一般情况sqlserver：" + String.Format(@"insert into SmallCardOther(Culture,Profession,YunCi,LastMenstruation,Delivery,YunOther,U_ID,CanCi,Menstrual,YunEarly,CardID,OperateTime,OperateUserId) values({0},'{1}',{2},'{3}','{4}','{5}',{6},0,'',0,0,GETDATE(),0)",
                            odr["meducation"] == DBNull.Value ? 0 : Convert.ToInt32(odr["meducation"]),
                            odr["professional"] == DBNull.Value ? "" : Convert.ToString(odr["professional"]),
                            odr["pregnancies"] == DBNull.Value ? 0 : Convert.ToInt32(odr["pregnancies"]),
                            ConvertTime(odr["lmperiod"] == DBNull.Value ? "" : Convert.ToString(odr["lmperiod"])),
                            ConvertTime(odr["expecteddate"] == DBNull.Value ? "" : Convert.ToString(odr["expecteddate"])),
                            odr["npregnantearlyoth"] == DBNull.Value ? "" : Convert.ToString(odr["npregnantearlyoth"]),
                            _dr["U_ID"] == DBNull.Value ? 0 : Convert.ToInt32(_dr["U_ID"])
                            ));

                        i_num = SqlHelper.ExecteNonQuery(CommandType.Text, String.Format(@"insert into SmallCardOther(Culture,Profession,YunCi,LastMenstruation,Delivery,YunOther,U_ID,CanCi,Menstrual,YunEarly,CardID,OperateTime,OperateUserId) values({0},'{1}',{2},'{3}','{4}','{5}',{6},0,'',0,0,GETDATE(),0)",
                            odr["meducation"] == DBNull.Value ? 0 : Convert.ToInt32(odr["meducation"]),
                            odr["professional"] == DBNull.Value ? "" : Convert.ToString(odr["professional"]),
                            odr["pregnancies"] == DBNull.Value ? 0 : Convert.ToInt32(odr["pregnancies"]),
                            ConvertTime(odr["lmperiod"] == DBNull.Value ? "" : Convert.ToString(odr["lmperiod"])),
                            ConvertTime(odr["expecteddate"] == DBNull.Value ? "" : Convert.ToString(odr["expecteddate"])),
                            odr["npregnantearlyoth"] == DBNull.Value ? "" : Convert.ToString(odr["npregnantearlyoth"]),
                            _dr["U_ID"] == DBNull.Value ? 0 : Convert.ToInt32(_dr["U_ID"])
                            ), null);
                        if (i_num == -1) //插入失败
                        {
                            LogInfo("建小卡（一般情况）插入失败！");
                        }
                        else //插入成功
                        {
                            LogInfo("建小卡（一般情况）插入成功！");
                           
                            SqlHelper.ExecteNonQuery(CommandType.Text, String.Format(@"delete from SmallCardOther where CardID in(select Card_ID from SmallCard where CardState = 2 and U_ID={0})",
                                Convert.ToInt32(_dr["U_ID"])
                                ), null);
                        }
                    }
                }
                #endregion

                #region 小卡基本情况
                LogInfo("获取小卡基本情况oracle：" + String.Format(@"select a.* from V_DMC_REGISTRATIONCARDS a where a.mcid='{0}'",
                    mcid
                    ));
                //根据身份证号查询小卡情况（基本信息）
                DataTable dt_2 = OracleHelper.GetDataTable(String.Format(@"select a.* from V_DMC_REGISTRATIONCARDS a where a.mcid='{0}'",
                    mcid
                    ));
                if (dt_2.Rows.Count > 0)
                {
                    DataRow odr_2 = dt_2.Rows[0];
                    //插入小卡(基本信息)
                    if (odr_2 != null)
                    {
                        LogInfo("插入小卡基本情况sqlserver：" + String.Format(@"insert into SmallCard(CreateTime,Card_Code,GravidaIdCard,GravidalName,GravidalWorkUnit,GravidalPhone,ManName,RelaxPhone,ManWorkUnit,U_ID,RelaxAddress,CreateHospital,CardState,GravidalAge,ZipCode,HomeAddress_DID,HomeAddress_SID,HomeAddress,HomeZipCode,RelaxAddress_DID,RelaxAddress_SID,School,YBCard,OperateTime,OperateUserId,HospitalCode) values('{0}','{1}','{2}','{3}','{4}','{5}','{6}','{7}','{8}',{9},'{10}',0,1,{11},'{12}','','','{13}','{14}','','',0,'',GETDATE(),0,'{15}')",
                            ConvertTime(odr_2["bbdate"] == DBNull.Value ? "" : Convert.ToString(odr_2["bbdate"])),
                            Convert.ToString(odr_2["healthnumber"] == DBNull.Value ? "" : odr_2["healthnumber"]),
                            Convert.ToString(odr_2["idnumber"] == DBNull.Value ? "" : odr_2["idnumber"]),
                            Convert.ToString(odr_2["mothername"] == DBNull.Value ? "" : odr_2["mothername"]),
                            Convert.ToString(odr_2["workunit"] == DBNull.Value ? "" : odr_2["workunit"]),
                            Convert.ToString(odr_2["contactphone"] == DBNull.Value ? "" : odr_2["contactphone"]),
                            Convert.ToString(odr_2["husbandname"] == DBNull.Value ? "" : odr_2["husbandname"]),
                            Convert.ToString(odr_2["husbandcontactphone"] == DBNull.Value ? "" : odr_2["husbandcontactphone"]),
                            Convert.ToString(odr_2["husbandworkunit"] == DBNull.Value ? "" : odr_2["husbandworkunit"]),
                            Convert.ToInt32(_dr["U_ID"] == DBNull.Value ? 0 : _dr["U_ID"]),
                            Convert.ToString(odr_2["paprovince"] == DBNull.Value ? "" : odr_2["paprovince"]),
                            Convert.ToInt32(odr_2["age"] == DBNull.Value ? 0 : odr_2["age"]),
                            Convert.ToString(odr_2["workunitzipcode"] == DBNull.Value ? "" : odr_2["workunitzipcode"]),
                            Convert.ToString(odr_2["cabuilding"] == DBNull.Value ? "" : odr_2["cabuilding"]),
                            Convert.ToString(odr_2["cazipcode"] == DBNull.Value ? "" : odr_2["cazipcode"]),
                            Convert.ToString(odr_2["bbunitcode"] == DBNull.Value ? "" : odr_2["bbunitcode"])
                            ));

                        j_num = SqlHelper.ExecteNonQuery(CommandType.Text, String.Format(@"insert into SmallCard(CreateTime,Card_Code,GravidaIdCard,GravidalName,GravidalWorkUnit,GravidalPhone,ManName,RelaxPhone,ManWorkUnit,U_ID,RelaxAddress,CreateHospital,CardState,GravidalAge,ZipCode,HomeAddress_DID,HomeAddress_SID,HomeAddress,HomeZipCode,RelaxAddress_DID,RelaxAddress_SID,School,YBCard,OperateTime,OperateUserId,HospitalCode) values('{0}','{1}','{2}','{3}','{4}','{5}','{6}','{7}','{8}',{9},'{10}',0,1,{11},'{12}','','','{13}','{14}','','',0,'',GETDATE(),0,'{15}')",
                            ConvertTime(odr_2["bbdate"] == DBNull.Value ? "" : Convert.ToString(odr_2["bbdate"])),
                            Convert.ToString(odr_2["healthnumber"] == DBNull.Value ? "" : odr_2["healthnumber"]),
                            Convert.ToString(odr_2["idnumber"] == DBNull.Value ? "" : odr_2["idnumber"]),
                            Convert.ToString(odr_2["mothername"] == DBNull.Value ? "" : odr_2["mothername"]),
                            Convert.ToString(odr_2["workunit"] == DBNull.Value ? "" : odr_2["workunit"]),
                            Convert.ToString(odr_2["contactphone"] == DBNull.Value ? "" : odr_2["contactphone"]),
                            Convert.ToString(odr_2["husbandname"] == DBNull.Value ? "" : odr_2["husbandname"]),
                            Convert.ToString(odr_2["husbandcontactphone"] == DBNull.Value ? "" : odr_2["husbandcontactphone"]),
                            Convert.ToString(odr_2["husbandworkunit"] == DBNull.Value ? "" : odr_2["husbandworkunit"]),
                            Convert.ToInt32(_dr["U_ID"] == DBNull.Value ? 0 : _dr["U_ID"]),
                            Convert.ToString(odr_2["paprovince"] == DBNull.Value ? "" : odr_2["paprovince"]),
                            Convert.ToInt32(odr_2["age"] == DBNull.Value ? 0 : odr_2["age"]),
                            Convert.ToString(odr_2["workunitzipcode"] == DBNull.Value ? "" : odr_2["workunitzipcode"]),
                            Convert.ToString(odr_2["cabuilding"] == DBNull.Value ? "" : odr_2["cabuilding"]),
                            Convert.ToString(odr_2["cazipcode"] == DBNull.Value ? "" : odr_2["cazipcode"]),
                            Convert.ToString(odr_2["bbunitcode"] == DBNull.Value ? "" : odr_2["bbunitcode"])
                            ), null);
                        if (j_num == -1) //更新失败
                        {
                            LogInfo("建小卡（基本信息）插入失败！");
                        }
                        else //更新成功
                        { 
                            LogInfo("建小卡（基本信息）插入成功！");
                          
                            //删除已写入的
                            SqlHelper.ExecteNonQuery(CommandType.Text, String.Format(@"delete from SmallCard where CardState = 2 and U_ID={0}",
                                Convert.ToInt32(_dr["U_ID"])
                                ), null);
                        }
                    }
                    if (i_num > 0 && j_num > 0)
                    {
                        LogInfo("更新流程表(小卡)sqlserver：" + String.Format(@"update ProcessState set SmallCardState = 1 where ID={0}", Convert.ToInt32(_dr["ID"])));
                        //更新流程表
                        int z_num = SqlHelper.ExecteNonQuery(CommandType.Text, String.Format(@"update ProcessState set SmallCardState = 1 where ID={0}", Convert.ToInt32(_dr["ID"])), null);
                        if (z_num == -1) //更新失败
                        {
                            LogInfo("流程表（新建小卡更新失败！");
                        }
                        else
                        { //更新成功
                            LogInfo("流程表（新建小卡更新成功！");
                        }
                    }
                }
                #endregion
            }
            catch (Exception e)
            {
                LogInfo("小卡对接：" + e.Message);
            }
        }

        //小卡、大卡产检对接
        protected void CJ(DataRow _dr)
        {
            /* 插入小卡产检（第一次）、更新业务流程状态
             * 孕周(string) YzNum--gagew
             * 身高(string) Height--height
             * 体重(string) Weight--weight
             * 基础血压(string) Jcxy--bplow
             * 妇科检查：外阴(string) WY--vulvaoth
             * 阴道(string) YD--vaginaloth
             * 宫颈(string) GJ--cervixoth
             * 宫体(string) GT--uterusoth
             * 附件(string) FJ--attachmentoth
             * 化验：Hb(string) Hb--hb
             * 血型(string) XX--BLOODTYPE_1
             * 尿糖(string) LT--glu
             * 处理(string) Handle--disposemeasure
             * 检测时间(DateTime) CheckTime--checkupdate
             * 医生(string) Doctor--checkupdoctorname
             */
            #region 插入小卡产检（第一次）
            if (Convert.ToInt32(_dr["CheckCardState1"]) == 0)
            {
                try
                {
                    LogInfo("获取小卡产检第一次oracle：" + String.Format(@"select b.*, nvl( (select CodeName from ggws.code_compare where codetype = 'FYHJXX' and Codevalue=b.BLOODTYPE),'') as BLOODTYPE_1 from v_dmc_firstrecord b where b.pcmcid='{0}' and b.havepfirst=1",
                       mcid
                        ));

                    //根据身份证号查询oracle数据库数据
                    DataTable dt = OracleHelper.GetDataTable(String.Format(@"select b.*, nvl( (select CodeName from ggws.code_compare where codetype = 'FYHJXX' and Codevalue=b.BLOODTYPE),'') as BLOODTYPE_1 from v_dmc_firstrecord b where b.pcmcid='{0}' and b.havepfirst=1",
                        mcid
                        ));
                    if (dt.Rows.Count > 0)
                    {
                        DataRow odr = dt.Rows[0];
                        if (odr != null)
                        {
                            LogInfo("插入小卡产检第一次sqlserver：" + String.Format(@"insert into FirstCheck(YzNum,Height,Weight,Jcxy,WY,YD,GJ,GT,FJ,Hb,XX,LT,Handle,CheckTime,Doctor,SmCard,XF,GP,JZ,SZ,PF,ZFXX,BDCG,LCG,GGL,HBsAg,HT,ZCWJ,ZGJJ,CLHT,ZhenDuan,DownTime) values('{0}','{1}','{2}','{3}','{4}','{5}','{6}','{7}','{8}','{9}','{10}','{11}','{12}','{13}','{14}',(select top(1) Card_Code from SmallCard where U_ID='{15}'),'','','','','','','','','','','','','','','',GETDATE())",
                                   Convert.ToString(odr["gagew"] == DBNull.Value ? "" : odr["gagew"]),
                                   Convert.ToString(odr["height"] == DBNull.Value ? "" : odr["height"]),
                                   Convert.ToString(odr["weight"] == DBNull.Value ? "" : odr["weight"]),
                                   Convert.ToString(odr["bplow"] == DBNull.Value ? "" : odr["bplow"]),
                                   Convert.ToString(odr["vulvaoth"] == DBNull.Value ? "" : odr["vulvaoth"]),
                                   Convert.ToString(odr["vaginaloth"] == DBNull.Value ? "" : odr["vaginaloth"]),
                                   Convert.ToString(odr["cervixoth"] == DBNull.Value ? "" : odr["cervixoth"]),
                                   Convert.ToString(odr["uterusoth"] == DBNull.Value ? "" : odr["uterusoth"]),
                                   Convert.ToString(odr["attachmentoth"] == DBNull.Value ? "" : odr["attachmentoth"]),
                                   Convert.ToString(odr["hb"] == DBNull.Value ? "" : odr["hb"]),
                                   Convert.ToString(odr["BLOODTYPE_1"] == DBNull.Value ? "" : odr["BLOODTYPE_1"]),
                                   Convert.ToString(odr["glu"] == DBNull.Value ? "" : odr["glu"]),
                                   Convert.ToString(odr["disposemeasure"] == DBNull.Value ? "" : odr["disposemeasure"]),
                                   ConvertTime(odr["checkupdate"] == DBNull.Value ? "" : Convert.ToString(odr["checkupdate"])),
                                   Convert.ToString(odr["checkupdoctorname"] == DBNull.Value ? "" : odr["checkupdoctorname"]),
                                   Convert.ToInt32(_dr["U_ID"])
                                   ));

                            int i_num = SqlHelper.ExecteNonQuery(CommandType.Text, String.Format(@"insert into FirstCheck(YzNum,Height,Weight,Jcxy,WY,YD,GJ,GT,FJ,Hb,XX,LT,Handle,CheckTime,Doctor,SmCard,XF,GP,JZ,SZ,PF,ZFXX,BDCG,LCG,GGL,HBsAg,HT,ZCWJ,ZGJJ,CLHT,ZhenDuan,DownTime) values('{0}','{1}','{2}','{3}','{4}','{5}','{6}','{7}','{8}','{9}','{10}','{11}','{12}','{13}','{14}',(select top(1) Card_Code from SmallCard where U_ID='{15}'),'','','','','','','','','','','','','','','',GETDATE())",
                                   Convert.ToString(odr["gagew"] == DBNull.Value ? "" : odr["gagew"]),
                                   Convert.ToString(odr["height"] == DBNull.Value ? "" : odr["height"]),
                                   Convert.ToString(odr["weight"] == DBNull.Value ? "" : odr["weight"]),
                                   Convert.ToString(odr["bplow"] == DBNull.Value ? "" : odr["bplow"]),
                                   Convert.ToString(odr["vulvaoth"] == DBNull.Value ? "" : odr["vulvaoth"]),
                                   Convert.ToString(odr["vaginaloth"] == DBNull.Value ? "" : odr["vaginaloth"]),
                                   Convert.ToString(odr["cervixoth"] == DBNull.Value ? "" : odr["cervixoth"]),
                                   Convert.ToString(odr["uterusoth"] == DBNull.Value ? "" : odr["uterusoth"]),
                                   Convert.ToString(odr["attachmentoth"] == DBNull.Value ? "" : odr["attachmentoth"]),
                                   Convert.ToString(odr["hb"] == DBNull.Value ? "" : odr["hb"]),
                                   Convert.ToString(odr["BLOODTYPE_1"] == DBNull.Value ? "" : odr["BLOODTYPE_1"]),
                                   Convert.ToString(odr["glu"] == DBNull.Value ? "" : odr["glu"]),
                                   Convert.ToString(odr["disposemeasure"] == DBNull.Value ? "" : odr["disposemeasure"]),
                                   ConvertTime(odr["checkupdate"] == DBNull.Value ? "" : Convert.ToString(odr["checkupdate"])),
                                   Convert.ToString(odr["checkupdoctorname"] == DBNull.Value ? "" : odr["checkupdoctorname"]),
                                   Convert.ToInt32(_dr["U_ID"])
                                    ), null);
                            if (i_num == -1) //插入失败
                            {
                                LogInfo("小卡产检第一次插入失败！");
                            }
                            else
                            {
                                LogInfo("小卡产检第一次插入成功！");
                                //更新流程表
                                int j_num = SqlHelper.ExecteNonQuery(CommandType.Text, String.Format(@"update ProcessState set CheckCardState1 = 1 where ID={0}", Convert.ToInt32(_dr["ID"])), null);
                                if (j_num == -1) //更新失败
                                {
                                    LogInfo("流程表（小卡产检第一次）更新失败！");
                                }
                                else
                                {
                                    LogInfo("流程表（小卡产检第一次）更新成功！");
                                }
                            }
                        }
                    }
                }
                catch (Exception e)
                {
                    LogInfo("小卡产检第一次：" + e.Message);
                }
            }
            #endregion

            /* 插入小卡产检（第二次，第三次）大卡产检、更新业务流程状态
             * 更新小卡产检（第二次，第三次）:
             * 日期(DateTime) CheckTime--checkupdate
             * 孕周(string) YzNum--gagew
             * 血压(string) XY--bphigh
             * 体重(string) Weight--weight
             * 宫高(string) GG--fheight
             * 腹围(string) FW--acircumference
             * 胎心(string) TX--fhrate
             * 浮肿(string) FZ--edema
             * 尿蛋白(string) LDB--pro
             * 高危评分及原因(string) GWPF--hrscore
             * 签名(string) Doctor--checkupdoctorname
             * 
             * 大卡产检:
             * 日期(DateTime) Check_Date--checkupdate
             * 孕周(string) YZ--gagew
             * 血压(string) XY--bphigh
             * 体重(string) Weight--weight
             * 宫高(string) GG--fheight
             * 胎位(string) TW--fposition
             * 胎心(string) TX--fhrate
             * 先露(string) XL--firstlyreveal
             * 衔接(string) XJ--linkup
             * 浮肿(string) FZh--edema
             * 尿蛋白(string) NDB--pro
             * 高危评分及因素(string) GWPF--hrscore
             * 签名(string) Doctor--checkupdoctorname
             */
            //获取未完成的(小卡第二次，第三次) 和 大卡
            #region 获取未完成的(小卡第二次，第三次) 和 大卡
            try
            {
                LogInfo("获取小卡产检第二次，第三次和 大卡oracle：" + String.Format(@"select b.* from v_DMC_REFERRALRECORDS b where b.pcmcid = '{0}' order by b.checkupdate asc",
                    mcid
                    ));

                //根据身份证号查询oracle数据库数据
                DataTable dt_2 = OracleHelper.GetDataTable(String.Format(@"select b.* from v_DMC_REFERRALRECORDS b where b.pcmcid = '{0}' order by b.checkupdate asc",
                    mcid
                    ));
                if (dt_2.Rows.Count > 0)
                {
                    DataRow odr_2 = null;
                    int count = 0;
                    for (int q = 0; q < dt_2.Rows.Count; q++)
                    {
                        odr_2 = dt_2.Rows[q];
                        count++;

                        if (count == 1 && Convert.ToInt32(_dr["CheckCardState2"]) == 0)//小卡第二次
                        {
                            LogInfo("插入小卡产检第二次sqlserver：" + String.Format(@"insert into SubsequentCheck(CheckTime,YzNum,XY,Weight,GG,FW,TX,FZ,LDB,GWPF,Doctor,SmCard,HBZCL,DownTime) values('{0}','{1}','{2}','{3}','{4}','{5}','{6}','{7}','{8}','{9}','{10}',(select top(1) Card_Code from SmallCard where U_ID='{11}'),'',GETDATE())",
                                ConvertTime(odr_2["checkupdate"] == DBNull.Value ? "" : Convert.ToString(odr_2["checkupdate"])),
                                odr_2["gagew"] == DBNull.Value ? "" : Convert.ToString(odr_2["gagew"]),
                                odr_2["bphigh"] == DBNull.Value ? "" : Convert.ToString(odr_2["bphigh"]),
                                odr_2["weight"] == DBNull.Value ? "" : Convert.ToString(odr_2["weight"]),
                                odr_2["fheight"] == DBNull.Value ? "" : Convert.ToString(odr_2["fheight"]),
                                odr_2["acircumference"] == DBNull.Value ? "" : Convert.ToString(odr_2["acircumference"]),
                                odr_2["fhrate"] == DBNull.Value ? "" : Convert.ToString(odr_2["fhrate"]),
                                odr_2["edema"] == DBNull.Value ? "" : Convert.ToString(odr_2["edema"]),
                                odr_2["pro"] == DBNull.Value ? "" : Convert.ToString(odr_2["pro"]),
                                odr_2["hrscore"] == DBNull.Value ? "" : Convert.ToString(odr_2["hrscore"]),
                                odr_2["checkupdoctorname"] == DBNull.Value ? "" : Convert.ToString(odr_2["checkupdoctorname"]),
                                Convert.ToInt32(_dr["U_ID"])
                                ));

                            //插入数据
                            int x_num = SqlHelper.ExecteNonQuery(CommandType.Text, String.Format(@"insert into SubsequentCheck(CheckTime,YzNum,XY,Weight,GG,FW,TX,FZ,LDB,GWPF,Doctor,SmCard,HBZCL,DownTime) values('{0}','{1}','{2}','{3}','{4}','{5}','{6}','{7}','{8}','{9}','{10}',(select top(1) Card_Code from SmallCard where U_ID='{11}'),'',GETDATE())",
                                ConvertTime(odr_2["checkupdate"] == DBNull.Value ? "" : Convert.ToString(odr_2["checkupdate"])),
                                odr_2["gagew"] == DBNull.Value ? "" : Convert.ToString(odr_2["gagew"]),
                                odr_2["bphigh"] == DBNull.Value ? "" : Convert.ToString(odr_2["bphigh"]),
                                odr_2["weight"] == DBNull.Value ? "" : Convert.ToString(odr_2["weight"]),
                                odr_2["fheight"] == DBNull.Value ? "" : Convert.ToString(odr_2["fheight"]),
                                odr_2["acircumference"] == DBNull.Value ? "" : Convert.ToString(odr_2["acircumference"]),
                                odr_2["fhrate"] == DBNull.Value ? "" : Convert.ToString(odr_2["fhrate"]),
                                odr_2["edema"] == DBNull.Value ? "" : Convert.ToString(odr_2["edema"]),
                                odr_2["pro"] == DBNull.Value ? "" : Convert.ToString(odr_2["pro"]),
                                odr_2["hrscore"] == DBNull.Value ? "" : Convert.ToString(odr_2["hrscore"]),
                                odr_2["checkupdoctorname"] == DBNull.Value ? "" : Convert.ToString(odr_2["checkupdoctorname"]),
                                Convert.ToInt32(_dr["U_ID"])
                                    ), null);
                            if (x_num == -1) //插入失败
                            {
                                LogInfo("小卡产检第二次插入失败！");
                            }
                            else
                            {
                                LogInfo("小卡产检第二次插入成功！");
                                //更新流程表
                                int i_num = SqlHelper.ExecteNonQuery(CommandType.Text, String.Format(@"update ProcessState set CheckCardState2 = 1 where ID={0}", Convert.ToInt32(_dr["ID"])), null);
                                if (i_num == -1) //更新失败
                                {
                                    LogInfo("流程表（小卡产检第二次）更新失败！");
                                }
                                else
                                {
                                    LogInfo("流程表（小卡产检第二次）更新成功！");
                                }
                            }
                        }
                        else if (count == 2 && Convert.ToInt32(_dr["CheckCardState3"]) == 0)//小卡第三次
                        {
                            LogInfo("插入小卡产检第三次sqlserver：" + String.Format(@"insert into SubsequentCheck(CheckTime,YzNum,XY,Weight,GG,FW,TX,FZ,LDB,GWPF,Doctor,SmCard,HBZCL,DownTime) values('{0}','{1}','{2}','{3}','{4}','{5}','{6}','{7}','{8}','{9}','{10}',(select top(1) Card_Code from SmallCard where U_ID='{11}'),'',GETDATE())",
                                ConvertTime(odr_2["checkupdate"] == DBNull.Value ? "" : Convert.ToString(odr_2["checkupdate"])),
                                odr_2["gagew"] == DBNull.Value ? "" : Convert.ToString(odr_2["gagew"]),
                                odr_2["bphigh"] == DBNull.Value ? "" : Convert.ToString(odr_2["bphigh"]),
                                odr_2["weight"] == DBNull.Value ? "" : Convert.ToString(odr_2["weight"]),
                                odr_2["fheight"] == DBNull.Value ? "" : Convert.ToString(odr_2["fheight"]),
                                odr_2["acircumference"] == DBNull.Value ? "" : Convert.ToString(odr_2["acircumference"]),
                                odr_2["fhrate"] == DBNull.Value ? "" : Convert.ToString(odr_2["fhrate"]),
                                odr_2["edema"] == DBNull.Value ? "" : Convert.ToString(odr_2["edema"]),
                                odr_2["pro"] == DBNull.Value ? "" : Convert.ToString(odr_2["pro"]),
                                odr_2["hrscore"] == DBNull.Value ? "" : Convert.ToString(odr_2["hrscore"]),
                                odr_2["checkupdoctorname"] == DBNull.Value ? "" : Convert.ToString(odr_2["checkupdoctorname"]),
                                Convert.ToInt32(_dr["U_ID"])
                                ));
                            //插入数据
                            int y_num = SqlHelper.ExecteNonQuery(CommandType.Text, String.Format(@"insert into SubsequentCheck(CheckTime,YzNum,XY,Weight,GG,FW,TX,FZ,LDB,GWPF,Doctor,SmCard,HBZCL,DownTime) values('{0}','{1}','{2}','{3}','{4}','{5}','{6}','{7}','{8}','{9}','{10}',(select top(1) Card_Code from SmallCard where U_ID='{11}'),'',GETDATE())",
                                ConvertTime(odr_2["checkupdate"] == DBNull.Value ? "" : Convert.ToString(odr_2["checkupdate"])),
                                odr_2["gagew"] == DBNull.Value ? "" : Convert.ToString(odr_2["gagew"]),
                                odr_2["bphigh"] == DBNull.Value ? "" : Convert.ToString(odr_2["bphigh"]),
                                odr_2["weight"] == DBNull.Value ? "" : Convert.ToString(odr_2["weight"]),
                                odr_2["fheight"] == DBNull.Value ? "" : Convert.ToString(odr_2["fheight"]),
                                odr_2["acircumference"] == DBNull.Value ? "" : Convert.ToString(odr_2["acircumference"]),
                                odr_2["fhrate"] == DBNull.Value ? "" : Convert.ToString(odr_2["fhrate"]),
                                odr_2["edema"] == DBNull.Value ? "" : Convert.ToString(odr_2["edema"]),
                                odr_2["pro"] == DBNull.Value ? "" : Convert.ToString(odr_2["pro"]),
                                odr_2["hrscore"] == DBNull.Value ? "" : Convert.ToString(odr_2["hrscore"]),
                                odr_2["checkupdoctorname"] == DBNull.Value ? "" : Convert.ToString(odr_2["checkupdoctorname"]),
                                Convert.ToInt32(_dr["U_ID"])
                                ), null);
                            if (y_num == 0) //插入失败
                            {
                                LogInfo("小卡产检第三次插入失败！");
                            }
                            else
                            {
                                LogInfo("小卡产检第三次插入成功！");
                                //更新流程表
                                int j_num = SqlHelper.ExecteNonQuery(CommandType.Text, String.Format(@"update ProcessState set CheckCardState3 = 1 where ID={0}", Convert.ToInt32(_dr["ID"])), null);
                                if (j_num == -1) //更新失败
                                {
                                    LogInfo("流程表（小卡产检第三次）更新失败！");
                                }
                                else
                                { //更新成功
                                    LogInfo("流程表（小卡产检第三次）更新成功！");
                                }
                            }
                        }
                        else if (count >= 3 && Convert.ToInt32(_dr["BigCardCheckState"]) == 0)//大卡
                        {
                            if (count == 3)
                            {
                                //删除
                                int c_num = SqlHelper.ExecteNonQuery(CommandType.Text, String.Format(@"delete from BigCardCheckInfo where U_ID='{0}'", Convert.ToInt32(_dr["U_ID"])), null);
                                LogInfo("大卡产检删除成功！");
                            }

                            LogInfo("插入大卡产检sqlserver：" + String.Format(@"insert into BigCardCheckInfo(Check_Date,YZ,XY,Weight,GG,TW,TX,XL,XJ,FZh,NDB,GWPF,Doctor,U_ID,FZ,XSS,Remark,HBZCL,Res_Date) values('{0}','{1}','{2}','{3}','{4}','{5}','{6}','{7}','{8}','{9}','{10}','{11}','{12}','{13}','','','','',GETDATE())",
                                ConvertTime(odr_2["checkupdate"] == DBNull.Value ? "" : Convert.ToString(odr_2["checkupdate"])),
                                odr_2["gagew"] == DBNull.Value ? "" : Convert.ToString(odr_2["gagew"]),
                                odr_2["bphigh"] == DBNull.Value ? "" : Convert.ToString(odr_2["bphigh"]),
                                odr_2["weight"] == DBNull.Value ? "" : Convert.ToString(odr_2["weight"]),
                                odr_2["fheight"] == DBNull.Value ? "" : Convert.ToString(odr_2["fheight"]),
                                odr_2["fposition"] == DBNull.Value ? "" : Convert.ToString(odr_2["fposition"]),
                                odr_2["fhrate"] == DBNull.Value ? "" : Convert.ToString(odr_2["fhrate"]),
                                odr_2["firstlyreveal"] == DBNull.Value ? "" : Convert.ToString(odr_2["firstlyreveal"]),
                                odr_2["linkup"] == DBNull.Value ? "" : Convert.ToString(odr_2["linkup"]),
                                odr_2["edema"] == DBNull.Value ? "" : Convert.ToString(odr_2["edema"]),
                                odr_2["pro"] == DBNull.Value ? "" : Convert.ToString(odr_2["pro"]),
                                odr_2["hrscore"] == DBNull.Value ? "" : Convert.ToString(odr_2["hrscore"]),
                                odr_2["checkupdoctorname"] == DBNull.Value ? "" : Convert.ToString(odr_2["checkupdoctorname"]),
                                Convert.ToInt32(_dr["U_ID"])
                               ));
                            //插入数据
                            int z_num = SqlHelper.ExecteNonQuery(CommandType.Text, String.Format(@"insert into BigCardCheckInfo(Check_Date,YZ,XY,Weight,GG,TW,TX,XL,XJ,FZh,NDB,GWPF,Doctor,U_ID,FZ,XSS,Remark,HBZCL,Res_Date) values('{0}','{1}','{2}','{3}','{4}','{5}','{6}','{7}','{8}','{9}','{10}','{11}','{12}','{13}','','','','',GETDATE())",
                                ConvertTime(odr_2["checkupdate"] == DBNull.Value ? "" : Convert.ToString(odr_2["checkupdate"])),
                                odr_2["gagew"] == DBNull.Value ? "" : Convert.ToString(odr_2["gagew"]),
                                odr_2["bphigh"] == DBNull.Value ? "" : Convert.ToString(odr_2["bphigh"]),
                                odr_2["weight"] == DBNull.Value ? "" : Convert.ToString(odr_2["weight"]),
                                odr_2["fheight"] == DBNull.Value ? "" : Convert.ToString(odr_2["fheight"]),
                                odr_2["fposition"] == DBNull.Value ? "" : Convert.ToString(odr_2["fposition"]),
                                odr_2["fhrate"] == DBNull.Value ? "" : Convert.ToString(odr_2["fhrate"]),
                                odr_2["firstlyreveal"] == DBNull.Value ? "" : Convert.ToString(odr_2["firstlyreveal"]),
                                odr_2["linkup"] == DBNull.Value ? "" : Convert.ToString(odr_2["linkup"]),
                                odr_2["edema"] == DBNull.Value ? "" : Convert.ToString(odr_2["edema"]),
                                odr_2["pro"] == DBNull.Value ? "" : Convert.ToString(odr_2["pro"]),
                                odr_2["hrscore"] == DBNull.Value ? "" : Convert.ToString(odr_2["hrscore"]),
                                odr_2["checkupdoctorname"] == DBNull.Value ? "" : Convert.ToString(odr_2["checkupdoctorname"]),
                                Convert.ToInt32(_dr["U_ID"])
                                 ), null);
                            if (z_num == -1) //插入失败
                            {
                                LogInfo("大卡产检插入失败！");
                            }
                            else
                            { //插入成功
                                LogInfo("大卡产检插入成功！");
                            }
                        }
                    }
                }
            }
            catch (Exception e)
            {
                LogInfo("小卡产检第二次、第三次、大卡产检：" + e.Message);
            }
            #endregion
        }

        //分娩记录（母亲）（新生儿）
        protected void FMJL(DataRow _dr)
        {
            /* 插入分娩记录（母亲）（新生儿）、更新业务流程状态
             * 
             * 母亲：
             * 住院号（医院）(string) HospitalNum--inpartienno
             * 分娩日期(DateTime) DeliveryTime--deliverytime
             * 分娩孕周(string) GestationalWeeks--cgagew + cgaged
             * 分娩方式（0顺产 1剖宫产）(int) DeliveryType--modelivery
             * 分娩方式其它(方式为6时)(string) DeliveryOtherType--modeliveryoth
             * 产妇出院日期(DateTime) LeaveHospital--leavehospitaltime
             * 
             * 新生儿：
             * 性别 0 女 1男 2 其它(int) Sex--neosex
             * 胎数(0单1双2多胎）(int)Tirenumber--fetusnum
             * 出生体重(克)(double) Weight--bweight
             * Apgar评分(int) ApgarScore--apgarscore_10
             * 存活情况0活产1死胎2死产(int) Survival--birthoutcomes
             * 新生儿疾病筛查(0否，1是）(int) NeonateScreen--neoscreening
             * 筛查明细项（0 1 2）(string) ScreenDetail--neoscreeningyes
             * 新生儿出院时间(DateTime) LeaveHospitalTime--neoleavehospital
             * 
             */
            try
            {
                int i_num = 0;
                int j_num = 0;
                LogInfo("获取分娩记录母亲oracle：" + String.Format(@"select b.* from v_DMC_BIRTHRECORDS_M b where b.PCMCID='{0}'",
                    mcid
                    ));

                //根据身份证号分娩记录（母亲）
                DataTable dt = OracleHelper.GetDataTable(String.Format(@"select b.* from v_DMC_BIRTHRECORDS_M b where b.PCMCID='{0}'",
                    mcid
                    ));
                if (dt.Rows.Count > 0)
                {
                    DataRow odr = dt.Rows[0];
                    //插入分娩记录（母亲）
                    if (odr != null)
                    {
                        string GestationalWeeks = (odr["cgagew"] == DBNull.Value ? "" : Convert.ToString(odr["cgagew"])) + "+" + (odr["cgaged"] == DBNull.Value ? "" : Convert.ToString(odr["cgaged"]));

                        LogInfo("插入分娩记录母亲sqlserver：" + String.Format(@"insert into DeliveryRecord(HospitalNum,DeliveryTime,GestationalWeeks,DeliveryType,DeliveryOtherType,LeaveHospital,U_ID,DeliveryUrl,OperateUserId,OperateTime,DRState) values('{0}','{1}','{2}',{3},'{4}','{5}',{6},'',0,GETDATE(),1)",
                            odr["inpartienno"] == DBNull.Value ? "" : Convert.ToString(odr["inpartienno"]),
                            ConvertTime(odr["deliverytime"] == DBNull.Value ? "" : Convert.ToString(odr["deliverytime"])),
                            GestationalWeeks,
                            odr["modelivery"] == DBNull.Value ? 0 : Convert.ToInt32(odr["modelivery"]),
                            odr["modeliveryoth"] == DBNull.Value ? "" : Convert.ToString(odr["modeliveryoth"]),
                            ConvertTime(odr["leavehospitaltime"] == DBNull.Value ? "" : Convert.ToString(odr["leavehospitaltime"])),
                            Convert.ToInt32(_dr["U_ID"])
                            ));

                        i_num = SqlHelper.ExecteNonQuery(CommandType.Text, String.Format(@"insert into DeliveryRecord(HospitalNum,DeliveryTime,GestationalWeeks,DeliveryType,DeliveryOtherType,LeaveHospital,U_ID,DeliveryUrl,OperateUserId,OperateTime,DRState) values('{0}','{1}','{2}',{3},'{4}','{5}',{6},'',0,GETDATE(),1)",
                            odr["inpartienno"] == DBNull.Value ? "" : Convert.ToString(odr["inpartienno"]),
                            ConvertTime(odr["deliverytime"] == DBNull.Value ? "" : Convert.ToString(odr["deliverytime"])),
                            GestationalWeeks,
                            odr["modelivery"] == DBNull.Value ? 0 : Convert.ToInt32(odr["modelivery"]),
                            odr["modeliveryoth"] == DBNull.Value ? "" : Convert.ToString(odr["modeliveryoth"]),
                            ConvertTime(odr["leavehospitaltime"] == DBNull.Value ? "" : Convert.ToString(odr["leavehospitaltime"])),
                            Convert.ToInt32(_dr["U_ID"])
                            ), null);
                        if (i_num == -1) //插入失败
                        {
                            LogInfo("分娩记录（母亲）插入失败！");
                        }
                        else
                        { //插入成功
                            LogInfo("分娩记录（母亲）插入成功！");
                            //删除已写入的
                            SqlHelper.ExecteNonQuery(CommandType.Text, String.Format(@"delete from DeliveryRecord where DRState = 2 and U_ID={0}",
                                Convert.ToInt32(_dr["U_ID"])
                                ), null);
                        }
                    }
                }

                LogInfo("获取分娩记录新生儿oracle：" + String.Format(@"select b.* from  v_DMC_BIRTHRECORDS_C b where b.PCMCID='{0}'",
                    mcid
                    ));

                //根据身份证号分娩记录（新生儿)（可能有多条）
                DataTable dt_2 = OracleHelper.GetDataTable(String.Format(@"select b.* from  v_DMC_BIRTHRECORDS_C b where b.PCMCID='{0}'",
                    mcid
                    ));
                if (dt_2.Rows.Count > 0)
                {
                    for (int i = 0; i < dt_2.Rows.Count; i++)
                    {
                        DataRow odr_2 = dt_2.Rows[i];
                        if (odr_2 != null)
                        {
                            LogInfo("插入分娩记录新生儿sqlserver：" + String.Format(@"insert into NeonateRecord(Sex,Tirenumber,Weight,ApgarScore,Survival,NeonateScreen,ScreenDetail,LeaveHospitalTime,U_ID,OperateUserId,OperateTime,NRState) values({0},{1},{2},{3},{4},{5},'{6}',{7},{8},0,GETDATE(),1)",
                                odr_2["neosex"] == DBNull.Value ? 0 : Convert.ToInt32(odr_2["neosex"]),
                                odr_2["fetusnum"] == DBNull.Value ? 0 : Convert.ToInt32(odr_2["fetusnum"]),
                                odr_2["bweight"] == DBNull.Value ? 0 : Convert.ToDouble(odr_2["bweight"]),
                                odr_2["apgarscore_10"] == DBNull.Value ? 0 : Convert.ToInt32(odr_2["apgarscore_10"]),
                                odr_2["birthoutcomes"] == DBNull.Value ? 0 : Convert.ToInt32(odr_2["birthoutcomes"]),
                                odr_2["neoscreening"] == DBNull.Value ? 0 : Convert.ToInt32(odr_2["neoscreening"]),
                                odr_2["neoscreeningyes"] == DBNull.Value ? "" : Convert.ToString(odr_2["neoscreeningyes"]),
                                ConvertTime(odr_2["neoleavehospital"] == DBNull.Value ? "" : Convert.ToString(odr_2["neoleavehospital"])),
                                Convert.ToInt32(_dr["U_ID"])
                                ));
                            //插入分娩记录（新生儿）
                            j_num = SqlHelper.ExecteNonQuery(CommandType.Text, String.Format(@"insert into NeonateRecord(Sex,Tirenumber,Weight,ApgarScore,Survival,NeonateScreen,ScreenDetail,LeaveHospitalTime,U_ID,OperateUserId,OperateTime,NRState) values({0},{1},{2},{3},{4},{5},'{6}',{7},{8},0,GETDATE(),1)",
                                odr_2["neosex"] == DBNull.Value ? 0 : Convert.ToInt32(odr_2["neosex"]),
                                odr_2["fetusnum"] == DBNull.Value ? 0 : Convert.ToInt32(odr_2["fetusnum"]),
                                odr_2["bweight"] == DBNull.Value ? 0 : Convert.ToDouble(odr_2["bweight"]),
                                odr_2["apgarscore_10"] == DBNull.Value ? 0 : Convert.ToInt32(odr_2["apgarscore_10"]),
                                odr_2["birthoutcomes"] == DBNull.Value ? 0 : Convert.ToInt32(odr_2["birthoutcomes"]),
                                odr_2["neoscreening"] == DBNull.Value ? 0 : Convert.ToInt32(odr_2["neoscreening"]),
                                odr_2["neoscreeningyes"] == DBNull.Value ? "" : Convert.ToString(odr_2["neoscreeningyes"]),
                                ConvertTime(odr_2["neoleavehospital"] == DBNull.Value ? "" : Convert.ToString(odr_2["neoleavehospital"])),
                                Convert.ToInt32(_dr["U_ID"])
                                ), null);
                            if (j_num == -1) //插入失败
                            {
                                LogInfo("分娩记录（新生儿）插入失败！");
                            }
                            else
                            { //插入成功
                                LogInfo("分娩记录（新生儿）插入成功！");
                                if (i == 0)
                                {
                                    //删除已写入的
                                    SqlHelper.ExecteNonQuery(CommandType.Text, String.Format(@"delete from NeonateRecord where NRState = 2 and U_ID={0}",
                                        Convert.ToInt32(_dr["U_ID"])
                                        ), null);
                                }
                            }
                        }
                    }
                }

                if (i_num > 0 && j_num > 0)
                {
                    //更新流程表
                    int z_num = SqlHelper.ExecteNonQuery(CommandType.Text, String.Format(@"update ProcessState set DeliveryState = 1 where ID={0}", Convert.ToInt32(_dr["ID"])), null);
                    if (z_num == -1) //更新失败
                    {
                        LogInfo("流程表（分娩记录（母亲）（新生儿））更新失败！");
                    }
                    else  //更新成功
                    {
                        LogInfo("流程表（分娩记录（母亲）（新生儿））更新成功！");
                    }
                }
            }
            catch (Exception e)
            {
                LogInfo("分娩记录（母亲）（新生儿）：" + e.Message);
            }
        }

        //修养接收
        protected void XYJS(DataRow _dr) 
        {
            try
            {
                /* 插入修养接收、更新业务流程状态
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
                LogInfo("获取修养接收oracle：" + String.Format(@"select b.* from vw_fi_pregwom_returnfile b where b.JCQX='{0}'",
                       mcid
                       ));
                //根据身份证号修养接收
                DataTable dt = OracleHelper.GetDataTable(String.Format(@"select b.* from vw_fi_pregwom_returnfile b where b.JCQX='{0}'",
                    mcid
                    ));
                if (dt.Rows.Count > 0)
                {
                    DataRow odr = dt.Rows[0];
                    if (odr != null)
                    {
                        LogInfo("插入修养接收sqlserver：" + String.Format(@"insert into RestReceive(RestHospital,FanShiTime,Photo1,Photo2,U_ID,State,OperateTime,OperateUserId,JKBH,XM,CSRQ,NL,JCDW,CHXYQX,XZZ,CHXYDZ,YEXB,CSTZ,FMRQ,ZFXM,LXDH,JCYY,CYRQ,CKJD) values(isnull((select top 1 Hos_Code from Hospital_Info where DicCode='{0}'),0),'1970-01-01','','',{1},1,GETDATE(),0,'{2}','{3}','{4}',{5},'{6}','{7}','{8}','{9}','{10}','{11}','{12}','{13}','{14}','{15}','{16}','{17}')",
                            odr["CHXYQX"] == DBNull.Value ? "" : Convert.ToString(odr["CHXYQX"]),
                            Convert.ToInt32(_dr["U_ID"]),
                            odr["JCQX"] == DBNull.Value ? "" : Convert.ToString(odr["JCQX"]),
                            odr["XM"] == DBNull.Value ? "" : Convert.ToString(odr["XM"]),
                            ConvertTime(odr["CSRQ"] == DBNull.Value ? "" : Convert.ToString(odr["CSRQ"])),
                            odr["NL"] == DBNull.Value ? 0 : Convert.ToInt32(odr["NL"]),
                            odr["JCDW"] == DBNull.Value ? "" : Convert.ToString(odr["JCDW"]),
                            odr["CHXYQX"] == DBNull.Value ? "" : Convert.ToString(odr["CHXYQX"]),
                            odr["XZZ"] == DBNull.Value ? "" : Convert.ToString(odr["XZZ"]),
                            odr["CHXYDZ"] == DBNull.Value ? "" : Convert.ToString(odr["CHXYDZ"]),
                            odr["YEXB"] == DBNull.Value ? "" : Convert.ToString(odr["YEXB"]),
                            odr["CSTZ"] == DBNull.Value ? "" : Convert.ToString(odr["CSTZ"]),
                            ConvertTime(odr["FMRQ"] == DBNull.Value ? "" : Convert.ToString(odr["FMRQ"])),
                            odr["ZFXM"] == DBNull.Value ? "" : Convert.ToString(odr["ZFXM"]),
                            odr["LXDH"] == DBNull.Value ? "" : Convert.ToString(odr["LXDH"]),
                            odr["JCYY"] == DBNull.Value ? "" : Convert.ToString(odr["JCYY"]),
                            ConvertTime(odr["CYRQ"] == DBNull.Value ? "" : Convert.ToString(odr["CYRQ"])),
                            odr["CKJD"] == DBNull.Value ? "" : Convert.ToString(odr["CKJD"])
                            ));
                        //插入修养接收
                        int i = SqlHelper.ExecteNonQuery(CommandType.Text, String.Format(@"insert into RestReceive(RestHospital,FanShiTime,Photo1,Photo2,U_ID,State,OperateTime,OperateUserId,JKBH,XM,CSRQ,NL,JCDW,CHXYQX,XZZ,CHXYDZ,YEXB,CSTZ,FMRQ,ZFXM,LXDH,JCYY,CYRQ,CKJD) values(isnull((select top 1 Hos_Code from Hospital_Info where DicCode='{0}'),0),'1970-01-01','','',{1},1,GETDATE(),0,'{2}','{3}','{4}',{5},'{6}','{7}','{8}','{9}','{10}','{11}','{12}','{13}','{14}','{15}','{16}','{17}')",
                            odr["CHXYQX"] == DBNull.Value ? "" : Convert.ToString(odr["CHXYQX"]),
                            Convert.ToInt32(_dr["U_ID"]),
                            odr["JCQX"] == DBNull.Value ? "" : Convert.ToString(odr["JCQX"]),
                            odr["XM"] == DBNull.Value ? "" : Convert.ToString(odr["XM"]),
                            ConvertTime(odr["CSRQ"] == DBNull.Value ? "" : Convert.ToString(odr["CSRQ"])),
                            odr["NL"] == DBNull.Value ? 0 : Convert.ToInt32(odr["NL"]),
                            odr["JCDW"] == DBNull.Value ? "" : Convert.ToString(odr["JCDW"]),
                            odr["CHXYQX"] == DBNull.Value ? "" : Convert.ToString(odr["CHXYQX"]),
                            odr["XZZ"] == DBNull.Value ? "" : Convert.ToString(odr["XZZ"]),
                            odr["CHXYDZ"] == DBNull.Value ? "" : Convert.ToString(odr["CHXYDZ"]),
                            odr["YEXB"] == DBNull.Value ? "" : Convert.ToString(odr["YEXB"]),
                            odr["CSTZ"] == DBNull.Value ? "" : Convert.ToString(odr["CSTZ"]),
                            ConvertTime(odr["FMRQ"] == DBNull.Value ? "" : Convert.ToString(odr["FMRQ"])),
                            odr["ZFXM"] == DBNull.Value ? "" : Convert.ToString(odr["ZFXM"]),
                            odr["LXDH"] == DBNull.Value ? "" : Convert.ToString(odr["LXDH"]),
                            odr["JCYY"] == DBNull.Value ? "" : Convert.ToString(odr["JCYY"]),
                            ConvertTime(odr["CYRQ"] == DBNull.Value ? "" : Convert.ToString(odr["CYRQ"])),
                            odr["CKJD"] == DBNull.Value ? "" : Convert.ToString(odr["CKJD"])
                            ), null);
                        if (i == -1) //插入失败 
                        {
                            LogInfo("插入修养接收失败");
                        }
                        else //插入成功 
                        {
                            LogInfo("插入修养接收成功");

                            //删除已写入的
                            SqlHelper.ExecteNonQuery(CommandType.Text, String.Format(@"delete from RestReceive where State = 2 and U_ID={0}",
                                Convert.ToInt32(_dr["U_ID"])
                                ), null);

                            //更新流程表
                            int z_num = SqlHelper.ExecteNonQuery(CommandType.Text, String.Format(@"update ProcessState set RestReceiveState = 1 where ID={0}", Convert.ToInt32(_dr["ID"])), null);
                            if (z_num == -1) //更新失败
                            {
                                LogInfo("流程表（修养接收）更新失败！");
                            }
                            else  //更新成功
                            {
                                LogInfo("流程表（修养接收）更新成功！");
                            }
                        }
                    }
                }
            }
            catch (Exception e) {
                LogInfo("修养接收：" + e.Message);
            }
        }

        //产后访视（第一次）
        protected void CHFS_1(DataRow _dr)
        {
            /* 插入产后访视、更新业务流程状态
             * 访视日期(DateTime) VisitTime--visitdate
             * 产后第几天(int) PostpartumDay--afterbirthday
             * 体温(double) Temperature--btemperature --
             * 一般健康情况(string) Healthy--generalhealth
             * 一般心理情况(string) Psychologic--generalpsychology
             * 血压(string) Blood--diaspressure --
             * 乳房(int) Breast--breast
             * 乳房其它(string) BreastOther--breasto
             * 恶露(int) Lochia--lochia
             * 子宫(int) Uterus--uterus
             * 子宫其它(string) UterusOther--uteruso
             * 伤口(int) Wound--wound
             * 伤口其它(string) WoundOther--woundo
             * 其它(string) Other--othersituation
             * 分类(int) Classify--classification
             * 分类其它(string) ClassifyOther--classificationo
             * 指导(string) Guide--tgopinion
             * 指导其他(string) GuideOther--tgopiniono
             * 转诊(int) Referral--referral
             * 转诊原因(string) ReferralOther--referralreason
             * 下次随访日期(DateTime) NextVisitTime--nextvisitdate
             * 随访医生签名(string) Doctor--checkupdoctor
             */
            try
            {
                int i_num = 0;
                int j_num = 0;

                LogInfo("获取产后访视母亲（第一次）oracle：" + String.Format(@"select b.* from  v_DMC_PARTUMVISIT b where b.maternalmcid='{0}' order by b.update_date asc",
                    mcid
                    ));

                //根据身份证号产后访视（母亲）
                DataTable dt = OracleHelper.GetDataTable(String.Format(@"select b.* from  v_DMC_PARTUMVISIT b where b.maternalmcid='{0}' order by b.update_date asc",
                    mcid
                    ));
                if (dt.Rows.Count > 0)
                {
                    DataRow odr = dt.Rows[0];
                    //插入产后访视（母亲）
                    if (odr != null)
                    {
                        LogInfo("插入产后访视母亲（第一次）sqlserver：" + String.Format(@"insert into PostpartumVisit(VisitTime,PostpartumDay,Temperature,Healthy,Psychologic,Blood,Breast,BreastOther,Lochia,Uterus,UterusOther,Wound,WoundOther,Other,Classify,ClassifyOther,Guide,GuideOther,Referral,ReferralOther,NextVisitTime,Doctor,U_ID,LochiaOther,DoctorCode,OperateUserId,OperateTime,PVisitState) values('{0}',{1},{2},'{3}','{4}','{5}',{6},'{7}',{8},{9},'{10}',{11},'{12}','{13}',{14},'{15}','{16}','{17}',{18},'{19}','{20}','{21}',{22},'',0,0,GETDATE(),1)",
                            ConvertTime(odr["visitdate"] == DBNull.Value ? "" : Convert.ToString(odr["visitdate"])),
                            odr["afterbirthday"] == DBNull.Value ? 0 : Convert.ToInt32(odr["afterbirthday"]),
                            odr["btemperature"] == DBNull.Value ? 0 : Convert.ToDouble(odr["btemperature"]),
                            odr["generalhealth"] == DBNull.Value ? "" : Convert.ToString(odr["generalhealth"]),
                            odr["generalpsychology"] == DBNull.Value ? "" : Convert.ToString(odr["generalpsychology"]),
                            odr["diaspressure"] == DBNull.Value ? "" : Convert.ToString(odr["diaspressure"]),
                            odr["breast"] == DBNull.Value ? 0 : Convert.ToInt32(odr["breast"]),
                            odr["breasto"] == DBNull.Value ? "" : Convert.ToString(odr["breasto"]),
                            odr["lochia"] == DBNull.Value ? 0 : Convert.ToInt32(odr["lochia"]),
                            odr["uterus"] == DBNull.Value ? 0 : Convert.ToInt32(odr["uterus"]),
                            odr["uteruso"] == DBNull.Value ? "" : Convert.ToString(odr["uteruso"]),
                            odr["wound"] == DBNull.Value ? 0 : Convert.ToInt32(odr["wound"]),
                            odr["woundo"] == DBNull.Value ? "" : Convert.ToString(odr["woundo"]),
                            odr["othersituation"] == DBNull.Value ? "" : Convert.ToString(odr["othersituation"]),
                            odr["classification"] == DBNull.Value ? 0 : Convert.ToInt32(odr["classification"]),
                            odr["classificationo"] == DBNull.Value ? "" : Convert.ToString(odr["classificationo"]),
                            odr["tgopinion"] == DBNull.Value ? "" : Convert.ToString(odr["tgopinion"]),
                            odr["tgopiniono"] == DBNull.Value ? "" : Convert.ToString(odr["tgopiniono"]),
                            odr["referral"] == DBNull.Value ? 0 : Convert.ToInt32(odr["referral"]),
                            odr["referralreason"] == DBNull.Value ? "" : Convert.ToString(odr["referralreason"]),
                            ConvertTime(odr["nextvisitdate"] == DBNull.Value ? "" : Convert.ToString(odr["nextvisitdate"])),
                            odr["checkupdoctor"] == DBNull.Value ? "" : Convert.ToString(odr["checkupdoctor"]),
                            _dr["U_ID"] == DBNull.Value ? 0 : Convert.ToInt32(_dr["U_ID"])
                            ));

                        i_num = SqlHelper.ExecteNonQuery(CommandType.Text, String.Format(@"insert into PostpartumVisit(VisitTime,PostpartumDay,Temperature,Healthy,Psychologic,Blood,Breast,BreastOther,Lochia,Uterus,UterusOther,Wound,WoundOther,Other,Classify,ClassifyOther,Guide,GuideOther,Referral,ReferralOther,NextVisitTime,Doctor,U_ID,LochiaOther,DoctorCode,OperateUserId,OperateTime,PVisitState) values('{0}',{1},{2},'{3}','{4}','{5}',{6},'{7}',{8},{9},'{10}',{11},'{12}','{13}',{14},'{15}','{16}','{17}',{18},'{19}','{20}','{21}',{22},'',0,0,GETDATE(),1)",
                            ConvertTime(odr["visitdate"] == DBNull.Value ? "" : Convert.ToString(odr["visitdate"])),
                            odr["afterbirthday"] == DBNull.Value ? 0 : Convert.ToInt32(odr["afterbirthday"]),
                            odr["btemperature"] == DBNull.Value ? 0 : Convert.ToDouble(odr["btemperature"]),
                            odr["generalhealth"] == DBNull.Value ? "" : Convert.ToString(odr["generalhealth"]),
                            odr["generalpsychology"] == DBNull.Value ? "" : Convert.ToString(odr["generalpsychology"]),
                            odr["diaspressure"] == DBNull.Value ? "" : Convert.ToString(odr["diaspressure"]),
                            odr["breast"] == DBNull.Value ? 0 : Convert.ToInt32(odr["breast"]),
                            odr["breasto"] == DBNull.Value ? "" : Convert.ToString(odr["breasto"]),
                            odr["lochia"] == DBNull.Value ? 0 : Convert.ToInt32(odr["lochia"]),
                            odr["uterus"] == DBNull.Value ? 0 : Convert.ToInt32(odr["uterus"]),
                            odr["uteruso"] == DBNull.Value ? "" : Convert.ToString(odr["uteruso"]),
                            odr["wound"] == DBNull.Value ? 0 : Convert.ToInt32(odr["wound"]),
                            odr["woundo"] == DBNull.Value ? "" : Convert.ToString(odr["woundo"]),
                            odr["othersituation"] == DBNull.Value ? "" : Convert.ToString(odr["othersituation"]),
                            odr["classification"] == DBNull.Value ? 0 : Convert.ToInt32(odr["classification"]),
                            odr["classificationo"] == DBNull.Value ? "" : Convert.ToString(odr["classificationo"]),
                            odr["tgopinion"] == DBNull.Value ? "" : Convert.ToString(odr["tgopinion"]),
                            odr["tgopiniono"] == DBNull.Value ? "" : Convert.ToString(odr["tgopiniono"]),
                            odr["referral"] == DBNull.Value ? 0 : Convert.ToInt32(odr["referral"]),
                            odr["referralreason"] == DBNull.Value ? "" : Convert.ToString(odr["referralreason"]),
                            ConvertTime(odr["nextvisitdate"] == DBNull.Value ? "" : Convert.ToString(odr["nextvisitdate"])),
                            odr["checkupdoctor"] == DBNull.Value ? "" : Convert.ToString(odr["checkupdoctor"]),
                            _dr["U_ID"] == DBNull.Value ? 0 : Convert.ToInt32(_dr["U_ID"])
                            ), null);

                        if (i_num == -1) //插入失败
                        {
                            LogInfo("产后访视（母亲）（第一次）插入失败！");
                        }
                        else
                        { //插入成功
                            LogInfo("产后访视（母亲）（第一次）插入成功！");

                            //更新上个流程大卡产检
                            int x_num = SqlHelper.ExecteNonQuery(CommandType.Text, String.Format(@"update ProcessState set BigCardCheckState = 1 where ID={0}", Convert.ToInt32(_dr["ID"])), null);
                            if (x_num > 0) //更新成功
                            {
                                LogInfo("流程表（大卡产检）更新成功！");
                            }
                            //更新小卡第一次
                            int p_num = SqlHelper.ExecteNonQuery(CommandType.Text, String.Format(@"update ProcessState set CheckCardState1 = 1 where ID={0} and CheckCardState1 = 0", Convert.ToInt32(_dr["ID"])), null);
                            if (p_num > 0) //更新成功
                            {
                                LogInfo("流程表小卡产检第一次更新成功！");
                            }
                            //更新小卡第二次
                            int w_num = SqlHelper.ExecteNonQuery(CommandType.Text, String.Format(@"update ProcessState set CheckCardState2 = 1 where ID={0} and CheckCardState2 = 0", Convert.ToInt32(_dr["ID"])), null);
                            if (w_num > 0) //更新成功
                            {
                                LogInfo("流程表小卡产检第二次更新成功！");
                            }
                            //更新小卡第三次
                            int q_num = SqlHelper.ExecteNonQuery(CommandType.Text, String.Format(@"update ProcessState set CheckCardState3 = 1 where ID={0} and CheckCardState3 = 0", Convert.ToInt32(_dr["ID"])), null);
                            if (q_num > 0) //更新成功
                            {
                                LogInfo("流程表小卡产检第三次更新成功！");
                            }

                            //删除已写入的
                            SqlHelper.ExecteNonQuery(CommandType.Text, String.Format(@"delete from PostpartumVisit where ID in (select top 1 ID from PostpartumVisit where PVisitState = 2 and U_ID={0} order by OperateTime asc)",
                                Convert.ToInt32(_dr["U_ID"])
                                ), null);
                        }
                    }
                }

                LogInfo("获取产后访视新生儿（第一次）oracle：" + String.Format(@"select aa.* from (select b.* from  v_dmc_newbornvisit b where b.MATERNALMCID='{0}' order by b.update_date asc) aa where rownum <=1",
                    _dr["U_CardID"].ToString()
                   ));

                //根据身份证号产后访视（新生儿）
                DataTable dt_2 = OracleHelper.GetDataTable(String.Format(@"select aa.* from (select b.* from  v_dmc_newbornvisit b where b.MATERNALMCID='{0}' order by b.update_date asc) aa where rownum <=1",
                    _dr["U_CardID"].ToString()
                    ));
                if (dt_2.Rows.Count > 0)
                {
                    DataRow odr_2 = dt_2.Rows[0];
                    //插入产后访视（新生儿）
                    if (odr_2 != null)
                    {
                        LogInfo("插入产后访视新生儿（第一次）sqlserver：" + String.Format(@"insert into PostpartumVisitChild(VisitTime,Temperature,FeedingMode,Skin,Weight,VisitDoctor,U_ID,Navel,YanJing,Thrush,DBNum,DBS,DBYJ,Handle,VisitDoctorCode,OperateUserId,OperateTime,PVisitChildState) values('{0}',{1},'{2}','{3}',{4},'{5}',{6},'','','',0,'','','',0,0,GETDATE(),1)",
                            ConvertTime(odr_2["VISITDATE"] == DBNull.Value ? "" : Convert.ToString(odr_2["VISITDATE"])),
                            odr_2["btemperature"] == DBNull.Value ? 0 : Convert.ToDouble(odr_2["btemperature"]),
                            odr_2["FEEDING"] == DBNull.Value ? "" : Convert.ToString(odr_2["FEEDING"]),
                            odr_2["SKINO"] == DBNull.Value ? "" : Convert.ToString(odr_2["SKINO"]),
                            odr_2["WEIGHT"] == DBNull.Value ? 0 : Convert.ToDouble(odr_2["WEIGHT"]),
                            odr_2["CHECKUPDOCTOR"] == DBNull.Value ? "" : Convert.ToString(odr_2["CHECKUPDOCTOR"]),
                            Convert.ToInt32(_dr["U_ID"])
                            ));

                        j_num = SqlHelper.ExecteNonQuery(CommandType.Text, String.Format(@"insert into PostpartumVisitChild(VisitTime,Temperature,FeedingMode,Skin,Weight,VisitDoctor,U_ID,Navel,YanJing,Thrush,DBNum,DBS,DBYJ,Handle,VisitDoctorCode,OperateUserId,OperateTime,PVisitChildState) values('{0}',{1},'{2}','{3}',{4},'{5}',{6},'','','',0,'','','',0,0,GETDATE(),1)",
                            ConvertTime(odr_2["VISITDATE"] == DBNull.Value ? "" : Convert.ToString(odr_2["VISITDATE"])),
                            odr_2["btemperature"] == DBNull.Value ? 0 : Convert.ToDouble(odr_2["btemperature"]),
                            odr_2["FEEDING"] == DBNull.Value ? "" : Convert.ToString(odr_2["FEEDING"]),
                            odr_2["SKINO"] == DBNull.Value ? "" : Convert.ToString(odr_2["SKINO"]),
                            odr_2["WEIGHT"] == DBNull.Value ? 0 : Convert.ToDouble(odr_2["WEIGHT"]),
                            odr_2["CHECKUPDOCTOR"] == DBNull.Value ? "" : Convert.ToString(odr_2["CHECKUPDOCTOR"]),
                            Convert.ToInt32(_dr["U_ID"])
                            ), null);
                        if (j_num == -1) //插入失败
                        {
                            LogInfo("产后访视（新生儿）（第一次）插入失败！");
                        }
                        else
                        { //插入成功
                            LogInfo("产后访视（新生儿）（第一次）插入成功！");
                            //删除已写入的
                            SqlHelper.ExecteNonQuery(CommandType.Text, String.Format(@"delete from PostpartumVisitChild where ID in (select top 1 ID from PostpartumVisitChild where PVisitChildState = 2 and U_ID={0} order by OperateTime asc)",
                                Convert.ToInt32(_dr["U_ID"])
                                ), null);
                        }
                    }
                }

                if (i_num > 0 || j_num > 0)
                {
                    //更新流程表
                    int z_num = SqlHelper.ExecteNonQuery(CommandType.Text, String.Format(@"update ProcessState set PostpartumVisitState1 = 1 where ID={0}", Convert.ToInt32(_dr["ID"])), null);
                    if (z_num == -1) //更新失败
                    {
                        LogInfo("流程表（产后访视（第一次））更新失败！");
                    }
                    else
                    { //更新成功
                        LogInfo("流程表（产后访视（第一次））更新成功！");
                    }
                }
            }
            catch (Exception e)
            {
                LogInfo("产后访视第一次：" + e.Message);
            }
        }

        //产后访视（第二次）
        protected void CHFS_2(DataRow _dr)
        {
            /* 插入产后访视、更新业务流程状态
              * 访视日期(DateTime) VisitTime--visitdate
              * 产后第几天(int) PostpartumDay--afterbirthday
              * 体温(double) Temperature--btemperature --
              * 一般健康情况(string) Healthy--generalhealth
              * 一般心理情况(string) Psychologic--generalpsychology
              * 血压(string) Blood--diaspressure --
              * 乳房(int) Breast--breast
              * 乳房其它(string) BreastOther--breasto
              * 恶露(int) Lochia--lochia
              * 子宫(int) Uterus--uterus
              * 子宫其它(string) UterusOther--uteruso
              * 伤口(int) Wound--wound
              * 伤口其它(string) WoundOther--woundo
              * 其它(string) Other--othersituation
              * 分类(int) Classify--classification
              * 分类其它(string) ClassifyOther--classificationo
              * 指导(string) Guide--tgopinion
              * 指导其他(string) GuideOther--tgopiniono
              * 转诊(int) Referral--referral
              * 转诊原因(string) ReferralOther--referralreason
              * 下次随访日期(DateTime) NextVisitTime--nextvisitdate
              * 随访医生签名(string) Doctor--checkupdoctor
              */
            try
            {
                int i_num = 0;
                //int j_num = 0;

                LogInfo("获取产后访视母亲（第二次）oracle：" + String.Format(@"select * from (select rownum hh,w.* from (select b.* from  v_DMC_PARTUMVISIT b where b.maternalmcid='{0}' order by b.update_date asc) w) ww where ww.hh = 2",
                    mcid
                    ));

                //根据身份证号产后访视（母亲）
                DataTable dt = OracleHelper.GetDataTable(String.Format(@"select * from (select rownum hh,w.* from (select b.* from  v_DMC_PARTUMVISIT b where b.maternalmcid='{0}' order by b.update_date asc) w) ww where ww.hh = 2",
                    mcid
                    ));
                if (dt.Rows.Count > 0)
                {
                    DataRow odr = dt.Rows[0];
                    //插入产后访视（母亲）
                    if (odr != null)
                    {
                        LogInfo("插入产后访视母亲（第二次）sqlserver：" + String.Format(@"insert into PostpartumVisit(VisitTime,PostpartumDay,Temperature,Healthy,Psychologic,Blood,Breast,BreastOther,Lochia,Uterus,UterusOther,Wound,WoundOther,Other,Classify,ClassifyOther,Guide,GuideOther,Referral,ReferralOther,NextVisitTime,Doctor,U_ID,LochiaOther,DoctorCode,OperateUserId,OperateTime,PVisitState) values('{0}',{1},{2},'{3}','{4}','{5}',{6},'{7}',{8},{9},'{10}',{11},'{12}','{13}',{14},'{15}','{16}','{17}',{18},'{19}','{20}','{21}',{22},'',0,0,GETDATE(),1)",
                            ConvertTime(odr["visitdate"] == DBNull.Value ? "" : Convert.ToString(odr["visitdate"])),
                            odr["afterbirthday"] == DBNull.Value ? 0 : Convert.ToInt32(odr["afterbirthday"]),
                            odr["btemperature"] == DBNull.Value ? 0 : Convert.ToDouble(odr["btemperature"]),
                            odr["generalhealth"] == DBNull.Value ? "" : Convert.ToString(odr["generalhealth"]),
                            odr["generalpsychology"] == DBNull.Value ? "" : Convert.ToString(odr["generalpsychology"]),
                            odr["diaspressure"] == DBNull.Value ? "" : Convert.ToString(odr["diaspressure"]),
                            odr["breast"] == DBNull.Value ? 0 : Convert.ToInt32(odr["breast"]),
                            odr["breasto"] == DBNull.Value ? "" : Convert.ToString(odr["breasto"]),
                            odr["lochia"] == DBNull.Value ? 0 : Convert.ToInt32(odr["lochia"]),
                            odr["uterus"] == DBNull.Value ? 0 : Convert.ToInt32(odr["uterus"]),
                            odr["uteruso"] == DBNull.Value ? "" : Convert.ToString(odr["uteruso"]),
                            odr["wound"] == DBNull.Value ? 0 : Convert.ToInt32(odr["wound"]),
                            odr["woundo"] == DBNull.Value ? "" : Convert.ToString(odr["woundo"]),
                            odr["othersituation"] == DBNull.Value ? "" : Convert.ToString(odr["othersituation"]),
                            odr["classification"] == DBNull.Value ? 0 : Convert.ToInt32(odr["classification"]),
                            odr["classificationo"] == DBNull.Value ? "" : Convert.ToString(odr["classificationo"]),
                            odr["tgopinion"] == DBNull.Value ? "" : Convert.ToString(odr["tgopinion"]),
                            odr["tgopiniono"] == DBNull.Value ? "" : Convert.ToString(odr["tgopiniono"]),
                            odr["referral"] == DBNull.Value ? 0 : Convert.ToInt32(odr["referral"]),
                            odr["referralreason"] == DBNull.Value ? "" : Convert.ToString(odr["referralreason"]),
                            ConvertTime(odr["nextvisitdate"] == DBNull.Value ? "" : Convert.ToString(odr["nextvisitdate"])),
                            odr["checkupdoctor"] == DBNull.Value ? "" : Convert.ToString(odr["checkupdoctor"]),
                            _dr["U_ID"] == DBNull.Value ? 0 : Convert.ToInt32(_dr["U_ID"])
                            ));

                        i_num = SqlHelper.ExecteNonQuery(CommandType.Text, String.Format(@"insert into PostpartumVisit(VisitTime,PostpartumDay,Temperature,Healthy,Psychologic,Blood,Breast,BreastOther,Lochia,Uterus,UterusOther,Wound,WoundOther,Other,Classify,ClassifyOther,Guide,GuideOther,Referral,ReferralOther,NextVisitTime,Doctor,U_ID,LochiaOther,DoctorCode,OperateUserId,OperateTime,PVisitState) values('{0}',{1},{2},'{3}','{4}','{5}',{6},'{7}',{8},{9},'{10}',{11},'{12}','{13}',{14},'{15}','{16}','{17}',{18},'{19}','{20}','{21}',{22},'',0,0,GETDATE(),1)",
                            ConvertTime(odr["visitdate"] == DBNull.Value ? "" : Convert.ToString(odr["visitdate"])),
                            odr["afterbirthday"] == DBNull.Value ? 0 : Convert.ToInt32(odr["afterbirthday"]),
                            odr["btemperature"] == DBNull.Value ? 0 : Convert.ToDouble(odr["btemperature"]),
                            odr["generalhealth"] == DBNull.Value ? "" : Convert.ToString(odr["generalhealth"]),
                            odr["generalpsychology"] == DBNull.Value ? "" : Convert.ToString(odr["generalpsychology"]),
                            odr["diaspressure"] == DBNull.Value ? "" : Convert.ToString(odr["diaspressure"]),
                            odr["breast"] == DBNull.Value ? 0 : Convert.ToInt32(odr["breast"]),
                            odr["breasto"] == DBNull.Value ? "" : Convert.ToString(odr["breasto"]),
                            odr["lochia"] == DBNull.Value ? 0 : Convert.ToInt32(odr["lochia"]),
                            odr["uterus"] == DBNull.Value ? 0 : Convert.ToInt32(odr["uterus"]),
                            odr["uteruso"] == DBNull.Value ? "" : Convert.ToString(odr["uteruso"]),
                            odr["wound"] == DBNull.Value ? 0 : Convert.ToInt32(odr["wound"]),
                            odr["woundo"] == DBNull.Value ? "" : Convert.ToString(odr["woundo"]),
                            odr["othersituation"] == DBNull.Value ? "" : Convert.ToString(odr["othersituation"]),
                            odr["classification"] == DBNull.Value ? 0 : Convert.ToInt32(odr["classification"]),
                            odr["classificationo"] == DBNull.Value ? "" : Convert.ToString(odr["classificationo"]),
                            odr["tgopinion"] == DBNull.Value ? "" : Convert.ToString(odr["tgopinion"]),
                            odr["tgopiniono"] == DBNull.Value ? "" : Convert.ToString(odr["tgopiniono"]),
                            odr["referral"] == DBNull.Value ? 0 : Convert.ToInt32(odr["referral"]),
                            odr["referralreason"] == DBNull.Value ? "" : Convert.ToString(odr["referralreason"]),
                            ConvertTime(odr["nextvisitdate"] == DBNull.Value ? "" : Convert.ToString(odr["nextvisitdate"])),
                            odr["checkupdoctor"] == DBNull.Value ? "" : Convert.ToString(odr["checkupdoctor"]),
                            _dr["U_ID"] == DBNull.Value ? 0 : Convert.ToInt32(_dr["U_ID"])
                            ), null);

                        if (i_num == -1) //插入失败
                        {
                            LogInfo("产后访视（母亲）（第二次）插入失败！");
                        }
                        else
                        { //插入成功
                            LogInfo("产后访视（母亲）（第二次）插入成功！");
                            //删除已写入的
                            SqlHelper.ExecteNonQuery(CommandType.Text, String.Format(@"delete from PostpartumVisit where ID in (select top 1 ID from PostpartumVisit where PVisitState = 2 and U_ID={0} order by OperateTime desc)",
                                Convert.ToInt32(_dr["U_ID"])
                                ), null);
                        }
                    }
                }

                /*
                LogInfo("获取产后访视新生儿（第二次）oracle：" + String.Format(@"select * from (select rownum hh,aa.*, nvl( (select CodeName from ggws.code_compare where codetype = 'WYFS' and Codevalue=aa.FEEDING),'') as FEEDING_1 from (select b.* from v_dmc_newbornvisit b where b.MATERNALMCID='{0}' order by b.update_date asc) aa) aaa where aaa.hh = 2",
                    mcid
                   ));

                //根据身份证号产后访视（新生儿）
                DataTable dt_2 = OracleHelper.GetDataTable(String.Format(@"select * from (select rownum hh,aa.*, nvl( (select CodeName from ggws.code_compare where codetype = 'WYFS' and Codevalue=aa.FEEDING),'') as FEEDING_1 from (select b.* from v_dmc_newbornvisit b where b.MATERNALMCID='{0}' order by b.update_date asc) aa) aaa where aaa.hh = 2",
                    mcid
                    ));
                if (dt_2.Rows.Count > 0)
                {
                    DataRow odr_2 = dt_2.Rows[0];
                    //插入产后访视（新生儿）
                    if (odr_2 != null)
                    {
                        LogInfo("插入产后访视新生儿（第二次）sqlserver：" + String.Format(@"insert into PostpartumVisitChild(VisitTime,Temperature,FeedingMode,Skin,Weight,VisitDoctor,U_ID,Navel,YanJing,Thrush,DBNum,DBS,DBYJ,Handle,VisitDoctorCode,OperateUserId,OperateTime,PVisitChildState) values('{0}',{1},'{2}','{3}',{4},'{5}',{6},'','','',0,'','','',0,0,GETDATE(),1)",
                            ConvertTime(odr_2["VISITDATE"] == DBNull.Value ? "" : Convert.ToString(odr_2["VISITDATE"])),
                            odr_2["btemperature"] == DBNull.Value ? 0 : Convert.ToDouble(odr_2["btemperature"]),
                            odr_2["FEEDING_1"] == DBNull.Value ? "" : Convert.ToString(odr_2["FEEDING_1"]),
                            odr_2["SKINO"] == DBNull.Value ? "" : Convert.ToString(odr_2["SKINO"]),
                            odr_2["WEIGHT"] == DBNull.Value ? 0 : Convert.ToDouble(odr_2["WEIGHT"]),
                            odr_2["CHECKUPDOCTOR"] == DBNull.Value ? "" : Convert.ToString(odr_2["CHECKUPDOCTOR"]),
                            Convert.ToInt32(_dr["U_ID"])
                            ));

                        j_num = SqlHelper.ExecteNonQuery(CommandType.Text, String.Format(@"insert into PostpartumVisitChild(VisitTime,Temperature,FeedingMode,Skin,Weight,VisitDoctor,U_ID,Navel,YanJing,Thrush,DBNum,DBS,DBYJ,Handle,VisitDoctorCode,OperateUserId,OperateTime,PVisitChildState) values('{0}',{1},'{2}','{3}',{4},'{5}',{6},'','','',0,'','','',0,0,GETDATE(),1)",
                            ConvertTime(odr_2["VISITDATE"] == DBNull.Value ? "" : Convert.ToString(odr_2["VISITDATE"])),
                            odr_2["btemperature"] == DBNull.Value ? 0 : Convert.ToDouble(odr_2["btemperature"]),
                            odr_2["FEEDING_1"] == DBNull.Value ? "" : Convert.ToString(odr_2["FEEDING_1"]),
                            odr_2["SKINO"] == DBNull.Value ? "" : Convert.ToString(odr_2["SKINO"]),
                            odr_2["WEIGHT"] == DBNull.Value ? 0 : Convert.ToDouble(odr_2["WEIGHT"]),
                            odr_2["CHECKUPDOCTOR"] == DBNull.Value ? "" : Convert.ToString(odr_2["CHECKUPDOCTOR"]),
                            Convert.ToInt32(_dr["U_ID"])
                            ), null);
                        if (j_num == -1) //插入失败
                        {
                            LogInfo("产后访视（新生儿）（第二次）插入失败！");
                        }
                        else
                        { //插入成功
                            LogInfo("产后访视（新生儿）（第二次）插入成功！");
                            //删除已写入的
                            SqlHelper.ExecteNonQuery(CommandType.Text, String.Format(@"delete from PostpartumVisitChild where ID in (select top 1 ID from PostpartumVisitChild where PVisitChildState = 2 and U_ID={0} order by OperateTime desc)",
                                Convert.ToInt32(_dr["U_ID"])
                                ), null);
                        }
                    }
                }
                */ 

                //if (i_num > 0 || j_num > 0)
                if (i_num > 0)
                {
                    //更新流程表
                    int z_num = SqlHelper.ExecteNonQuery(CommandType.Text, String.Format(@"update ProcessState set PostpartumVisitState2 = 1 where ID={0}", Convert.ToInt32(_dr["ID"])), null);
                    if (z_num == -1) //更新失败
                    {
                        LogInfo("流程表（产后访视（第二次））更新失败！");
                    }
                    else
                    { //更新成功
                        LogInfo("流程表（产后访视（第二次））更新成功！");
                    }
                }
            }
            catch (Exception e)
            {
                LogInfo("产后访视第二次：" + e.Message);
            }
        }

        //42天检查
        protected void JC_42(DataRow _dr)
        {
            /* 插入42天检查、更新业务流程状态
             * 检查日期 CheckTime--checkupdate
             * 产后天数 PostpartumDay--afterbirthday
             * 一般情况 GeneralSituation--generalhealtho
             * 血压 XY--diaspressure
             * 乳房 Breast--breast
             * 妇科检查-外阴 FKWY--pudendum
             * 妇科检查-阴道 FKYD--vagina
             * 妇科检查-宫劲 FKGJ--cervix
             * 妇科检查-子宫 FKZG--uterus
             * 妇科检查-附件 FKFJ--ubodyseat
             * 处理 Handle--handle_1
             * 签名 VisitDoctor--checkupdoctorname
             */
            try
            {
                LogInfo("获取42天检查oracle：" + String.Format(@"select  b.*, nvl( (select CodeName1 from ggws.code_compare where codetype = 'CHCL' and Codevalue1=handle),'') as handle_1 from  v_DMC_PARTUM42CHECK b where b.MATERNALMCID='{0}'",
                    mcid
                  ));

                //根据身份证号42天检查
                DataTable dt = OracleHelper.GetDataTable(String.Format(@"select  b.*, nvl( (select CodeName1 from ggws.code_compare where codetype = 'CHCL' and Codevalue1=handle),'') as handle_1 from  v_DMC_PARTUM42CHECK b where b.MATERNALMCID='{0}'",
                    mcid
                    ));
                if (dt.Rows.Count > 0)
                {
                    DataRow odr = dt.Rows[0];
                    //插入42天检查
                    if (odr != null)
                    {
                        LogInfo("插入42天检查sqlserver：" + String.Format(@"insert into PostpartumCheck(CheckTime,PostpartumDay,GeneralSituation,XY,Breast,FKWY,FKYD,FKGJ,FKZG,FKFJ,Handle,VisitDoctor,U_ID,JYCS,YEQK,FeedingMode,OperateUserId,OperateTime) values('{0}',{1},'{2}','{3}','{4}','{5}','{6}','{7}','{8}','{9}','{10}','{11}',{12},'','','',0,GETDATE())",
                            ConvertTime(odr["checkupdate"] == DBNull.Value ? "" : Convert.ToString(odr["checkupdate"])),
                            odr["afterbirthday"] == DBNull.Value ? 0 : Convert.ToInt32(odr["afterbirthday"]),
                            odr["generalhealtho"] == DBNull.Value ? "" : Convert.ToString(odr["generalhealtho"]),
                            odr["diaspressure"] == DBNull.Value ? "" : Convert.ToString(odr["diaspressure"]),
                            odr["breast"] == DBNull.Value ? "" : Convert.ToString(odr["breast"]),
                            odr["pudendum"] == DBNull.Value ? "" : Convert.ToString(odr["pudendum"]),
                            odr["vagina"] == DBNull.Value ? "" : Convert.ToString(odr["vagina"]),
                            odr["cervix"] == DBNull.Value ? "" : Convert.ToString(odr["cervix"]),
                            odr["uterus"] == DBNull.Value ? "" : Convert.ToString(odr["uterus"]),
                            odr["ubodyseat"] == DBNull.Value ? "" : Convert.ToString(odr["ubodyseat"]),
                            odr["handle_1"] == DBNull.Value ? "" : Convert.ToString(odr["handle_1"]),
                            odr["checkupdoctorname"] == DBNull.Value ? "" : Convert.ToString(odr["checkupdoctorname"]),
                            Convert.ToInt32(_dr["U_ID"])
                            ));

                        int i_num = SqlHelper.ExecteNonQuery(CommandType.Text, String.Format(@"insert into PostpartumCheck(CheckTime,PostpartumDay,GeneralSituation,XY,Breast,FKWY,FKYD,FKGJ,FKZG,FKFJ,Handle,VisitDoctor,U_ID,JYCS,YEQK,FeedingMode,OperateUserId,OperateTime) values('{0}',{1},'{2}','{3}','{4}','{5}','{6}','{7}','{8}','{9}','{10}','{11}',{12},'','','',0,GETDATE())",
                            ConvertTime(odr["checkupdate"] == DBNull.Value ? "" : Convert.ToString(odr["checkupdate"])),
                            odr["afterbirthday"] == DBNull.Value ? 0 : Convert.ToInt32(odr["afterbirthday"]),
                            odr["generalhealtho"] == DBNull.Value ? "" : Convert.ToString(odr["generalhealtho"]),
                            odr["diaspressure"] == DBNull.Value ? "" : Convert.ToString(odr["diaspressure"]),
                            odr["breast"] == DBNull.Value ? "" : Convert.ToString(odr["breast"]),
                            odr["pudendum"] == DBNull.Value ? "" : Convert.ToString(odr["pudendum"]),
                            odr["vagina"] == DBNull.Value ? "" : Convert.ToString(odr["vagina"]),
                            odr["cervix"] == DBNull.Value ? "" : Convert.ToString(odr["cervix"]),
                            odr["uterus"] == DBNull.Value ? "" : Convert.ToString(odr["uterus"]),
                            odr["ubodyseat"] == DBNull.Value ? "" : Convert.ToString(odr["ubodyseat"]),
                            odr["handle_1"] == DBNull.Value ? "" : Convert.ToString(odr["handle_1"]),
                            odr["checkupdoctorname"] == DBNull.Value ? "" : Convert.ToString(odr["checkupdoctorname"]),
                            Convert.ToInt32(_dr["U_ID"])
                            ), null);
                        if (i_num == -1) //插入失败
                        {
                            LogInfo("42天检查插入失败！");
                        }
                        else
                        {
                            LogInfo("42天检查插入成功！");

                            //更新流程表
                            int j_num = SqlHelper.ExecteNonQuery(CommandType.Text, String.Format(@"update ProcessState set PostpartumCheckState = 1 where ID={0}", Convert.ToInt32(_dr["ID"])), null);
                            if (j_num == -1) //更新失败
                            {
                                LogInfo("流程表（42天检查）更新失败！");
                            }
                            else
                            {
                                LogInfo("流程表（42天检查）更新成功！");
                            }
                        }
                    }
                }
            }
            catch (Exception e)
            {
                LogInfo("42天检查：" + e.Message);
            }
        }

        //保健所医生
        protected void YSXX()
        {
            /* 保健所医生
             * 保健所编号 Hos_Code
             * 姓名 string Doc_Name -- PSN_NAME
             * 登录名 string Doc_LoginName -- PSN_ACCOUNT
             * 密码 string Doc_Password -- PSN_PWD
             */
            try
            {
                LogInfo("获取保健所医生oracle------------------------------------：" + String.Format(@"select A_CODE,PSN_NAME,PSN_ACCOUNT,PSN_PWD from vw_ggwsapp_orguser"));
                //获取保健所医生
                DataTable dt = OracleHelper.GetDataTable(String.Format(@"select A_CODE,PSN_NAME,PSN_ACCOUNT,PSN_PWD from vw_ggwsapp_orguser"));
                if (dt.Rows.Count > 0) {
                    for(int i = 0;i<dt.Rows.Count;i++){
                        DataRow odr = dt.Rows[i];

                        bool isBool = SqlHelper.Exists(String.Format(@"select count(1) from Doctor_Info where Doc_LoginName='{0}'",
                            odr["PSN_ACCOUNT"] == DBNull.Value ? "" : Convert.ToString(odr["PSN_ACCOUNT"])
                            ));
                        if (!isBool) //没有就添加
                        {
                            SqlHelper.ExecteNonQuery(CommandType.Text, String.Format(@"insert into Doctor_Info(Hos_Code,Doc_State,Doc_Name,Doc_CardID,Doc_Sex,Doc_Age,Doc_Photo,Doc_Intro,Doc_LoginName,Doc_Password,Doc_Mobile,OperateTime,OperateUserId,Doc_Time,Gold,Doc_NiChen) values(isnull((select top 1 Hos_Code from Hospital_Info where DicCode = '{0}'),0),1,'{1}','',0,0,'','','{2}','{3}','',GETDATE(),0,GETDATE(),0,'{4}')",
                                odr["A_CODE"] == DBNull.Value ? "" : Convert.ToString(odr["A_CODE"]),
                                odr["PSN_NAME"] == DBNull.Value ? "" : Convert.ToString(odr["PSN_NAME"]),
                                odr["PSN_ACCOUNT"] == DBNull.Value ? "" : Convert.ToString(odr["PSN_ACCOUNT"]),
                                odr["PSN_PWD"] == DBNull.Value ? "" : Convert.ToString(odr["PSN_PWD"]),
                                odr["PSN_NAME"] == DBNull.Value ? "" : Convert.ToString(odr["PSN_NAME"])
                                ), null);
                            LogInfo("插入保健所医生：" + (odr["PSN_ACCOUNT"] == DBNull.Value ? "" : Convert.ToString(odr["PSN_ACCOUNT"])));
                        }

                        //休眠
                        Thread.Sleep(200);
                    }
                }
            }
            catch (Exception e) 
            {
                LogInfo("保健所医生：" + e.Message);
            }
        }

        //根据身份证获取最新的mcid
        protected string GETNEWESTMCID(string idnumber, int u_id)
        {
            //根据身份证号查询最近一年最新记录,以及过了一年的数据就不再更新
            LogInfo("根据身份证号查询最近一年最新记录oracle：" + String.Format(@"select a.mcid,a.bbdate from V_DMC_REGISTRATIONCARDS a  where a.idnumber='{0}' and to_char(add_months(sysdate,-12),'yyyymmddhh24miss') <= a.bbdate order by a.bbdate desc",
                idnumber
                ));
            DataTable dt = OracleHelper.GetDataTable(String.Format(@"select a.mcid,a.bbdate from V_DMC_REGISTRATIONCARDS a  where a.idnumber='{0}' and to_char(add_months(sysdate,-12),'yyyymmddhh24miss') <= a.bbdate order by a.bbdate desc",
                idnumber
                ));

            if (dt.Rows.Count > 0 && dt.Rows[0]["mcid"] != null)
            {
                //是否有预约信息,有的话，就不再获取预约之前的数据
                LogInfo("是否有预约信息sqlserver：" + String.Format(@"select CreateTime from SmallCard where U_ID={0} and CardState <> 1 order by CreateTime desc", u_id));
                DataTable dt_2 = SqlHelper.ExecuteDataSet(CommandType.Text, String.Format(@"select CreateTime from SmallCard where U_ID={0} and CardState <> 1 order by CreateTime desc", u_id), null).Tables[0];
                if (dt_2.Rows.Count > 0)
                {
                    DateTime dateTime_1 = Convert.ToDateTime(ConvertTime(Convert.ToString(dt.Rows[0]["bbdate"])));
                    DateTime dateTime_2 = Convert.ToDateTime(Convert.ToDateTime(dt_2.Rows[0]["CreateTime"]).ToString("yyyy-MM-dd"));
                    if (DateTime.Compare(dateTime_1, dateTime_2) >= 0)
                    {
                        return dt.Rows[0]["mcid"].ToString();
                    }
                    else {
                        return "";
                    }
                }else{
                    return dt.Rows[0]["mcid"].ToString();
                }           
            }
            else
            {
                return "";
            }
        }

        //时间转换 年月日
        protected string ConvertTime(string time)
        {
            string outtime = "1970-01-01"; //默认值
            if (!string.IsNullOrEmpty(time))
            {
                if (time.Length > 8)
                {
                    outtime = time.Substring(0, 4); //年
                    outtime += "-";
                    outtime += time.Substring(4, 2); //月
                    outtime += "-";
                    outtime += time.Substring(6, 2); //日
                }
            }

            return outtime;
        }

        //写入日记
        protected void LogInfo(string str)
        {
            if (!Directory.Exists("C:\\FYLOG"))
            {
                Directory.CreateDirectory("C:\\FYLOG");
            }

            using (System.IO.StreamWriter sw = new System.IO.StreamWriter("C:\\FYLOG\\" + DateTime.Now.ToString("yyyyMMdd") + ".txt", true))
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
