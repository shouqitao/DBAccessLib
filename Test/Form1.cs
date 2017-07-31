using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using System.Configuration;
using DBAccessLib;

namespace Test
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            //DBAccessLib.DBAccessOfOracle9i.ConnectStr = ConfigurationManager.ConnectionStrings["Test.Properties.Settings.connStr"].ConnectionString;
            //Object obj = DBAccessLib.DBAccessOfOracle9i.RunSQLReturnVal("select * from pub_users");
            //DataTable tbl = DBAccessLib.DBAccessOfOracle9i.RunSQLReturnTable("select * from pub_users");

            //DBAccessLib.DBAccessOfSql.ConnectStr = ConfigurationManager.ConnectionStrings["Test.Properties.Settings.connStr"].ConnectionString;
            //DataTable dt = DBAccessLib.DBAccessOfSql.RunSQLReturnTable("select * from jcxx_dwgx ");
            
            //DBAccessLib.DBAccessOfOleDb.ConnectStr = ConfigurationManager.ConnectionStrings["Test.Properties.Settings.connOleStr"].ConnectionString;
            //int nid = DBAccessLib.DBAccessOfOleDb.RunSQLReturnId("insert into doctype(doctypename,parentid) values('传奇',0)");
            //int tmp = nid;

            //DBAccessLib.DBAccessOfOdbc.ConnectStr = ConfigurationManager.ConnectionStrings["Test.Properties.Settings.connOdbcStr"].ConnectionString;
            //int nid = DBAccessLib.DBAccessOfOdbc.RunSQLReturnId("insert into doctype(doctypename,parentid) values('传奇',0)");
            //int tmp = nid;

            List<string> sqls = new List<string>();
            sqls.Add("insert into table_1(col01,col02) values('1', '2');");
            sqls.Add("insert into table_1(col01,col02) values('3', '4');");
            sqls.Add("insert into table_2(col01,col02) values('1', '2');");
            sqls.Add("insert into table_2(col01,col02) values('3', '4');");

            try
            {
                //DBAccessLib.DBAccessOfOleDb.ConnectStr = ConfigurationManager.ConnectionStrings["Test.Properties.Settings.connOleStr"].ConnectionString;
                //int nid = DBAccessLib.DBAccessOfOleDb.RunSQL(sqls);
                //MessageBox.Show(nid.ToString());
            }
            catch (Exception exc)
            {

                MessageBox.Show(exc.Message);
            }
        }

        private void btnSaveFile_Click(object sender, EventArgs e)
        {
            DBAccessLib.DBAccessOfOleDb.ConnectStr = ConfigurationManager.ConnectionStrings["Test.Properties.Settings.connStrSql"].ConnectionString;
            DBAccessLib.DBAccessOfOleDb.RunSQLImportFile("update djzl set img=@file where nid=1", "D:\\hr.jpg");
        }

        private void btnRead_Click(object sender, EventArgs e)
        {
            DBAccessLib.DBAccessOfOleDb.ConnectStr = ConfigurationManager.ConnectionStrings["Test.Properties.Settings.connStrSql"].ConnectionString;
            DBAccessLib.DBAccessOfOleDb.RunSQLExportFile("select img from djzl where nid=65", "D:\\11.jpg");
        }

        private void button2_Click(object sender, EventArgs e)
        {
            string dbPath = Environment.CurrentDirectory + "\\test.db"; // 指定数据库路径
            DBAccessLib.DBAccess.DbType = DBAccessLib.DBAccess.DB_TYPE_SQLITE;
            DBAccessLib.DBAccess.ConnectStr = "Data Source =" + dbPath; // 创建连接
            bool rtnval = DBAccessLib.DBAccess.IsExits("select name from student;");
            //MessageBox.Show(rtnval.ToString());
            string sql = "CREATE TABLE IF NOT EXISTS student(id integer, name varchar(20), sex varchar(2));";//建表语句
            DBAccessLib.DBAccess.RunSQL(sql);
            sql = "INSERT INTO student VALUES(1, '小红', '男')"; // 插入几条数据
            DBAccessLib.DBAccess.RunSQL(sql);
            sql = "INSERT INTO student VALUES(2, '小李', '女')"; // 插入几条数据
            DBAccessLib.DBAccess.RunSQL(sql);
            sql = "INSERT INTO student VALUES(3, '小明', '男')";
            DBAccessLib.DBAccess.RunSQL(sql);
            object obj = DBAccessLib.DBAccess.RunSQLReturnVal("select name from student;");
            //MessageBox.Show(obj.ToString());
            List<string> list = new List<string>();
            list.Add("INSERT INTO student VALUES(0, 'A11', '女')");
            list.Add("INSERT INTO student VALUES(5, 'B22', '女')");
            int cnt = DBAccess.RunSQL(list);
            MessageBox.Show(cnt.ToString());
            DataTable dt = DBAccess.RunSQLReturnTable("select name from student;");
            string str = "";
            if (null != dt)
            {
                foreach (DataRow row in dt.Rows)
                {
                    str += row["name"].ToString();
                }
            }
            MessageBox.Show(str);
        }
    }
}