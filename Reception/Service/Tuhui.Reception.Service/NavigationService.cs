using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Tuhui.Common45.Utility;
using Tuhui.Common45.Framework;
using Tuhui.Reception.Model;
using Tuhui.Reception.Repository;

namespace Tuhui.Reception.Service
{
    public class NavigationService: BaseService,INavigationService
    {
        private INavigationRepository repository;

        public NavigationService()
        {
            repository = base.InstanceRepository<NavigationRepository>();
        }

        //获取全部
        public List<Navigation> GetList()
        {
            return repository.GetList();
        }
        
        //获取单个
        public Navigation Get(string id)
        {
            return repository.Get(id);
        }
        
        //获取全部分页
        public PagedList<Navigation> GetPageList(Navigation model, int pageIndex, int pageSize)
        {
            return repository.GetPageList(model,pageIndex,pageSize);
        }
        
        //添加
        public int Insert(Navigation model)
        {
            return repository.Insert(model);
        }
        
        //修改
        public int Update(Navigation model)
        {
            return repository.Update(model);
        }
        
        //删除
        public int Delete(string id)
        {
            return repository.Delete(id);
        }

        //获取最新一条导航
        public Navigation GetNewest()
        {
            return repository.GetNewest();
        }
    }
}