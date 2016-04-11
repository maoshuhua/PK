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
    public interface IReception_ResourceService : IBaseService
    {
        //获取全部
        List<Reception_Resource> GetList();
        
        //获取单个
        Reception_Resource Get(string id);
        
        //获取全部分页
        PagedList<Reception_Resource> GetPageList(Reception_Resource model, int pageIndex, int pageSize);
        
        //添加
        int Insert(Reception_Resource model);
        
        //修改
        int Update(Reception_Resource model);
        
        //删除
        int Delete(string id);

        //获取资源分类及其资源列表
        List<Reception_Resource_Type> GetResource_Type();

        //根据资源分类删除
        int DeleteByTypeID(string id);

        //搜索结果
        PagedList<Reception_Resource> GetSearchPageList(Reception_Resource model, int pageIndex, int pageSize);
    }
}
