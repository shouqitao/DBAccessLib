using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Text;

namespace DBAccessLib
{
    /// <summary>
    /// SQL Server �����ࡣ
    /// </summary>
    public class DBAccessOfSql
    {
        #region ȡ�����Ӷ��󡾹ؼ����ԡ�
        /// <summary>
        /// �������ݿ��ַ�����
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
        #endregion ȡ�����Ӷ���

        #region �Ͽ�����
        /// <summary>
        /// �Ͽ�����
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
                throw new Exception(ex.Message + " [Disconnect on DBAccessOfSql] �Ͽ����ӳ���");
            }
            finally
            {
                conn = null;
            }
        }
        #endregion

        #region ����ǲ��Ǵ���
        /// <summary>
        /// ����ǲ��Ǵ���
        /// </summary>
        /// <param name="sql"></param>
        /// <returns>true,����;false,�����ڡ�</returns>
        public static bool IsExits(string sql)
        {
            if (null == RunSQLReturnVal(sql))
            {
                return false;
            }

            return true;
        }
        #endregion

        #region ִ��SQL ��������������
        /// <summary>
        /// ��ѯ����ֵ�ķ���
        /// </summary>
        /// <param name="sql"></param>
        /// <returns>����ֵ</returns>
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

        #region ���� RunSQLReturnTable
        /// <summary>
        /// ����SQL��������ص�DataTable��
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
            // ���������״̬���͵ȴ�
            while (lock_msSQL_ReturnTable)
            {
                ;
            }

            // ����
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

                // ����ǰһ��Ҫ����
                lock_msSQL_ReturnTable = false;
            }
        }
        #endregion

        #region ִ�� SQL ��䣬������Ӱ�������
        /// <summary>
        /// ִ�� SQL ��䣬������Ӱ���������
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

        #region ����ִ�� SQL ��䣬������Ӱ�������
        /// <summary>
        /// ����ִ�� SQL ��䣬������Ӱ���������
        /// </summary>
        /// <param name="sqls">Ҫ����ִ�е�sqls���</param>
        /// <returns>��Ӱ�������</returns>
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
