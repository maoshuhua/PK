using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Tuhui.Common45.Utility;
using Tuhui.Common45.Framework;
using Tuhui.Reception.Model;
using Tuhui.Reception.Repository;

namespace Tuhui.Reception.Service
{
    public class ImageService: BaseService,IImageService
    {
        private IImageRepository repository;

        public ImageService()
        {
            repository = base.InstanceRepository<ImageRepository>();
        }

        //获取全部
        public List<Image> GetList()
        {
            return repository.GetList();
        }
        
        //获取单个
        public Image Get(string id)
        {
            return repository.Get(id);
        }
        
        //获取全部分页
        public PagedList<Image> GetPageList(Image model, int pageIndex, int pageSize)
        {
            return repository.GetPageList(model,pageIndex,pageSize);
        }
        
        //添加
        public int Insert(Image model)
        {
            return repository.Insert(model);
        }
        
        //修改
        public int Update(Image model)
        {
            return repository.Update(model);
        }
        
        //删除
        public int Delete(string id)
        {
            return repository.Delete(id);
        }

        //删除资源图片
        public int DeleteList(string id)
        {
            return repository.DeleteList(id);
        }
    }
}