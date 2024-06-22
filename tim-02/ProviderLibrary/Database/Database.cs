using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data;
using System.IO;
using ProviderLibrary.Loggers;
using ProviderLibrary.Interface;
using System.Xml.Schema;
namespace ProviderLibrary.Database
{
    public class Database
    {
        private static Database Instance;
        private IDbConnection conn;
        private EventLogger logger;
        private Database(string conStr) {
            if (conn != null && conn.State == ConnectionState.Open)
                conn.Close();
            logger = new EventLogger();
            try
            {
                conn = ConnectionProviderFactory.CreateConnection(conStr);
                conn.Open();
                Console.WriteLine("Connection open");
                
                logger.AddObserver(new FileLogger("Logs", "database_logs.txt"));
                logger.LogEvent("Connection opened");
            } catch(Exception ex)
            {
                Console.WriteLine(ex.Message);
                logger.LogEvent($"{ex.Message}");
            }
        }
        /*  Nacin pozivanja query-a
        *  
        *  
        *  string sql = "select * from Klijent where id = @0 and ime = @1;";
           List<object> parametri = new List<object>();

           parametri.Add(1);
           parametri.Add("Stefan");*/
        public List<Dictionary<string, object>> Execute_query(string sql, List<object> parameters = null)
        {
            List<Dictionary<string, object>> results = new List<Dictionary<string, object>>();


            //koristi se using kako bi se cim se zavrsi radnja sa ovim objektom on obrisao
            using (IDbCommand command = conn.CreateCommand())
            {
                command.CommandText = sql;
                if (parameters != null)
                {
                    int countParam = 0;
                    foreach (var parameter in parameters)
                    {
                        var p = command.CreateParameter();
                        p.ParameterName = "@" + countParam;
                        p.Value = parameter;
                        command.Parameters.Add(p);
                        countParam++;
                    }
                }

                using (IDataReader reader = command.ExecuteReader())
                {
                    while (reader.Read())
                    {


                        IDataRecord record = (IDataRecord)reader;
                        Dictionary<string, object> row = new Dictionary<string, object>();
                        for (int i = 0; i < record.FieldCount; i++)
                        {
                            row[record.GetName(i)] = record.GetValue(i);

                        }

                        results.Add(row);
                    }
                }
            }

            return results;
        }


        //sluzi za izvrsavanje delete,insert i update sql komandi
        //Note: Ovde su parametri obazvezni
        public int ExecuteCommit(string sql, List<object> parameters)
        {
            int rowsAffected = 0;
  
            using (IDbCommand command = conn.CreateCommand())
            {
                command.CommandText = sql;
                string sqlquery = command.CommandText;
                int countParam = 0;
                foreach (var parameter in parameters)
                {
                    var p = command.CreateParameter();
                    p.ParameterName = "@" + countParam;
                    p.Value = parameter;
                    sqlquery = sqlquery.Replace(p.ParameterName.ToString(), p.Value.ToString());
                    command.Parameters.Add(p);
                    countParam++;
                }
                logger.LogEvent(sqlquery);
                rowsAffected = command.ExecuteNonQuery();
            }
            return rowsAffected;
        }
        public static Database GetDatabase()
        {
            if (Instance == null)
            {
                try
                {
                   
                    string[] lines = File.ReadAllLines("..\\..\\..\\config.txt");
                    Instance = new Database(lines[1]);

                }
                catch (Exception ex)
                {

                    Console.WriteLine($"Connection string is not valid: {ex.Message}");
                }
            }

            return Instance;
        }

    }
}
