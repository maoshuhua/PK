using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Tuhui.Common45.Framework;
using Tuhui.Reception.Model;

namespace Tuhui.Reception.Service
{
    public interface IReception_UserInfoService : IBaseService
    {
        //登录
        bool ValidationUser(Reception_UserInfo model);

        //获取用户信息
        Reception_UserInfo GetUserByUserName(string userName);
    }
}
