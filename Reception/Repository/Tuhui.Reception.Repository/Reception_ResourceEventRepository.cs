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
    public class Reception_ResourceEventRepository : BaseRepository<DbReceptionContext>, IReception_ResourceEventRepository
    {
        //获取全部
        public List<Reception_ResourceEvent> GetList()
        {
            var list = base.SearchList<Reception_ResourceEvent>().ToList();

            return list;
        }
        
        //获取单个
        public Reception_ResourceEvent Get(string id)
        {
            return base.SearchFirstOrDefault<Reception_ResourceEvent>(p => p.RE_ID == id);
        }
        
        //获取全部分页
        public PagedList<Reception_ResourceEvent> GetPageList(Reception_ResourceEvent model, int pageIndex, int pageSize)
        {
            var query = base.Search<Reception_ResourceEvent>();

            if (!string.IsNullOrEmpty(model.Name)) 
            {
                query = query.Where(p => p.Name == model.Name);
            }

            if (!string.IsNullOrEmpty(model.R_ID))
            {
                query = query.Where(p => p.R_ID == model.R_ID);
            }
            
            return new PagedList<Reception_ResourceEvent>(query.OrderByDescending(p => p.AddTime), pageIndex, pageSize);
        }
        
        //添加
        public int Insert(Reception_ResourceEvent model)
        {
            return base.Insert<Reception_ResourceEvent>(model);
        }
        
        //修改
        public int Update(Reception_ResourceEvent model)
        {
            return base.Update<Reception_ResourceEvent>(p => p.RE_ID == model.RE_ID, p => {
                p.RE_ID = model.RE_ID;
                p.R_ID = model.R_ID;
                p.Name = model.Name;
                p.Content = model.Content;
                p.AddTime = model.AddTime;
            });
        }
        
        //删除
        public int Delete(string id)
        {
            return base.Delete<Reception_ResourceEvent>(p => p.RE_ID == id);
        }
    }
}
