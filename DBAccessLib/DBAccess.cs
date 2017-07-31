using System;
using System.Collections.Generic;
using System.Data;
using System.Text;

namespace DBAccessLib
{
    /// <summary>
    /// 通用的数据库操作类。
    /// 首先设置数据库类型，然后设置连接字符串，再调用数据库操作函数。
    /// </summary>
    public class DBAccess
    {
        #region 数据库类型
        private static string dbType;

        /// <summary>
        /// 支持的数据库类型
        /// </summary>
        public const string DB_TYPE_ODBC = "odbc";
        public const string DB_TYPE_OLEDB = "oledb";
        public const string DB_TYPE_ORACLE9I = "oracle9i";
        public const string DB_TYPE_SQL = "sql";
        public const string DB_TYPE_SQLITE = "sqlite";

        /// <summary>
        /// 数据库类型odbc|oledb|oracle9i|sql。
        /// </summary>
        public static string DbType
        {
            get { return DBAccess.dbType; }
            set { DBAccess.dbType = value; }
        }
        #endregion

        #region 连接字符串
        private static string connectStr;

        /// <summary>
        /// 连接字符串。
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

        #region 检查是不是存在
        /// <summary>
        /// 检查是不是存在
        /// </summary>
        /// <param name="sql">SQL语句</param>
        /// <returns>true，存在；false，不存在。</returns>
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

        #region 查询单个值的方法
        /// <summary>
        /// 查询单个值的方法
        /// </summary>
        /// <param name="sql"></param>
        /// <returns>单个值</returns>
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

        #region 运行sql结果返回到DataTable
        /// <summary>
        /// 运行sql结果返回到DataTable
        /// </summary>
        /// <param name="sql">Sql语句</param>
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

        #region 执行 SQL 语句，返回受影响的行数
        /// <summary>
        /// 执行 SQL 语句，返回受影响的行数
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

        #region 批量执行 SQL 语句，返回受影响的行数
        /// <summary>
        /// 批量执行 SQL 语句，返回受影响的行数
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
