using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web.Mvc;
using Tuhui.Common45.Environment;
using Tuhui.Common45.Exception;
using Tuhui.Common45.Framework;

namespace Tuhui.Common45.Mvc
{
    /// <summary>
    /// 用户异常Action类型
    /// </summary>
    public class UserExceptionResult : ActionResult
    {
        public UserExceptionResult(string messageId, params object[] parameters)
        {
            throw new UserException(messageId, parameters);
        }

        public override void ExecuteResult(ControllerContext context)
        {
        }
    }
}
