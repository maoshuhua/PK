using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Tuhui.Common45.Environment
{
    /// <summary>
    /// 缓存类型
    /// </summary>
    public enum CacheType
    {
        /// <summary>
        /// 无缓存模式
        /// </summary>
        None,
        /// <summary>
        /// Http运行时缓存方式
        /// </summary>
        HttpRuntime
    }

    /// <summary>
    /// 缓存设置
    /// </summary>
    public class CacheSetting
    {
        /// <summary>
        /// 缓存类型
        /// </summary>
        public CacheType Type { get; set; }

        /// <summary>
        /// HttpRuntime类型缓存设置
        /// </summary>
        public HttpRuntimeCacheSetting CacheSetting_HttpRuntime { get; set; }

        public void LoadType(string type)
        {
            switch (type)
            {
                case "HttpRuntime":
                    this.Type = CacheType.HttpRuntime;
                    break;
                default:
                    this.Type = CacheType.None;
                    break;
            }
        }

        public void LoadHttpRuntimeCacheSetting(string absolute, string duration, string priority)
        {
            this.CacheSetting_HttpRuntime = new HttpRuntimeCacheSetting(absolute, duration, priority);
        }

    }

    /// <summary>
    /// HttpRuntime类型缓存配置结构
    /// </summary>
    public class HttpRuntimeCacheSetting
    {
        /// <summary>
        /// 绝对过期时间(设置了此参数则缓存数据存放后到指定的绝对时间进行清除)
        /// </summary>
        public DateTime? AbsoluteExpiration { get; set; }

        /// <summary>
        /// 最后一次访问所添加对象时与该对象到期时之间的时间间隔。如果该值等效于 20 分钟，则对象在最后一次被访问 20 分钟之后将到期并从缓存中移除。
        /// </summary>
        public TimeSpan? DurationExpiration { get; set; }

        /// <summary>
        /// 1：Low  2：BelowNormal  3：Normal  4：Default  5：AboveNormal  6：High  7：NotRemovable  默认值是4
        /// </summary>
        public int Priority { get; set; }

        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="absolute"></param>
        /// <param name="duration"></param>
        /// <param name="priority"></param>
        public HttpRuntimeCacheSetting(string absolute, string duration, string priority)
        {
            if (!string.IsNullOrEmpty(absolute))
            {
                DateTime dt;
                if (DateTime.TryParse(absolute, out dt))
                {
                    this.AbsoluteExpiration = dt;
                }
            }
            if (!string.IsNullOrEmpty(duration))
            {
                int t;
                if (int.TryParse(duration, out t))
                {
                    this.DurationExpiration = TimeSpan.FromSeconds(t);
                }
            }
            if (!string.IsNullOrEmpty(priority))
            {
                int t;
                if (int.TryParse(priority, out t))
                {
                    this.Priority = t;
                }
                else
                {
                    this.Priority = 4;
                }
            }
            else
            {
                this.Priority = 4;
            }
        }
    }
}
