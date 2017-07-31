using System;
using System.Data.OracleClient;
using System.Data;
using System.Collections.Generic;

namespace DBAccessLib
{
    /// <summary>
    /// Oracle9i 的访问类。
    /// </summary>
    public class DBAccessOfOracle9i
    {
        /// <summary>
        /// 默认构造函数。
        /// </summary>
        public DBAccessOfOracle9i()
        {
        }

        #region 取得连接对象【关键属性】
        //static string connectStr = System.Configuration.ConfigurationSettings.AppSettings["connectStr"];//connectStr connectStr-26
        static string connectStr;

        public static string ConnectStr
        {
            get { return DBAccessOfOracle9i.connectStr; }
            set { DBAccessOfOracle9i.connectStr = value; }
        }

        public static OracleConnection GetConn
        {
            get
            {
                OracleConnection conn = new OracleConnection(connectStr);

                return conn;

            }
        }
        #endregion 取得连接对象

        #region 断开连接
        /// <summary>
        /// 断开连接
        /// </summary>
        private static void Disconnect(OracleConnection conn)
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
                throw new Exception(ex.Message + " [Disconnect on DBAccessOfOracle9i] 断开连接出错。");
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
        /// <param name="selectSQL">SQL语句</param>
        /// <returns>true,存在;false,不存在。</returns>
        public static bool IsExits(string selectSQL)
        {
            if (RunSQLReturnVal(selectSQL) == null)
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
        /// <returns></returns>
        public static object RunSQLReturnVal(string sql)
        {
            object val = null;

            OracleConnection conn = null;
            OracleCommand cmd = null;

            try
            {
                conn = GetConn;
                if (ConnectionState.Open != conn.State)
                {
                    conn.Open();
                }
                cmd = new OracleCommand(sql, conn);
                cmd.CommandType = CommandType.Text;
                val = cmd.ExecuteScalar();
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message + " [RunSQLReturnVal on OracleConnection] " + sql);
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
        ///远行Sql结果返回到DataTable
        /// </summary>
        /// <param name="sql">Sql语句</param>
        /// <returns></returns>
        public static DataTable RunSQLReturnTable(string sql)
        {
            return RunSQLReturnTable(sql, GetConn, CommandType.Text);
        }

        private static bool lock_msSQL_ReturnTable = false;

        public static DataTable RunSQLReturnTable(string msSQL, OracleConnection conn, CommandType sqlType)
        {
            // 如果是锁定状态，就等待
            while (lock_msSQL_ReturnTable)
            {
                ;
            }

            lock_msSQL_ReturnTable = true; // 锁定
            OracleDataAdapter da = null;

            try
            {
                da = new OracleDataAdapter(msSQL, conn);
                da.SelectCommand.CommandType = sqlType;
                DataTable dt = new DataTable("mstb");
                da.Fill(dt);

                return dt;
            }
            catch (System.Exception ex)
            {
                //AppLog.WriteLineAppLog("Error:" + System.DateTime.Now.ToString("yyyy年M月d日,HH:mm:ss fff ")+ex.Message+" Oracle [RunSQLReturnTable on SqlConnection] "+msSQL);
                throw new Exception(ex.Message + " Oracle [RunSQLReturnTable on OracleConnection] " + msSQL);
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
        /// <param name="sql"></param>
        /// <returns></returns>
        public static int RunSQL(string sql)
        {
            return RunSQL(sql, GetConn, CommandType.Text);
        }

        public static int RunSQL(string sql, OracleConnection conn, CommandType sqlType)
        {
            OracleCommand cmd = null;

            try
            {
                cmd = new OracleCommand(sql, conn);
                cmd.CommandType = sqlType;

                if (conn.State != System.Data.ConnectionState.Open)
                {
                    conn.Open();
                }
                int i = cmd.ExecuteNonQuery();

                return i;

            }
            catch (System.Exception ex)
            {
                //AppLog.WriteLineAppLog("Error:" + System.DateTime.Now.ToString("yyyy年M月d日,HH:mm:ss fff ")+ex.Message+" Oracle [RunSQLReturnTable on SqlConnection] "+sql);
                throw new Exception(ex.Message + sql);
            }
            finally
            {
                if (null != cmd)
                {
                    cmd.Dispose();
                    cmd = null;
                }

                // 处理完数据要关闭与数据库的连接
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
            OracleConnection conn = GetConn;
            OracleCommand cmd = null;
            OracleTransaction tx = null;

            try
            {
                if (conn.State != ConnectionState.Open)
                {
                    conn.Open();
                }

                cmd = new OracleCommand();
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

    }

}
