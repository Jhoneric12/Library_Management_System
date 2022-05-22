using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MySql.Data.MySqlClient;

namespace Finals_Computer_Programming
{
    class DBServer
    {
        public string connection()
        {
            string conn = ("server = localhost; username = root; password = JEAton123; database = library_system");
            return conn;
        }
        public string deletedConnection()
        {
            string deleteConn = ("server = localhost; username= root; password = JEAton123; database = dbdeleted_records");
            return deleteConn;     
        }
    }
}
