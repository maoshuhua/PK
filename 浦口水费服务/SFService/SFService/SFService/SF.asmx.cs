using System;
using System.Configuration;
using System.IO;
using System.Net;
using System.Text;
using System.Web.Services;

namespace SFService
{
    /// <summary>
    /// SF 的摘要说明
    /// </summary>
    [WebService(Namespace = "http://tempuri.org/")]
    [WebServiceBinding(ConformsTo = WsiProfiles.BasicProfile1_1)]
    [System.ComponentModel.ToolboxItem(false)]
    // 若要允许使用 ASP.NET AJAX 从脚本中调用此 Web 服务，请取消注释以下行。 
    [System.Web.Script.Services.ScriptService]
    public class SF : System.Web.Services.WebService
    {

        [WebMethod]
        public string GetSF(string sendStr)
        {
            //获取IP
            string ip = ConfigurationManager.AppSettings["ip"].ToString();
            string url = "http://" + ip + "/SF/GetSF?sendStr=" + sendStr;
            Uri httpURL = new Uri(url);
            HttpWebRequest httpReq = (HttpWebRequest)WebRequest.Create(httpURL);
            HttpWebResponse httpResp = (HttpWebResponse)httpReq.GetResponse();
            Stream respStream = httpResp.GetResponseStream();
            StreamReader respStreamReader = new StreamReader(respStream, Encoding.UTF8);
            string strBuff = respStreamReader.ReadToEnd();

            return strBuff;
        }
    }
}
