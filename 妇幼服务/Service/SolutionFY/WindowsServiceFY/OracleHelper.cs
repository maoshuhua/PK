using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Configuration;
using System.Data;
using System.Data.OracleClient;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WindowsServiceFY
{

    /// <summary>  
    /// Oracle数据库操作类  
    /// </summary>  
    public class OracleHelper
    {
         public static OracleCommand cmd = null;

        public static OracleConnection conn = null;

        public static string connstr = ConfigurationManager.ConnectionStrings["oracle"].ConnectionString;

        public OracleHelper()

        { }

        #region 建立Oracle数据库连接对象

        /// <returns>返回一个数据库连接的OracleConnection对象</returns>  

        public static OracleConnection init()
        {

            try
            {

                conn = new OracleConnection(connstr);

                if (conn.State != ConnectionState.Open)
                {

                    conn.Open();

                }

            }

            catch (Exception e)
            {

                throw new Exception(e.Message.ToString());

            }

            return conn;

        }

        #endregion

        #region 设置OracleCommand对象

        /// <param name="cmd">OracleCommand对象 </param>  

        /// <param name="cmdText">命令文本</param>  

        /// <param name="cmdType">命令类型</param>  

        /// <param name="cmdParms">参数集合</param>  

        private static void SetCommand(OracleCommand cmd, string cmdText, CommandType cmdType, OracleParameter[] cmdParms)
        {

            cmd.Connection = conn;

            cmd.CommandText = cmdText;

            cmd.CommandType = cmdType;

            if (cmdParms != null)
            {

                cmd.Parameters.AddRange(cmdParms);

            }

        }

        #endregion

        #region 执行相应的Oracle sql语句，返回相应的DataSet对象
        /// <param name="sqlstr">sql语句</param>  

        /// <returns>返回相应的DataSet对象</returns>  

        public static DataSet GetDataSet(string sqlstr)
        {

            DataSet set = new DataSet();

            try
            {

                init();

                OracleDataAdapter adp = new OracleDataAdapter(sqlstr, conn);

                adp.Fill(set);

                conn.Close();

            }

            catch (Exception e)
            {

                throw new Exception(e.Message.ToString());

            }

            return set;

        }
        #endregion

        #region 执行相应的Oracle sql语句，返回相应的DataTable对象

        /// <param name="sqlstr">sql语句</param>  

        /// <returns>返回相应的DataTable对象</returns>  

        public static DataTable GetDataTable(string sqlstr)
        {

            DataTable table = new DataTable();

            try
            {

                init();

                OracleDataAdapter adp = new OracleDataAdapter(sqlstr, conn);

                adp.Fill(table);

                conn.Close();

            }

            catch (Exception e)
            {

                throw new Exception(e.Message.ToString());

            }

            return table;

        }

        #endregion

        #region 执行sql语句，返回DataSet对象

        /// <param name="sqlstr">sql语句</param>  

        /// <param name="tableName">表名</param>  

        /// <returns>返回DataSet对象</returns>  

        public static DataSet GetDataSet(string sqlstr, string tableName)
        {

            DataSet set = new DataSet();

            try
            {

                init();

                OracleDataAdapter adp = new OracleDataAdapter(sqlstr, conn);

                adp.Fill(set, tableName);

                conn.Close();

            }

            catch (Exception e)
            {

                throw new Exception(e.Message.ToString());

            }

            return set;

        }

        #endregion

        #region 执行不带参数的sql语句，返回受影响的行数

        /// <param name="cmdstr">增，删，改sql语句</param>  

        /// <returns>返回受影响的行数</returns>  

        public static int ExecuteNonQuery(string cmdText)
        {

            int count;

            try
            {

                init();

                cmd = new OracleCommand(cmdText, conn);

                count = cmd.ExecuteNonQuery();

                conn.Close();

            }

            catch (Exception ex)
            {

                throw new Exception(ex.Message.ToString());

            }

            return count;

        }

        #endregion

        #region 执行带参数的Oracle sql语句或存储过程，返回行数

        /// <param name="cmdText">带参数的sql语句和存储过程名</param>  

        /// <param name="cmdType">命令类型</param>  

        /// <param name="cmdParms">参数集合</param>  

        /// <returns>返回受影响的行数</returns>  

        public static int ExecuteNonQuery(string cmdText, CommandType cmdType, OracleParameter[] cmdParms)
        {

            int count;

            try
            {

                init();

                cmd = new OracleCommand();

                SetCommand(cmd, cmdText, cmdType, cmdParms);

                count = cmd.ExecuteNonQuery();

                cmd.Parameters.Clear();

                conn.Close();

            }

            catch (Exception ex)
            {

                throw new Exception(ex.Message.ToString());

            }

            return count;

        }

        #endregion

        #region 执行不带参数sql语句，返回一个从数据源读取数据的OracleDataReader对象

        /// <param name="cmdstr">相应的sql语句</param>  

        /// <returns>返回一个从数据源读取数据的OracleDataReader对象</returns>  

        public static OracleDataReader ExecuteReader(string cmdText)
        {

            OracleDataReader reader;

            try
            {

                init();

                cmd = new OracleCommand(cmdText, conn);

                reader = cmd.ExecuteReader(CommandBehavior.CloseConnection);

            }

            catch (Exception ex)
            {

                throw new Exception(ex.Message.ToString());

            }

            return reader;

        }

        #endregion

        #region 执行带参数的sql语句或存储过程，返回一个从数据源读取数据的OracleDataReader对象
        /// <param name="cmdText">sql语句或存储过程名</param>  

        /// <param name="cmdType">命令类型</param>  

        /// <param name="cmdParms">参数集合</param>  

        /// <returns>返回一个从数据源读取数据的OracleDataReader对象</returns>  

        public static OracleDataReader ExecuteReader(string cmdText, CommandType cmdType, OracleParameter[] cmdParms)
        {

            OracleDataReader reader;

            try
            {

                init();

                cmd = new OracleCommand();

                SetCommand(cmd, cmdText, cmdType, cmdParms);

                reader = cmd.ExecuteReader(CommandBehavior.CloseConnection);

            }

            catch (Exception ex)
            {

                throw new Exception(ex.Message.ToString());

            }

            return reader;

        }

        #endregion

        #region 执行不带参数sql语句,返回结果集首行首列的值object

        /// <param name="cmdstr">相应的sql语句</param>  

        /// <returns>返回结果集首行首列的值object</returns>  

        public static object ExecuteScalar(string cmdText)
        {

            object obj;

            try
            {

                init();

                cmd = new OracleCommand(cmdText, conn);

                obj = cmd.ExecuteScalar();

                conn.Close();

            }

            catch (Exception ex)
            {

                throw new Exception(ex.Message.ToString());

            }

            return obj;

        }

        #endregion

        #region 执行带参数sql语句或存储过程,返回结果集首行首列的值object

        /// <param name="cmdText">sql语句或存储过程名</param>  

        /// <param name="cmdType">命令类型</param>  

        /// <param name="cmdParms">返回结果集首行首列的值object</param>  

        /// <returns></returns>  

        public static object ExecuteScalar(string cmdText, CommandType cmdType, OracleParameter[] cmdParms)
        {

            object obj;

            try
            {

                init();

                cmd = new OracleCommand();

                SetCommand(cmd, cmdText, cmdType, cmdParms);

                obj = cmd.ExecuteScalar();

                conn.Close();

            }

            catch (Exception ex)
            {

                throw new Exception(ex.Message.ToString());

            }

            return obj;

        }

        #endregion
    }  

}
