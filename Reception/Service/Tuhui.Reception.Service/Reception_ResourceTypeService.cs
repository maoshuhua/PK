using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Tuhui.Common45.Framework;
using Tuhui.Reception.Model;
using Tuhui.Reception.Repository;

namespace Tuhui.Reception.Service
{
    public class Reception_ResourceTypeService: BaseService,IReception_ResourceTypeService
    {
        private IReception_ResourceTypeRepository _reception_ResourceTypeRepository;

        public Reception_ResourceTypeService()
        {
            _reception_ResourceTypeRepository = base.InstanceRepository<Reception_ResourceTypeRepository>();
        }

        //获取资源分类列表
        public List<Reception_ResourceType> GetResourceTypeList()
        {
            return _reception_ResourceTypeRepository.GetResourceTypeList();
        }

        //添加资源分类
        public int ResourceTypeInsert(Reception_ResourceType model)
        {
            return _reception_ResourceTypeRepository.ResourceTypeInsert(model);
        }

        //修改资源分类
        public int ResourceTypeUpdate(Reception_ResourceType model)
        {
            return _reception_ResourceTypeRepository.ResourceTypeUpdate(model);
        }

        //获取单个资源分类
        public Reception_ResourceType GetResourceTypeById(string id) 
        {
            return _reception_ResourceTypeRepository.GetResourceTypeById(id);
        }

        //删除资源分类
        public int ResourceTypeDelete(string id)
        {
            return _reception_ResourceTypeRepository.ResourceTypeDelete(id);
        }
    }
}
