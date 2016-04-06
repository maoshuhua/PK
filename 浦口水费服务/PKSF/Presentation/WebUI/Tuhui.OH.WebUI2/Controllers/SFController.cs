using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Text.RegularExpressions;
using System.Web;
using System.Web.Mvc;
using Tuhui.OH.Utility;

namespace Tuhui.OH.WebUI2.Controllers
{
    public class SFController : Controller
    {
        //浦口水费
        public string GetSF(string sendStr)
        {
            //日志
            LogHelper log = new LogHelper(SFConfig.GetUrl("log") + DateTime.Now.ToString("yyyyMMdd") + ".txt");

            log.WriteLine("Socket发送字符串：" + sendStr);

            string recvStr = ""; //请求回来数据
            StringBuilder output = new StringBuilder(); //处理后数据
            try
            {
                int port = Convert.ToInt32(SFConfig.GetUrl("port"));
                string host = SFConfig.GetUrl("ip");
                ///创建终结点EndPoint
                IPAddress ip = IPAddress.Parse(host);
                IPEndPoint ipe = new IPEndPoint(ip, port);//把ip和端口转化为IPEndpoint实例

                ///创建socket并连接到服务器
                Socket c = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);//创建Socket
                log.WriteLine("Socket Connect...");
                c.Connect(ipe);//连接到服务器
                if (c.Connected)
                {
                    log.WriteLine("Socket Connected");
                    ///向服务器发送信息
                    //string sendStr = "000102810016  201501  201511  ";  //获取账单
                    //string sendStr = "011102810016  "; //获取用户信息
                    byte[] bs = Encoding.Default.GetBytes(sendStr);//把字符串编码为字节
                    log.WriteLine("Socket Send...");
                    c.Send(bs, bs.Length, 0);//发送信息
                    log.WriteLine("Socket Sended");
                    ///接受从服务器返回的信息
                    byte[] recvBytes = new byte[1024];
                    int bytes;
                    log.WriteLine("Socket Receive...");
                    bytes = c.Receive(recvBytes, recvBytes.Length, 0);//从服务器端接受返回信息
                    if (bytes > 1)
                    {
                        log.WriteLine("Socket Received");
                        recvStr += Encoding.Default.GetString(recvBytes, 0, bytes);
                        ///一定记着用完socket后要关闭
                        c.Close();
                        log.WriteLine("Socket Close");
                        log.WriteLine("Socket接收字符串：" + recvStr);
                    }
                    else
                    {
                        log.WriteLine("无法从远程服务器获取数据");
                    }
                }
            }
            catch (ArgumentNullException e)
            {
                log.WriteLine("ArgumentNullException:" + e.Message);
                output.Append("ArgumentNullException:" + e.Message);
            }
            catch (SocketException e)
            {
                log.WriteLine("SocketException:" + e.Message);
                output.Append("SocketException:" + e.Message);
            }

            //数据处理
            List<string> list = SFSplite.GetList(sendStr);
            if (output.Length == 0)
            {
                if (list.Count == 1) //验证用户
                {
                    if (recvStr.IndexOf("000111") == 0) //无此用户
                    {
                        output.Append("{\"result\":0,\"msg\":\"用户身份验证出错;\"}");
                    }
                    else 
                    {
                        List<string> list_out = SFSplite.GetList(recvStr);
                        if (list_out.Count >= 3)
                        {
                            output.Append("{\"result\":1,\"realname\":\"" + list_out[1] + "\",\"address\":\"" + list_out[2] + "\"}");
                        }
                        else
                        {
                            output.Append("{\"result\":1}");
                        }
                    }
                }
                else //查询水费情况
                {
                    output.Append("{\"result\":1,\"msg\":\"\",\"data\":[");
                    //recvStr数据处理

                    SFMessage sfmessage = SFSplite.GetInfo(recvStr);
                    if (sfmessage != null)
                    {
                        List<MonthMessage> monthmessage = sfmessage.YXX;
                        if (monthmessage != null) {
                            if (monthmessage.Count > 0)
                            {
                                monthmessage.ForEach(s =>
                                {
                                    output.Append("{");
                                    //实收水量
                                    output.Append("\"accWaterSum\":\"" + s.BYSL + "\",");
                                    //客户账户
                                    output.Append("\"custID\":\"" + sfmessage.YHH + "\",");
                                    //未用到
                                    output.Append("\"feeFrac\":\"\",");
                                    //垃圾费
                                    output.Append("\"garbageFee\":\"" + SFSplite.MoneyConvert(s.LJF) + "\",");
                                    //未用到
                                    output.Append("\"meterData\":\"\",");
                                    //未用到
                                    output.Append("\"oughtReadDate\":\"\",");
                                    //缴费时间
                                    output.Append("\"payDate\":\"" + s.JFRQ + "\",");
                                    //缴费地点
                                    output.Append("\"payPlace\":\"\",");
                                    //违约金
                                    output.Append("\"penalty\":\"" + SFSplite.MoneyConvert(s.ZNJ) + "\",");
                                    //上期结余
                                    output.Append("\"prevBalance\":\"" + SFSplite.MoneyConvert(s.JY) + "\",");
                                    //抄表日期
                                    output.Append("\"readDate\":\"" + s.CBRQ + "\",");
                                    //水费金额
                                    output.Append("\"waterFee\":\"" + SFSplite.MoneyConvert(s.SFJE) + "\",");
                                    //总金额
                                    output.Append("\"waterFeeAll\":\"" + SFSplite.MoneyConvert(s.ZJE) + "\",");
                                    //总水量
                                    output.Append("\"waterSum\":\"" + s.YL + "\"");
                                    output.Append("}");
                                    output.Append(",");
                                });

                                output.Remove(output.Length -1,1);
                            }
                        }
                    }
                   
                    output.Append("]}");
                }
            }

            return output.ToString();
        }
    }
}
