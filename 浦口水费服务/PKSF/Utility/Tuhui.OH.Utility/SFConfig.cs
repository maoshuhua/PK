using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml;
using System.Web;

namespace Tuhui.OH.Utility
{
    public static class SFConfig
    {
        public static string GetUrl(string key)
        {
            string url = "";
            XmlTextReader reader = new XmlTextReader(System.Web.HttpContext.Current.Server.MapPath("~\\sf.config"));
            XmlDocument doc = new XmlDocument();
            doc.Load(reader);

            foreach (XmlNode node in doc.DocumentElement.ChildNodes)
            {
                if (node.Attributes["key"].Value == key)
                {
                    url = node.Attributes["url"].Value;
                    break;
                }
            }
            return url;

        }
    }
}
