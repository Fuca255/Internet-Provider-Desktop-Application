using ProviderLibrary.Builder;
using ProviderLibrary.Facade;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Configuration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProviderLibrary.TemplateMethod
{
    public class TvPaketDetalji : PaketDetaljiTemplate
    {
        
        protected override string ispisSpecificniPodaci(Packet paket)
        {
            return "Broj kanala: " + paket.NumOfChannels.ToString();   
            
        }
    }
}
