using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MySql.Data.MySqlClient;

namespace Bunifu.Connection
{
    class Connection
    {
        public static MySqlConnection
            GetDBConnection(string host, int port, string database, string username, string password)
        {
            string connString = "Server=" + host
                                            + ";Port=" + port
                                            + ";Database=" + database
                                            + ";Username=" + username
                                            + ";Password=" + password;

            MySqlConnection conn = new MySqlConnection(connString);

            return conn;
        }
    }


    class DBGetConnection
    {
        public static MySqlConnection
            GetDBConnection()
        {
            string host = "localhost";
            int port = 3306;
            string database = "testing";
            string username = "root";
            string password = "P28Rfc!q";

            return Connection.GetDBConnection(host, port, database, username, password);
        }
    }

}

