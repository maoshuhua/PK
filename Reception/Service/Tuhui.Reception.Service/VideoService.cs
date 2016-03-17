using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Tuhui.Common45.Framework;
using Tuhui.Common45.Utility;
using Tuhui.Reception.Model;
using Tuhui.Reception.Repository;

namespace Tuhui.Reception.Service
{
    public class VideoService : BaseService, IVideoService
    {
        private IVideoRepository _videoRespository;

        public VideoService()
        {
            _videoRespository = base.InstanceRepository<VideoRepository>();
        }


        public List<tv_video> GetTV_VideoList(string keyword)
        {
            if (true == string.IsNullOrEmpty(keyword))
            {
                return _videoRespository.SearchList<tv_video>();
            }
            else
            {
                return _videoRespository.SearchList<tv_video>(p => p.videoname.Contains(keyword));
            }
        }


        //如何分页传4个参数
        public PagedList<tv_video> GetTV_VideoPagedList(string keyword, int? videosource, int? justEnable, int? videostatus, int pageIndex, int pageSize)
        {
            var query = _videoRespository.Search<tv_video>();

            if (!string.IsNullOrEmpty(keyword))
            {
                query = query.Where(p => p.videoname.Contains(keyword));
            }

            if (videosource != null)
            {
                query = query.Where(p => p.videosource == videosource.Value);
            }

            if (videostatus != null)
            {
                query = query.Where(p => p.videostatus == videostatus.Value);
            }

            if (justEnable != null)
            {
                query = query.Where(p => p.enable == justEnable);
            }

            return new PagedList<tv_video>(query.OrderByDescending(p => p.videoid), pageIndex, pageSize);
        }

        public void DeleteVideoById(int id)
        {
            _videoRespository.Delete<tv_video>(p => p.videoid == id);
        }

        public tv_video AddVideo(tv_video model)
        {
            _videoRespository.Insert<tv_video>(model);
            return model;

        }

        public List<tv_video> SearchVideo(int videid)
        {
            return _videoRespository.SearchList<tv_video>(p => p.videoid.Equals(videid));
        }

        public bool EditVideoById(int videid, tv_video model)
        {
            _videoRespository.Update<tv_video>(p => p.videoid == model.videoid, p =>
                {
                    p.videoname = model.videoname;
                    p.videolng = model.videolng;
                    p.videolat = model.videolat;
                    p.videostatus = model.videostatus;
                    p.videounit = model.videounit;
                    p.videosource = model.videosource;
                    p.videoaccess = model.videoaccess;
                    p.enable = model.enable;
                    p.videodesc = model.videodesc;
                });
            return true;
        }


        public tv_video GetTV_VideoById(int videoid)
        {
            return _videoRespository.SearchFirstOrDefault<tv_video>(p => p.videoid == videoid);
        }


        public List<tv_subject> GetAllSubjects(bool containVideoIds = false)
        {
            var _list = _videoRespository.SearchList<tv_subject>();
            if (containVideoIds == true)
            {

                foreach (var item in _list)
                {
                    item.videoIds = _videoRespository.SearchList<tv_subject_video>(p => p.subjectid == item.subjectid).Select(p => p.videoid).ToList();
                }
            }
            return _list;
        }


        public List<int> GetVideoListBySubjectId(int subjectId)
        {
            return _videoRespository.SearchList<tv_subject_video>(p => p.subjectid == subjectId).Select(p => p.videoid).ToList();
        }

        public bool AddSubject(tv_subject model)
        {
            _videoRespository.Insert<tv_subject>(model);
            return true;
        }

        public bool UpdateSubject(tv_subject model)
        {
            _videoRespository.Update<tv_subject>(model);
            return true;
        }

        public bool UpdateSubjectVideoList(int subjectId, List<int> videoIds)
        {
            _videoRespository.Delete<tv_subject_video>(p => p.subjectid == subjectId);
            if (videoIds != null)
            {
                foreach (var item in videoIds)
                {
                    _videoRespository.Insert<tv_subject_video>(new tv_subject_video { videoid = item, subjectid = subjectId });
                }
            }

            return true;

        }

        public int DeleteSubject(int subjectId)
        {
            var _subList = _videoRespository.SearchList<tv_subject>(p => p.parentid == subjectId);
            if (_subList.Count != 0) return 100;
            _videoRespository.Delete<tv_subject>(p => p.subjectid == subjectId);
            _videoRespository.Delete<tv_subject_video>(p => p.subjectid == subjectId);
            return 1;
        }

        public string GetVideoCount(int num = 0)
        {
            var video_string = "";
            if (num == 10)//交通局视频
            {
                video_string = _videoRespository.SearchList<tv_video>(p => p.videosource == 10).Count().ToString();
            }
            else if (num == 20)//320视频
            {
                video_string = _videoRespository.SearchList<tv_video>(p => p.videosource == 20).Count().ToString();
            }
            else if (num == 30)//省厅视频
            {
                video_string = _videoRespository.SearchList<tv_video>(p => p.videosource == 30 && p.enable == 1).Count().ToString();
            }
            //三个视频个数输出用','分割
            if (num == 0)
            {
                var traffic_video_count = _videoRespository.SearchList<tv_video>(p => p.videosource == 10).Count().ToString();
                var video_320_count = _videoRespository.SearchList<tv_video>(p => p.videosource == 20).Count().ToString();
                var st_video_count = _videoRespository.SearchList<tv_video>(p => p.videosource == 30 && p.enable == 1).Count().ToString();
                video_string = traffic_video_count + "," + video_320_count + "," + st_video_count;
            }
            return video_string;
        }

        public string GetVideoStatusCount(int num = 2)
        {
            var video_string = "";
            if (num == 1)//正常
            {
                video_string = _videoRespository.SearchList<tv_video>(p => p.videostatus == 1).Count().ToString();
            }
            else if (num == 0)//故障
            {
                video_string = _videoRespository.SearchList<tv_video>(p => p.videostatus == 0).Count().ToString();
            }
            else if (num == 2)
            {
                //俩个状态视频个数输出用','分割
                var ok_video_count = _videoRespository.SearchList<tv_video>(p => p.videostatus == 1).Count().ToString();
                var false_video_count = _videoRespository.SearchList<tv_video>(p => p.videostatus == 0).Count().ToString();
                video_string = ok_video_count + "," + false_video_count;
            }
            return video_string;
        }

        public string GetSubjectVideoAllCount()
        {
            return _videoRespository.SearchList<tv_subject_video>().Count().ToString();
        }

        public string GetSubjectAllCount()
        {
            return _videoRespository.SearchList<tv_subject>().Count().ToString();
        }

    }
}
