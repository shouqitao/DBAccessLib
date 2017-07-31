using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Text;

namespace DBAccessLib
{
    /// <summary>
    /// SQL Server 访问类。
    /// </summary>
    public class DBAccessOfSql
    {
        #region 取得连接对象【关键属性】
        /// <summary>
        /// 连接数据库字符串。
        /// </summary>
        private static string connectStr;

        public static string ConnectStr
        {
            get { return DBAccessOfSql.connectStr; }
            set { DBAccessOfSql.connectStr = value; }
        }


        public static SqlConnection GetConn
        {
            get
            {
                SqlConnection conn = new SqlConnection(ConnectStr);
                return conn;
            }
        }
        #endregion 取得连接对象

        #region 断开连接
        /// <summary>
        /// 断开连接
        /// </summary>
        private static void Disconnect(SqlConnection conn)
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
                throw new Exception(ex.Message + " [Disconnect on DBAccessOfSql] 断开连接出错。");
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
        /// <returns>true,存在;false,不存在。</returns>
        public static bool IsExits(string sql)
        {
            if (null == RunSQLReturnVal(sql))
            {
                return false;
            }

            return true;
        }
        #endregion

        #region 执行SQL ，返回首行首列
        /// <summary>
        /// 查询单个值的方法
        /// </summary>
        /// <param name="sql"></param>
        /// <returns>单个值</returns>
        public static object RunSQLReturnVal(string sql)
        {
            object val = null;
            SqlConnection conn = null;
            SqlCommand cmd = null;

            try
            {
                conn = GetConn;
                if (ConnectionState.Open != conn.State)
                {
                    conn.Open();
                }
                cmd = new SqlCommand(sql, conn);
                val = cmd.ExecuteScalar();
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message + " [RunSQLReturnVal on DBAccessOfSql] " + sql);
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
        /// 运行SQL，结果返回到DataTable。
        /// </summary>
        /// <param name="sql"></param>
        /// <returns></returns>
        public static DataTable RunSQLReturnTable(string sql)
        {
            return RunSQLReturnTable(sql, GetConn, CommandType.Text);
        }

        private static bool lock_msSQL_ReturnTable = false;

        public static DataTable RunSQLReturnTable(string msSQL, SqlConnection conn, CommandType sqlType)
        {
            // 如果是锁定状态，就等待
            while (lock_msSQL_ReturnTable)
            {
                ;
            }

            // 锁定
            lock_msSQL_ReturnTable = true;
            SqlDataAdapter da = null;

            try
            {
                da = new SqlDataAdapter(msSQL, conn);
                da.SelectCommand.CommandType = sqlType;
                DataTable dt = new DataTable("mstb");
                da.Fill(dt);

                return dt;
            }
            catch (System.Exception ex)
            {
                throw new Exception(ex.Message + " [RunSQLReturnTable on DBAccessOfSql] " + msSQL);
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
        /// 执行 SQL 语句，返回受影响的行数。
        /// </summary>
        /// <param name="sql"></param>
        /// <returns></returns>
        public static int RunSQL(string sql)
        {
            return RunSQL(sql, GetConn, CommandType.Text);
        }

        public static int RunSQL(string sql, SqlConnection conn, CommandType sqlType)
        {
            SqlCommand cmd = null;

            try
            {
                cmd = new SqlCommand(sql, conn);
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
                throw new Exception(ex.Message + " [RunSQL on DBAccessOfSql] " + sql);
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
            SqlConnection conn = GetConn;
            SqlCommand cmd = null;
            SqlTransaction tx = null;

            try
            {
                if (conn.State != ConnectionState.Open)
                {
                    conn.Open();
                }

                cmd = new SqlCommand();
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
                throw new Exception(ex.Message + " [RunSQL on DBAccessOfSql] " + sql);
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
    }
}
