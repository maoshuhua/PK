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
    public class FaceService : BaseService, IFaceService
    {
        private IFaceRepository _faceRespository;

        public FaceService()
        {
            _faceRespository = base.InstanceRepository<FaceRepository>();
        }

        public PagedList<tv_face> GetTV_FacePagedList(string keyword, int pageIndex, int pageSize)
        {
            var query = _faceRespository.Search<tv_face>();

            if (!string.IsNullOrEmpty(keyword))
            {
                query = query.Where(p => p.facecontent.Contains(keyword));
            }

            return new PagedList<tv_face>(query.OrderByDescending(p => p.faceid), pageIndex, pageSize);
        }

        public tv_face AddFace(tv_face model)
        {
            _faceRespository.Insert<tv_face>(model);
            return model;
        }

        public tv_face GetTV_FaceById(int faceid)
        {
            return _faceRespository.SearchFirstOrDefault<tv_face>(p => p.faceid == faceid);
        }

        public bool EditFaceById(int faceid, tv_face model)
        {
            _faceRespository.Update<tv_face>(p => p.faceid == model.faceid, p =>
            {
                p.facecontent = model.facecontent;
                p.facesize = model.facesize;
                p.sortno = model.sortno;
                p.facetype = model.facetype;
            });
            return true;
        }

        public void DeleteFaceById(int faceid)
        {
            _faceRespository.Delete<tv_face>(p => p.faceid == faceid);
        }

        public List<tv_face> GetTVFaceList(string keyword)
        {
            if (true == string.IsNullOrEmpty(keyword))
            {
                return _faceRespository.SearchList<tv_face>().OrderBy(p => p.sortno).ToList();
            }
            else
            {
                return _faceRespository.SearchList<tv_face>(p => p.facecontent.Contains(keyword)).OrderBy(p => p.sortno).ToList();
            }
        }

        public List<tv_subject> GetParentSubjectList()
        {
            return _faceRespository.SearchList<tv_subject>(p => p.parentid == null);
        }

        public List<tv_subject> GetChildSubjectListByParent(int parent)
        {
            return _faceRespository.SearchList<tv_subject>(p => p.parentid == parent);
        }

        public List<tv_subject_video> GetVideoListBySubjectId(int temp)
        {
            return _faceRespository.SearchList<tv_subject_video>(p => p.subjectid == temp);
        }

        public List<tv_busLine> GetBusLine(string lineCode, string directCode)
        {
            return _faceRespository.SearchList<tv_busLine>(p => p.lineCode == lineCode && p.upOrDownCode == directCode).OrderBy(p=>p.sort).ToList();
        }
    }
}

