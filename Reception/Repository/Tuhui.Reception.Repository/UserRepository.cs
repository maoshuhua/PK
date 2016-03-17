using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Tuhui.Common45.Framework;
using Tuhui.Reception.Model;

namespace Tuhui.Reception.Repository
{
    public class UserRepository : BaseRepository<DbReceptionContext>, IUserRepository
    {
        public bool ValidationUser(string username, string password)
        {
            var _result = base.ContextObj.tv_user.Where(p => p.username == username && p.password == password).FirstOrDefault() != null;
            if (_result)
            {
                base.Update<tv_user>(p => p.username == username, p =>
                {
                    p.lastlogintime = DateTime.Now;
                });
            }
            return _result;
        }


        public tv_user GetUserByUserName(string username)
        {
            return base.ContextObj.tv_user.Where(p => p.username == username).FirstOrDefault();
        }
    }
}
