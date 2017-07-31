[ADO.NET数据库操作类库]
作者：孙靖
本类库提供了如下5种数据库操作方式：
DBAccessOfOdbc
DBAccessOfOleDb
DBAccessOfOracle9i
DBAccessOfSql
DBAccessOfSQLite
数据库类型分别定义为：
DBAccessLib.DBAccess.DB_TYPE_ODBC
DBAccessLib.DBAccess.DB_TYPE_OLEDB
DBAccessLib.DBAccess.DB_TYPE_ORACLE9I
DBAccessLib.DBAccess.DB_TYPE_SQL
DBAccessLib.DBAccess.DB_TYPE_SQLITE

连接字符串写法：
DBAccessLib.DBAccessOfOdbc.ConnectStr = ""
DBAccessLib.DBAccessOfOleDb.ConnectStr = "Provider=SQLOLEDB;Server=127.0.0.1,1433;Database=dbname;uid=sa;pwd=sunjing;";
DBAccessLib.DBAccessOfSql.ConnectStr = "Data Source=10.7.1.11,1433;Initial Catalog=dbname;Persist Security Info=True;User ID=sa;Password=passwd"
DBAccessLib.DBAccessOfOracle9i.ConnectStr = "Data Source=XYYT_10.7.1.10;Persist Security Info=True;User ID=username;Password=passwd"
DBAccessLib.DBAccessOfSQLite.ConnectStr = "Data Source =" + Environment.CurrentDirectory + "\\test.db";

5个类中包含了相同的操作函数。
使用本类库之前，需要首先设置连接字符串ConnectStr，包含的功能如下：
// 执行SQL语句，判断是否存在
public static bool IsExits(string sql)
// 执行SQL语句，返回首行首列单值
public static object RunSQLReturnVal(string sql)
// 执行SQL语句，返回DataTable
public static DataTable RunSQLReturnTable(string sql)
// 执行SQL语句，返回受影响的行数
public static int RunSQL(string sql)
// 批量执行SQL语句，返回受影响的行数
public static int RunSQL(List<string> sqls)

// 下述三个函数目前只在DBAccessOfOleDb类中实现
// 执行 INSERT SQL 语句，返回自增主键
public static int RunSQLReturnId(string sql)
// 保存文件到数据库中，返回受影响的行数
public static int RunSQLImportFile(string sql, string fileName)
// 从数据库中导出文件
public static bool RunSQLExportFile(string sql, string fileName)


使用此动态库的两种方式如下：
        private void button1_Click(object sender, EventArgs e)
        {
            string connStr1 = "Data Source=10.7.1.11,1433;Initial Catalog=sysg;Persist Security Info=True;User ID=zdhsa;Password=success@zdh";
            string connStr2 = "Provider=SQLOLEDB;Server=10.7.1.11,1433;Database=sysg;uid=zdhsa;pwd=success@zdh;";
            DBAccessLib.DBAccessOfSql.ConnectStr = connStr1;
            DataTable dt = DBAccessLib.DBAccessOfSql.RunSQLReturnTable("select * from jcb_dwxx ");
            MessageBox.Show(dt.Rows.Count.ToString());

            DBAccessLib.DBAccessOfOleDb.ConnectStr = connStr2;
            dt = DBAccessLib.DBAccessOfOleDb.RunSQLReturnTable("select * from jcb_dwxx ");
            MessageBox.Show(dt.Rows.Count.ToString());
        }

        private void button2_Click(object sender, EventArgs e)
        {
            string connStr1 = "Data Source=10.7.1.11,1433;Initial Catalog=sysg;Persist Security Info=True;User ID=zdhsa;Password=success@zdh";
            string connStr2 = "Provider=SQLOLEDB;Server=10.7.1.11,1433;Database=sysg;uid=zdhsa;pwd=success@zdh;";

            DBAccessLib.DBAccess.DbType = DBAccessLib.DBAccess.DB_TYPE_SQL;
            DBAccessLib.DBAccess.ConnectStr = connStr1;
            DataTable dt = DBAccessLib.DBAccess.RunSQLReturnTable("select * from jcb_dwxx ");
            MessageBox.Show(dt.Rows.Count.ToString());

            DBAccessLib.DBAccessOfOleDb.ConnectStr = connStr2;
            dt = DBAccessLib.DBAccess.RunSQLReturnTable("select * from jcb_dwxx ");
            MessageBox.Show(dt.Rows.Count.ToString());
        }

使用SQLite封装类示例：
            string dbPath = Environment.CurrentDirectory + "\\test.db"; // 指定数据库路径
            DBAccessLib.DBAccess.DbType = DBAccessLib.DBAccess.DB_TYPE_SQLITE;
            DBAccessLib.DBAccess.ConnectStr = "Data Source =" + dbPath; // 创建连接
            bool rtnval = DBAccessLib.DBAccess.IsExits("select * from student where name='张三';");

[V 1.6.1]  2015-06-16
在DBAccess类中定义五种数据库类型的字符串常量：
DBAccessLib.DBAccess.DB_TYPE_ODBC
DBAccessLib.DBAccess.DB_TYPE_OLEDB
DBAccessLib.DBAccess.DB_TYPE_ORACLE9I
DBAccessLib.DBAccess.DB_TYPE_SQL
DBAccessLib.DBAccess.DB_TYPE_SQLITE

[V 1.6]  2015-06-15
增加对SQLite数据库的支持（DBAccessOfSQLite类）。

[V 1.5]  2014-05-15
增加存储过程的处理。

[V 1.4]  2014-04-29
增加批量执行SQL语句函数：public static int RunSQL(List<string> sqls)

[V 1.3]  2013-07-27
DBAccessOfOleDb增加RunSQLReturnId、RunSQLImportFile、RunSQLExportFile。

[V 1.2]  2013-05-13
使用VS2010重新编译。

[V 1.1]  2012-07-15
增加通用的数据库操作类DBAccess，使用步骤如下：
1.设置数据库类型odbc|oledb|oracle9i|sql，
2.设置连接字符串，
3.调用类中的数据库操作函数。

[V 1.0]  2012-06-20
ADO.NET数据库操作类库。
使用VS2005编写。