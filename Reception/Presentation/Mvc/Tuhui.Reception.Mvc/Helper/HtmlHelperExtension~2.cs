using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web.Mvc;
using System.Data;
using Tuhui.Common45.Utility;
using Tuhui.Reception.Model;
using System.Xml;

namespace Tuhui.Reception.Mvc
{
    public static partial class ReceptionHtmlHelperExtension
    {
        public static List<tv_video> SerializeXMLFromProvinceAPI(this Controller controller, string xml)
        {
            var XMLReader = new XMLReader(xml);
            var _baseNode = XMLReader.FindNode("江苏省交通厅");
            var _resultList = new List<tv_video>();
            foreach (XmlNode item in _baseNode.ChildNodes)
            {
                if (!item.Name.Contains("宁") && !item.Name.Contains("南京")) continue;
                foreach (XmlNode subItem in item.ChildNodes)
                {
                    var _cameraID = subItem.ChildNodes[0].InnerText;
                    var _cameraName = subItem.ChildNodes[1].InnerText;
                    var _rtspurl = subItem.ChildNodes[2].InnerText;

                    _resultList.Add(new tv_video
                    {
                        videoname = _cameraName,
                        videolng = 0,
                        videolat = 0,
                        videostatus = 1,
                        videounit = "江苏省交通厅",
                        videosource = 30,
                        videoaccess = _rtspurl,
                        enable = 0,
                        videodesc = item.Name,
                        thirdid = _cameraID
                    });

                }
            }
            return _resultList;
        }
    }
}
