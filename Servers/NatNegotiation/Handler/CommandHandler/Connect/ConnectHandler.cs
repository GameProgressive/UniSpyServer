using GameSpyLib.Common.Entity.Interface;
using GameSpyLib.Logging;
using NatNegotiation.Entity.Enumerator;
using NatNegotiation.Entity.Structure;
using NatNegotiation.Entity.Structure.Packet;
using NatNegotiation.Server;
using System.Linq;

namespace NatNegotiation.Handler.CommandHandler
{
    public class ConnectHandler : NatNegCommandHandlerBase
    {
        public ConnectHandler(IClient _clientInfo, NatNegClientInfo clientInfo, byte[] _recv) : base(_clientInfo, clientInfo, _recv)
        {
        }

        public static void SendConnectPacket()
        {
            //ConnectPacket connPacket = new ConnectPacket
            //{
            //    PacketType = (byte)NatPacketType.Connect,
            //    Cookie = _clientInfo.Cookie,
            //    Finished = 0,
            //    RemoteIP = _clientInfo.PublicIP,
            //    RemotePort = _clientInfo.PublicPort,
            //};
            //connPacket.Parse(_clientInfo.RemoteEndPoint, _recv);

            //byte[] buffer = connPacket.GenerateByteArray();

            //server.SendAsync(_clientInfo.RemoteEndPoint, buffer);
            //_clientInfo.SentConnectPacketTime = DateTime.Now;
            //_clientInfo.IsConnected = true;

            //send to other _clientInfo
            //if (other == null)
            //{
            //    return; 
            //}

            //connPacket.RemoteIP = NNFormat.IPToByte(other.RemoteEndPoint);
            //connPacket.RemotePort = NNFormat.PortToByte(other.RemoteEndPoint);

            //buffer = connPacket.GenerateByteArray();
            //server.SendAsync(other.RemoteEndPoint, buffer);
            //other.SentConnectPacketTime = DateTime.Now;
        }

        public void SendDeadHeartBeatNotice()
        {
            ConnectPacket connPacket = new ConnectPacket();
            connPacket.Parse(_clientInfo.RemoteEndPoint, _recv);
            byte[] buffer = connPacket.GenerateResponse(NatPacketType.Connect);
            _client.SendAsync(buffer);
        }

        protected override void CheckRequest()
        {
            _connPacket = new ConnectPacket();
            _connPacket.Parse(_recv);
        }

        protected override void DataOperation()
        {
        }

        protected override void ConstructResponse()
        {
        }

        protected override void Response()
        {
            var other = NatNegServer.ClientInfoList.Values.Where(
                c => c.RemoteEndPoint == _connPacket.RemoteEndPoint);

            if (other.Count() < 1)
            {
                LogWriter.ToLog("Can not find client2 that client1 tries to connect!");
                return;
            }

            NatNegClientInfo client2 = other.First();

            _sendingBuffer = _connPacket.GenerateResponse(NatPacketType.Connect);
            //_client.Server.SendAsync(client2.RemoteEndPoint, _sendingBuffer);
        }
    }
}
