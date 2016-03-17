using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Tuhui.Common45.Framework;
using Tuhui.Reception.Model;

namespace Tuhui.Reception.Repository
{
    public interface IUserRepository : IBaseRepository<DbReceptionContext>
    {
        bool ValidationUser(string username, string password);
        //List<tv_user> User_List();
    }
}
