using GameSpyLib.Database.Entity;
using GameSpyLib.Network;
using NatNegotiation.Handler.CommandHandler.CommandSwitcher;
using System.Net;
using System.Collections.Concurrent;
using NATNegotiation.Entity.Structure;
using System.Collections.Generic;
using System.Linq;
using NatNegotiation.Entity.Enumerator;
using NatNegotiation.Entity.Structure.Packet;

namespace NatNegotiation
{
    public class NatNegServer : TemplateUdpServer
    {

        public static List< ClientInfo> ClientList = new List<ClientInfo>();
        /// <summary>
        /// 
        /// </summary>
        /// <param name="dbdriver">If choose sqlite for database you should pass the connection to server
        /// ,maybe NatNeg server dose not need connected to database.</param>
        /// <param name="bindTo"></param>
        /// <param name="MaxConnections"></param>
        public NatNegServer(string serverName, DatabaseEngine engine, IPAddress address, int port) : base(serverName, address, port)
        {

        }

        protected override void OnReceived(EndPoint endPoint, byte[] message)
        {
            //check and add client into clientList
            if (ClientList.Where(c => c.EndPoint == endPoint).Count() == 0)
            {
                ClientList.Add(new ClientInfo { EndPoint = endPoint});
            }
            CommandSwitcher.Switch(this, endPoint, message);
        }


        /// <summary>
        /// Get repsonse packet size from natneg recieved packet type
        /// </summary>
        /// <param name="type">recieved packet type</param>
        /// <returns></returns>
        public static int GetReplyPacketSize(NatPacketType type)
        {
            //The size is initially CommonInfo size
            int size = BasePacket.Size;

            switch (type)
            {
                case NatPacketType.PreInit:
                case NatPacketType.PreInitAck:
                    size += 6;
                    break;
                case NatPacketType.AddressCheck:
                case NatPacketType.NatifyRequest:
                case NatPacketType.ErtTest:
                case NatPacketType.ErtAck:
                case NatPacketType.Init:
                case NatPacketType.InitAck:
                case NatPacketType.ConnectAck:
                case NatPacketType.ReportAck:
                    size += 9;
                    break;
                case NatPacketType.Connect:
                case NatPacketType.ConnectPing:
                    size += 8;
                    break;
                case NatPacketType.Report:
                    size += 61;
                    break;
                default:
                    break;
            }
            return size;
        }
    }
}
