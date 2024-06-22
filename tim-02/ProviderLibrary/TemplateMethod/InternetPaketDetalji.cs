using ProviderLibrary.Builder;
using ProviderLibrary.Facade;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProviderLibrary.TemplateMethod
{
    public class InternetPaketDetalji : PaketDetaljiTemplate
    {
        protected override string ispisSpecificniPodaci(Packet paket)
        {
            
            return "Brzina interneta: " + paket.InternetSpeed.ToString();

        }
    }
}
