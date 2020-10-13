using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MySql.Data.MySqlClient;

namespace Test_BarcodeReader.Model
{
    public class DbConnection
    {
        public DbConnection(string host, int port, string databaseName, string username, string password)
        {
            Host = host;
            Port = port;
            DatabaseName = databaseName;
            Username = username;
            Password = password;
        }

        public DbConnection()
        {

        }

        public DbConnection(string host, string databaseName, string username, string password)
        {
            Host = host;
            Port = 3306;
            DatabaseName = databaseName;
            Username = username;
            Password = password;
        }

        public bool ConnectMySql()
        {
            try
            {
                String connString = "Server=" + Host + ";Database=" + DatabaseName + ";User Id=" + Username +
                                    ";password=" + Password;

                MySqlConnection = new MySqlConnection(connString);
                return true;
            }
            catch (Exception connectException)
            {
                Error = connectException.Message;
                return false;
            }
        }

        public bool OpenConnection()
        {
            try
            {
                MySqlConnection.Open();
                return true;
            }
            catch (MySqlException ex)
            {
                //When handling errors, you can your application's response based 
                //on the error number.
                //The two most common error numbers when connecting are as follows:
                //0: Cannot connect to server.
                //1045: Invalid user name and/or password.
                return false;
            }
        }

        private bool CloseConnection()
        {
            try
            {
                MySqlConnection.Close();
                return true;
            }
            catch (MySqlException ex)
            {
                return false;
            }
        }

        public bool Insert(string query)
        {
            //open connection
            if (OpenConnection())
            {
                //create command and assign the query and connection from the constructor
                MySqlCommand cmd = new MySqlCommand(query, MySqlConnection);

                //Execute command
                cmd.ExecuteNonQuery();

                //close connection
                this.CloseConnection();
                return true;
            }

            return false;
        }

        public string Host { get; set; }
        public int Port { get; set; }
        public string DatabaseName { get; set; }
        public string Username { get; set; }
        public string Password { get; set; }
        public string Error { get; set; }
        public MySqlConnection MySqlConnection { get; set; }    

    }
}
