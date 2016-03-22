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
    public interface IVideoService : IBaseService
    {
        //获取全部
        List<Video> GetList();
        
        //获取单个
        Video Get(string id);
        
        //获取全部分页
        PagedList<Video> GetPageList(Video model, int pageIndex, int pageSize);
        
        //添加
        int Insert(Video model);
        
        //修改
        int Update(Video model);
        
        //删除
        int Delete(string id);

        //删除资源视频
        int DeleteList(string id);
    }
}
