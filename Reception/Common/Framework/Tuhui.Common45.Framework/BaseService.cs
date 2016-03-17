using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Tuhui.Common45.Framework
{
    public class BaseService : IBaseService
    {
        #region 接口实例化
        /// <summary>
        /// 实例化BL接口
        /// </summary>
        /// <typeparam name="BL"></typeparam>
        /// <returns></returns>
        public DAL InstanceRepository<DAL>() where DAL : CoreRepository, new()
        {
            return new DAL();
        }
        #endregion
    }
}
