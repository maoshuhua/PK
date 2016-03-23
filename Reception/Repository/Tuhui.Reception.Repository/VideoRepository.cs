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
    public class VideoRepository : BaseRepository<DbReceptionContext>, IVideoRepository
    {
        //获取全部
        public List<Video> GetList()
        {
            var list = base.SearchList<Video>().ToList();

            return list;
        }
        
        //获取单个
        public Video Get(string id)
        {
            return base.SearchFirstOrDefault<Video>(p => p.V_ID == id);
        }
        
        //获取全部分页
        public PagedList<Video> GetPageList(Video model, int pageIndex, int pageSize)
        {
            var count = base.Search<Video>().Count();

            PagedList<Video> pageList = new PagedList<Video>(count, pageIndex, pageSize);

            if (model != null)
            {
                //根据条件筛选

            }
            else
            {
                pageList.PageData = base.Search<Video>().OrderByDescending(p => p.V_ID).Skip((pageIndex - 1) * pageSize).Take(pageSize).ToList();
            }

            return pageList;
        }
        
        //添加
        public int Insert(Video model)
        {
            return base.Insert<Video>(model);
        }
        
        //修改
        public int Update(Video model)
        {
            return base.Update<Video>(p => p.V_ID == model.V_ID, p => {
                p.V_ID = model.V_ID;
                p.Obj_ID = model.Obj_ID;
                p.Name = model.Name;
                p.VideoPath = model.VideoPath;
                p.AddTime = model.AddTime;
                p.VideoSource = model.VideoSource;
            });
        }
        
        //删除
        public int Delete(string id)
        {
            return base.Delete<Video>(p => p.V_ID == id);
        }

        //删除资源视频
        public int DeleteList(string id)
        {
            return base.Delete<Video>(p => p.Obj_ID == id);
        }

        //根据外键获取视频列表
        public List<Video> GetList(string id)
        {
            var list = base.SearchList<Video>(p => p.Obj_ID == id).ToList();

            return list;
        }
    }
}
