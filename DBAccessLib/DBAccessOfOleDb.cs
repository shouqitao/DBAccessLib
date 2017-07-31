using System;
using System.Configuration;
using System.Data.OleDb;
using System.Data;
using System.IO;
using System.Collections.Generic;

namespace DBAccessLib
{
    /// <summary>
    /// OLE DB �ķ����ࡣ
    /// </summary>
    public class DBAccessOfOleDb
    {
        #region Ĭ�Ϲ��캯��
        /// <summary>
        /// Ĭ�Ϲ��캯����
        /// </summary>
        public DBAccessOfOleDb()
        {
        }
        #endregion

        #region ȡ�����Ӷ��󡾹ؼ����ԡ�
        /// <summary>
        /// �������ݿ��ַ�����
        /// </summary>
        private static string connectStr;

        public static string ConnectStr
        {
            get { return DBAccessOfOleDb.connectStr; }
            set { DBAccessOfOleDb.connectStr = value; }
        }

        public static OleDbConnection GetConn
        {
            get
            {
                OleDbConnection conn = new OleDbConnection(ConnectStr);
                return conn;
            }
        }
        #endregion ȡ�����Ӷ���

        #region �Ͽ�����
        /// <summary>
        /// �Ͽ�����
        /// </summary>
        private static void Disconnect(OleDbConnection conn)
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
                throw new Exception(ex.Message + " [Disconnect on DBAccessOfOleDb] �Ͽ����ӳ���");
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
            OleDbConnection conn = null;
            OleDbCommand cmd = null;

            try
            {
                conn = GetConn;
                if (ConnectionState.Open != conn.State)
                {
                    conn.Open();
                }
                cmd = new OleDbCommand(sql, conn);
                val = cmd.ExecuteScalar();
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message + " [RunSQLReturnVal on DBAccessOfOleDb] " + sql);
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

        public static DataTable RunSQLReturnTable(string msSQL, OleDbConnection conn, CommandType sqlType)
        {
            // ���������״̬���͵ȴ�
            while (lock_msSQL_ReturnTable)
            {
                ;
            }

            // ����
            lock_msSQL_ReturnTable = true;
            OleDbDataAdapter da = null;

            try
            {
                da = new OleDbDataAdapter(msSQL, conn);
                da.SelectCommand.CommandType = sqlType;
                DataTable dt = new DataTable("mstb");
                da.Fill(dt);

                return dt;
            }
            catch (System.Exception ex)
            {
                throw new Exception(ex.Message + " [RunSQLReturnTable on DBAccessOfOleDb] " + msSQL);
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

        public static int RunSQL(string sql, OleDbConnection conn, CommandType sqlType)
        {
            OleDbCommand cmd = null;

            try
            {
                cmd = new OleDbCommand(sql, conn);
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
                throw new Exception(ex.Message + " [RunSQL on DBAccessOfOleDb] " + sql);
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
            OleDbConnection conn = GetConn;
            OleDbCommand cmd = null;
            OleDbTransaction tx = null;

            try
            {
                if (conn.State != ConnectionState.Open)
                {
                    conn.Open();
                }

                cmd = new OleDbCommand();
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

        #region ִ�� INSERT SQL ��䣬������������
        /// <summary>
        /// ִ�� INSERT SQL ��䣬������������
        /// </summary>
        /// <param name="sql">INSERT SQL���</param>
        /// <returns></returns>
        public static int RunSQLReturnId(string sql)
        {
            OleDbConnection conn = GetConn;
            OleDbCommand cmd = null;
            sql = sql + ";select SCOPE_IDENTITY()";

            try
            {
                cmd = new OleDbCommand(sql, conn);
                cmd.CommandType = CommandType.Text;

                if (conn.State != ConnectionState.Open)
                {
                    conn.Open();
                }

                Object obj = cmd.ExecuteScalar();
                int id = Convert.ToInt32(obj);
                return id;
            }
            catch (System.Exception ex)
            {
                throw new Exception(ex.Message + " [RunSQLReturnId on DBAccessOfOle] " + sql);
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

        /// <summary>
        /// �����ļ������ݿ��С�
        /// </summary>
        /// <param name="sql"></param>
        /// <param name="fileName"></param>
        /// <returns></returns>
        public static int RunSQLImportFile(string sql, string fileName)
        {
            OleDbConnection conn = GetConn;
            OleDbCommand cmd = null;

            try
            {
                FileInfo fi = new FileInfo(fileName);
                FileStream fs = fi.OpenRead();
                byte[] bytes = new byte[fs.Length];
                fs.Read(bytes, 0, Convert.ToInt32(fs.Length));
                cmd = new OleDbCommand(sql, conn);
                cmd.CommandType = CommandType.Text; //.StoredProcedure;
                if (conn.State != ConnectionState.Open)
                {
                    conn.Open();
                }
                //cmd.CommandText = "insert into "+tableName+"("+fieldName+") values(@file)";
                OleDbParameter spFile = new OleDbParameter("@file", OleDbType.Binary); // ("file", OleDbType.Binary);
                spFile.Value = bytes;
                cmd.Parameters.Add(spFile);
                int cnt = cmd.ExecuteNonQuery();
                return cnt;
            }
            catch (System.Exception ex)
            {
                throw new Exception(ex.Message + " [RunSQLImportFile on DBAccessOfOle] " + sql);
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

        /// <summary>
        /// �����ݿ��е����ļ���
        /// </summary>
        /// <param name="sql"></param>
        /// <param name="fileName"></param>
        /// <returns></returns>
        public static bool RunSQLExportFile(string sql, string fileName)
        {
            OleDbDataReader reader = null;
            OleDbConnection conn = GetConn;
            OleDbCommand cmd = null;

            try
            {
                cmd = new OleDbCommand(sql, conn);
                cmd.CommandType = CommandType.Text;

                if (conn.State != ConnectionState.Open)
                {
                    conn.Open();
                }

                reader = cmd.ExecuteReader();
                byte[] file = null;
                if (reader.Read())
                {
                    file = (byte[])reader[0];
                }
                FileStream fs;
                FileInfo fi = new System.IO.FileInfo(fileName);
                fs = fi.OpenWrite();
                fs.Write(file, 0, file.Length);
                fs.Close();

                return true;
            }
            catch (System.Exception ex)
            {
                throw new Exception(ex.Message + " [RunSQLExportFile on DBAccessOfOle] " + sql);
            }
            finally
            {
                if (null != reader)
                {
                    reader.Close();
                    reader = null;
                }

                if (null != cmd)
                {
                    cmd.Dispose();
                    cmd = null;
                }

                Disconnect(conn);
            }
        }
    }
}
