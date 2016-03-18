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
    public interface IReception_ResourceRepository : IBaseRepository<DbReceptionContext>
    {
        //获取资源列表
        PagedList<Reception_Resource> GetResourceList(string keyword, int pageIndex, int pageSize);
    }
}
