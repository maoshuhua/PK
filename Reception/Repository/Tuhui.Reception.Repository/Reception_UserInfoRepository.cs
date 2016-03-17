using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Tuhui.Common45.Framework;
using Tuhui.Reception.Model;

namespace Tuhui.Reception.Repository
{
    public class Reception_UserInfoRepository : IReception_UserInfoRepository
    {
        //登录
        public bool ValidationUser(Reception_UserInfo model)
        {
            //var _result = base.ContextObj.Reception_UserInfo.Where(p => p.Name == model.Name && p.Pwd == model.Pwd).FirstOrDefault() != null;
            


            //return _result;
            using (var ctx = new DbReceptionContext())
            {
                var user = new Reception_UserInfo
                {
                   U_ID  = "22",
                   Name = "gg",
                   Pwd = "111",
                   RegTime = DateTime.Now
                };

                ctx.Reception_UserInfo.Add(user);
                ctx.SaveChanges();
            }
            
            return true;
        }

        //获取用户信息
        public Reception_UserInfo GetUserByUserName(string userName)
        {
            //var _result = base.ContextObj.Reception_UserInfo.Where(p => p.Name == userName).FirstOrDefault();

            //return _result;
            return null;
        }
    }
}
