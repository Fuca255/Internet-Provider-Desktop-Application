using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProviderLibrary.Entities
{
    public class Ugovor
    {
        public int Id { get; set; }
        public int Klijent_id { get; set; }
        public int Paket_id { get; set; }
        public DateTime Datum_od {  get; set; }
        public DateTime Datum_do { get; set; }
        
        public bool Aktivan {
            get
            {
                return this.setAktivan();
            } 
        }

        public bool setAktivan()
        {
            if (DateTime.Now > Datum_do) { return false; }
            else return true;
        }
    }
}
