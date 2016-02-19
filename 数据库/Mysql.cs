using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data.Odbc;
using System.Data;
using MySql.Data.MySqlClient;
using MySql.Data;
using System.Windows.Forms;

namespace OnlineHis
{
    public class CMysql
    {
        private String str_DbName;
        private String str_DbCharset;
        private String str_DbUser;
        private String str_DbPassword;
        private String str_DbConnectionHost;
        private String str_DbPort;
        private String Connection_str;
        MySql.Data.MySqlClient.MySqlConnection p_Connection;

        private String MysqlErrorInfo;
        /// <summary>
        /// 类默认去查询
        /// </summary>
        public CMysql()
        {
            str_DbName = "####";
            str_DbCharset = "utf8";
            str_DbUser = "#####";
            str_DbPassword = "########";
            str_DbConnectionHost = "#####";
            str_DbPort = "3306";
            this.MargeConnectStr();
            this.Connection_Db();
        }

        public CMysql(String dbName, String dbUser, String dbPassword, String dbHost, String dbPort, String dbCharset = "utf8")
        {
            str_DbName = dbName;
            str_DbCharset = dbCharset;
            str_DbUser = dbUser;
            str_DbPassword = dbPassword;
            str_DbConnectionHost = dbHost;
            str_DbPort = dbPort;
            this.MargeConnectStr();
            this.Connection_Db();
        }

     
        /// <summary>
        /// 将上面的构造函数当中的所有连接信息在此进行连接组合
        /// </summary>
        private void MargeConnectStr()
        {
            Connection_str = String.Format(@"Server={0};Port={1};Database={2};Uid={3};Pwd={4};",
                                          this.str_DbConnectionHost,
                                          Convert.ToInt32(this.str_DbPort),
                                          this.str_DbName,
                                          this.str_DbUser,
                                          this.str_DbPassword);

        }

        /// <summary>
        /// 连接到服务器
        /// </summary>
        public void Connection_Db()
        {
            p_Connection = new MySql.Data.MySqlClient.MySqlConnection(this.Connection_str);
            p_Connection.Open();
        }

        /// <summary>
        /// 获取错误信息
        /// </summary>
        /// <param name="ErrorMsg"></param>
        public void getErrorLastInfo(ref object ErrorMsg)
        {
            if (this.MysqlErrorInfo == null)
                ErrorMsg = true;
            else
                ErrorMsg = this.MysqlErrorInfo;
        }

        /// <summary>
        /// 返回MysqlDataReader对象
        /// </summary>
        /// <param name="Query_sql"></param>
        /// <returns></returns>
        public MySqlDataReader getReader(string Query_sql)
        {
            try
            {

                if (this.p_Connection.State != ConnectionState.Open)
                    this.p_Connection.Open();
                MySqlCommand cmd = new MySqlCommand(Query_sql, this.p_Connection);
                MySqlDataReader tempReader = cmd.ExecuteReader();
                return tempReader;
            }
            catch (MySqlException ex)
            {
                this.MysqlErrorInfo = ex.Message.ToString();
                return null;
            }

        }

        public bool IsCreateTable(string tablename, string CreateTableSQL) {
            string find_sql = "show tables;";
            List<string> tableList = new List<string>();
            try {
                if (this.p_Connection.State != ConnectionState.Open)
                    this.p_Connection.Open();
                MySqlCommand cmd = new MySqlCommand();
                cmd.CommandText = find_sql;
                cmd.CommandType = CommandType.Text;
                cmd.Connection = p_Connection;

                using (MySqlDataReader reader = cmd.ExecuteReader())
                {
                   if(reader.HasRows)
                       while (reader.Read()) {
                           string temp = reader.GetString(0);
                           tableList.Add(temp);
                       }
                }
                string[] tempArr = tableList.ToArray();
                if (!tempArr.Contains(tablename))
                {
                    if (MessageBox.Show("当前系统中不存在扩展回访功能表，您是否要让OnlineHis创建此表？如果创建请单击【YES是】否则单击【NO否】\n\r请选择", "OnlineHis功能扩展", MessageBoxButtons.YesNo, MessageBoxIcon.Asterisk) == DialogResult.Yes)
                    {
                        cmd.CommandText = CreateTableSQL;
                        int result = cmd.ExecuteNonQuery();
                        if (result >= 0)
                        {
                            return true;
                        }
                    }
                }
                return true;
            }
            catch (MySqlException ex) {
                this.MysqlErrorInfo = ex.Message.ToString();
            }
            return false;
        }

        /// <summary>
        /// 获取总条数
        /// </summary>
        /// <param name="sql_query"></param>
        /// <returns></returns>
        public int GetCount(string sql_query){
            try
            {
                if (this.p_Connection.State != ConnectionState.Open)
                    this.p_Connection.Open();
                MySqlCommand cmd = new MySqlCommand();
                cmd.CommandText = sql_query;
                cmd.CommandType = CommandType.Text;
                cmd.Connection = p_Connection;

                using(MySqlDataReader  reader =cmd.ExecuteReader()){
                    reader.Read();
                    return reader.GetInt32(0);
                }
            }catch(MySqlException ex){
                this.MysqlErrorInfo = ex.Message.ToString();
                return 0;
            }
        }

        /// <summary>
        /// 获取DataAdapter对象
        /// </summary>
        /// <param name=”SqlString”></param>
        /// <returns></returns>
        public MySqlDataAdapter GetDataAdapter(string SqlString)
        {
            try
            {
                if (this.p_Connection.State != ConnectionState.Open)
                    this.p_Connection.Open();
                MySqlDataAdapter dadapter = new MySqlDataAdapter(SqlString, this.p_Connection);
                return dadapter;
            }
            catch (MySqlException ex)
            {
                this.MysqlErrorInfo = ex.Message.ToString();
                return null;
            }
        }

        public bool InsertQuery(string str_query) {
            if (this.p_Connection.State != ConnectionState.Open)
                this.p_Connection.Open();
            MySqlCommand cmd = new MySqlCommand();
            cmd.CommandText = str_query;
            cmd.CommandType = CommandType.Text;
            cmd.Connection = p_Connection;
            int ExecuteFlag = cmd.ExecuteNonQuery();
            return ExecuteFlag > 0 ? true : false;
        }

        /// <summary>
        /// 获取一条数据
        /// </summary>
        /// <returns></returns>
        public object getOne(string str_query)
        {
            try
            {
                if (this.p_Connection.State != ConnectionState.Open)
                    this.p_Connection.Open();
                MySqlCommand cmd = new MySqlCommand(str_query, p_Connection);
                return cmd.ExecuteReader();
            }
            catch (System.Exception ex)
            {
                this.MysqlErrorInfo = ex.Message.ToString();
            }
            return null;
        }



    }
}
