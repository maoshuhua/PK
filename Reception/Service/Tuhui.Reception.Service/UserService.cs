using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Tuhui.Common45.Framework;
using Tuhui.Common45.Utility;
using Tuhui.Reception.Model;
using Tuhui.Reception.Repository;

namespace Tuhui.Reception.Service
{
    public class UserService : BaseService, IUserService
    {
        private IUserRepository _userRespository;
        private IUserRepository _logRespository;

        public UserService()
        {
            _userRespository = base.InstanceRepository<UserRepository>();
            _logRespository = base.InstanceRepository<UserRepository>();
        }

        public bool ValidationUser(string username, string password)
        {
            return _userRespository.ValidationUser(username, password);
        }

        public List<tv_user> GetUserList(string keyword)
        {
            if (true == String.IsNullOrEmpty(keyword))
            {
                return _userRespository.SearchList<tv_user>();
            }
            else
            {
                return _userRespository.SearchList<tv_user>(p => p.username.Contains(keyword));
            }
        }

        public List<tv_log> GetLogList()
        {
            return _logRespository.SearchList<tv_log>();
        }

        public void DeleteUserById(int id)
        {
            _userRespository.Delete<tv_user>(p => p.userid == id);
        }

        public void DeleteLogById(int id)
        {
            _logRespository.Delete<tv_log>(p => p.logid == id);
        }

        public PagedList<tv_user> GetTV_UserPagedList(string keyword, int pageIndex, int pageSize)
        {
            if (true == string.IsNullOrEmpty(keyword))
            {
                return _userRespository.SearchPagedList<tv_user>(null,
                                                                p => p.OrderBy(q => q.userid), pageIndex,
                                                                pageSize);
            }
            else
            {
                return _userRespository.SearchPagedList<tv_user>(p => p.username.Contains(keyword),
                                                             p => p.OrderBy(q => q.userid), pageIndex,
                                                             pageSize);
            }
        }

        public tv_user GetUserByUserName(string username)
        {
            return _userRespository.SearchFirstOrDefault<tv_user>(p => p.username == username);
        }

        public PagedList<tv_log> GetTV_LogPageList(string keyword, int pageIndex, int pageSize)
        {
            if (true == string.IsNullOrEmpty(keyword))
            {
                return _logRespository.SearchPagedList<tv_log>(null,
                                                                p => p.OrderBy(q => q.logid), pageIndex,
                                                                pageSize);
            }
            else
            {
                return _logRespository.SearchPagedList<tv_log>(p => p.logcontent.Contains(keyword),
                                                             p => p.OrderBy(q => q.logid), pageIndex,
                                                             pageSize);
            }
        }

        public tv_log AddLog(tv_log model)
        {
            _logRespository.Insert<tv_log>(model);
            return model;
        }

        public bool EditLogById(int logid,tv_log model)
        {
            _logRespository.Update<tv_log>(p => p.logid == logid, p =>
                {
                    p.logtime = model.logtime;
                    p.logtype = model.logtype;
                    p.logcontent = model.logcontent;
                });
            return true;
        }

        public tv_log GetTV_LogById(int logid)
        {
            return _logRespository.SearchFirstOrDefault<tv_log>(p => p.logid == logid);
        }

        public tv_user GetUserByUserId(int userid)
        {
            return _userRespository.SearchFirstOrDefault<tv_user>(p => p.userid == userid);
        }

        public tv_user AddUser(tv_user model)
        {
            _userRespository.Insert<tv_user>(model);
            return model;
        }

        public bool EditUserById(int userid, tv_user model)
        {
            _userRespository.Update<tv_user>(p=>p.userid ==userid, p =>
                {
                    p.lastlogintime = model.lastlogintime;
                    p.userlevel = model.userlevel;
                    p.username = model.username;
                    p.nickname = model.nickname;
                    p.password = model.password;
                    p.userstate = model.userstate;
                    p.lastloginIP = model.lastloginIP;
                });
            return true;
        }
    }
}
