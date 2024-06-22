using ProviderLibrary.Proxy;
using ProviderLibrary.Builder;
using ProviderLibrary.Entities;
using ProviderLibrary.Database;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Management.Instrumentation;
using System.IO;


namespace ProviderLibrary.Facade
{
    public class DatabaseCommands
    {
        private static Database.Database db = Database.Database.GetDatabase();
        private static Builder.Builder builder = new Builder.Builder();
        private static Proxy.ProxyOperations po = new Proxy.ProxyOperations(new Proxy.OperationsDB(db));


        public static string dajNazivProvajdera()
        {
            try
            {

                string[] lines = File.ReadAllLines("..\\..\\..\\config.txt");
                return lines[0];

            }
            catch (Exception ex)
            {

                Console.WriteLine($"Provider name error: {ex.Message}");
                return "";
            }
        }


        public static List<Klijent> dajSveKlijente()
        {
            List<Klijent> Klijenti = new List<Klijent>();


            List<Dictionary<string, object>> KlijentiL = new List<Dictionary<string, object>>();
            string sql = "select * from Klijent";

            KlijentiL = db.Execute_query(sql);



            foreach (var klijent in KlijentiL)
            {
                Klijent k = new Klijent();
                k.Id = Convert.ToInt32(klijent["id"]);
                k.Ime = Convert.ToString(klijent["ime"]);
                k.Prezime = Convert.ToString(klijent["prezime"]);
                k.Username = Convert.ToString(klijent["username"]);
                k.JMBG = Convert.ToString(klijent["jbmg"]);

                Klijenti.Add(k);
            }

            return Klijenti;
        }

        public static List<Klijent> dajKlijentaPoImenu(string ime)
        {
            List<Dictionary<string, object>> KlijentiL = new List<Dictionary<string, object>>();

            List<Klijent> Klijenti = new List<Klijent>();

            string sql = "SELECT * FROM Klijent WHERE ime = @0";
            List<object> parametri = new List<object>();
            parametri.Add(ime);

            KlijentiL = db.Execute_query(sql, parametri);

            foreach (var klijent in KlijentiL)
            {
                Klijent k = new Klijent();
                k.Id = Convert.ToInt32(klijent["id"]);
                k.Ime = Convert.ToString(klijent["ime"]);
                k.Prezime = Convert.ToString(klijent["prezime"]);
                k.Username = Convert.ToString(klijent["username"]);
                k.JMBG = Convert.ToString(klijent["jbmg"]);

                Klijenti.Add(k);
            }


            return Klijenti;
        }

        public static List<Klijent> dajKlijentaPoPrezimenu(string prezime)
        {
            List<Dictionary<string, object>> KlijentiL = new List<Dictionary<string, object>>();

            List<Klijent> Klijenti = new List<Klijent>();

            string sql = "SELECT * FROM Klijent WHERE ime = @0";
            List<object> parametri = new List<object>();
            parametri.Add(prezime);

            KlijentiL = db.Execute_query(sql, parametri);

            foreach (var klijent in KlijentiL)
            {
                Klijent k = new Klijent();
                k.Id = Convert.ToInt32(klijent["id"]);
                k.Ime = Convert.ToString(klijent["ime"]);
                k.Prezime = Convert.ToString(klijent["prezime"]);
                k.Username = Convert.ToString(klijent["username"]);
                k.JMBG = Convert.ToString(klijent["jbmg"]);

                Klijenti.Add(k);
            }


            return Klijenti;
        }

        public static Klijent dajKlijentaPoUsernameu(string username)
        {
            List<Dictionary<string, object>> KlijentiL = new List<Dictionary<string, object>>();


            
            string sql = "SELECT * FROM Klijent WHERE username = @0";
            List<object> parametri = new List<object>();
            parametri.Add(username);

            KlijentiL = db.Execute_query(sql, parametri);
            Klijent k = new Klijent();
            foreach (var klijent in KlijentiL)
            {
                
                k.Id = Convert.ToInt32(klijent["id"]);
                k.Ime = Convert.ToString(klijent["ime"]);
                k.Prezime = Convert.ToString(klijent["prezime"]);
                k.Username = Convert.ToString(klijent["username"]);
                k.JMBG = Convert.ToString(klijent["jbmg"]);

            }
            return k;
        }

        public static List<Klijent> dajKlijentaPoUsernameuLike(string username)
        {
            List<Dictionary<string, object>> KlijentiL = new List<Dictionary<string, object>>();
            List<Klijent> klijenti = new List<Klijent>();


            string sql = "SELECT id, ime, prezime, username,jbmg FROM Klijent WHERE username LIKE '%" + username + "%'";
            List<object> parametri = new List<object>();
            parametri.Add(username);

            KlijentiL = db.Execute_query(sql, parametri);
            
            foreach (var klijent in KlijentiL)
            {
                Klijent k = new Klijent();
                k.Id = Convert.ToInt32(klijent["id"]);
                k.Ime = Convert.ToString(klijent["ime"]);
                k.Prezime = Convert.ToString(klijent["prezime"]);
                k.Username = Convert.ToString(klijent["username"]);
                k.JMBG = Convert.ToString(klijent["jbmg"]);

                klijenti.Add(k);
            }
            return klijenti;
        }

        public static List<Packet> dajTvPakete()
        {
            List<Dictionary<string, object>> _TvPaketi = new List<Dictionary<string, object>>();
            List<Packet> TvPaketi = new List<Packet>();

            string sql = "SELECT * FROM TvPaket INNER JOIN Paket on paket_id = id";

            _TvPaketi = db.Execute_query(sql);

            foreach (var paket in _TvPaketi)
            {
                builder.setPrice(Convert.ToDouble(paket["cena"]));
                builder.setName(Convert.ToString(paket["ime"]));
                builder.setID(Convert.ToInt32(paket["paket_id"]));
                builder.setNumOfChannels(Convert.ToInt32(paket["broj_kanal"]));
                builder.setDescription(Convert.ToString(paket["opis"]));

                TvPaketi.Add(builder.BuildPacket());
            }


            return TvPaketi;
        }

        public static List<Packet> dajInternetPakete()
        {
            List<Dictionary<string, object>> _InternetPaketi = new List<Dictionary<string, object>>();
            List<Packet> InternetPaketi = new List<Packet>();

            string sql = "SELECT * FROM Internet INNER JOIN Paket on paket_id = id";

            _InternetPaketi = db.Execute_query(sql);

            foreach (var paket in _InternetPaketi)
            {
                builder.setPrice(Convert.ToDouble(paket["cena"]));
                builder.setName(Convert.ToString(paket["ime"]));
                builder.setID(Convert.ToInt32(paket["paket_id"]));
                builder.setInternetSpeed(Convert.ToInt32(paket["brzina"]));
                builder.setDescription(Convert.ToString(paket["opis"]));

                InternetPaketi.Add(builder.BuildPacket());
            }


            return InternetPaketi;
        }

        public static List<Packet> dajKombinovanePakete()
        {
            List<Dictionary<string, object>> _KombinovaniPaketi = new List<Dictionary<string, object>>();
            List<Packet> KombinovaniPaketi = new List<Packet>();

            string sql = "SELECT * FROM Kombinovani INNER JOIN Paket on paket_id = id";

            _KombinovaniPaketi = db.Execute_query(sql);

            foreach (var paket in _KombinovaniPaketi)
            {
                builder.setPrice(Convert.ToDouble(paket["cena"]));
                builder.setName(Convert.ToString(paket["ime"]));
                builder.setID(Convert.ToInt32(paket["id"]));
                builder.setInternetSpeed(Convert.ToInt32(paket["int_id"]));
                builder.setNumOfChannels(Convert.ToInt32(paket["tv_id"]));
                builder.setDescription(Convert.ToString(paket["opis"]));

                KombinovaniPaketi.Add(builder.BuildPacket());
            }


            return KombinovaniPaketi;
        }

        public static List<Ugovor> dajUgovore(string id)
        {
            List<Dictionary<string, object>> _Ugovori = new List<Dictionary<string, object>>();
            List<Ugovor> Ugovori = new List<Ugovor>();

            string sql = "SELECT * FROM Ugovor WHERE klijent_id = @0";
            List<object> parametri = new List<object>();
            parametri.Add(id);
            _Ugovori = db.Execute_query(sql, parametri);

            foreach (var ugovor in _Ugovori)
            {
                Ugovor u = new Ugovor();
                u.Id = Convert.ToInt32(ugovor["id"]);
                u.Klijent_id = Convert.ToInt32(ugovor["klijent_id"]);
                u.Paket_id = Convert.ToInt32(ugovor["paket_id"]);
                u.Datum_od = Convert.ToDateTime(ugovor["datum_od"]);
                u.Datum_do = Convert.ToDateTime(ugovor["datum_do"]);


                Ugovori.Add(u);
            }

            return Ugovori;
        }

        
        public static List<Packet> dajTvPakete(List<Ugovor> ugovori)
        {
            List<Dictionary<string, object>> _paket = new List<Dictionary<string, object>>();
            Packet paket = new Packet();
            List<Packet> paketi = new List<Packet>();
            foreach (var u in ugovori)
            {
                string sql = "SELECT * FROM Paket INNER JOIN TvPaket ON id=paket_id WHERE id=@0";
                List<object> parametri = new List<object>();
                parametri.Add(Convert.ToString(u.Paket_id));
                _paket = db.Execute_query(sql,parametri);

                foreach(var p in _paket)
                {
                    builder.setName(Convert.ToString(p["ime"]));
                    builder.setID(Convert.ToInt32(p["id"]));
                    builder.setPrice(Convert.ToDouble(p["cena"]));
                    builder.setNumOfChannels(Convert.ToInt32(p["broj_kanal"]));
                    builder.setDescription(Convert.ToString(p["opis"]));

                    paketi.Add(builder.BuildPacket());
                }
                
            }
            return paketi;

        }

        public static List<Packet> dajInternetPakete(List<Ugovor> ugovori)
        {
            List<Dictionary<string, object>> _paket = new List<Dictionary<string, object>>();
            Packet paket = new Packet();
            List<Packet> paketi = new List<Packet>();
            foreach (var u in ugovori)
            {
                string sql = "SELECT * FROM Paket INNER JOIN Internet ON id=paket_id WHERE id=@0";
                List<object> parametri = new List<object>();
                parametri.Add(Convert.ToString(u.Paket_id));
                _paket = db.Execute_query(sql, parametri);

                foreach (var p in _paket)
                {
                    builder.setName(Convert.ToString(p["ime"]));
                    builder.setID(Convert.ToInt32(p["id"]));
                    builder.setPrice(Convert.ToDouble(p["cena"]));
                    builder.setInternetSpeed(Convert.ToInt32(p["brzina"]));
                    builder.setDescription(Convert.ToString(p["opis"]));

                    paketi.Add(builder.BuildPacket());
                }

            }
            return paketi;

        }

        public static List<Packet> dajKombinovanePakete(List<Ugovor> ugovori)
        {
            List<Dictionary<string, object>> _paket = new List<Dictionary<string, object>>();
            Packet paket = new Packet();
            List<Packet> paketi = new List<Packet>();
            foreach (var u in ugovori)
            {
                string sql = "SELECT * FROM Paket INNER JOIN Kombinovani ON id=paket_id WHERE id=@0";
                List<object> parametri = new List<object>();
                parametri.Add(Convert.ToString(u.Paket_id));
                _paket = db.Execute_query(sql, parametri);

                foreach (var p in _paket)
                {
                    builder.setName(Convert.ToString(p["ime"]));
                    builder.setID(Convert.ToInt32(p["id"]));
                    builder.setPrice(Convert.ToDouble(p["cena"]));
                    builder.setInternetSpeed(Convert.ToInt32(p["int_id"]));
                    builder.setNumOfChannels(Convert.ToInt32(p["tv_id"]));
                    builder.setDescription(Convert.ToString(p["opis"]));

                    paketi.Add(builder.BuildPacket());
                }

            }
            return paketi;

        }

        public static void registrujNovogKorisnika(string ime, string prezime, string jmbg, string username)
        {
            string sql = "INSERT INTO Klijent(ime, prezime, username, jbmg) VALUES(@0, @1, @2, @3)";
            List<object> parametri = new List<object>();
            parametri.Add(ime);
            parametri.Add(prezime);
            parametri.Add(username);
            parametri.Add(jmbg);
            

            db.ExecuteCommit(sql, parametri);
        }

        public static void dodajNoviTvPaket(string naziv, double cena, int brojKanala, string opis)
        {
            string sql = "INSERT INTO Paket(ime, cena, opis) VALUES(@0, @1, @2)";
            List<object> parametri = new List<object>();
            parametri.Add(naziv);
            parametri.Add(cena.ToString());
            parametri.Add(opis);

            db.ExecuteCommit(sql, parametri);

            sql = "SELECT * FROM Paket WHERE ime = @0";
            parametri.Clear();
            parametri.Add(naziv);

            List<Dictionary<string, object>> _paketId = new List<Dictionary<string, object>>();
            _paketId = db.Execute_query(sql, parametri);


            int idPaketa = 0;
            foreach (var p in _paketId)
            {
                idPaketa = Convert.ToInt32(p["id"]);
            }

            sql = "INSERT INTO TvPaket(paket_id, broj_kanal) VALUES(@0, @1)";
            parametri.Clear();
            parametri.Add(idPaketa.ToString());
            parametri.Add(brojKanala.ToString());

            db.ExecuteCommit(sql, parametri);
        }

        public static void dodajNoviInternetPaket(string naziv, double cena, int brzinaInterneta, string opis)
        {
            string sql = "INSERT INTO Paket(ime, cena, opis) VALUES(@0, @1, @2)";
            List<object> parametri = new List<object>();
            parametri.Add(naziv);
            parametri.Add(cena.ToString());
            parametri.Add(opis);

            db.ExecuteCommit(sql, parametri);

            sql = "SELECT * FROM Paket WHERE ime = @0";
            parametri.Clear();
            parametri.Add(naziv);

            List<Dictionary<string, object>> _paketId = new List<Dictionary<string, object>>();
            _paketId = db.Execute_query(sql, parametri);


            int idPaketa = 0;
            foreach (var p in _paketId)
            {
                idPaketa = Convert.ToInt32(p["id"]);
            }

            sql = "INSERT INTO Internet(paket_id, brzina) VALUES(@0, @1)";
            parametri.Clear();
            parametri.Add(idPaketa.ToString());
            parametri.Add(brzinaInterneta.ToString());

            db.ExecuteCommit(sql, parametri);
        }

        public static void dodajNoviKombinovaniPaket(int int_id, int tv_id, string naziv, double cena, string opis)
        {
            string sql = "INSERT INTO Paket(ime, cena, opis) VALUES(@0, @1, @2)";
            List<object> parametri = new List<object>();
            parametri.Add(naziv);
            parametri.Add(cena.ToString());
            parametri.Add(opis);

            db.ExecuteCommit(sql, parametri);

            sql = "SELECT * FROM Paket WHERE ime = @0";
            parametri.Clear();
            parametri.Add(naziv);

            List<Dictionary<string, object>> _paketId = new List<Dictionary<string, object>>();
            _paketId = db.Execute_query(sql, parametri);


            int idPaketa = 0;
            foreach (var p in _paketId)
            {
                idPaketa = Convert.ToInt32(p["id"]);
            }

            sql = "INSERT INTO Kombinovani(paket_id, tv_id, int_id) VALUES(@0, @1, @2)";
            parametri.Clear();
            parametri.Add(idPaketa.ToString());
            parametri.Add(tv_id.ToString());
            parametri.Add(int_id.ToString());

            db.ExecuteCommit(sql, parametri);
        }

        public static Packet dajPaketPoNazivu(string naziv) 
        {
            string sql = "SELECT * FROM Paket WHERE ime = @0";
            List<object> parametri = new List<object>();
            parametri.Add(naziv);
            List<Dictionary<string, object>> _paketi = new List<Dictionary<string, object>>();

            _paketi = db.Execute_query(sql, parametri);

            Packet packet = new Packet();
            foreach(var p in _paketi)
            {
                builder.setName(Convert.ToString(p["ime"]));
                builder.setID(Convert.ToInt32(p["id"]));
                builder.setDescription(Convert.ToString(p["opis"]));
                packet = builder.BuildPacket();
            }

            return packet;
        }
        public static string dajNazivPoID(int id)
        {
            string sql = "SELECT * FROM Paket WHERE id = @0";
            List<object> parametri = new List<object>();
            parametri.Add(id);
            List<Dictionary<string, object>> _paketi = new List<Dictionary<string, object>>();

            _paketi = db.Execute_query(sql, parametri);

            Packet packet = new Packet();
            foreach (var p in _paketi)
            {
                builder.setName(Convert.ToString(p["ime"]));
                builder.setDescription(Convert.ToString(p["opis"]));
                packet = builder.BuildPacket();
            }

            return packet.Name;
        }

        public static bool kreirajUgovor(int id_klijenta, int id_paketa, string datum_od, string datum_do)
        {
            return po.kreirajUgovor(id_klijenta,id_paketa,datum_od,datum_do);
        }

        public static Packet dajSpecificniPaket(Packet packet)
        {
            int pId = packet.ID;

            string sql = "SELECT * FROM Paket WHERE id=@0";
            List<object> parametri = new List<object>();
            parametri.Add(pId.ToString());
            List<Dictionary<string, object>> _paketi = new List<Dictionary<string, object>>();

            _paketi = db.Execute_query(sql, parametri);

            foreach(var p in _paketi)
            {
                builder.setName(Convert.ToString(p["ime"]));
                builder.setPrice(Convert.ToDouble(p["cena"]));
                builder.setDescription(Convert.ToString(p["opis"]));
            }



            sql = "SELECT * FROM TvPaket WHERE paket_id = @0";
            

            
            _paketi = db.Execute_query(sql, parametri);

            List<Dictionary<string, object>> __paketi = new List<Dictionary<string, object>>();

            if ( _paketi.Count == 0 )
            {
                sql = "SELECT * FROM Internet WHERE paket_id = @0";
                _paketi = db.Execute_query(sql, parametri);

                if(_paketi.Count == 0 ) // sigurno je kombinovani
                {
                    sql = "SELECT * FROM Kombinovani WHERE paket_id = @0";
                    _paketi = db.Execute_query(sql, parametri);

                    foreach(var p in _paketi)
                    {
                        builder.setInternetPacketID(Convert.ToInt32(p["int_id"]));
                        builder.setTvPacketID(Convert.ToInt32(p["tv_id"]));

                        sql = "SELECT * FROM Internet WHERE paket_id = @0";
                        parametri.Clear();
                        parametri.Add(Convert.ToString(p["int_id"]));

                        __paketi = db.Execute_query(sql, parametri);

                        foreach(var pp in __paketi)
                        {
                            builder.setInternetSpeed(Convert.ToInt32(pp["brzina"]));
                        }


                        sql = "SELECT * FROM TvPaket WHERE paket_id = @0";
                        parametri.Clear();
                        parametri.Add(Convert.ToString(p["tv_id"]));

                        __paketi = db.Execute_query(sql, parametri);

                        foreach (var pp in __paketi)
                        {
                            builder.setNumOfChannels(Convert.ToInt32(pp["broj_kanal"]));
                        }

                        packet = builder.BuildPacket();
                        return packet;
                    }
                }
                else //gradim Internet paket ceo
                {
                    foreach (var p in _paketi)
                    {
                        builder.setInternetSpeed(Convert.ToInt32(p["brzina"]));

                        packet = builder.BuildPacket();
                        return packet;
                    }
                }
            }
            else // gradim TV PAKET CEO
            {
                foreach(var p in _paketi)
                {
                    builder.setNumOfChannels(Convert.ToInt32(p["broj_kanal"]));

                    packet = builder.BuildPacket();
                    return packet;
                } 
            }
            return packet;
        }

        public static Packet dajPaketPoUgovoru(int idUgovora)
        {
            string sql = "SELECT * FROM Ugovor INNER JOIN Paket p ON paket_id = p.id WHERE Ugovor.id = @0";
            List<object> parametri = new List<object>();
            parametri.Add(idUgovora.ToString());
            List<Dictionary<string, object>> _paketi = new List<Dictionary<string, object>>();

            _paketi = db.Execute_query(sql, parametri);

            Packet packet = new Packet();
            foreach (var p in _paketi)
            {
                builder.setName(Convert.ToString(p["ime"]));
                builder.setID(Convert.ToInt32(p["id"]));
                builder.setDescription(Convert.ToString(p["opis"]));

                packet = builder.BuildPacket();
            }

            return packet;
        }

        public static Ugovor dajIdUgovoraPoPaketuiKlijetu(int id_paketa, int id_klijenta)
        {
            string sql = "SELECT * FROM Ugovor WHERE paket_id=@0 AND klijent_id=@1";
            List<object> parametri = new List<object>();
            parametri.Add(id_paketa.ToString());
            parametri.Add(id_klijenta.ToString());

            List<Dictionary<string, object>> _ugovori = new List<Dictionary<string, object>>();

            _ugovori = db.Execute_query(sql, parametri);

            
            Ugovor ugovor = new Ugovor();
            foreach (var u in _ugovori)
            {
                ugovor.Id = Convert.ToInt32(u["id"]);
                ugovor.Datum_od = Convert.ToDateTime(u["datum_od"]);
                ugovor.Datum_do = Convert.ToDateTime(u["datum_do"]);
                //Console.WriteLine(ugovor.Datum_do);

            }
            return ugovor;
        }

        public static void izbrisiUgovor(int id_ugovora)
        {
            string sql = "DELETE FROM Ugovor WHERE id = @0";
            List<object> parametri = new List<object>();
            parametri.Add(id_ugovora.ToString());

            db.ExecuteCommit(sql, parametri);
        }

        public static bool obrisiPaket(int id_paketa)
        {
            return po.ObrisiPaket(id_paketa);
        }

        public static bool obrisiKorisnika(int id_korsnika)
        {
            return po.ObrisiKorisnika(id_korsnika);
        }
    }
}
