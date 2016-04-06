using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Tuhui.Common.Mvc;
using Tuhui.OH.Entity;

namespace Tuhui.OH.WebUI.Controllers
{
    public partial class ManagerController
    {
        public ActionResult GetAllUser()
        {
            return Json(_userBusiness.GetUserList(null), JsonRequestBehavior.AllowGet);
        }

        public ActionResult GetAllNotice()
        {
            return Json(_noticeBusiness.GetNoticeList(), JsonRequestBehavior.AllowGet);
        }

        public ActionResult GetNoticeByNoticeId(int? noticeid)
        {
            return Json(_noticeBusiness.GetNoticeByNoticeId(noticeid.Value));
        }

        public ActionResult GetUserByUserId(int? userid)
        {
            return Json(_userBusiness.GetUserByUserId(userid.Value));
        }

        public ActionResult AddUser(nz_user model)
        {
            return Json(_userBusiness.AddUser(model));
        }

        public ActionResult DeleteUserById(int? id)
        {
            if (!id.HasValue)
            {
                //抛出异常
            }
            _userBusiness.DeleteUserById(id.Value);
            return Json(true);
        }

        public ActionResult AddNotice(nz_notice model)
        {
            return Json(_noticeBusiness.AddNotice(model));
        }

        public ActionResult DeleteNoticeById(int? id)
        {
            if (!id.HasValue)
            {
                //抛出异常
            }
            _noticeBusiness.DeleteNoticeById(id.Value);
            return Json(true);
        }

        public ActionResult GetNoticeByTimeNow(int? effecttype)
        {
            return Json(_noticeBusiness.GetNoticeByTimeNow(effecttype.Value));
        }
    }
}