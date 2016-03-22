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
    public class ImageRepository : BaseRepository<DbReceptionContext>, IImageRepository
    {
        //获取全部
        public List<Image> GetList()
        {
            var list = base.SearchList<Image>().ToList();

            return list;
        }
        
        //获取单个
        public Image Get(string id)
        {
            return base.SearchFirstOrDefault<Image>(p => p.I_ID == id);
        }
        
        //获取全部分页
        public PagedList<Image> GetPageList(Image model, int pageIndex, int pageSize)
        {
            var count = base.Search<Image>().Count();

            PagedList<Image> pageList = new PagedList<Image>(count, pageIndex, pageSize);

            if (model != null)
            {
                //根据条件筛选

            }
            else
            {
                pageList.PageData = base.Search<Image>().OrderByDescending(p => p.I_ID).Skip((pageIndex - 1) * pageSize).Take(pageSize).ToList();
            }

            return pageList;
        }
        
        //添加
        public int Insert(Image model)
        {
            return base.Insert<Image>(model);
        }
        
        //修改
        public int Update(Image model)
        {
            return base.Update<Image>(p => p.I_ID == model.I_ID, p => {
                p.I_ID = model.I_ID;
                p.Obj_ID = model.Obj_ID;
                p.ImagePath = model.ImagePath;
                p.ImageSource = model.ImageSource;
                p.AddTime = model.AddTime;
            });
        }
        
        //删除
        public int Delete(string id)
        {
            return base.Delete<Image>(p => p.I_ID == id);
        }

        //删除资源图片
        public int DeleteList(string id)
        {
            return base.Delete<Image>(p => p.Obj_ID == id);
        }
    }
}
