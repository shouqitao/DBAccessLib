[ADO.NET���ݿ�������]
���ߣ��ﾸ
������ṩ������5�����ݿ������ʽ��
DBAccessOfOdbc
DBAccessOfOleDb
DBAccessOfOracle9i
DBAccessOfSql
DBAccessOfSQLite
���ݿ����ͷֱ���Ϊ��
DBAccessLib.DBAccess.DB_TYPE_ODBC
DBAccessLib.DBAccess.DB_TYPE_OLEDB
DBAccessLib.DBAccess.DB_TYPE_ORACLE9I
DBAccessLib.DBAccess.DB_TYPE_SQL
DBAccessLib.DBAccess.DB_TYPE_SQLITE

�����ַ���д����
DBAccessLib.DBAccessOfOdbc.ConnectStr = ""
DBAccessLib.DBAccessOfOleDb.ConnectStr = "Provider=SQLOLEDB;Server=127.0.0.1,1433;Database=dbname;uid=sa;pwd=sunjing;";
DBAccessLib.DBAccessOfSql.ConnectStr = "Data Source=10.7.1.11,1433;Initial Catalog=dbname;Persist Security Info=True;User ID=sa;Password=passwd"
DBAccessLib.DBAccessOfOracle9i.ConnectStr = "Data Source=XYYT_10.7.1.10;Persist Security Info=True;User ID=username;Password=passwd"
DBAccessLib.DBAccessOfSQLite.ConnectStr = "Data Source =" + Environment.CurrentDirectory + "\\test.db";

5�����а�������ͬ�Ĳ���������
ʹ�ñ����֮ǰ����Ҫ�������������ַ���ConnectStr�������Ĺ������£�
// ִ��SQL��䣬�ж��Ƿ����
public static bool IsExits(string sql)
// ִ��SQL��䣬�����������е�ֵ
public static object RunSQLReturnVal(string sql)
// ִ��SQL��䣬����DataTable
public static DataTable RunSQLReturnTable(string sql)
// ִ��SQL��䣬������Ӱ�������
public static int RunSQL(string sql)
// ����ִ��SQL��䣬������Ӱ�������
public static int RunSQL(List<string> sqls)

// ������������Ŀǰֻ��DBAccessOfOleDb����ʵ��
// ִ�� INSERT SQL ��䣬������������
public static int RunSQLReturnId(string sql)
// �����ļ������ݿ��У�������Ӱ�������
public static int RunSQLImportFile(string sql, string fileName)
// �����ݿ��е����ļ�
public static bool RunSQLExportFile(string sql, string fileName)


ʹ�ô˶�̬������ַ�ʽ���£�
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

ʹ��SQLite��װ��ʾ����
            string dbPath = Environment.CurrentDirectory + "\\test.db"; // ָ�����ݿ�·��
            DBAccessLib.DBAccess.DbType = DBAccessLib.DBAccess.DB_TYPE_SQLITE;
            DBAccessLib.DBAccess.ConnectStr = "Data Source =" + dbPath; // ��������
            bool rtnval = DBAccessLib.DBAccess.IsExits("select * from student where name='����';");

[V 1.6.1]  2015-06-16
��DBAccess���ж����������ݿ����͵��ַ���������
DBAccessLib.DBAccess.DB_TYPE_ODBC
DBAccessLib.DBAccess.DB_TYPE_OLEDB
DBAccessLib.DBAccess.DB_TYPE_ORACLE9I
DBAccessLib.DBAccess.DB_TYPE_SQL
DBAccessLib.DBAccess.DB_TYPE_SQLITE

[V 1.6]  2015-06-15
���Ӷ�SQLite���ݿ��֧�֣�DBAccessOfSQLite�ࣩ��

[V 1.5]  2014-05-15
���Ӵ洢���̵Ĵ���

[V 1.4]  2014-04-29
��������ִ��SQL��亯����public static int RunSQL(List<string> sqls)

[V 1.3]  2013-07-27
DBAccessOfOleDb����RunSQLReturnId��RunSQLImportFile��RunSQLExportFile��

[V 1.2]  2013-05-13
ʹ��VS2010���±��롣

[V 1.1]  2012-07-15
����ͨ�õ����ݿ������DBAccess��ʹ�ò������£�
1.�������ݿ�����odbc|oledb|oracle9i|sql��
2.���������ַ�����
3.�������е����ݿ����������

[V 1.0]  2012-06-20
ADO.NET���ݿ������⡣
ʹ��VS2005��д��