using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web;
using System.Web.Mvc;
using System.Web.Mvc.Ajax;
using System.Web.Routing;

namespace Tuhui.OH.Mvc
{
    public static partial class ActionLinkImageHelper
    {
        /// <summary>
        /// 图片链接
        /// </summary>
        /// <param name="helper"></param>
        /// <param name="imageUrl">图片地址</param>
        /// <param name="altText">alt文字</param>
        /// <param name="titleText">title文字</param>
        /// <param name="actionName">action</param>
        /// <param name="routeValues">参数</param>
        /// <param name="ajaxOptions">ajaxOptions</param>
        /// <returns></returns>
        public static string ActionLinkImage(this AjaxHelper helper, string imageUrl, string altText, string titleText, string actionName, object routeValues, AjaxOptions ajaxOptions, object imageHtmlAttributes)
        {
            var builder = new TagBuilder("img");
            builder.MergeAttribute("src", imageUrl);
            builder.MergeAttribute("alt", altText);
            builder.MergeAttribute("title", titleText);
            builder.MergeAttributes(new RouteValueDictionary(imageHtmlAttributes));
            var link = helper.ActionLink("[replaceme]", actionName, routeValues, ajaxOptions);
            var tmp = link.ToHtmlString().Replace("[replaceme]", builder.ToString(TagRenderMode.SelfClosing));
            return tmp.ToString();
        }


        public static MvcHtmlString ActionLinkWithImage(this HtmlHelper html, string imgSrc, string actionName)
        {
            var urlHelper = new UrlHelper(html.ViewContext.RequestContext);
            string imgUrl = urlHelper.Content(imgSrc);
            TagBuilder imgTagBuilder = new TagBuilder("img");
            imgTagBuilder.MergeAttribute("src", imgUrl);
            string img = imgTagBuilder.ToString(TagRenderMode.SelfClosing);
            string url = urlHelper.Action(actionName);
            TagBuilder tagBuilder = new TagBuilder("a")
            {
                InnerHtml = img
            };
            tagBuilder.MergeAttribute("href", url);
            return new MvcHtmlString(tagBuilder.ToString(TagRenderMode.Normal));
        }
        public static MvcHtmlString ActionLinkWithImage(this HtmlHelper html, string imgSrc, string actionName, string controllerName, object routeValue = null)
        {
            var urlHelper = new UrlHelper(html.ViewContext.RequestContext);
            string imgUrl = urlHelper.Content(imgSrc);
            TagBuilder imgTagBuilder = new TagBuilder("img");
            imgTagBuilder.MergeAttribute("src", imgUrl);
            string img = imgTagBuilder.ToString(TagRenderMode.SelfClosing);
            string url = urlHelper.Action(actionName, controllerName, routeValue);
            TagBuilder tagBuilder = new TagBuilder("a")
            {
                InnerHtml = img
            };
            tagBuilder.MergeAttribute("href", url);
            return new MvcHtmlString(tagBuilder.ToString(TagRenderMode.Normal));
        } 
    }
}
