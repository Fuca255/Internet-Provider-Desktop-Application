using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ProviderLibrary.Interface;

namespace ProviderLibrary.Loggers
{
    public class FileLogger : IObserver
    {
        private readonly string filepath;
        
        public FileLogger(string filepath,string file)
        {
            this.filepath = Path.Combine(filepath, file);
            Directory.CreateDirectory(filepath);
            if (File.Exists(this.filepath))
            {
                File.AppendAllText(this.filepath, $"{DateTime.Now}: Log file accessed.\n");
            }
            else
            {
                File.WriteAllText(this.filepath, $"{DateTime.Now}: Log file created.\n");
            }
         }
        public void logEvent(string message)
        {
            File.AppendAllText(filepath, $"{DateTime.Now}:{message}\n");
        }
    }
}
