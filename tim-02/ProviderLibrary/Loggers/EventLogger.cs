using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ProviderLibrary.Interface;

namespace ProviderLibrary.Loggers
{
    public class EventLogger
    {
        private List<IObserver> logs = new List<IObserver>();

        public void AddObserver(IObserver observer)
        {
            logs.Add(observer);
        }

        public void RemoveObserver(IObserver observer)
        {
            logs.Remove(observer);
        }
        public void LogEvent(string message)
        {
            foreach (var observer in logs)
            {
                observer.logEvent(message);
            }
        }
    }
}
