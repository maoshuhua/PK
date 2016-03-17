using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web.Mvc;
using Tuhui.Reception.Service;

namespace Tuhui.Reception.Mvc
{
    public static partial class ReceptionHtmlHelperExtension
    {
        private static IUserService _userBL
        {
            get
            {
                return new UserService();
            }
        }

        private static IFaceService _faceBL
        {
            get
            {
                return new FaceService();
            }
        }
    }
}
