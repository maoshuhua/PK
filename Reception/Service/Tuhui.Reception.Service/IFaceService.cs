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
    public interface IFaceService:IBaseService
    {
        PagedList<tv_face> GetTV_FacePagedList(string keyword, int pageIndex, int pageSize);

        tv_face AddFace(tv_face model);

        tv_face GetTV_FaceById(int faceid);

        bool EditFaceById(int faceid, tv_face model);

        void DeleteFaceById(int faceid);

        List<tv_face> GetTVFaceList(string keyword);

        List<tv_subject> GetParentSubjectList();

        List<tv_subject> GetChildSubjectListByParent(int parent);

        List<tv_subject_video> GetVideoListBySubjectId(int temp);

        List<tv_busLine> GetBusLine(string lineCode, string directCode);
    }
}
