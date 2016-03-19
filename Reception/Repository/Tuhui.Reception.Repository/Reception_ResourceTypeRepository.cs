using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Tuhui.Common45.Framework;
using Tuhui.Reception.Model;

namespace Tuhui.Reception.Repository
{
    public class Reception_ResourceTypeRepository : BaseRepository<DbReceptionContext>, IReception_ResourceTypeRepository
    {
        //获取资源分类列表
        public List<Reception_ResourceType> GetResourceTypeList()
        {
            var list = base.SearchList<Reception_ResourceType>().OrderByDescending(p => p.CreateTime).ToList();

            return list;
        }

        //添加资源分类
        public int ResourceTypeInsert(Reception_ResourceType model)
        {
            return base.Insert<Reception_ResourceType>(model);
        }

        //修改资源分类
        public int ResourceTypeUpdate(Reception_ResourceType model)
        {
            return base.Update<Reception_ResourceType>(p => p.RT_ID == model.RT_ID, p => {
                p.Name = model.Name;
            });
        }

        //获取单个资源分类
        public Reception_ResourceType GetResourceTypeById(string id)
        {    
            var list = base.SearchFirstOrDefault<Reception_ResourceType>(p => p.RT_ID == id);
        
            return list;
        }

        //删除资源分类
        public int ResourceTypeDelete(string id)
        {
            return base.Delete<Reception_ResourceType>(p => p.RT_ID == id);
        }
    }
}
