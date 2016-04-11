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

            if (!string.IsNullOrEmpty(model.Name))
            {
                query = query.Where(p => p.Name.Contains(model.Name));
            }

            if (!string.IsNullOrEmpty(model.SSJD))
            {
                query = query.Where(p => p.SSJD.Contains(model.SSJD));
            }

            if (model.RStatus != "0")
            {
                query = query.Where(p => p.RStatus == model.RStatus);
            }

            if (!string.IsNullOrEmpty(model.RT_ID)) 
            {
                query = query.Where(p => p.RT_ID == model.RT_ID);
            }

            return new PagedList<Reception_Resource>(query.OrderByDescending(p => p.AddTime), pageIndex, pageSize);
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
            });
        }
        
        //删除
        public int Delete(string id)
        {
            return base.Delete<Reception_Resource>(p => p.R_ID == id);
        }

        //获取资源分类及其资源列表
        public List<Reception_Resource_Type> GetResource_Type()
        {
            var query = from a in ContextObj.Reception_ResourceType
                        join b in ContextObj.Reception_Resource                       
                        on a.RT_ID equals b.RT_ID into ab_join
                        from x in ab_join.DefaultIfEmpty()
                        orderby a.CreateTime descending,x.AddTime descending
                        select new Reception_Resource_Type { RT_ID = a.RT_ID, RT_Name = a.Name, R_ID = x.R_ID, R_Name = x.Name, Long = x.Long, Lat = x.Lat, RRDW = x.RRDW, SFDW = x.SFDW, StartTime = x.StartTime, EndTime = x.EndTime, RStatus = x.RStatus, SSJD = x.SSJD };

            return query.ToList();
        }

        //根据资源分类删除
        public int DeleteByTypeID(string id)
        {
            return base.Delete<Reception_Resource>(p => p.RT_ID == id);
        }

        //搜索结果
        public PagedList<Reception_Resource> GetSearchPageList(Reception_Resource model, int pageIndex, int pageSize)
        {
            var query = base.Search<Reception_Resource>();

            if (!string.IsNullOrEmpty(model.Name))
            {
                query = query.Where(p => p.Name.Contains(model.Name));
            }

            return new PagedList<Reception_Resource>(query.OrderByDescending(p => p.AddTime), pageIndex, pageSize);
        }
    }
}
