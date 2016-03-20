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
        //获取全部
        public List<Reception_Resource> GetList()
        {
            var list = base.SearchList<Reception_Resource>().ToList();

            return list;
        }
        
        //获取单个
        public Reception_Resource Get(string id)
        {
            return base.SearchFirstOrDefault<Reception_Resource>(p => p.R_ID == id);
        }
        
        //获取全部分页
        public PagedList<Reception_Resource> GetPageList(Reception_Resource model, int pageIndex, int pageSize)
        {
            var query = base.Search<Reception_Resource>();

            if (model != null)
            {
                //根据条件筛选
                
            }

            return new PagedList<Reception_Resource>(query.OrderByDescending(p => p.R_ID), pageIndex, pageSize);
        }
        
        //添加
        public int Insert(Reception_Resource model)
        {
            return base.Insert<Reception_Resource>(model);
        }
        
        //修改
        public int Update(Reception_Resource model)
        {
            return base.Update<Reception_Resource>(p => p.R_ID == model.R_ID, p => {
                p.R_ID = model.R_ID;
                p.Name = model.Name;
                p.Long = model.Long;
                p.Lat = model.Lat;
                p.RRDW = model.RRDW;
                p.SFDW = model.SFDW;
                p.StartTime = model.StartTime;
                p.EndTime = model.EndTime;
                p.Content = model.Content;
                p.RStatus = model.RStatus;
                p.SSJD = model.SSJD;
                p.AddTime = model.AddTime;
            });
        }
        
        //删除
        public int Delete(string id)
        {
            return base.Delete<Reception_Resource>(p => p.R_ID == id);
        }
    }
}
