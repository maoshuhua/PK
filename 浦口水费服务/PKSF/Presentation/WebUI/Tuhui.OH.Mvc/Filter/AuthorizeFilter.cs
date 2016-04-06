using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;
using Tuhui.Common.Mvc;
using Tuhui.OH.Entity;

namespace Tuhui.OH.Mvc
{
    public class AuthorizeFilter : FilterAttribute, IAuthorizationFilter
    {
        public AuthorizeFilter()
        {

        }

        public void OnAuthorization(AuthorizationContext filterContext)
        {
            
        }
    }
}
