using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data.Entity;
using System.Data.Entity.Infrastructure;
using System.Data.Entity.ModelConfiguration.Conventions;

namespace Tuhui.Common45.Framework
{
    public abstract class BaseContext:DbContext
    {
        public string ConnectionString;

        #region Constructor
        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="database"></param>
        public BaseContext(string database)
            : base(database)
        {
            ConnectionString = Database.Connection.ConnectionString;
            // 是否启动延迟加载
            Configuration.LazyLoadingEnabled = false;
            // 是否启动代理
            Configuration.ProxyCreationEnabled = false;
            //Configuration.AutoDetectChangesEnabled = false;
            Configuration.ValidateOnSaveEnabled = false;
            // 防止Entity变更导致数据库自动更新
            Database.SetInitializer<BaseContext>(null);
        }
        #endregion
    }
}
