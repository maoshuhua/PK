using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Configuration;
using System.Data;
using System.Data.OracleClient;
using Tuhui.Common.Framework;
using Tuhui.Common.Utility;

namespace Tuhui.OH.Api.Utility
{
    public class OracleBaseDataAccess
    {
        private string connectionString { get; set; }

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

        public IEnumerable<T> ExecuteModelCollection<T>(string sql, object parameter = null)
        {
            var returnList = new List<T>();

            CommandAction(sql, parameter, false, reader =>
            {
                object result = null;
                while (reader.Read())
                {
                    result = ReaderModel<T>(reader);
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

        public IEnumerable<T> ExecuteModelCollection_gywr<T>(string sql, object parameter = null)
        {
            var returnList = new List<T>();

            CommandAction_gywr(sql, parameter, false, reader =>
            {
                object result = null;
                while (reader.Read())
                {
                    result = ReaderModel<T>(reader);
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

        private void CommandAction(string text, object parameter, bool isProcedure, Action<OracleDataReader> action)
        {
            OracleConnection connection = new OracleConnection(ConfigurationManager.ConnectionStrings["huanbaoju"].ConnectionString);
            OracleCommand command = new OracleCommand(text, connection);

            command.CommandType = isProcedure ? CommandType.StoredProcedure : CommandType.Text;
            if (parameter != null)
            {
                ObjectUtility.AnonymousObjectToHtmlAttributes(parameter).ForEach(p =>
                {
                    command.Parameters.Add("@" + p.Key, p.Value);
                });
            }

            try
            {
                connection.Open();
                OracleDataReader reader = command.ExecuteReader();
                action(reader);
                reader.Close();
            }
            catch(Exception e)
            {
                //throw new UserException(e.Message);
            }
            finally
            {
                connection.Close();
            }
        }

        private void CommandAction_gywr(string text, object parameter, bool isProcedure, Action<OracleDataReader> action)
        {
            OracleConnection connection = new OracleConnection(ConfigurationManager.ConnectionStrings["huanbaoju_gywr"].ConnectionString);
            OracleCommand command = new OracleCommand(text, connection);

            command.CommandType = isProcedure ? CommandType.StoredProcedure : CommandType.Text;
            if (parameter != null)
            {
                ObjectUtility.AnonymousObjectToHtmlAttributes(parameter).ForEach(p =>
                {
                    command.Parameters.Add("@" + p.Key, p.Value);
                });
            }

            try
            {
                connection.Open();
                OracleDataReader reader = command.ExecuteReader();
                action(reader);
                reader.Close();
            }
            catch
            {
                throw new UserException("E001002");
            }
            finally
            {
                connection.Close();
            }
        }

        private object ReaderObject<T>(OracleDataReader reader)
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
                result = ReaderElement(reader, 0, typeof(DateTime));
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

        private object ReaderElement(OracleDataReader reader, int count, Type type)
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
            //else if (typeof(DateTimeOffset) == type)
            //{
            //    return reader.GetDateTimeOffset(count);
            //}
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
                return reader.IsDBNull(count)?"":reader.GetString(count);
            }
        }

        private object ReaderModel<T>(OracleDataReader reader)
        {
            object result = null;

            result = System.Activator.CreateInstance<T>();
            int i = 0;

            foreach (PropertyDescriptor property in TypeDescriptor.GetProperties(result))
            {
                try
                {
                    var type = property.PropertyType;
                    object val = null;

                    // 针对可空类型 不同情况进行判断
                    if (reader.IsDBNull(i))
                    {
                        // 可空类型 如果为空 任何类型 统一赋空值
                        val = null;
                        //// string类型 为空的情况 设置为空字符串，
                        //if (type == typeof(string))
                        //{
                        //    val = null;
                        //}
                        //// DateTime类型 为空的情况 设置为
                        //else if(type==typeof(DateTime?))
                        //{
                        //    val = null;
                        //}
                        //// 其它类型设置为0
                        //else
                        //{
                        //    val = 0;
                        //}
                    }
                    else
                    {
                        val = reader.GetValue(i);
                    }
                    property.SetValue(result, val);
                    i++;
                }
                catch (Exception ex)
                {
                    throw new ArgumentException("绑定属性【" + property.Name + "】发生异常:" + ex.Message);
                }
            }

            return result;
        }
    }
}
