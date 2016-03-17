using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Tuhui.Common45.Utility;
using System.Xml;

namespace Tuhui.Common45.Environment
{
    /// <summary>
    /// 全局环境设置相关类
    /// </summary>
    public static class EnvironmentHandle
    {
        private static EnvironmentSetting _environmentSetting;

        #region Constructor
        /// <summary>
        /// 静态构造函数
        /// </summary>
        static EnvironmentHandle()
        {
            _environmentSetting = new EnvironmentSetting();
        }
        #endregion

        #region Property
        /// <summary>
        /// 全局相关配置
        /// </summary>
        public static GlobalSetting GlobalSetting
        {
            get
            {
                return _environmentSetting.Setting_Global;
            }
        }

        /// <summary>
        /// 键值对相关配置
        /// </summary>
        public static NameValueSetting NameValueSetting
        {
            get
            {
                return _environmentSetting.Setting_NameValue;
            }
        }

        /// <summary>
        /// 缓存配置
        /// </summary>
        public static CacheSetting CacheSetting
        {
            get
            {
                return _environmentSetting.Setting_Cache;
            }
        }

        #endregion

        #region Public Method
        /// <summary>
        /// 全局环境配置注册
        /// </summary>
        public static void Register()
        {
            LoadEnvironment_Global();
            LoadEnvironment_NameValue();
            LoadEnvironment_Cache();
        }

        #endregion

        #region Loading Config File
        internal static void LoadEnvironment_Cache()
        {
            _environmentSetting.Setting_Cache = new CacheSetting();

            XMLReader _reader = new XMLReader();

            _reader.LoadFile(PathUtility.GetApplicationPath(CommonCode.ConfigName.EnvironmentCache));
            _environmentSetting.Setting_Cache.LoadType(_reader.FindValue("Type"));

            _environmentSetting.Setting_Cache.LoadHttpRuntimeCacheSetting(
                _reader.FindValue("HttpRuntime/Absolute"),
                _reader.FindValue("HttpRuntime/Duration"),
                _reader.FindValue("HttpRuntime/Priority"));
        }

        internal static void LoadEnvironment_Global()
        {
            _environmentSetting.Setting_Global = new GlobalSetting();

            XMLReader _reader = new XMLReader();

            _reader.LoadFile(PathUtility.GetApplicationPath(CommonCode.ConfigName.EnvironmentGlobal));
            #region Site
            var _site_id = _reader.FindValue("Site/ID");
            if (string.IsNullOrEmpty(_site_id))
            {
                _site_id = Guid.NewGuid().ToString();
            }
            _environmentSetting.Setting_Global.SiteID = _site_id;
            _environmentSetting.Setting_Global.SiteName = _reader.FindValue("Site/Name");
            _environmentSetting.Setting_Global.SiteUrl = _reader.FindValue("Site/Url");
            _environmentSetting.Setting_Global.ErrorUrl = _reader.FindValue("Site/ErrorUrl");
            _environmentSetting.Setting_Global.Debugger = _reader.FindValueAsBool("Site/Debugger", false);
            #endregion

        }

        internal static void LoadEnvironment_NameValue()
        {
            _environmentSetting.Setting_NameValue = new NameValueSetting();

            XMLReader _reader = new XMLReader();

            _reader.LoadFile(PathUtility.GetApplicationPath(CommonCode.ConfigName.EnvironmentNameValue));

            _environmentSetting.Setting_NameValue.NameValues = new Dictionary<string, string>();

            var _nameValueItems = _reader.FindNodes("Item/add");

            if (_nameValueItems != null)
            {
                foreach (XmlNode item in _nameValueItems)
                {
                    var _key = item.Attributes["key"].Value;

                    if (_environmentSetting.Setting_NameValue.NameValues.ContainsKey(_key))
                    {
                        throw new ArgumentException("Environment_NameValue.config exist repetitive key：the key is [" + _key + "]");
                    }
                    _environmentSetting.Setting_NameValue.NameValues.Add(_key, item.Attributes["value"].Value);
                }
            }

        }
        
        #endregion
    }
}
