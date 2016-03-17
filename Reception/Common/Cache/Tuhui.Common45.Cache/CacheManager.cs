using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web;
using System.Web.Caching;

namespace Tuhui.Common45.Cache
{
    /// <summary>
    /// 缓存管理类
    /// </summary>
    public static class CacheManager
    {
        /// <summary>
        /// 设置缓存
        /// </summary>
        /// <param name="key"></param>
        /// <param name="data"></param>
        public static void Set(string key, object data)
        {
            HttpRuntime.Cache.Insert(key,
                data,
                null,
                System.Web.Caching.Cache.NoAbsoluteExpiration,
                System.Web.Caching.Cache.NoSlidingExpiration,
                System.Web.Caching.CacheItemPriority.Normal,
                null);
        }

        /// <summary>
        /// 获取缓存
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="key"></param>
        /// <returns></returns>
        public static T Get<T>(string key) where T : class
        {
            return HttpRuntime.Cache.Get(key) as T;
        }

        /// <summary>
        /// 获取缓存
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="key"></param>
        /// <param name="_func"></param>
        /// <returns></returns>
        public static T Get<T>(string key, Func<T> _func) where T : class
        {
            var _result = Get<T>(key);
            if (_result == null)
            {
                _result = _func();
                Set(key, _result);
            }
            return _result;
        }

        /// <summary>
        /// 删除缓存
        /// </summary>
        /// <param name="key"></param>
        /// <returns></returns>
        public static object Remove(string key)
        {
            return HttpRuntime.Cache.Remove(key);
        }
    }
}
