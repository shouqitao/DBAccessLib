using System;
using System.Configuration;
using System.Data.Odbc;
using System.Data;
using System.Collections.Generic;

namespace DBAccessLib
{
    /// <summary>
    /// DBAccessOfOdbc 的摘要说明。
    /// </summary>
    public class DBAccessOfOdbc
    {
        #region 构造函数
        /// <summary>
        /// 构造函数。
        /// </summary>
        public DBAccessOfOdbc()
        {
            //
            // TODO: 在此处添加构造函数逻辑
            //
        }
        #endregion

        #region 取得连接对象【关键属性】
        // ConfigurationManager.ConnectionStrings["GPSToArcGIS.Properties.Settings.ConString"].ConnectionString;
        static string connectStr = "";

        /// <summary>
        /// 连接字符串
        /// </summary>
        public static string ConnectStr
        {
            get { return DBAccessOfOdbc.connectStr; }
            set { DBAccessOfOdbc.connectStr = value; }
        }

        public static OdbcConnection GetConn
        {
            get
            {
                OdbcConnection conn = new OdbcConnection(ConnectStr);
                return conn;
            }
        }
        #endregion 取得连接对象

        #region 断开连接
        /// <summary>
        /// 断开连接
        /// </summary>
        private static void Disconnect(OdbcConnection conn)
        {
            try
            {
                if (null != conn && ConnectionState.Closed != conn.State)
                {
                    conn.Close();
                }
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message + " [Disconnect on DBAccessOfOdbc] 断开连接出错。");
            }
            finally
            {
                conn = null;
            }
        }
        #endregion

        #region 检查是不是存在
        /// <summary>
        /// 检查是不是存在
        /// </summary>
        /// <param name="sql"></param>
        /// <returns>检查是不是存在</returns>
        public static bool IsExits(string sql)
        {
            if (null == RunSQLReturnVal(sql))
            {
                return false;
            }

            return true;
        }
        #endregion

        #region 执行SQL，返回首行首列
        /// <summary>
        /// 查询单个值的方法
        /// </summary>
        /// <param name="sql"></param>
        /// <returns></returns>
        public static object RunSQLReturnVal(string sql)
        {
            object val = null;
            OdbcConnection conn = null;
            OdbcCommand cmd = null;

            try
            {
                conn = GetConn;
                if (ConnectionState.Open != conn.State)
                {
                    conn.Open();
                }
                cmd = new OdbcCommand(sql, conn);
                cmd.CommandType = CommandType.Text;

                val = cmd.ExecuteScalar();
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message + " [RunSQLReturnVal on DBAccessOfOdbc] " + sql);
            }
            finally
            {
                if (null != cmd)
                {
                    cmd.Dispose();
                    cmd = null;
                }

                Disconnect(conn);
            }

            return val;
        }
        #endregion

        #region 重载 RunSQLReturnTable
        /// <summary>
        /// 运行Sql结果返回到DataTable
        /// </summary>
        /// <param name="sql"></param>
        /// <returns></returns>
        public static DataTable RunSQLReturnTable(string sql)
        {
            return RunSQLReturnTable(sql, GetConn, CommandType.Text);
        }

        private static bool lock_msSQL_ReturnTable = false;

        /// <summary>
        /// 运行sql，返回DataTable
        /// </summary>
        /// <param name="msSQL"></param>
        /// <param name="conn"></param>
        /// <param name="sqlType"></param>
        /// <returns></returns>
        public static DataTable RunSQLReturnTable(string msSQL, OdbcConnection conn, CommandType sqlType)
        {
            // 如果是锁定状态，就等待
            while (lock_msSQL_ReturnTable)
            {
                ;
            }

            // 锁定
            lock_msSQL_ReturnTable = true;
            OdbcDataAdapter da = null;

            try
            {
                da = new OdbcDataAdapter(msSQL, conn);
                da.SelectCommand.CommandType = sqlType;
                DataTable dtb = new DataTable("mstb");
                da.Fill(dtb);

                return dtb;
            }
            catch (System.Exception ex)
            {
                throw new Exception(ex.Message + " [RunSQLReturnTable on DBAccessOfOdbc] " + msSQL);
            }
            finally
            {
                if (null != da)
                {
                    da.Dispose();
                    da = null;
                }

                Disconnect(conn);

                // 返回前一定要开锁
                lock_msSQL_ReturnTable = false;
            }
        }
        #endregion

        #region 执行 SQL 语句，返回受影响的行数
        /// <summary>
        /// 执行 SQL 语句，返回受影响的行数
        /// </summary>
        /// <param name="sql">SQL语句</param>
        /// <returns></returns>
        public static int RunSQL(string sql)
        {
            return RunSQL(sql, GetConn, CommandType.Text);
        }

        public static int RunSQL(string sql, OdbcConnection conn, CommandType sqlType)
        {
            OdbcCommand cmd = null;

            try
            {
                cmd = new OdbcCommand(sql, conn);
                cmd.CommandType = sqlType;

                if (conn.State != ConnectionState.Open)
                {
                    conn.Open();
                }

                int i = cmd.ExecuteNonQuery();
                return i;
            }
            catch (System.Exception ex)
            {
                throw new Exception(ex.Message + " [RunSQL on DBAccessOfOdbc] " + sql);
            }
            finally
            {
                if (null != cmd)
                {
                    cmd.Dispose();
                    cmd = null;
                }

                Disconnect(conn);
            }
        }
        #endregion

        #region 批量执行 SQL 语句，返回受影响的行数
        /// <summary>
        /// 批量执行 SQL 语句，返回受影响的行数。
        /// </summary>
        /// <param name="sqls">要批量执行的sqls语句</param>
        /// <returns>受影响的行数</returns>
        public static int RunSQL(List<string> sqls)
        {
            int count = 0;
            string sql = "";
            OdbcConnection conn = GetConn;
            OdbcCommand cmd = null;
            OdbcTransaction tx = null;

            try
            {
                if (conn.State != ConnectionState.Open)
                {
                    conn.Open();
                }

                cmd = new OdbcCommand();
                cmd.CommandType = CommandType.Text;
                cmd.Connection = conn;
                tx = conn.BeginTransaction();
                cmd.Transaction = tx;

                for (int i = 0; i < sqls.Count; i++)
                {
                    if (null != sqls[i] && sqls[i].Trim().Length > 0)
                    {
                        sql = sqls[i].Trim();
                        cmd.CommandText = sql;
                        int n = cmd.ExecuteNonQuery();
                        count += n;
                    }
                }

                tx.Commit();
                return count;
            }
            catch (System.Exception ex)
            {
                tx.Rollback();
                throw new Exception(ex.Message + " [RunSQL on DBAccessOfOleDb] " + sql);
            }
            finally
            {
                if (null != tx)
                {
                    tx.Dispose();
                    tx = null;
                }

                if (null != cmd)
                {
                    cmd.Dispose();
                    cmd = null;
                }

                Disconnect(conn);
            }
        }
        #endregion

        /*#region 执行 INSERT SQL 语句，返回自增主键
        /// <summary>
        /// 执行 INSERT SQL 语句，返回自增主键
        /// </summary>
        /// <param name="sql">INSERT SQL语句</param>
        /// <returns></returns>
        public static int RunSQLReturnId(string sql)
        {
            OdbcConnection conn = GetConn;
            OdbcCommand cmd = null;

            try
            {
                cmd = new OdbcCommand(sql, conn);
                cmd.CommandType = CommandType.Text;

                if (conn.State != ConnectionState.Open)
                {
                    conn.Open();
                }

                if (cmd.ExecuteNonQuery() > 0)
                {
                    sql = sql.ToUpper().Replace("INSERT").Replace("INTO").Trim();
                    int pos = sql.IndexOf('(');
                    int pos2 = sql.IndexOf(' ');
                    if (pos > pos2)
                    {
                         pos = pos2;
                    }
                    string tblName = sql.Remove(pos);
                    cmd.CommandText = "select top 1 * from " + tblName;
                    Object obj = cmd.ExecuteScalar();
                    int id = Convert.ToInt32(obj);
                    return id;
                }

                //int id = Convert.ToInt32(obj);
                return 1;
            }
            catch (System.Exception ex)
            {
                throw new Exception(ex.Message + " [RunSQLReturnId on DBAccessOfOdbc] " + sql);
            }
            finally
            {
                if (null != cmd)
                {
                    cmd.Dispose();
                    cmd = null;
                }

                Disconnect(conn);
            }
        }
        #endregion*/
    }
}
