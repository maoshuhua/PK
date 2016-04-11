using System;
using System.Collections.Generic;
using System.Configuration;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Web;

namespace SFService
{
    /// <summary>
    /// SF1 的摘要说明
    /// </summary>
    public class SF1 : IHttpHandler
    {

        public void ProcessRequest(HttpContext context)
        {
            context.Response.ContentType = "text/plain";

            string sendStr = context.Request.QueryString["sendStr"];
            //获取IP
            string ip = ConfigurationManager.AppSettings["ip"].ToString();
            string url = "http://" + ip + "/SF/GetSF?sendStr=" + sendStr;
            Uri httpURL = new Uri(url);
            HttpWebRequest httpReq = (HttpWebRequest)WebRequest.Create(httpURL);
            HttpWebResponse httpResp = (HttpWebResponse)httpReq.GetResponse();
            Stream respStream = httpResp.GetResponseStream();
            StreamReader respStreamReader = new StreamReader(respStream, Encoding.UTF8);
            string strBuff = respStreamReader.ReadToEnd();

            context.Response.Write(strBuff);
        }

        public bool IsReusable
        {
            get
            {
                return false;
            }
        }
    }
}