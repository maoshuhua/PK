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
    public class Reception_ResourceEventService: BaseService,IReception_ResourceEventService
    {
        private IReception_ResourceEventRepository repository;

        public Reception_ResourceEventService()
        {
            repository = base.InstanceRepository<Reception_ResourceEventRepository>();
        }

        //获取全部
        public List<Reception_ResourceEvent> GetList()
        {
            return repository.GetList();
        }
        
        //获取单个
        public Reception_ResourceEvent Get(string id)
        {
            return repository.Get(id);
        }
        
        //获取全部分页
        public PagedList<Reception_ResourceEvent> GetPageList(Reception_ResourceEvent model, int pageIndex, int pageSize)
        {
            return repository.GetPageList(model,pageIndex,pageSize);
        }
        
        //添加
        public int Insert(Reception_ResourceEvent model)
        {
            return repository.Insert(model);
        }
        
        //修改
        public int Update(Reception_ResourceEvent model)
        {
            return repository.Update(model);
        }
        
        //删除
        public int Delete(string id)
        {
            return repository.Delete(id);
        }

        //搜索结果
        public PagedList<Reception_ResourceEvent> GetSearchPageList(Reception_ResourceEvent model, int pageIndex, int pageSize)
        {
            return repository.GetSearchPageList(model, pageIndex, pageSize);
        }
    }
}