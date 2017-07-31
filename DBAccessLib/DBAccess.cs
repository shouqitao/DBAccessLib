using System;
using System.Collections.Generic;
using System.Data;
using System.Text;

namespace DBAccessLib
{
    /// <summary>
    /// ͨ�õ����ݿ�����ࡣ
    /// �����������ݿ����ͣ�Ȼ�����������ַ������ٵ������ݿ����������
    /// </summary>
    public class DBAccess
    {
        #region ���ݿ�����
        private static string dbType;

        /// <summary>
        /// ֧�ֵ����ݿ�����
        /// </summary>
        public const string DB_TYPE_ODBC = "odbc";
        public const string DB_TYPE_OLEDB = "oledb";
        public const string DB_TYPE_ORACLE9I = "oracle9i";
        public const string DB_TYPE_SQL = "sql";
        public const string DB_TYPE_SQLITE = "sqlite";

        /// <summary>
        /// ���ݿ�����odbc|oledb|oracle9i|sql��
        /// </summary>
        public static string DbType
        {
            get { return DBAccess.dbType; }
            set { DBAccess.dbType = value; }
        }
        #endregion

        #region �����ַ���
        private static string connectStr;

        /// <summary>
        /// �����ַ�����
        /// </summary>
        public static string ConnectStr
        {
            get { return DBAccess.connectStr; }
            set
            {
                DBAccess.connectStr = value;

                if ("odbc".Equals(dbType))
                {
                    DBAccessOfOdbc.ConnectStr = connectStr;
                }
                else if ("oledb".Equals(dbType))
                {
                    DBAccessOfOleDb.ConnectStr = connectStr;
                }
                else if ("oracle9i".Equals(dbType))
                {
                    DBAccessOfOracle9i.ConnectStr = connectStr;
                }
                else if ("sql".Equals(dbType))
                {
                    DBAccessOfSql.ConnectStr = connectStr;
                }
                else if ("sqlite".Equals(dbType))
                {
                    DBAccessOfSQLite.ConnectStr = connectStr;
                }
            }
        }
        #endregion

        #region ����ǲ��Ǵ���
        /// <summary>
        /// ����ǲ��Ǵ���
        /// </summary>
        /// <param name="sql">SQL���</param>
        /// <returns>true�����ڣ�false�������ڡ�</returns>
        public static bool IsExits(string sql)
        {
            if ("odbc".Equals(dbType))
            {
                return DBAccessOfOdbc.IsExits(sql);
            }
            else if ("oledb".Equals(dbType))
            {
                return DBAccessOfOleDb.IsExits(sql);
            }
            else if ("oracle9i".Equals(dbType))
            {
                return DBAccessOfOracle9i.IsExits(sql);
            }
            else if ("sql".Equals(dbType))
            {
                return DBAccessOfSql.IsExits(sql);
            }
            else if ("sqlite".Equals(dbType))
            {
                return DBAccessOfSQLite.IsExits(sql);
            }
            else
            {
                return false;
            }
        }
        #endregion

        #region ��ѯ����ֵ�ķ���
        /// <summary>
        /// ��ѯ����ֵ�ķ���
        /// </summary>
        /// <param name="sql"></param>
        /// <returns>����ֵ</returns>
        public static object RunSQLReturnVal(string sql)
        {
            if ("odbc".Equals(dbType))
            {
                return DBAccessOfOdbc.RunSQLReturnVal(sql);
            }
            else if ("oledb".Equals(dbType))
            {
                return DBAccessOfOleDb.RunSQLReturnVal(sql);
            }
            else if ("oracle9i".Equals(dbType))
            {
                return DBAccessOfOracle9i.RunSQLReturnVal(sql);
            }
            else if ("sql".Equals(dbType))
            {
                return DBAccessOfSql.RunSQLReturnVal(sql);
            }
            else if ("sqlite".Equals(dbType))
            {
                return DBAccessOfSQLite.RunSQLReturnVal(sql);
            }
            else
            {
                return null;
            }
        }
        #endregion

        #region ����sql������ص�DataTable
        /// <summary>
        /// ����sql������ص�DataTable
        /// </summary>
        /// <param name="sql">Sql���</param>
        /// <returns></returns>
        public static DataTable RunSQLReturnTable(string sql)
        {
            if ("odbc".Equals(dbType))
            {
                return DBAccessOfOdbc.RunSQLReturnTable(sql);
            }
            else if ("oledb".Equals(dbType))
            {
                return DBAccessOfOleDb.RunSQLReturnTable(sql);
            }
            else if ("oracle9i".Equals(dbType))
            {
                return DBAccessOfOracle9i.RunSQLReturnTable(sql);
            }
            else if ("sql".Equals(dbType))
            {
                return DBAccessOfSql.RunSQLReturnTable(sql);
            }
            else if ("sqlite".Equals(dbType))
            {
                return DBAccessOfSQLite.RunSQLReturnTable(sql);
            }
            else
            {
                return null;
            }
        }
        #endregion

        #region ִ�� SQL ��䣬������Ӱ�������
        /// <summary>
        /// ִ�� SQL ��䣬������Ӱ�������
        /// </summary>
        /// <param name="sql"></param>
        /// <returns></returns>
        public static int RunSQL(string sql)
        {
            if ("odbc".Equals(dbType))
            {
                return DBAccessOfOdbc.RunSQL(sql);
            }
            else if ("oledb".Equals(dbType))
            {
                return DBAccessOfOleDb.RunSQL(sql);
            }
            else if ("oracle9i".Equals(dbType))
            {
                return DBAccessOfOracle9i.RunSQL(sql);
            }
            else if ("sql".Equals(dbType))
            {
                return DBAccessOfSql.RunSQL(sql);
            }
            else if ("sqlite".Equals(dbType))
            {
                return DBAccessOfSQLite.RunSQL(sql);
            }
            else
            {
                return 0;
            }
        }
        #endregion

        #region ����ִ�� SQL ��䣬������Ӱ�������
        /// <summary>
        /// ����ִ�� SQL ��䣬������Ӱ�������
        /// </summary>
        /// <param name="sql"></param>
        /// <returns></returns>
        public static int RunSQL(List<string> sqls)
        {
            if ("odbc".Equals(dbType))
            {
                return DBAccessOfOdbc.RunSQL(sqls);
            }
            else if ("oledb".Equals(dbType))
            {
                return DBAccessOfOleDb.RunSQL(sqls);
            }
            else if ("oracle9i".Equals(dbType))
            {
                return DBAccessOfOracle9i.RunSQL(sqls);
            }
            else if ("sql".Equals(dbType))
            {
                return DBAccessOfSql.RunSQL(sqls);
            }
            else if ("sqlite".Equals(dbType))
            {
                return DBAccessOfSQLite.RunSQL(sqls);
            }
            else
            {
                return 0;
            }
        }
        #endregion
    }
}
