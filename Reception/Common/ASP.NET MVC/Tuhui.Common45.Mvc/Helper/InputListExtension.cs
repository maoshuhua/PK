using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Linq.Expressions;
using System.Web.Mvc;
using Tuhui.Common45.Utility;

namespace Tuhui.Common45.Mvc
{
    public static partial class HtmlHelperExtension
    {
        #region CheckBoxList
        /// <summary>
        /// 生成CheckBox组
        /// </summary>
        /// <typeparam name="TModel">Model对象类型</typeparam>
        /// <param name="html">HtmlHelper对象</param>
        /// <param name="expression">lambada对象</param>
        /// <param name="selectList">数据源</param>
        /// <param name="format">显示格式,默认是‘<li>{0} {1}</li>’</param>
        /// <param name="htmlAttributes">元素Html属性</param>
        /// <returns></returns>
        public static MvcHtmlString CheckBoxListFor<TModel>(

            this HtmlHelper<TModel> html,

            Expression<Func<TModel, string[]>> expression,

            IEnumerable<SelectListItem> selectList,

            string format = null,

            object htmlAttributes = null)
        {

            return CheckBoxList(html, GetName(expression), selectList, format, htmlAttributes, GetCheckBoxValue<TModel, string>(html, expression));

        }

        /// <summary>
        /// 生成CheckBox组
        /// </summary>
        /// <typeparam name="TModel"></typeparam>
        /// <typeparam name="T"></typeparam>
        /// <param name="html"></param>
        /// <param name="expression"></param>
        /// <param name="selectList"></param>
        /// <param name="format"></param>
        /// <param name="htmlAttributes"></param>
        /// <returns></returns>
        public static MvcHtmlString CheckBoxListFor<TModel, T>(

            this HtmlHelper<TModel> html,

            Expression<Func<TModel, T[]>> expression,

            IEnumerable<SelectListItem> selectList,

            string format = null,

            object htmlAttributes = null)
        {

            return CheckBoxList(html, GetName(expression), selectList, format, htmlAttributes, GetCheckBoxValue<TModel, T>(html, expression));

        }


        /// <summary>
        /// 生成CheckBox组
        /// </summary>
        /// <param name="html">HtmlHelper对象</param>
        /// <param name="name">checkbox的name属性</param>
        /// <param name="selectList">数据源</param>
        /// <param name="format">显示格式,默认是‘<li>{0} {1}</li>’</param>
        /// <param name="htmlAttributes">元素Html属性</param>
        /// <param name="values">选中的checkbox数值集合</param>
        /// <returns></returns>
        public static MvcHtmlString CheckBoxList(

           this HtmlHelper html,

           string name,

           IEnumerable<SelectListItem> selectList,

           string format = null,

           object htmlAttributes = null,

            object[] values = null)
        {

            return InputListInternal(html, name, selectList, true, format, HtmlHelper.AnonymousObjectToHtmlAttributes(htmlAttributes), values);

        }

        /// <summary>
        /// 生成CheckBox组
        /// </summary>
        /// <param name="html">HtmlHelper对象</param>
        /// <param name="name">checkbox的name属性</param>
        /// <param name="selectList">数据源</param>
        /// <param name="format">显示格式,默认是‘<li>{0} {1}</li>’</param>
        /// <param name="htmlAttributes">元素Html属性</param>
        /// <param name="values">选中的checkbox数值集合</param>
        /// <returns></returns>
        public static MvcHtmlString CheckBoxList<T>(

           this HtmlHelper html,

           string name,

           IEnumerable<SelectListItem> selectList,

           string format = null,

           object htmlAttributes = null,

            T[] values = null)
        {
            if (values == null)
            {
                return InputListInternal(html, name, selectList, true, format, HtmlHelper.AnonymousObjectToHtmlAttributes(htmlAttributes), null);
            }
            else
            {
                List<object> _obj = new List<object>();
                values.ForEach(p => _obj.Add(p));
                return InputListInternal(html, name, selectList, true, format, HtmlHelper.AnonymousObjectToHtmlAttributes(htmlAttributes), _obj.ToArray());
            }
        }

        #endregion


        #region RadioButtonList

        /// <summary>
        /// 生成radiobutton组
        /// </summary>
        /// <param name="html">HtmlHelper对象</param>
        /// <param name="name">radiobutton的name属性值</param>
        /// <param name="selectList">数据源集合</param>
        /// <param name="format">显示格式,默认：'<li>{0} {1}</li>'</param>
        /// <param name="htmlAttributes">元素的html属性</param>
        /// <param name="value">选中的radiobutton的是值</param>
        /// <returns></returns>
        public static MvcHtmlString RadioButtonList(

         this HtmlHelper html,

         string name,

         IEnumerable<SelectListItem> selectList,

         string format = null,

         object htmlAttributes = null,

         string value = null)
        {

            return InputListInternal(html, name, selectList, false, format, HtmlHelper.AnonymousObjectToHtmlAttributes(htmlAttributes), new string[] { value });
        }

        /// <summary>
        /// 生成RadioButton组
        /// </summary>
        /// <typeparam name="TModel">Model对象类型</typeparam>
        /// <typeparam name="T">元素数据类型</typeparam>
        /// <param name="html">HtmlHelper对象</param>
        /// <param name="expression">lambada对象</param>
        /// <param name="selectList">数据源集合</param>
        /// <param name="format">显示格式,默认：'<li>{0} {1}</li>'</param>
        /// <param name="htmlAttributes">元素的html属性</param>
        /// <returns></returns>
        public static MvcHtmlString RadioButtonListFor<TModel, T>(

            this HtmlHelper<TModel> html,

            Expression<Func<TModel, T>> expression,

            IEnumerable<SelectListItem> selectList,

            string format = null,

            object htmlAttributes = null)
        {
            var _val = GetRadioButtonValue<TModel, T>(html, expression);
            return RadioButtonList(html, GetName(expression), selectList, format, htmlAttributes, _val != null ? _val.ToString() : null);
        }

        #endregion


        /*-------------------------------------


         * Core Function

         --------------------------------------*/
        private static MvcHtmlString InputListInternal(
            this HtmlHelper html,
            string name,
            IEnumerable<SelectListItem> selectList,
            bool allowMultiple,
            string format,
            IDictionary<string, object> htmlAttributes,
            object[] values
           )
        {

            string fullHtmlFieldName = html.ViewContext.ViewData.TemplateInfo.GetFullHtmlFieldName(name);

            if (string.IsNullOrEmpty(fullHtmlFieldName))
            {

                throw new ArgumentException("filed can't be null or empty !", "name");

            }

            if (format == null)

                format = "<label for='{0}'>{1}{2}</label>";

            StringBuilder strBuilder = new StringBuilder();

            TagBuilder tagBuilder = null;
            int count = 0;
            foreach (var item in selectList)
            {   //Clear first
                count++;
                if (item.Text == null || item.Value == null) continue;

                tagBuilder = new TagBuilder("input");


                if (allowMultiple)
                {

                    tagBuilder.MergeAttribute("type", "checkbox", true);

                }

                else
                {

                    tagBuilder.MergeAttribute("type", "radio", true);

                }


                tagBuilder.MergeAttribute("value", item.Value, true);
                tagBuilder.MergeAttribute("text", item.Text, true);

                if (item.Selected)
                {

                    tagBuilder.MergeAttribute("checked", "checked", true);
                }
                else if (values != null && values.Select(p => { if (p == null)return string.Empty; else return p.ToString(); }).Contains(item.Value))
                {
                    tagBuilder.MergeAttribute("checked", "checked", true);
                }


                tagBuilder.MergeAttributes<string, object>(htmlAttributes);

                tagBuilder.MergeAttribute("name", fullHtmlFieldName, true);

                tagBuilder.MergeAttribute("id", fullHtmlFieldName + "_" + count, true);

                var btnHtmlString = new MvcHtmlString(tagBuilder.ToString(TagRenderMode.SelfClosing));

                var inputItem = new InputListItem { Button = btnHtmlString, LabelText = fullHtmlFieldName + "_" + count };

                var s = string.Format(format, inputItem.LabelText, inputItem.Button, item.Text);

                strBuilder.Append(s);
            }

            return new MvcHtmlString(strBuilder.ToString());

        }

        private static string GetName(LambdaExpression expression)
        {

            if (expression == null)
            {

                throw new ArgumentNullException("expression");

            }

            return ExpressionHelper.GetExpressionText(expression);

        }

        private static object[] GetCheckBoxValue<TModel, T>(HtmlHelper<TModel> helper, Expression<Func<TModel, T[]>> expression)
        {
            ModelMetadata metadata = ModelMetadata.FromLambdaExpression(expression, helper.ViewData);
            var returnValue = metadata.Model as T[];
            if (returnValue == null)
            {
                return null;
            }
            else
            {
                return returnValue.Select(p => p as object).ToArray();
            }
        }

        private static T GetRadioButtonValue<TModel, T>(HtmlHelper<TModel> helper, Expression<Func<TModel, T>> expression)
        {
            ModelMetadata metadata = ModelMetadata.FromLambdaExpression(expression, helper.ViewData);
            if (metadata.Model != null)
            {
                return (T)metadata.Model;
            }
            else
            {
                return default(T);
            }
        }

    }

    /// <summary>
    /// checkbox/radio每一项元素对象
    /// </summary>
    public class InputListItem
    {
        /// <summary>
        /// checkbox/radio元素
        /// </summary>
        public MvcHtmlString Button { get; set; }

        /// <summary>
        /// label元素
        /// </summary>
        public string LabelText { get; set; }
    }
}
