using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MySql.Data.MySqlClient;

namespace ProviderLibrary.Database
{
    public class MysqlConnectionProvider : IConnection
    {
        public IDbConnection createConnection(string connString)
        {
            return new MySqlConnection(connString);
        }
    }
}
