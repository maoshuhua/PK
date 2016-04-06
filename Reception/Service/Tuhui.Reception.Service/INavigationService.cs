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
    public interface INavigationService : IBaseService
    {
        //获取全部
        List<Navigation> GetList();
        
        //获取单个
        Navigation Get(string id);
        
        //获取全部分页
        PagedList<Navigation> GetPageList(Navigation model, int pageIndex, int pageSize);
        
        //添加
        int Insert(Navigation model);
        
        //修改
        int Update(Navigation model);
        
        //删除
        int Delete(string id);

        //获取最新一条导航
        Navigation GetNewest();
    }
}
