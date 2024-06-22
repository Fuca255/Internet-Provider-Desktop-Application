using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ProviderLibrary.Database;

namespace ProviderLibrary.Proxy
{
    internal class OperationsDB : IOperations
    {
        public Database.Database db;
        public OperationsDB(Database.Database db) { this.db = db; }
        public bool ObrisiKorisnika(int idKorisnika)
        {
            string sql = "delete from Klijent where id = @0";
            List<object> parametri = new List<object>();

            parametri.Add(idKorisnika.ToString());
            
            db.ExecuteCommit(sql, parametri);
            return true;
        }

        public bool ObrisiPaket(int idPaket)
        {
            string sql1 = "delete from TvPaket where paket_id = @0";
            List<object> parametri = new List<object>();
            parametri.Add(idPaket);

            string sql2 = "delete from Internet where paket_id = @0";
            string sql3 = "delete from Kombinovani where paket_id = @0";
            string sql4 = "delete from Paket where id = @0";

            db.ExecuteCommit(sql1, parametri);
            db.ExecuteCommit(sql2, parametri);
            db.ExecuteCommit(sql3, parametri);
            db.ExecuteCommit(sql4, parametri);
            return true;
        }
        public bool kreirajUgovor(int id_klijenta, int id_paketa, string datum_od, string datum_do)
        {
            string sql = "INSERT INTO Ugovor(klijent_id, paket_id, datum_od, datum_do) VALUES(@0, @1, @2, @3)";
            List<object> parametri = new List<object>();
            parametri.Add(id_klijenta);
            parametri.Add(id_paketa);
            parametri.Add(Convert.ToDateTime(datum_od));
            parametri.Add(Convert.ToDateTime(datum_do));

            int rows = db.ExecuteCommit(sql, parametri);

            return true;
        }
    }
}
