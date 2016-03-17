using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web.Mvc;
using System.Net;
using System.Collections.Specialized;
using System.Xml;
using Tuhui.Common45.Framework;
using Tuhui.Reception.Model;

namespace Tuhui.Reception.Mvc
{
    public static partial class ReceptionHtmlHelperExtension
    {
        public static List<SelectListItem> GetAllTypeGroups(this HtmlHelper helper)
        {
            var result = new List<SelectListItem>();
            //result.Add(new SelectListItem{Text = "封面时钟",Value = "1"});
            result.Add(new SelectListItem { Text = "专题封面", Value = "2" });
            result.Add(new SelectListItem { Text = "图片封面", Value = "3" });
            result.Add(new SelectListItem { Text = "视频封面", Value = "4" });
            return result;
        }

        public static List<SelectListItem> GetAllSubjectGroups(this HtmlHelper helper)
        {
            var result = _faceBL.GetParentSubjectList().Select(p => new SelectListItem
            {
                Value = p.subjectid.ToString(),
                Text = p.subjectname,
                //Selected = false
            }).ToList();
            //result.Insert(0, new SelectListItem
            //{
            //    Text = "所有专题",
            //    Value = "",
            //    Selected = true
            //});
            return result;
        }

        public static List<tv_subject> GetSubjectTreeData(this HtmlHelper helper, List<tv_subject> list, List<int> subjectids)
        {

            List<tv_subject> _resultList = null;
            if (subjectids.Count == 0)
            {
                _resultList = list.Where(p => p.parentid == null).ToList();
            }
            else
            {
                _resultList = list.Where(p => p.parentid == null && subjectids.Contains(p.subjectid)).ToList();
            }
            _resultList.ForEach(p =>
            {
                GetSubjectTreeDataForeach(p, list, 1);
            });
            return _resultList;
        }

        private static void GetSubjectTreeDataForeach(tv_subject model, List<tv_subject> source, int level)
        {
            model.children = source.Where(p => p.parentid == model.subjectid).ToList();
            model.children.ForEach(p =>
            {
                p.treeLevel = level;
                GetSubjectTreeDataForeach(p, source, level + 1);
            });
        }
    }
}
