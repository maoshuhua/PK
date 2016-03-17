using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web.Mvc;
using System.Web;
using System.Web.Script.Serialization;
using Tuhui.Common45.Utility;

namespace Tuhui.Common45.Mvc
{
    /// <summary>
    /// Ajax登陆Action类型
    /// </summary>
    public class AjaxLogOnResult : ActionResult
    {
        public override void ExecuteResult(ControllerContext context)
        {
            HttpResponseBase response = context.HttpContext.Response;
            response.ContentType = "application/json";
            JavaScriptSerializer serializer = new JavaScriptSerializer();
            response.Write(serializer.Serialize(new JsonError("authorize")));
        }
    }
}
