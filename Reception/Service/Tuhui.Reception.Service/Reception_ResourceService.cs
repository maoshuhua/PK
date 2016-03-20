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
    public class Reception_ResourceService: BaseService,IReception_ResourceService
    {
        private IReception_ResourceRepository repository;

        public Reception_ResourceService()
        {
            repository = base.InstanceRepository<Reception_ResourceRepository>();
        }

        //获取全部
        public List<Reception_Resource> GetList()
        {
            return repository.GetList();
        }
        
        //获取单个
        public Reception_Resource Get(string id)
        {
            return repository.Get(id);
        }
        
        //获取全部分页
        public PagedList<Reception_Resource> GetPageList(Reception_Resource model, int pageIndex, int pageSize)
        {
            return repository.GetPageList(model,pageIndex,pageSize);
        }
        
        //添加
        public int Insert(Reception_Resource model)
        {
            return repository.Insert(model);
        }
        
        //修改
        public int Update(Reception_Resource model)
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