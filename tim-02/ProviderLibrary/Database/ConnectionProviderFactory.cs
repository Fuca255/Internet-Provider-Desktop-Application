using Microsoft.SqlServer.Server;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProviderLibrary.Database
{
    internal class ConnectionProviderFactory
    {
        public static IDbConnection CreateConnection(string connStr)
        {
            IConnection connection = null;
            Console.WriteLine("Creating");
            if (CheckMysql(connStr))
            {
                connection = new MysqlConnectionProvider();
                Console.WriteLine("Mysql base connection created");
                return connection.createConnection(connStr);
            }
            
            if (CheckSqlite(connStr))
            {
                connection = new SqliteConnectionProvider();
                Console.WriteLine("Sqlite base connection created");
                return connection.createConnection(connStr);
            }
            
            throw new Exception("The following database is not Mysql db nor Sqlite db");
        }

        private static bool CheckSqlite(string connStr)
        {
            return connStr.Contains("Data Source");
        }
        private static bool CheckMysql(string connStr)
        {

            return connStr.Contains("Server");
        }
    }
}
