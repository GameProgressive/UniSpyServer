using System.Collections.Generic;
using System.Net;
using GameSpyLib.Extensions;
using NATNegotiation.Abstraction.BaseClass;
using NATNegotiation.Entity.Enumerate;
using NATNegotiation.Entity.Structure.Request;

namespace NATNegotiation.Entity.Structure.Response
{
    public class InitResponse : NNResponseBase
    {
        public NatPortType PortType { get; protected set; }
        public byte ClientIndex { get; protected set; }
        public byte UseGamePort { get; protected set; }
        public string LocalIP { get; protected set; }
        public ushort LocalPort { get; protected set; }

        public InitResponse(InitRequest request,EndPoint endPoint) : base(request)
        {
            PacketType = NatPacketType.InitAck;
            PortType = request.PortType;
            ClientIndex = request.ClientIndex;
            UseGamePort = request.UseGamePort;
            LocalIP = ((IPEndPoint)endPoint).Address.ToString();
            LocalPort = (ushort)((IPEndPoint)endPoint).Port;
        }

        public override byte[] BuildResponse()
        {
            List<byte> data = new List<byte>();
            data.AddRange(base.BuildResponse());

            data.Add((byte)PortType);
            data.Add(ClientIndex);
            data.Add(UseGamePort);

            data.AddRange(HtonsExtensions.IPStringToBytes(LocalIP));
            data.AddRange(HtonsExtensions.UshortPortToBytes(LocalPort));

            return data.ToArray();
        }
    }
}
