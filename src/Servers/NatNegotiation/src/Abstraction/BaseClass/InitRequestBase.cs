using System;
using System.Linq;
using System.Net;
using UniSpyServer.Servers.NatNegotiation.Abstraction.BaseClass;
using UniSpyServer.Servers.NatNegotiation.Entity.Enumerate;
using UniSpyServer.UniSpyLib.MiscMethod;

namespace UniSpyServer.Servers.NatNegotiation.Entity.Structure.Request
{
    public abstract class InitRequestBase : RequestBase
    {
        public NatPortType PortType { get; protected set; }
        public byte ClientIndex { get; protected set; }
        public byte UseGamePort { get; protected set; }

        [Newtonsoft.Json.JsonConverter(typeof(IPEndPointConverter))]
        public IPEndPoint RemoteIPEndPoint { get; protected set; }
        public InitRequestBase(byte[] rawRequest) : base(rawRequest)
        {
        }
        public override void Parse()
        {
            base.Parse();
            PortType = (NatPortType)RawRequest[12];
            ClientIndex = RawRequest[13];
            UseGamePort = RawRequest[14];
            var ipBytes = RawRequest.Skip(15).Take(4).ToArray();
            var portBytes = RawRequest.Skip(19).Take(2).ToArray();
            var port = BitConverter.ToUInt16(portBytes);
            RemoteIPEndPoint = new IPEndPoint(new IPAddress(ipBytes), port);
        }
    }
}
