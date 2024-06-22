using ProviderLibrary.Builder;
using ProviderLibrary.Facade;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Reflection.Emit;
using System.Text;
using System.Threading.Tasks;

namespace ProviderLibrary.TemplateMethod
{
    public abstract class PaketDetaljiTemplate
    {
        
        protected string ispisNaziv(Packet paket)
        {
            return paket.Name;
        }
        protected string ispisCena(Packet paket)
        {
            return paket.Price.ToString();
        }
        protected string ispisOpis(Packet paket)
        {
            return paket.Description.ToString();
        }
        protected abstract string ispisSpecificniPodaci(Packet paket);

        public string[] ispisDetalji(int brUgovora, Packet paket)
        {
            string[] vrednosti = new string[5];
            vrednosti[0] = ("Naziv: " + ispisNaziv(paket));
            vrednosti[1] = ("Cena: " + ispisCena(paket))+" RSD";
            vrednosti[2] = (ispisOpis(paket));
            vrednosti[3] = (ispisSpecificniPodaci(paket));
            vrednosti[4] = "Broj ugovora: " + brUgovora.ToString();
            return vrednosti;
        }
    }
}
