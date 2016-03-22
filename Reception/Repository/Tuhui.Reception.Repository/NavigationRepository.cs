﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Tuhui.Common45.Framework;
using Tuhui.Common45.Utility;
using Tuhui.Reception.Model;

namespace Tuhui.Reception.Repository
{
    public class NavigationRepository : BaseRepository<DbReceptionContext>, INavigationRepository
    {
        //获取全部
        public List<Navigation> GetList()
        {
            var list = base.SearchList<Navigation>().ToList();

            return list;
        }
        
        //获取单个
        public Navigation Get(string id)
        {
            return base.SearchFirstOrDefault<Navigation>(p => p.N_ID == id);
        }
        
        //获取全部分页
        public PagedList<Navigation> GetPageList(Navigation model, int pageIndex, int pageSize)
        {
            var query = base.Search<Navigation>();

            if (model != null)
            {
                //根据条件筛选

            }
            
            return new PagedList<Navigation>(query.OrderByDescending(p => p.N_ID), pageIndex, pageSize);
        }
        
        //添加
        public int Insert(Navigation model)
        {
            return base.Insert<Navigation>(model);
        }
        
        //修改
        public int Update(Navigation model)
        {
            return base.Update<Navigation>(p => p.N_ID == model.N_ID, p => {
                p.N_ID = model.N_ID;
                p.Obj_ID = model.Obj_ID;
                p.Name = model.Name;
                p.GHLJ = model.GHLJ;
                p.AddTime = model.AddTime;
            });
        }
        
        //删除
        public int Delete(string id)
        {
            return base.Delete<Navigation>(p => p.N_ID == id);
        }
    }
}
