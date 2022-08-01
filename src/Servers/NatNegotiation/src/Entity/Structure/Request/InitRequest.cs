using System;
using System.Linq;
using System.Net;
using UniSpyServer.Servers.NatNegotiation.Abstraction.BaseClass;

using UniSpyServer.Servers.NatNegotiation.Entity.Enumerate;
using UniSpyServer.UniSpyLib.Encryption;
using UniSpyServer.UniSpyLib.MiscMethod;

namespace UniSpyServer.Servers.NatNegotiation.Entity.Structure.Request
{

    public sealed class InitRequest : CommonRequestBase
    {
        public string GameName { get; private set; }
        [Newtonsoft.Json.JsonConverter(typeof(IPEndPointConverter))]
        public IPEndPoint PrivateIPEndPoint { get; private set; }
        public InitRequest(byte[] rawRequest) : base(rawRequest)
        {
        }
        public override void Parse()
        {
            base.Parse();
            var ipBytes = RawRequest.Skip(15).Take(4).ToArray();
            var portBytes = RawRequest.Skip(19).Take(2).Reverse().ToArray();
            // we found that the local port is not correct in gamespy and wireshark
            ushort port;
            if (PortType == NatPortType.NN2 || PortType == NatPortType.NN3)
            {
                port = (ushort)(BitConverter.ToUInt16(portBytes) + 1);
            }
            else
            {
                port = BitConverter.ToUInt16(portBytes);
            }
            PrivateIPEndPoint = new IPEndPoint(new IPAddress(ipBytes), port);
            if (RawRequest.Length > 21)
            {
                if (RawRequest[RawRequest.Length - 1] == 0)
                {
                    var gameNameBytes = RawRequest.Skip(21).Take(RawRequest.Length - 22).ToArray();
                    GameName = UniSpyEncoding.GetString(gameNameBytes);
                }
            }
        }
    }
}
