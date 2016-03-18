using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Tuhui.Common45.Framework;
using Tuhui.Common45.Utility;
using Tuhui.Reception.Model;

namespace Tuhui.Reception.Repository
{
    public class Reception_ResourceRepository : BaseRepository<DbReceptionContext>, IReception_ResourceRepository
    {
        //获取资源列表
        public PagedList<Reception_Resource> GetResourceList(string keyword, int pageIndex, int pageSize) 
        {
            var query = base.Search<Reception_Resource>();

            if (!string.IsNullOrEmpty(keyword))
            {
                query = query.Where(p => p.Name.Contains(keyword));
            }

            return new PagedList<Reception_Resource>(query.OrderByDescending(p => p.AddTime), pageIndex, pageSize);
        } 
    }
}
