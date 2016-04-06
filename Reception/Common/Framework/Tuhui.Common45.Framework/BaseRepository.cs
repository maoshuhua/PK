using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data.Entity;
using System.Data;
using System.Linq.Expressions;
using System.Data.SqlClient;
using Tuhui.Common45.Utility;
using System.ComponentModel;
using System.Reflection;

namespace Tuhui.Common45.Framework
{
    /// <summary>
    /// 数据库访问层基类
    /// </summary>
    /// <typeparam name="TContext"></typeparam>
    public abstract class BaseRepository<TContext> : CoreRepository, IBaseRepository<TContext> where TContext : BaseContext, new()
    {
        #region 上下文
        protected TContext ContextObj;
        public BaseRepository()
        {
            ContextObj = new TContext();
        }
        #endregion

        #region 增删改

        /// <summary>
        /// 新增实体对象
        /// </summary>
        /// <typeparam name="TEntity"></typeparam>
        /// <param name="model"></param>
        /// <returns></returns>
        public int Insert<TEntity>(TEntity model) where TEntity : BaseEntity
        {
            //茅书华
            //利用反射对string赋值默认值空
            Type type = typeof(TEntity);
            PropertyInfo[] props = type.GetProperties(BindingFlags.Public | BindingFlags.Instance);
            if (props.Length > 0) { 
                foreach(PropertyInfo prop in props)
                {
                    string propType = prop.PropertyType.Name;
                    object obj = prop.GetValue(model,null);
                    if (propType is string)
                    {
                        if(obj == null)
                        {
                            prop.SetValue(model,"");
                        }
                    }
                }
            }

            return this.ChangeObjectState<TEntity>(model, EntityState.Added);
        }

        /// <summary>
        /// 新增实体对象集合
        /// </summary>
        /// <typeparam name="TEntity"></typeparam>
        /// <param name="models"></param>
        /// <returns></returns>
        public int Insert<TEntity>(IEnumerable<TEntity> models) where TEntity : BaseEntity
        {
            return this.ChangeObjectState<TEntity>(models, EntityState.Added);
        }

        /// <summary>
        /// 持久化对象更改
        /// </summary>
        /// <typeparam name="TEntity"></typeparam>
        /// <param name="model"></param>
        /// <returns></returns>
        public int Update<TEntity>(TEntity model) where TEntity : BaseEntity
        {
            return this.ChangeObjectState<TEntity>(model, EntityState.Modified);
        }

        /// <summary>
        /// 更新对象集合
        /// </summary>
        /// <typeparam name="TEntity"></typeparam>
        /// <param name="models"></param>
        /// <returns></returns>
        public int Update<TEntity>(IEnumerable<TEntity> models) where TEntity : BaseEntity
        {
            return this.ChangeObjectState<TEntity>(models, EntityState.Modified);
        }

        /// <summary>
        /// 更新对象集合
        /// </summary>
        /// <typeparam name="TEntity"></typeparam>
        /// <param name="predicate"></param>
        /// <param name="updateAction"></param>
        /// <returns></returns>
        public int Update<TEntity>(Expression<Func<TEntity, bool>> predicate, Action<TEntity> updateAction) where TEntity : BaseEntity
        {
            if (predicate == null)
                throw new ArgumentNullException("predicate");
            if (updateAction == null)
                throw new ArgumentNullException("updateAction");
            using (var _context = new TContext())
            {
                _context.Configuration.AutoDetectChangesEnabled = true;
                var _model = _context.Set<TEntity>().Where(predicate).ToList();
                if (_model == null) return 0;
                _model.ForEach(p =>
                {
                    updateAction(p);
                });
                return _context.SaveChanges();
            }
        }

        /// <summary>
        /// 删除实体对象
        /// </summary>
        /// <typeparam name="TEntity"></typeparam>
        /// <param name="model"></param>
        /// <returns></returns>
        public int Delete<TEntity>(TEntity model) where TEntity : BaseEntity
        {
            return this.ChangeObjectState<TEntity>(model, EntityState.Deleted);
        }

        /// <summary>
        /// 删除实体对象集合
        /// </summary>
        /// <typeparam name="TEntity"></typeparam>
        /// <param name="models"></param>
        /// <returns></returns>
        public int Delete<TEntity>(IEnumerable<TEntity> models) where TEntity : BaseEntity
        {
            return this.ChangeObjectState<TEntity>(models, EntityState.Deleted);
        }

        /// <summary>
        /// 删除实体对象集合
        /// </summary>
        /// <typeparam name="TEntity"></typeparam>
        /// <param name="predicate"></param>
        /// <returns></returns>
        public int Delete<TEntity>(Func<TEntity, bool> predicate) where TEntity : BaseEntity
        {
            List<TEntity> _list = null;
            using (var _context = new TContext())
            {
                _list = _context.Set<TEntity>().Where(predicate).ToList();
                foreach (var item in _list)
                {
                    _context.Entry<TEntity>(item).State = EntityState.Deleted;
                }
                return _context.SaveChanges();
            }
        }

        #endregion 增删改

        #region 查询
        public IQueryable<TEntity> Search<TEntity>(Func<TEntity, bool> predicate = null) where TEntity : BaseEntity
        {
            var _context = new TContext();

            if (predicate == null)
            {
                return _context.Set<TEntity>();
            }
            else
            {
                return _context.Set<TEntity>().Where(predicate) as IQueryable<TEntity>;
            }

        }
        /// <summary>
        /// 查询单个记录
        /// </summary>
        /// <typeparam name="TEntity"></typeparam>
        /// <param name="predicate"></param>
        /// <returns></returns>
        public TEntity SearchFirstOrDefault<TEntity>(Func<TEntity, bool> predicate = null) where TEntity : BaseEntity
        {
            using (var _context = new TContext())
            {
                var model =  predicate == null
                    ? _context.Set<TEntity>().FirstOrDefault()
                    : _context.Set<TEntity>().Where(predicate).FirstOrDefault();

                //茅书华
                //利用反射对string赋值默认值空
                Type type = typeof(TEntity);
                PropertyInfo[] props = type.GetProperties(BindingFlags.Public | BindingFlags.Instance);
                if (props.Length > 0)
                {
                    foreach (PropertyInfo prop in props)
                    {
                        string propType = prop.PropertyType.Name;
                        object obj = prop.GetValue(model, null);
                        if (propType is string)
                        {
                            if (obj == null)
                            {
                                prop.SetValue(model, "");
                            }
                        }
                    }
                }

                return model;
            }
        }

        /// <summary>
        /// 查询多笔记录
        /// </summary>
        /// <typeparam name="TEntity"></typeparam>
        /// <param name="predicate"></param>
        /// <returns></returns>
        public List<TEntity> SearchList<TEntity>(Func<TEntity, bool> predicate = null) where TEntity : BaseEntity
        {
            using (var _context = new TContext())
            {
                var list =  predicate == null ?
                    _context.Set<TEntity>().ToList()
                    : _context.Set<TEntity>().Where(predicate).ToList();

                if (list.Count > 0)
                {
                    list.ForEach(model =>
                    {
                        //茅书华
                        //利用反射对string赋值默认值空
                        Type type = typeof(TEntity);
                        PropertyInfo[] props = type.GetProperties(BindingFlags.Public | BindingFlags.Instance);
                        if (props.Length > 0)
                        {
                            foreach (PropertyInfo prop in props)
                            {
                                string propType = prop.PropertyType.Name;
                                object obj = prop.GetValue(model, null);
                                if (propType is string)
                                {
                                    if (obj == null)
                                    {
                                        prop.SetValue(model, "");
                                    }
                                }
                            }
                        }
                    });
                }

                return list;
            }
        }

        /// <summary>
        /// 查询分页记录
        /// </summary>
        /// <typeparam name="TEntity"></typeparam>
        /// <param name="predicate"></param>
        /// <param name="order"></param>
        /// <param name="pageIndex"></param>
        /// <param name="pageSize"></param>
        /// <returns></returns>
        public PagedList<TEntity> SearchPagedList<TEntity>(Func<TEntity, bool> predicate, Func<IQueryable<TEntity>, IOrderedQueryable<TEntity>> order, int pageIndex, int pageSize) where TEntity : BaseEntity
        {
            if (order == null)
            {
                throw new ArgumentNullException("SearchPagedList_OrderedQueryable could not NULL！");
            }

            using (var _context = new TContext())
            {
                IQueryable<TEntity> _result;
                if (predicate == null)
                {
                    _result = _context.Set<TEntity>();
                }
                else
                {
                    _result = _context.Set<TEntity>().Where(predicate).AsQueryable();
                }

                _result = order(_result);

                return new PagedList<TEntity>(_result, pageIndex, pageSize);
            }
        }

        #endregion

        #region SQL&StoredProcedure
        /// <summary>
        /// 执行SQL语句
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="sql"></param>
        /// <param name="parameter"></param>
        /// <returns></returns>
        public T ExecuteText<T>(string sql, object parameter = null)
        {

            object result = null;

            CommandAction(sql, parameter, false, reader =>
            {
                while (reader.Read())
                {
                    result = ReaderObject<T>(reader);
                }
            });

            return result == null ? default(T) : (T)result;
        }

        /// <summary>
        /// 执行SQL语句,返回集合数据
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="sql"></param>
        /// <param name="parameter"></param>
        /// <returns></returns>
        public IEnumerable<T> ExecuteTextCollection<T>(string sql, object parameter = null)
        {
            var returnList = new List<T>();

            CommandAction(sql, parameter, false, reader =>
            {
                object result = null;
                while (reader.Read())
                {
                    result = ReaderObject<T>(reader);
                    if (result == null)
                    {
                        returnList.Add(default(T));
                    }
                    else
                    {
                        returnList.Add((T)result);
                    }
                }
            });

            return returnList;
        }

        /// <summary>
        /// 执行存储过程
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="procedureName"></param>
        /// <param name="parameter"></param>
        /// <returns></returns>
        public T ExecuteStoredProcedure<T>(string procedureName, object parameter = null)
        {

            object result = null;

            CommandAction(procedureName, parameter, true, reader =>
            {
                while (reader.Read())
                {
                    result = ReaderObject<T>(reader);
                }
            });

            return result == null ? default(T) : (T)result;
        }

        /// <summary>
        /// 执行存储过程,返回集合数据
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="procedureName"></param>
        /// <param name="parameter"></param>
        /// <returns></returns>
        public IEnumerable<T> ExecuteStoredProcedureCollection<T>(string procedureName, object parameter = null)
        {
            var returnList = new List<T>();

            CommandAction(procedureName, parameter, true, reader =>
            {
                object result = null;
                while (reader.Read())
                {
                    result = ReaderObject<T>(reader);
                    if (result == null)
                    {
                        returnList.Add(default(T));
                    }
                    else
                    {
                        returnList.Add((T)result);
                    }
                }
            });

            return returnList;
        }


        #endregion

        #region 私有方法

        /// <summary>
        /// 变更上下文管理器
        /// </summary>
        /// <typeparam name="TEntity"></typeparam>
        /// <param name="model"></param>
        /// <param name="state"></param>
        private int ChangeObjectState<TEntity>(TEntity model, EntityState state) where TEntity : BaseEntity
        {
            using (var _context = new TContext())
            {
                _context.Entry<TEntity>(model).State = state;
                return _context.SaveChanges();
            }
        }

        /// <summary>
        /// 变更上下文管理器
        /// </summary>
        /// <typeparam name="TEntity"></typeparam>
        /// <param name="model"></param>
        /// <param name="state"></param>
        private int ChangeObjectState<TEntity>(IEnumerable<TEntity> model, EntityState state) where TEntity : BaseEntity
        {
            if (model == null) return 0;
            using (var _context = new TContext())
            {
                model.ToList().ForEach(p => _context.Entry<TEntity>(p).State = state);
                return _context.SaveChanges();
            }
        }

        private void CommandAction(string text, object parameter, bool isProcedure, Action<SqlDataReader> action)
        {
            // Create the Command and Parameter objects.
            SqlConnection connection = new SqlConnection(ContextObj.ConnectionString);
            SqlCommand command = new SqlCommand(text, connection);
            command.CommandType = isProcedure ? CommandType.StoredProcedure : CommandType.Text;
            if (parameter != null)
            {
                CommonUtility.AnonymousObjectToHtmlAttributes(parameter).ForEach(p =>
                {
                    command.Parameters.AddWithValue("@" + p.Key, p.Value);
                });
            }

            try
            {
                connection.Open();
                SqlDataReader reader = command.ExecuteReader();
                action(reader);
                reader.Close();
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
                connection.Close();
            }
        }

        private object ReaderObject<T>(SqlDataReader reader)
        {
            object result = null;
            if (typeof(bool) == typeof(T))
            {
                result = ReaderElement(reader, 0, typeof(bool));
            }
            else if (typeof(byte) == typeof(T))
            {
                result = ReaderElement(reader, 0, typeof(byte));
            }
            else if (typeof(char) == typeof(T))
            {
                result = ReaderElement(reader, 0, typeof(char));
            }
            else if (typeof(DateTime) == typeof(T))
            {
                result = ReaderElement(reader, 0, typeof(DateTimeOffset));
            }
            else if (typeof(decimal) == typeof(T))
            {
                result = ReaderElement(reader, 0, typeof(decimal));
            }
            else if (typeof(double) == typeof(T))
            {
                result = ReaderElement(reader, 0, typeof(double));
            }
            else if (typeof(float) == typeof(T))
            {
                result = ReaderElement(reader, 0, typeof(float));
            }
            else if (typeof(Guid) == typeof(T))
            {
                result = ReaderElement(reader, 0, typeof(Guid));
            }
            else if (typeof(short) == typeof(T))
            {
                result = ReaderElement(reader, 0, typeof(short));
            }
            else if (typeof(int) == typeof(T))
            {
                result = ReaderElement(reader, 0, typeof(int));
            }
            else if (typeof(string) == typeof(T))
            {
                result = ReaderElement(reader, 0, typeof(string));
            }
            else
            {
                result = System.Activator.CreateInstance<T>();
                int i = 0;
                foreach (PropertyDescriptor property in TypeDescriptor.GetProperties(result))
                {
                    try
                    {
                        property.SetValue(result, ReaderElement(reader, i, property.PropertyType));
                        i++;
                    }
                    catch (Exception ex)
                    {
                        throw new ArgumentException("绑定属性【" + property.Name + "】发生异常:" + ex.Message);
                    }
                }
            }
            return result;
        }

        private object ReaderElement(SqlDataReader reader, int count, Type type)
        {
            if (typeof(bool) == type)
            {
                return reader.GetBoolean(count);
            }
            else if (typeof(byte) == type)
            {
                return reader.GetByte(count);
            }
            else if (typeof(char) == type)
            {
                return reader.GetChar(count);
            }
            else if (typeof(DateTime) == type)
            {
                return reader.GetDateTime(count);
            }
            else if (typeof(DateTimeOffset) == type)
            {
                return reader.GetDateTimeOffset(count);
            }
            else if (typeof(decimal) == type)
            {
                return reader.GetDecimal(count);
            }
            else if (typeof(double) == type)
            {
                return reader.GetDouble(count);
            }
            else if (typeof(float) == type)
            {
                return reader.GetFloat(count);
            }
            else if (typeof(Guid) == type)
            {
                return reader.GetGuid(count);
            }
            else if (typeof(short) == type)
            {
                return reader.GetInt16(count);
            }
            else if (typeof(int) == type)
            {
                return reader.GetInt32(count);
            }
            else if (typeof(long) == type)
            {
                return reader.GetInt64(count);
            }
            else
            {
                return reader.GetString(count);
            }
        }
        #endregion
    }
}
