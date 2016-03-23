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
    public class VideoService: BaseService,IVideoService
    {
        private IVideoRepository repository;

        public VideoService()
        {
            repository = base.InstanceRepository<VideoRepository>();
        }

        //获取全部
        public List<Video> GetList()
        {
            return repository.GetList();
        }
        
        //获取单个
        public Video Get(string id)
        {
            return repository.Get(id);
        }
        
        //获取全部分页
        public PagedList<Video> GetPageList(Video model, int pageIndex, int pageSize)
        {
            return repository.GetPageList(model,pageIndex,pageSize);
        }
        
        //添加
        public int Insert(Video model)
        {
            return repository.Insert(model);
        }
        
        //修改
        public int Update(Video model)
        {
            return repository.Update(model);
        }
        
        //删除
        public int Delete(string id)
        {
            return repository.Delete(id);
        }

        //删除资源视频
        public int DeleteList(string id)
        {
            return repository.DeleteList(id);
        }

        //根据外键获取视频列表
        public List<Video> GetList(string id)
        {
            return repository.GetList(id);
        }
    }
}