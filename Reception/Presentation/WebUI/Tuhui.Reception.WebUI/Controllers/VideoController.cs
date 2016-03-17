using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Tuhui.Common45.Mvc;
using Tuhui.Common45.Utility;
using Tuhui.Reception.Service;

namespace Tuhui.Reception.WebUI.Controllers
{
    public class VideoController : BaseController
    {
        private IVideoService _videoService;
        public VideoController()
        {
            _videoService = base.InstanceService<VideoService>();
        }
        public ActionResult Index(string s_id)
        {
            var list = _videoService.GetAllSubjects(true);
            var s_id_List = new List<int>();

            if (!string.IsNullOrWhiteSpace(s_id))
            {
                var _arr = s_id.Split(',');
                for (int i = 0; i < _arr.Length; i++)
                {
                    var _k = 0;
                    if (int.TryParse(_arr[i], out _k))
                    {
                        s_id_List.Add(_k);
                    }
                }
            }

            ViewBag.s_id_list = s_id_List;

            return View(list);
        }

        public ActionResult Mock()
        {
            return View();
        }

        public ActionResult PlayVideo(int videoid)
        {
            var _video = _videoService.GetTV_VideoById(videoid);

            if (_video == null) return Content("videoid不存在,请联系管理员!");

            if (_video.videosource == 20)
            {
                return View("PlayVideo_YCVLC",_video);
            }
            else if (_video.videosource == 30)
            {
                return View("PlayVideo_STVideo", _video);
            }
            else
            {
                if (!string.IsNullOrWhiteSpace(_video.videoaccess)&&_video.videoaccess.StartsWith("rtsp"))
                {
                    return View("PlayVideo_VLC_rtmp", _video);
                }
                else if (!string.IsNullOrWhiteSpace(_video.videodesc) && _video.videodesc.Contains("海康视频"))
                {
                    return View("PlayVideo_HaiKang", _video);
                }
                else
                {
                    return Redirect(_video.videoaccess);
                }
            }

            return View();
        }

        public ActionResult YCVLC()
        {
            return View();
        }

        public ActionResult YCVideo(string cameraId)
        {
            var _result = WebRequestHandle.Get("http://10.101.10.11:8080/cVideoMonitor/mobileAddressAjax.action", new
            {
                userAccount = "0mJ/BErVUAacv75XuBqKXQ==",
                userPassword = "0mJ/BErVUAacv75XuBqKXQ==",
                cameraId = cameraId,
                decodeType = "copy",
                isPublic = 1
            });
            return Content(_result.Response);
        }

        public ActionResult STVIDEO()
        {
            return View();
        }

        public ActionResult STVIDEO2()
        {
            return View();
        }

        public ActionResult STVIDEO2_GetUrl(string cameraId)
        {
            var _result = WebRequestHandle.Get("http://59.201.8.107:8123/" + cameraId + "?asdfas");
            return Content(_result.Response);
        }
    }
}