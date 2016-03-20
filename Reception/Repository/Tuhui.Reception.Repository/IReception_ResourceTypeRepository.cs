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
    public interface IReception_ResourceTypeRepository : IBaseRepository<DbReceptionContext>
    {
        //获取全部
        List<Reception_ResourceType> GetList();
        
        //获取单个
        Reception_ResourceType Get(string id);
        
        //获取全部分页
        PagedList<Reception_ResourceType> GetPageList(Reception_ResourceType model, int pageIndex, int pageSize);
        
        //添加
        int Insert(Reception_ResourceType model);
        
        //修改
        int Update(Reception_ResourceType model);
        
        //删除
        int Delete(string id);
    }
}