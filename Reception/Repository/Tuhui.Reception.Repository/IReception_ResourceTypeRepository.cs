using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Tuhui.Common45.Framework;
using Tuhui.Reception.Model;

namespace Tuhui.Reception.Repository
{
    public interface IReception_ResourceTypeRepository : IBaseRepository<DbReceptionContext>
    {
        //获取资源分类列表
        List<Reception_ResourceType> GetResourceTypeList();

        //添加资源分类
        int ResourceTypeInsert(Reception_ResourceType model);

        //修改资源分类
        int ResourceTypeUpdate(Reception_ResourceType model);

        //获取单个资源分类
        Reception_ResourceType GetResourceTypeById(string id);

        //删除资源分类
        int ResourceTypeDelete(string id);
    }
}
