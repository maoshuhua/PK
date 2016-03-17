using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Tuhui.Common45.Framework;
using Tuhui.Reception.Model;

namespace Tuhui.Reception.Repository
{
    public interface ILogRepository : IBaseRepository<DbReceptionContext>
    {
        List<tv_log> Log_List();
    }
}
