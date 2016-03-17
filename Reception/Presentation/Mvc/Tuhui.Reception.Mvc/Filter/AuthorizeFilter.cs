using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;
using Tuhui.Common45.Mvc;
using Tuhui.Reception.Model;
using Tuhui.Reception.Service;

namespace Tuhui.Reception.Mvc
{
    public class AuthorizeFilter : FilterAttribute, IAuthorizationFilter
    {
        private IUserService _userBL;
        public AuthorizeFilter()
        {
            _userBL = new UserService();
        }

        public void OnAuthorization(AuthorizationContext filterContext)
        {
            // 判断是否客户端已经登录认证
            if (filterContext.HttpContext.User.Identity.IsAuthenticated)
            {
                // 已经登录认证，如果session过期或丢失，则恢复session
                if (SessionHelper.LogOnUserObj == null)
                {
                    SessionHelper.LogOnUser<tv_user>(_userBL.GetUserByUserName(filterContext.HttpContext.User.Identity.Name));
                }
            }
            else
            {
                if (filterContext.HttpContext.Request.IsAjaxRequest())
                {
                    filterContext.Result = new AjaxLogOnResult();
                }
                else
                {
                    filterContext.Result = new RedirectToRouteResult(new RouteValueDictionary(new
                    {
                        controller = "Account",
                        action = "LogOn",
                        returnUrl = filterContext.HttpContext.Request.Url
                    }));
                }
            }
        }
    }
}
