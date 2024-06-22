using ProviderLibrary.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProviderLibrary.Proxy
{
    internal class ProxyOperations : IOperations
    {
        private OperationsDB operationsDB;

        public ProxyOperations(OperationsDB operationsDB) { this.operationsDB = operationsDB; }

        public bool ObrisiKorisnika(int idKorisnika)
        {
            if (!checkKorisnik(idKorisnika))
            {
                return operationsDB.ObrisiKorisnika(idKorisnika);
                
            }
            return false;
        }

        public bool ObrisiPaket(int idPaket)
        {
            if (!checkPaket(idPaket) && !checkCombinedPaket(idPaket))
            {
                return operationsDB.ObrisiPaket(idPaket);
               
            }
            return false;
        }

        public bool kreirajUgovor(int id_klijenta, int id_paketa, string datum_od, string datum_do)
        {
            if(!checkUgovor(id_klijenta,id_paketa))
            {
                return operationsDB.kreirajUgovor(id_klijenta,id_paketa,datum_od,datum_do);
            }

            return false;
        }

        private bool checkUgovor(int id_klijenta,int id_paketa)
        {
            string sql = "select * from Ugovor where klijent_id = @0 and paket_id = @1";
            List<object> parametri = new List<object>();

            parametri.Add(id_klijenta);
            parametri.Add(id_paketa);


            List<Dictionary<string,object>> res = operationsDB.db.Execute_query(sql,parametri);

            if (res.Count > 0)
                return true;

            return false;
        }

        public bool checkKorisnik(int idKorisnika) 
        {
            // provera da li postoji aktivan ugovor za ovog korisnika
            string sql = "select * FROM Ugovor where klijent_id = @0";
            List<object> parametri = new List<object>();

            parametri.Add(idKorisnika);
            List<Dictionary<string,object>> _ugovori = new List<Dictionary<string,object>>();

            _ugovori = operationsDB.db.Execute_query(sql, parametri);

            if(_ugovori.Count > 0) { return true; }
            return false;
        }
        private bool checkCombinedPaket(int idPaket)
        {
            string sql = "select * from Kombinovani where tv_id = @0 or int_id=@0;";
            List<object> parametri = new List<object>();

            parametri.Add(idPaket);
            List<Dictionary<string, object>> res;

            res = operationsDB.db.Execute_query(sql, parametri);
            if (res.Count > 0)
                return true;


            return false;
        }
        public bool checkPaket(int idPaket)
        {
            // ako postoji aktivan ugovor sa tim id-ijem paketa onda ne sme da se brise
            string sql = "select * FROM Ugovor where paket_id = @0";
            List<object> parametri = new List<object>();

            parametri.Add(idPaket);
            List<Dictionary<string, object>> _ugovori = new List<Dictionary<string, object>>();

            _ugovori = operationsDB.db.Execute_query(sql, parametri);

            if (_ugovori.Count > 0) { return true; }
            return false;
        }

    }
}
