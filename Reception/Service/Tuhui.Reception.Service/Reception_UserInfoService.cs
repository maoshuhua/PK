using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Tuhui.Common45.Framework;
using Tuhui.Reception.Model;
using Tuhui.Reception.Repository;

namespace Tuhui.Reception.Service
{
    public class Reception_UserInfoService : BaseService, IReception_UserInfoService
    {
        private IReception_UserInfoRepository _reception_UserInfoRepository;

        public Reception_UserInfoService()
        {
            _reception_UserInfoRepository = new Reception_UserInfoRepository();
        }

        //登录
        public bool ValidationUser(Reception_UserInfo model)
        {
            return _reception_UserInfoRepository.ValidationUser(model);
        }

        //获取用户信息
        public Reception_UserInfo GetUserByUserName(string userName)
        {
            return _reception_UserInfoRepository.GetUserByUserName(userName);
        }

    }
}
