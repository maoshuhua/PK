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
    public class Reception_ResourceTypeRepository : BaseRepository<DbReceptionContext>, IReception_ResourceTypeRepository
    {
        //获取全部
        public List<Reception_ResourceType> GetList()
        {
            var list = base.SearchList<Reception_ResourceType>().OrderByDescending(p => p.CreateTime).ToList();

            return list;
        }
        
        //获取单个
        public Reception_ResourceType Get(string id)
        {
            var list = base.SearchFirstOrDefault<Reception_ResourceType>(p => p.RT_ID == id);
            
            return list;
        }
        
        //获取全部分页
        public PagedList<Reception_ResourceType> GetPageList(Reception_ResourceType model, int pageIndex, int pageSize)
        {
            var query = base.Search<Reception_ResourceType>();

            if (model != null)
            {
                //根据条件筛选
                
            }

            return new PagedList<Reception_ResourceType>(query, pageIndex, pageSize);
        }
        
        //添加
        public int Insert(Reception_ResourceType model)
        {
            return base.Insert<Reception_ResourceType>(model);
        }
        
        //修改
        public int Update(Reception_ResourceType model)
        {
            int i = 0;
            i = base.Update<Reception_ResourceType>(p => p.RT_ID == model.RT_ID, p => {
                p.Name = model.Name;
            });
            
            return i;
        }
        
        //删除
        public int Delete(string id)
        {
            int i = 0;
            i = base.Delete<Reception_ResourceType>(p => p.RT_ID == id);
            
            return i;
        }
    }
}
