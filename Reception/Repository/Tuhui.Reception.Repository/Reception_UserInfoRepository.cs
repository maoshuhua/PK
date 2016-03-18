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
            using (var db = new DbReceptionContext())
            {
                var _result = db.Reception_UserInfo.Where(p => p.Name == model.Name && p.Pwd == model.Pwd).FirstOrDefault() != null;

                return _result;
            }
        }

        //获取用户信息
        public Reception_UserInfo GetUserByUserName(string userName)
        {
            using (var db = new DbReceptionContext())
            {
                var _result = db.Reception_UserInfo.Where(p => p.Name == userName).FirstOrDefault();

                return _result;
            }
        }
    }
}
