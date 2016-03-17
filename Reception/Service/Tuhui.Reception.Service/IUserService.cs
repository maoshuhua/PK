using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Tuhui.Common45.Framework;
using Tuhui.Common45.Utility;
using Tuhui.Reception.Model;

namespace Tuhui.Reception.Service
{
    public interface IUserService : IBaseService
    {
        bool ValidationUser(string username, string password);
        List<tv_user> GetUserList(string keyword);
        List<tv_log> GetLogList();
        void DeleteUserById(int id);
        void DeleteLogById(int id);
        PagedList<tv_log> GetTV_LogPageList(string keyword, int pageIndex, int pageSize);
        PagedList<tv_user> GetTV_UserPagedList(string keyword, int pageIndex, int pageSize);
        tv_user GetUserByUserName(string username);
        tv_log AddLog(tv_log model);
        bool EditLogById(int logid, tv_log model);
        tv_log GetTV_LogById(int logid);
        tv_user GetUserByUserId(int userid);
        tv_user AddUser(tv_user model);
        bool EditUserById(int userid, tv_user model);
    }
}
