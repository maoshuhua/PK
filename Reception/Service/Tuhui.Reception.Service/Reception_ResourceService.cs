using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Tuhui.Common45.Framework;
using Tuhui.Common45.Utility;
using Tuhui.Reception.Model;
using Tuhui.Reception.Repository;

namespace Tuhui.Reception.Service
{
    public class Reception_ResourceService : BaseService, IReception_ResourceService
    {
        private IReception_ResourceRepository _reception_ResourceRepository;

        public Reception_ResourceService()
        {
            _reception_ResourceRepository = base.InstanceRepository<Reception_ResourceRepository>();
        }

        //获取资源列表
        public PagedList<Reception_Resource> GetResourceList(string keyword, int pageIndex, int pageSize)
        {
            return _reception_ResourceRepository.GetResourceList(keyword, pageIndex, pageSize);
        }
    }
}
