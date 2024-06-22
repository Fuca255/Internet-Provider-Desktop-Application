using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProviderLibrary.Proxy
{
    internal interface IOperations
    {
        bool ObrisiKorisnika(int idKorisnika);
        bool ObrisiPaket(int idPaket);
        bool kreirajUgovor(int id_klijenta, int id_paketa, string datum_od, string datum_do);
    }
}
