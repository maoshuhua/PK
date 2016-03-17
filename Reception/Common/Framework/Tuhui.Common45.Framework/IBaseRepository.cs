using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data.Entity;
using System.Linq.Expressions;
using Tuhui.Common45.Utility;

namespace Tuhui.Common45.Framework
{
    /// =======================================================================
    /// 类名：IBaseDataAccess
    /// <summary>
    /// DataAccess基类接口
    /// </summary>
    /// <remarks>
    /// DataAccess基类接口
    /// </remarks>
    /// =======================================================================
    /// 更新履历
    /// 序号          修改日期          责任人           更新内容
    /// 001           2012/07/05        李根华           新建
    /// =======================================================================
    public interface IBaseRepository<TContext> where TContext : DbContext, new()
    {
        /// <summary>
        /// 新增
        /// </summary>
        /// <typeparam name="TEntity"></typeparam>
        /// <param name="model"></param>
        /// <returns></returns>
        int Insert<TEntity>(TEntity model) where TEntity : BaseEntity;

        /// <summary>
        /// 新增
        /// </summary>
        /// <typeparam name="TEntity"></typeparam>
        /// <param name="models"></param>
        /// <returns></returns>
        int Insert<TEntity>(IEnumerable<TEntity> models) where TEntity : BaseEntity;


        /// <summary>
        /// 更新
        /// </summary>
        /// <typeparam name="TEntity"></typeparam>
        /// <param name="model"></param>
        /// <returns></returns>
        int Update<TEntity>(TEntity model) where TEntity : BaseEntity;

        /// <summary>
        /// 更新
        /// </summary>
        /// <typeparam name="TEntity"></typeparam>
        /// <param name="models"></param>
        /// <returns></returns>
        int Update<TEntity>(IEnumerable<TEntity> models) where TEntity : BaseEntity;

        /// <summary>
        /// 更新
        /// </summary>
        /// <typeparam name="TEntity"></typeparam>
        /// <param name="predicate"></param>
        /// <param name="updateAction"></param>
        /// <returns></returns>
        int Update<TEntity>(Expression<Func<TEntity, bool>> predicate, Action<TEntity> updateAction) where TEntity : BaseEntity;

        /// <summary>
        /// 删除
        /// </summary>
        /// <typeparam name="TEntity"></typeparam>
        /// <param name="model"></param>
        /// <returns></returns>
        int Delete<TEntity>(TEntity model) where TEntity : BaseEntity;

        /// <summary>
        /// 删除
        /// </summary>
        /// <typeparam name="TEntity"></typeparam>
        /// <param name="models"></param>
        /// <returns></returns>
        int Delete<TEntity>(IEnumerable<TEntity> models) where TEntity : BaseEntity;

        /// <summary>
        /// 删除
        /// </summary>
        /// <typeparam name="TEntity"></typeparam>
        /// <param name="predicate"></param>
        /// <returns></returns>
        int Delete<TEntity>(Func<TEntity, bool> predicate) where TEntity : BaseEntity;

        IQueryable<TEntity> Search<TEntity>(Func<TEntity, bool> predicate = null) where TEntity : BaseEntity;

        /// <summary>
        /// 查询单笔记录
        /// </summary>
        /// <typeparam name="TEntity"></typeparam>
        /// <param name="predicate"></param>
        /// <returns></returns>
        TEntity SearchFirstOrDefault<TEntity>(Func<TEntity, bool> predicate = null) where TEntity : BaseEntity;

        /// <summary>
        /// 查询多笔记录
        /// </summary>
        /// <typeparam name="TEntity"></typeparam>
        /// <param name="predicate"></param>
        /// <returns></returns>
        List<TEntity> SearchList<TEntity>(Func<TEntity, bool> predicate = null) where TEntity : BaseEntity;

        /// <summary>
        /// 查询分页记录
        /// </summary>
        /// <typeparam name="TEntity"></typeparam>
        /// <param name="predicate"></param>
        /// <param name="order"></param>
        /// <param name="pageIndex"></param>
        /// <param name="pageSize"></param>
        /// <returns></returns>
        PagedList<TEntity> SearchPagedList<TEntity>(Func<TEntity, bool> predicate, Func<IQueryable<TEntity>, IOrderedQueryable<TEntity>> order, int pageIndex, int pageSize) where TEntity : BaseEntity;


        /// <summary>
        /// 执行SQL
        /// </summary>
        /// <typeparam name="T">结果数据类型</typeparam>
        /// <param name="sql">SQL文</param>
        /// <param name="parameter">参数</param>
        /// <returns>指定数据类型的结果</returns>
        T ExecuteText<T>(string sql, object parameter = null);

        /// <summary>
        /// 执行SQL,返回集合数据
        /// </summary>
        /// <typeparam name="T">结果数据类型</typeparam>
        /// <param name="sql">SQL文</param>
        /// <param name="parameter">参数</param>
        /// <returns>指定数据类型的结果的集合</returns>
        IEnumerable<T> ExecuteTextCollection<T>(string sql, object parameter = null);

        /// <summary>
        /// 执行存储过程
        /// </summary>
        /// <typeparam name="T">结果数据类型</typeparam>
        /// <param name="procedureName">存储过程名称</param>
        /// <param name="parameter">参数</param>
        /// <returns>指定数据类型的结果</returns>
        T ExecuteStoredProcedure<T>(string procedureName, object parameter = null);

        /// <summary>
        /// 执行存储过程，并返回集合数据
        /// </summary>
        /// <typeparam name="T">结果数据类型</typeparam>
        /// <param name="procedureName">存储过程名称</param>
        /// <param name="parameter">参数</param>
        /// <returns>指定数据类型的结果的集合</returns>
        IEnumerable<T> ExecuteStoredProcedureCollection<T>(string procedureName, object parameter = null);

    }
}
