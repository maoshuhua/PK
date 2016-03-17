using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace Tuhui.Common45.Utility
{
    /// <summary>
    /// 
    /// </summary>
    public class InvokeUtility
    {
           /// <summary>
           /// 
           /// </summary>
           /// <param name="typeName"></param>
           /// <param name="methodName"></param>
        public static void InvokeMethod(string typeName, string methodName)
        {      
            Type calledType = Type.GetType(typeName);

            calledType.InvokeMember(methodName,BindingFlags.InvokeMethod | BindingFlags.Public |BindingFlags.Static,null,null,null);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="typeName"></param>
        /// <param name="methodName"></param>
        /// <param name="param"></param>
        public static void InvokeMethod(string typeName, string methodName, object[] param)
        {
            Type calledType = Type.GetType(typeName);

            calledType.InvokeMember(methodName,BindingFlags.InvokeMethod | BindingFlags.Public |BindingFlags.Static,null,null,param);

        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="assemblyName"></param>
        /// <param name="namespaceName"></param>
        /// <param name="typeName"></param>
        /// <param name="methodName"></param>
        public static void InvokeMethod( string assemblyName, string namespaceName,  string typeName, string methodName)
        {
            Type calledType = Type.GetType(namespaceName + "." + typeName + "," + assemblyName);

            calledType.InvokeMember( methodName,BindingFlags.InvokeMethod | BindingFlags.Public | BindingFlags.Static,null,   null,     null);
        }

        /// <summary>
        /// 获取obj中的fieldname的值
        /// </summary>
        /// <param name="fieldName"></param>
        /// <param name="obj"></param>
        /// <returns></returns>
        public  static object GetFieldValue(string fieldName, object obj)
        {
            Type Ts = obj.GetType();
            object o = Ts.GetProperty(fieldName).GetValue(obj, null);
            string Value = Convert.ToString(o);
            if (string.IsNullOrEmpty(Value)) return null;
            return Value;

        }

    }



}
