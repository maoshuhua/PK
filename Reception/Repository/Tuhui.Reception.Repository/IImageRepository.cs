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
    public interface IImageRepository : IBaseRepository<DbReceptionContext>
    {
        //获取全部
        List<Image> GetList();
        
        //获取单个
        Image Get(string id);
        
        //获取全部分页
        PagedList<Image> GetPageList(Image model, int pageIndex, int pageSize);
        
        //添加
        int Insert(Image model);
        
        //修改
        int Update(Image model);
        
        //删除
        int Delete(string id);

        //删除资源图片
        int DeleteList(string id);

        //根据外键获取图片列表
        List<Image> GetList(string id);
    }
}