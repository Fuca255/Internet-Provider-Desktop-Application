using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProviderLibrary.Interface
{
    public interface IObserver
    {
        void logEvent(string logMessage);
    }
}
