using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Tuhui.Common45.Framework;
using Tuhui.Common45.Utility;
using Tuhui.Reception.Model;

namespace Tuhui.Reception.Service
{
    public interface IVideoService : IBaseService
    {
        List<tv_video> GetTV_VideoList(string keyword);

        PagedList<tv_video> GetTV_VideoPagedList(string keyword, int? videosource,int? justEnable,int? videostatus, int pageIndex, int pageSize);

        void DeleteVideoById(int id);

        tv_video AddVideo(tv_video model);

        List<tv_video> SearchVideo(int videoid);

        bool EditVideoById(int videid, tv_video model);

        tv_video GetTV_VideoById(int videoid);

        List<tv_subject> GetAllSubjects(bool containVideoIds = false);

        List<int> GetVideoListBySubjectId(int subjectId);

        bool AddSubject(tv_subject model);

        bool UpdateSubject(tv_subject model);

        bool UpdateSubjectVideoList(int subjectId, List<int> videoIds);

        int DeleteSubject(int subjectId);

        string GetVideoCount(int num = 0);

        string GetVideoStatusCount(int num = 2);

        string GetSubjectVideoAllCount();

        string GetSubjectAllCount();
        //List<tv_video> GetVideoListByVideoaccess(string keyword);

    }
}
