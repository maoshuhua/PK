using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Tuhui.Common45.Framework;
using Tuhui.Reception.Model;

namespace Tuhui.Reception.Repository
{
    public class LogRepository : BaseRepository<DbReceptionContext>, ILogRepository
    {
        public List<tv_log> Log_List()
        {
            return All.ToList();
        }

        public IQueryable<tv_log> All
        {
            get { return ContextObj.tv_log; }
        }

    }
}
