using System;
using System.Web.Mvc;
using Tuhui.Common45.Utility;

namespace Tuhui.Common45.Mvc
{
    /// =======================================================================
    /// 类名：PagerExtensions
    /// <summary>
    /// 分页扩展帮助类
    /// </summary>
    /// <remarks>
    /// 分页扩展帮助类
    /// </remarks>
    /// =======================================================================
    /// 更新履历
    /// 序号          修改日期          责任人           更新内容
    /// 001           2011/10/09        李根华           新建
    /// =======================================================================
    public static partial class HtmlHelperExtension
    {
        #region Public Method

        /// <summary>
        /// 页面分页控件
        /// </summary>
        /// <param name="helper"></param>
        /// <param name="pager"></param>
        /// <param name="formId"></param>
        /// <param name="showTotal"></param>
        /// <param name="totalFormat"></param>
        /// <returns></returns>
        public static MvcHtmlString PageHandle(this HtmlHelper helper, PagedList pager, string formId = null, bool showTotal = false, string totalFormat = null)
        {
            return PageHandle(helper, pager.PageIndex, pager.PageSize, pager.TotalItemCount, formId, showTotal, totalFormat);
        }

        /// <summary>
        /// 页面分页控件
        /// </summary>
        /// <param name="helper"></param>
        /// <param name="pageIndex"></param>
        /// <param name="pageSize"></param>
        /// <param name="total"></param>
        /// <param name="showTotal"></param>
        /// <param name="totalFormat"></param>
        /// <returns></returns>
        public static MvcHtmlString PageHandle(this HtmlHelper helper, int pageIndex, int pageSize, int total, bool showTotal = false, string totalFormat = null)
        {
            return PageHandle(helper, pageIndex, pageSize, total, null, 0, showTotal, totalFormat);
        }

        /// <summary>
        /// 页面分页控件
        /// </summary>
        /// <param name="helper"></param>
        /// <param name="pageIndex"></param>
        /// <param name="pageSize"></param>
        /// <param name="total"></param>
        /// <param name="pageCount"></param>
        /// <param name="showTotal"></param>
        /// <param name="totalFormat"></param>
        /// <returns></returns>
        public static MvcHtmlString PageHandle(this HtmlHelper helper, int pageIndex, int pageSize, int total, int pageCount, bool showTotal = false, string totalFormat = null)
        {
            return PageHandle(helper, pageIndex, pageSize, total, null, pageCount, showTotal, totalFormat);
        }

        /// <summary>
        /// 页面分页控件
        /// </summary>
        /// <param name="helper"></param>
        /// <param name="pageIndex"></param>
        /// <param name="pageSize"></param>
        /// <param name="total"></param>
        /// <param name="formId"></param>
        /// <param name="showTotal"></param>
        /// <param name="totalFormat"></param>
        /// <returns></returns>
        public static MvcHtmlString PageHandle(this HtmlHelper helper, int pageIndex, int pageSize, int total, string formId, bool showTotal = false, string totalFormat = null)
        {
            return PageHandle(helper, pageIndex, pageSize, total, formId, 0, showTotal, totalFormat);
        }

        /// <summary>
        /// 页面分页控件
        /// </summary>
        /// <param name="helper"></param>
        /// <param name="pageIndex"></param>
        /// <param name="pageSize"></param>
        /// <param name="total"></param>
        /// <param name="formId"></param>
        /// <param name="pageCount"></param>
        /// <param name="showTotal"></param>
        /// <param name="totalFormat"></param>
        /// <returns></returns>
        public static MvcHtmlString PageHandle(this HtmlHelper helper, int pageIndex, int pageSize, int total, string formId, int pageCount, bool showTotal = false, string totalFormat = null)
        {
            var _id = Guid.NewGuid();
            string str = string.Format("<div id=\"{0}\"", _id);

            if (showTotal)
            {
                str += " showTotal=\"1\"";
            }
            if (!string.IsNullOrEmpty(totalFormat))
            {
                str += " totalFormat=\"" + totalFormat + "\"";
            }

            str += "></div>";
            str += "\n<script type=\"text/javascript\">\n";
            str += "$('#" + _id + "').createPager({";
            str += string.Format("pageIndex:{0},pageSize:{1},total:{2},formId:{3},pageCount:{4}",
                pageIndex,
                pageSize,
                total,
                formId == null ? "null" : string.Format("'{0}'", formId),
                pageCount == 0 ? 10 : pageCount);
            str += "})";
            str += CheckForm(formId, pageIndex);
            str += "\n</script>";
            return new MvcHtmlString(str);
        }
        #endregion

        #region Private Method

        private static string CheckForm(string formId, int pageIndex)
        {
            if (string.IsNullOrEmpty(formId))
                return string.Empty;
            return "\n$('#" + formId + "').append(\"<input type='hidden' id='pageIndex' name='pageIndex' value=''/>\")";
        }
        #endregion
    }
}