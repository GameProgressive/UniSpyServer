using System;
using System.Collections.Generic;
using System.Net;
using UniSpyLib.Extensions;
using NATNegotiation.Abstraction.BaseClass;
using NATNegotiation.Entity.Enumerate;
using NATNegotiation.Entity.Structure.Request;

namespace NATNegotiation.Entity.Structure.Response
{
    public class ConnectResponse : NNResponseBase
    {
        public EndPoint RemoteEndPoint { get; protected set; }
        public byte GotYourData { get; set; }
        public byte Finished { get; set; }

        public ConnectResponse(byte version, uint cookie,EndPoint endPoint) : base(version, cookie)
        {
            Finished = 0;
            GotYourData = 1;
            RemoteEndPoint = endPoint;
            PacketType = NatPacketType.Connect;
        }

        public override byte[] BuildResponse()
        {
            List<byte> data = new List<byte>();

            data.AddRange(base.BuildResponse());

            data.AddRange(HtonsExtensions.
                EndPointToIPBytes(RemoteEndPoint));

            data.AddRange(HtonsExtensions.
                EndPointToHtonsPortBytes(RemoteEndPoint));

            data.Add(GotYourData);
            data.Add(Finished);

            return data.ToArray();
        }
    }
}
