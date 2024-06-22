using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProviderLibrary.Builder
{
    public class Packet
    {
        public enum PacketType
        {
            INTERNET,
            TV,
            COMBINED
        }
        //seteri moraju da budu internal(da se vide samo u datom namespace-u)
        public int ID {internal set; get; }
        public string Name {internal set; get; }
        public double Price {internal set; get; }
        public string Description { internal set; get; }
        public int NumOfChannels {internal set; get; }
        public int InternetSpeed {internal set; get; }
        public int InternetPacketID { internal set; get; }
        public int TvPacketID {  internal set; get; }
        public PacketType Type {internal set; get; }

        public Packet(){
            this.NumOfChannels = -1;
            this.InternetSpeed = -1;
            this.InternetPacketID = -1;
            this.TvPacketID = -1;
        }
        
    }
}
