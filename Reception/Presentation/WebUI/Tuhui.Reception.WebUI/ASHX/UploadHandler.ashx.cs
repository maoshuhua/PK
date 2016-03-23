using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using Tuhui.Common45.Utility;

namespace Tuhui.Reception.WebUI.ASHX
{
    /// <summary>
    /// UploadHandler 的摘要说明
    /// </summary>
    public class UploadHandler : IHttpHandler
    {

        public void ProcessRequest(HttpContext context)
        {
            context.Response.ContentType = "text/plain";
            context.Response.Charset = "utf-8";

            string datetime = DateTime.Now.ToString("yyyyMMdd");
            string guid = CommonFun.GenerGuid();

            HttpPostedFile file = context.Request.Files["Filedata"];
            string uploadPath =
                HttpContext.Current.Server.MapPath(context.Request["folder"]) + "\\" + datetime + "\\";

            if (file != null)
            {
                if (!Directory.Exists(uploadPath))
                {
                    Directory.CreateDirectory(uploadPath);
                }

                //后缀
                string hz = file.FileName.Substring(file.FileName.IndexOf("."));

                file.SaveAs(uploadPath + guid + hz);

                context.Response.Write("{'datetime':'" + datetime + "',filename:'" + guid + hz + "'}");
            }
            else
            {
                context.Response.Write("0");
            }  
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