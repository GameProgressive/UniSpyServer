using GameSpyLib.Extensions;
using NatNegotiation.Entity.Enumerator;
using System.Collections.Generic;
using System.Net;

namespace NatNegotiation.Entity.Structure.Packet
{
    public class ConnectPacket : BasePacket
    {
        public ConnectPacket()
        {
            Finished = 0;
            GotYourData = 1;
        }

        public EndPoint RemoteEndPoint { get; protected set; }
        public byte GotYourData { get; set; }
        public byte Finished { get; set; }



        public override byte[] BuildResponse()
        {
            List<byte> data = new List<byte>();

            PacketType = NatPacketType.Connect;
            data.AddRange(base.BuildResponse());

            data.AddRange(HtonsExtensions.
                EndPointToIPBytes(RemoteEndPoint));

            data.AddRange(HtonsExtensions.
                EndPointToHtonsPortBytes(RemoteEndPoint));

            data.Add(GotYourData);
            data.Add(Finished);

            return data.ToArray();
        }

        public ConnectPacket SetFinishedFlag(byte flag)
        {
            Finished = flag;
            return this;
        }

        public ConnectPacket SetVersionAndCookie(byte version, uint cookie)
        {
            Version = version;
            Cookie = cookie;
            return this;
        }

        public ConnectPacket SetRemoteEndPoint(EndPoint endPoint)
        {
            RemoteEndPoint = endPoint;
            return this;
        }
    }
}
