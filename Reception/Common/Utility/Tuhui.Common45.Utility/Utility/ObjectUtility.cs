using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ComponentModel;

namespace Tuhui.Common45.Utility
{
    public static class ObjectUtility
    {
        public static Dictionary<string, object> AnonymousObjectToHtmlAttributes(object htmlAttributes)
        {
            Dictionary<string, object> result = new Dictionary<string, object>();

            if (htmlAttributes != null)
            {
                foreach (PropertyDescriptor property in TypeDescriptor.GetProperties(htmlAttributes))
                {
                    result.Add(property.Name, property.GetValue(htmlAttributes));
                }
            }

            return result;
        }
    }
}
