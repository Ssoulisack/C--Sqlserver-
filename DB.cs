using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace WorkwithDB
{
    internal class DB
    {
        SqlConnection conn;
        SqlCommand cmd;
        public DB()
        {
            //1. Address Of SQL server and database(Connection String)
            //2. Establish connectio(c# sql connection class)
            conn = new SqlConnection("Data Source=DESKTOP-F01AA5A\\\\SQLEXPRESS;Initial Catalog=MyDB;User ID=Soulisack;Password=77477686");
                //3.Open Connection(c# Sql Connection Open)
                conn.Open();
        }
        public void Execute(string Query)
        {
            try
            {   
                //4.Prepare SQL query
                cmd = new SqlCommand(Query, conn);
                //5.Execute query (c# sqlcommand class)
                cmd.ExecuteNonQuery();
            }catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "ERROR", MessageBoxButtons.OK, MessageBoxIcon.Error) ;
            }
            finally
            {
                //6.Close connection(C# sqlconnection close)
                conn.Close();

            }
        }
        public SqlDataReader Select(string Query)
        {
            try
            {
                //3.Open Connection(c# Sql Connection Open)
                conn.Open();
                //4.Prepare SQL query
                cmd = new SqlCommand(Query, conn);
                //5.Execute query (c# sqlcommand class)
                return cmd.ExecuteReader(System.Data.CommandBehavior.CloseConnection);
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "ERROR", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            return null;
        }
    }
}
