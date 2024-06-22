using ProviderLibrary.Builder;
using ProviderLibrary.Facade;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProviderLibrary.TemplateMethod
{
    public class KombinovaniPaketDetalji:PaketDetaljiTemplate
    {
        protected override string ispisSpecificniPodaci(Packet paket)
        {
            return "TV paket:" + DatabaseCommands.dajNazivPoID(paket.TvPacketID) + "\nInternet paket: " + DatabaseCommands.dajNazivPoID(paket.InternetPacketID);

        }
    }
}
