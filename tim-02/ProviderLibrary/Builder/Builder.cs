using Mysqlx.Session;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProviderLibrary.Builder
{
    public class Builder
    {
        public Packet packet;

        public Builder()
        {
            Reset();
        }

        public void Reset()
        {
            packet = new Packet();   
        }

        //mora da bude setovano za internet i tv paketa
        public void setID(int id)
        {
            packet.ID = id;
        }

        public void setName(string Name)
        {
            packet.Name = Name;
        }

        public void setPrice(double price)
        {
            packet.Price = price;
        }

        public void setDescription(string Description)
        {
            packet.Description = Description;
        }

        public void setInternetSpeed(int internetSpeed)
        {
            packet.InternetSpeed = internetSpeed;
        }

        public void setNumOfChannels(int numOfChannels)
        {
            packet.NumOfChannels = numOfChannels;
        }

        //mora da bude za setovanje kombinovanih paketa

        public void setInternetPacketID(int internetPacketID)
        {
            packet.InternetPacketID = internetPacketID;
        }

        public void setTvPacketID(int TvPacketID)
        {
            packet.TvPacketID = TvPacketID;
        }


        public Packet BuildPacket() {
            
            if (packet.InternetPacketID != -1 && packet.TvPacketID != -1)
                packet.Type = Packet.PacketType.COMBINED;
            else if (packet.InternetSpeed != -1)
                packet.Type = Packet.PacketType.INTERNET;
            else
                packet.Type = Packet.PacketType.TV;

            Packet p = packet;
            Reset();
            return p; 
        }
    }
}
