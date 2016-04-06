using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Tuhui.Common.Mvc;
using Tuhui.OH.BusinessLogic;
using Tuhui.OH.Entity;
using Tuhui.OH.IBusinessLogic;
using Tuhui.OH.Mvc;

namespace Tuhui.OH.WebUI.Controllers
{
    public partial class ManagerController : BaseController<nz_user>
    {
        private IUserBusinessLogic _userBusiness;
        private INoticeBusinessLogic _noticeBusiness;

        public ManagerController()
        {
            _userBusiness = base.InstanceBusinessLogic<UserBusinessLogic>();
            _noticeBusiness = base.InstanceBusinessLogic<NoticeBusinessLogic>();
        }

        [AuthorizeFilter]
        public ActionResult Index()
        {
            return View();
        }

        [AuthorizeFilter]
        public ActionResult UserList()
        {
            return View();
        }

        [AuthorizeFilter]
        public ActionResult NoticeList()
        {
            return View();
        }

        public ActionResult UserModify(int? userid)
        {
            if (userid == null)
            {
                @ViewBag.TitleName = "用户管理 -> 添加页面";
                return View(new nz_user());
            }
            else
            {
                @ViewBag.TitleName = "用户管理 -> 编辑页面";
                return View(_userBusiness.GetUserByUserId(userid.Value));
            }
        }

        [HttpPost]
        public ActionResult UserModify(nz_user model)
        {
            if (model.userid == 0)
            {
                model.createtime = DateTime.Now;
                model.lastlogintime = DateTime.Now;
                _userBusiness.AddUser(model);
            }
            else
            {
                model.lastlogintime = DateTime.Now;
                _userBusiness.EditUserById(model.userid, model);
            }
            return RedirectToAction("UserList");
        }

        public ActionResult NoticeModify(int? noticeid)
        {
            if (noticeid == null)
            {
                @ViewBag.TitleName = "公告管理 -> 添加页面";
                return View(new nz_notice { effectivetime = DateTime.Now });
            }
            else
            {
                @ViewBag.TitleName = "公告管理 -> 编辑页面";
                return View(_noticeBusiness.GetNoticeByNoticeId(noticeid.Value));
            }
        }

        [HttpPost]
        public ActionResult NoticeModify(nz_notice model)
        {
            if (model.noticeid == 0)
            {
                model.createtime = DateTime.Now;
                model.createuser = SessionHelper.LogOnUser<nz_user>().userid;
                _noticeBusiness.AddNotice(model);
            }
            else
            {
                model.createtime = DateTime.Now;
                model.createuser = SessionHelper.LogOnUser<nz_user>().userid;
                _noticeBusiness.EditNoticeById(model.noticeid, model);
            }
            return RedirectToAction("NoticeList");
        }

        public ActionResult aaa()
        {
            //var id = 2001;
            var result = _noticeBusiness.GetobjStationList(null);
            return Json(result);
        }
    }

}