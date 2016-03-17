using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Tuhui.Common45.Mvc;
using Tuhui.Common45.Utility;
using Tuhui.Reception.Mvc;
using Tuhui.Reception.Model;
using Tuhui.Reception.Service;
using Tuhui.Reception.Utility;


namespace Tuhui.Reception.WebUI.Controllers
{
    public partial class ManagerController
    {

        public ActionResult GetAllVideo(string keyword, int? videosource, int? justEnable, int? videostatus, int pageIndex = 1, int pageSize = 10)
        {
            //分页格式返回
            return Json(_videoService.GetTV_VideoPagedList(keyword, videosource, justEnable, videostatus, pageIndex, pageSize));
            //return Json(_videoService.GetTV_VideoList(null), JsonRequestBehavior.AllowGet);
        }

        public ActionResult GetAllUser()
        {
            //return Json(_userService.GetTV_UserPagedList(keyword, pageIndex, pageSize));
            return Json(_userService.GetUserList(null), JsonRequestBehavior.AllowGet);
        }

        public ActionResult GetAllLog(string keyword, int pageIndex = 1, int pageSize = 10)
        {
            return Json(_logService.GetTV_LogPageList(keyword, pageIndex, pageSize));
        }

        public ActionResult DeleteUserById(int? id)
        {
            if (!id.HasValue)
            {
                //抛出异常
            }
            _userService.DeleteUserById(id.Value);
            return Json(true);
        }

        public ActionResult DeleteLogById(int? id)
        {
            if (!id.HasValue)
            {
                //抛出异常
            }
            _logService.DeleteLogById(id.Value);
            return Json(true);
        }

        public ActionResult DeleteVideoById(int? id)
        {
            if (!id.HasValue)
            {
                //抛出异常
            }
            _videoService.DeleteVideoById(id.Value);
            return Json(true);
        }

        public ActionResult AddVideo(string videoname, double videolng, double videolat, int videostatus, string videounit, int videosource, string videoaccess, string videodesc)
        {
            tv_video model = new tv_video();
            model.videoname = videoname;
            model.videolng = videolng;
            model.videolat = videolat;
            model.videostatus = videostatus;
            model.videounit = videounit;
            model.videosource = videosource;
            model.videoaccess = videoaccess;
            model.videodesc = videodesc;

            _videoService.AddVideo(model);
            return Json(true);
        }

        //通过videoid查找videolist属性
        public ActionResult SearchVideo(int? videoid)
        {
            if (!videoid.HasValue)
            {
                //抛出异常
            }

            return Json(_videoService.SearchVideo(videoid.Value));
        }

        //编辑video信息
        public ActionResult EditVideoById(int? videoid, string videoname, double videolng, double videolat, int videostatus, string videounit, int videosource, string videoaccess, string videodesc)
        {
            if (!videoid.HasValue)
            {
                //抛出异常
            }
            tv_video model = new tv_video();
            model.videoname = videoname;
            model.videolng = videolng;
            model.videolat = videolat;
            model.videostatus = videostatus;
            model.videounit = videounit;
            model.videosource = videosource;
            model.videoaccess = videoaccess;

            _videoService.EditVideoById(videoid.Value, model);
            return Json(true);
        }

        public ActionResult GetAllSubjects(bool containVideoIds = false)
        {
            return Json(_videoService.GetAllSubjects(containVideoIds));
        }

        public ActionResult GetVideoListBySubjectId(int subjectId)
        {
            return Json(_videoService.GetVideoListBySubjectId(subjectId));
        }

        public ActionResult AddSubject(tv_subject model)
        {
            return Json(_videoService.AddSubject(model));
        }

        public ActionResult UpdateSubject(tv_subject model)
        {
            return Json(_videoService.UpdateSubject(model));
        }

        public ActionResult DeleteSubject(int subjectId)
        {
            var _state = _videoService.DeleteSubject(subjectId);
            return Json(_state);
        }

        public ActionResult UpdateSubjectVideoList(int subjectId, List<int> videoIds)
        {
            return Json(_videoService.UpdateSubjectVideoList(subjectId, videoIds));
        }

        public ActionResult GetAllFace(string keyword)
        {
            return Json(_faceService.GetTVFaceList(null), JsonRequestBehavior.AllowGet);
            //return Json(_faceService.GetTV_FacePagedList(keyword, pageIndex, pageSize));
        }

        public ActionResult DeleteFaceById(int? faceid)
        {
            _faceService.DeleteFaceById(faceid.Value);
            return Json(true);
        }

        public ActionResult SearchVideoByAccessKeyword(string keyword = null)
        {
            return Json(_videoService.GetTV_VideoList(keyword), JsonRequestBehavior.AllowGet);
        }

        public ActionResult UpdateYcVideoNeedUpdate(string Camera_name, string ID, double longitude, double latitude)
        {
            return Json(_videoService.AddVideo(new tv_video
            {
                videoname = Camera_name,
                videolng = longitude,
                videolat = latitude,
                videostatus = 1,
                videounit = "云创320视频",
                videosource = 20,
                videoaccess = ID,
                enable = 1,
                thirdid = ID
            }));
        }

        public ActionResult GetStatistic()
        {
            //实时数据
            var _result1 = sssj();

            //区域指标分析
            var _result2 = qyzbfx();

            //来源指标分析
            var _result3 = lyzbfx();

            //视频有效率
            var _result4 = spyxl();

            //专题视频分析
            var _result5 = ztspfx();

            return Json(new
            {
                data1 = _result1,
                data2 = _result2,
                data3 = _result3,
                data4 = _result4,
                data5 = _result5
            });

        }

        #region 实时数据
        private List<StatisticModel> sssj()
        {
            var _result1 = new List<StatisticModel>();
            var num = _videoService.GetVideoCount();
            var temp = num.Split(',');
            _result1.Add(new StatisticModel
            {
                name = "320视频",
                value = temp[1],
                color = "#ec6e51"
            });
            _result1.Add(new StatisticModel
            {
                name = "交通视频",
                value = temp[0],
                color = "#f8f3f0"
            });
            _result1.Add(new StatisticModel
            {
                name = "省厅视频",
                value = temp[2],
                color = "#4572a7"
            });
            return _result1;
        }
        #endregion

        #region 区域指标分析
        private List<StatisticModel> qyzbfx()
        {
            var _result2 = new List<StatisticModel>();
            _result2.Add(new StatisticModel
            {
                name = "鼓楼区",
                value = "60",
                color = "#c00a0a"
            });
            _result2.Add(new StatisticModel
            {
                name = "秦淮区",
                value = "82",
                color = "#c0600a"
            });
            _result2.Add(new StatisticModel
            {
                name = "玄武区",
                value = "38",
                color = "#c0a80a"
            });
            _result2.Add(new StatisticModel
            {
                name = "建邺区",
                value = "58",
                color = "#a8c00a"
            });
            _result2.Add(new StatisticModel
            {
                name = "雨花区",
                value = "82",
                color = "#5bc00a"
            });
            _result2.Add(new StatisticModel
            {
                name = "栖霞区",
                value = "45",
                color = "#0ac0a0"
            });
            _result2.Add(new StatisticModel
            {
                name = "浦口区",
                value = "7",
                color = "#0aa4c0"
            });
            _result2.Add(new StatisticModel
            {
                name = "江宁区",
                value = "15",
                color = "#0a64c0"
            });
            _result2.Add(new StatisticModel
            {
                name = "六合区",
                value = "22",
                color = "#0a2cc0"
            });
            _result2.Add(new StatisticModel
            {
                name = "溧水区",
                value = "2",
                color = "#5b0ac0"
            });
            _result2.Add(new StatisticModel
            {
                name = "高淳区",
                value = "2",
                color = "#c00a71"
            });

            return _result2;
        }
        #endregion

        #region 来源指标分析
        private List<StatisticModel> lyzbfx()
        {
            var _result3 = new List<StatisticModel>();
            var num = _videoService.GetVideoCount();
            var temp = num.Split(',');
            _result3.Add(new StatisticModel
            {
                name = "320视频",
                value = temp[1],
                color = "#8fa3c1"
            });
            _result3.Add(new StatisticModel
            {
                name = "交通视频",
                value = temp[0],
                color = "#afd192"
            });
            _result3.Add(new StatisticModel
            {
                name = "省厅视频",
                value = temp[2],
                color = "#aa4643"
            });
            return _result3;
        }
        #endregion

        #region 视频有效率
        private List<StatisticModel> spyxl()
        {
            var _result4 = new List<StatisticModel>();
            var num = _videoService.GetVideoStatusCount();
            var temp = num.Split(',');
            _result4.Add(new StatisticModel
            {
                name = "正常",
                value = temp[0],
                color = "#8fa3c1"
            });
            _result4.Add(new StatisticModel
            {
                name = "异常",
                value = temp[1],
                color = "#afd192"
            });
            return _result4;
        }
        #endregion

        #region 专题视频分析
        private List<StatisticModel> ztspfx()
        {
            var _result5 = new List<StatisticModel>();
            //var parentid_num = _faceService.GetParentSubjectList().ToList().Count();
            //var parentid = _faceService.GetParentSubjectList();
            ////int videocount = 0;
            //for (int i = 0; i < parentid_num; i++)
            //{
            //    var videocount = 0;
            //    var childlist = _faceService.GetChildSubjectListByParent(parentid[i].subjectid);
            //    for (int j = 0; j < childlist.ToList().Count(); j++)
            //    {
            //        var child_temp = _faceService.GetChildSubjectListByParent(parentid[i].subjectid);

            //        if (child_temp.Count() == 0)
            //        {
            //            videocount += _faceService.GetVideoListBySubjectId(childlist[j].subjectid).Count();
            //        }
            //        else
            //        {
            //            videocount += SearchChildVideoCount(childlist[j].subjectid);
            //        }
            //        //videocount += _faceService.GetVideoListBySubjectId(childlist[j].subjectid).ToList().Count();
            //    }
            //    _result5.Add(new StatisticModel
            //    {
            //        name = parentid[i].subjectname,//parentid[i].subjectname
            //        value = videocount.ToString(),//videocount
            //        color = yanse()
            //    });
            //    videocount = 0;
            //}

            var Parentsubjectnum = _faceService.GetParentSubjectList().Count();
            var parent = _faceService.GetParentSubjectList();
            for (int i = 0; i < Parentsubjectnum; i++)
            {
                var videocount = 0;
                var childlist = _faceService.GetChildSubjectListByParent(parent[i].subjectid);
                //videocount = abc(childlist,0,0,childlist.Count());
                for (int j = 0; j < childlist.Count(); j++)
                {
                    var child_temp = _faceService.GetChildSubjectListByParent(childlist[j].subjectid);
                    //判断向下有没有节点
                    if (child_temp.Count() == 0)
                    {
                        videocount += _faceService.GetVideoListBySubjectId(childlist[j].subjectid).Count();
                    }
                    else
                    {
                        videocount += SearchChildVideoCount(childlist[j].subjectid);
                    }
                }

                _result5.Add(new StatisticModel
                {
                    name = parent[i].subjectname,//parentid[i].subjectid
                    value = videocount.ToString(),//videocount
                    color = yanse()
                });
                videocount = 0;
            }
            return _result5;
        }
        #endregion


        private string yanse()
        {
            Random ran = new Random();
            double RandKey = ran.NextDouble();
            long a = (Int64)Math.Round(RandKey * 16777215);

            var str = Convert.ToString(a, 16);
            while (str.Length < 6)
            {
                str = '0' + str;
            }
            return '#' + str;
        }

        //获得parentid为null的专题信息
        public ActionResult GetParentSubject()
        {
            return Json(_faceService.GetParentSubjectList());
        }

        //获得视频个数根据不同视频分类
        public ActionResult GetVideoCount()
        {
            return Json(_videoService.GetVideoCount());
        }

        public ActionResult GetProvinceVideoApi()
        {
            var _result = WebRequestHandle.Get(Constant.ProvinceVideoResourceAPI);
            if (_result.Success == false)
            {
                return Json(new { error = "Error: Api Error!<br/> 地址:【" + Constant.ProvinceVideoResourceAPI + "】" });
            }
            else
            {
                var aaa = this.SerializeXMLFromProvinceAPI(_result.Response);
                //foreach (var item in aaa)
                //{
                //    _videoService.AddVideo(item);
                //}
                return Json(aaa);
            }
        }

        public ActionResult GetParentSubjectVideoCount()
        {
            var video = _videoService.GetVideoCount();
            var subjectvideoallcount = _videoService.GetSubjectAllCount();
            var _result = new List<StatisticModel>();
            var Parentsubjectnum = _faceService.GetParentSubjectList().Count();
            var parent = _faceService.GetParentSubjectList();
            for (int i = 0; i < Parentsubjectnum; i++)
            {
                var videocount = 0;
                var childlist = _faceService.GetChildSubjectListByParent(parent[i].subjectid);
                //videocount = abc(childlist,0,0,childlist.Count());
                for (int j = 0; j < childlist.Count(); j++)
                {
                    var child_temp = _faceService.GetChildSubjectListByParent(childlist[j].subjectid);
                    //判断向下有没有节点
                    if (child_temp.Count() == 0)
                    {
                        videocount += _faceService.GetVideoListBySubjectId(childlist[j].subjectid).Count();
                    }
                    else
                    {
                        videocount += SearchChildVideoCount(childlist[j].subjectid);
                    }
                }

                _result.Add(new StatisticModel
                {
                    name = parent[i].subjectid.ToString(),//parentid[i].subjectid
                    value = videocount.ToString(),//videocount
                    color = "#ffffff"
                });
                videocount = 0;
            }
            return Json(new
            {
                data = _result,
                data1 = video,
                data2 = subjectvideoallcount
            });
        }

        private int SearchChildVideoCount(int num)
        {
            var videocount = 0;
            var childlist = _faceService.GetChildSubjectListByParent(num);
            for (int j = 0; j < childlist.Count(); j++)
            {
                var child_temp = _faceService.GetChildSubjectListByParent(childlist[j].subjectid);
                //判断向下有没有节点
                if (child_temp.Count() == 0)
                {
                    videocount += _faceService.GetVideoListBySubjectId(childlist[j].subjectid).Count();
                }
                else
                {
                    SearchChildVideoCount(childlist[j].subjectid);
                }
            }
            return videocount;
        }

    }

}