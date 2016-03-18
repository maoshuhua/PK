using ReceptionApp.Web.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace ReceptionApp.Web.Controllers
{
    public class ManagerController : Controller
    {
        //
        // GET: /Manager/

        public ActionResult Index()
        {
            return View();
        }

        public void Test()
        {
            using (var db = new OracleDbContext())
            {
                var user = new Reception_UserInfo
                {
                    U_ID = "22333",
                    Name = "ggfdsafdasf",
                    Pwd = "111fdsfsdafsda",
                    RegTime = DateTime.Now
                };

                db.Reception_UserInfo.Add(user);
                db.SaveChanges();
            }
        }

    }
}
