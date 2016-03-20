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
    public class Reception_ResourceTypeService: BaseService,IReception_ResourceTypeService
    {
        private IReception_ResourceTypeRepository repository;

        public Reception_ResourceTypeService()
        {
            repository = base.InstanceRepository<Reception_ResourceTypeRepository>();
        }

        //获取全部
        public List<Reception_ResourceType> GetList()
        {
            return repository.GetList();
        }
        
        //获取单个
        public Reception_ResourceType Get(string id)
        {
            return repository.Get(id);
        }
        
        //获取全部分页
        public PagedList<Reception_ResourceType> GetPageList(Reception_ResourceType model, int pageIndex, int pageSize)
        {
            return repository.GetPageList(model,pageIndex,pageSize);
        }
        
        //添加
        public int Insert(Reception_ResourceType model)
        {
            return repository.Insert(model);
        }
        
        //修改
        public int Update(Reception_ResourceType model)
        {
            return repository.Update(model);
        }
        
        //删除
        public int Delete(string id)
        {
            return repository.Delete(id);
        }
    }
}