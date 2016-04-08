using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Tuhui.Common45.Utility;
using Tuhui.Common45.Framework;
using Tuhui.Reception.Model;

namespace Tuhui.Reception.Service
{
    public interface IReception_ResourceEventService : IBaseService
    {
        //获取全部
        List<Reception_ResourceEvent> GetList();
        
        //获取单个
        Reception_ResourceEvent Get(string id);
        
        //获取全部分页
        PagedList<Reception_ResourceEvent> GetPageList(Reception_ResourceEvent model, int pageIndex, int pageSize);
        
        //添加
        int Insert(Reception_ResourceEvent model);
        
        //修改
        int Update(Reception_ResourceEvent model);
        
        //删除
        int Delete(string id);

        //搜索结果
        PagedList<Reception_ResourceEvent> GetSearchPageList(Reception_ResourceEvent model, int pageIndex, int pageSize);
    }
}
