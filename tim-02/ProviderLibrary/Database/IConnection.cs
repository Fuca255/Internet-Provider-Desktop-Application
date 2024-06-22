using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data;
namespace ProviderLibrary.Database
{
    public interface IConnection
    {
        IDbConnection createConnection(string connString);
    }
}
