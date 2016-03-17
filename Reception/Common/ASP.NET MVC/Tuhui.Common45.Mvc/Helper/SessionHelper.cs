using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web;
using Tuhui.Common45.Utility;
using System.Web.SessionState;
using Tuhui.Common45.Environment;

namespace Tuhui.Common45.Mvc
{
    /// <summary>
    /// Session帮助类
    /// </summary>
    public static class SessionHelper
    {
        #region Public Method

        #region HttpContext
        private static HttpSessionState HSession
        {
            get
            {
                return HttpContext.Current.Session;
            }
        }

        private static HttpRequest HRequest
        {
            get
            {
                return HttpContext.Current.Request;
            }
        }

        private static HttpResponse HReponse
        {
            get
            {
                return HttpContext.Current.Response;
            }
        }
        #endregion

        #region LogOnUser

        /// <summary>
        /// 返回当前登陆者信息对象
        /// </summary>
        public static object LogOnUserObj
        {
            get
            {
                return GetSession(CommonCode.GlobalConstant.UserSessionKey);
            }
        }

        /// <summary>
        /// 返回当前登陆者信息对象
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="_user"></param>
        /// <returns></returns>
        public static T LogOnUser<T>(T _user = null) where T : class
        {
            if (_user == null)
            {
                return GetSession<T>(CommonCode.GlobalConstant.UserSessionKey);
            }
            else
            {
                SetSession(CommonCode.GlobalConstant.UserSessionKey, _user);
                return _user;
            }
        }
        #endregion

        /// <summary>
        /// 获取Session中存放的对象
        /// </summary>
        /// <param name="key"></param>
        /// <returns></returns>
        public static object Get(string key)
        {
            return GetSession(key);
        }

        /// <summary>
        /// 获取Session中存放的对象并转换成指定的类型
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="key"></param>
        /// <returns></returns>
        public static T Get<T>(string key)
        {
            return GetSession<T>(key);
        }

        /// <summary>
        /// 获取Session中存放的对象以及转换成指定的类型
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="key"></param>
        /// <param name="_func">在Session中没有该键的存储时候的获取函数</param>
        /// <returns></returns>
        public static T Get<T>(string key, Func<T> _func) where T : class
        {
            var _value = GetSession<T>(key);
            if (_value == null)
            {
                return SetSession(key, _func()) as T;
            }
            else
            {
                return _value;
            }
        }

        /// <summary>
        /// 存储Session键值对
        /// </summary>
        /// <param name="key"></param>
        /// <param name="value"></param>
        /// <returns></returns>
        public static object Set(string key, object value)
        {
            return SetSession(key, value);
        }

        /// <summary>
        /// 存储Session键值对
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="key"></param>
        /// <param name="value"></param>
        /// <returns></returns>
        public static T Set<T>(string key, T value) where T : class
        {
            return SetSession(key, value) as T;
        }

        /// <summary>
        /// 存储Session键值对
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="key"></param>
        /// <param name="_func"></param>
        /// <returns></returns>
        public static T Set<T>(string key, Func<T> _func) where T : class
        {
            return SetSession(key, _func()) as T;
        }
        #endregion

        #region Private Method
        /// <summary>
        /// Gets the session.
        /// </summary>
        /// <param name="key">The key.</param>
        /// <returns></returns>
        private static object GetSession(string key)
        {
            return HSession[key];
        }

        /// <summary>
        /// Gets the session.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="key">The key.</param>
        /// <returns></returns>
        private static T GetSession<T>(string key)
        {
            var _obj = GetSession(key);
            if (_obj == null) return default(T);
            return (T)_obj;
        }

        private static object SetSession(string key, object value)
        {
            HSession[key] = value;
            return value;
        }
        #endregion
    }
}
