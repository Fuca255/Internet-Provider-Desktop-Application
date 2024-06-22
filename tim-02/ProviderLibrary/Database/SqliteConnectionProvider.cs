using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.SQLite;
using System.Data.SqlClient;

namespace ProviderLibrary.Database
{
    public class SqliteConnectionProvider : IConnection
    {
        public IDbConnection createConnection(string connString)
        {
            return new SQLiteConnection(connString);
        }
    }
}
