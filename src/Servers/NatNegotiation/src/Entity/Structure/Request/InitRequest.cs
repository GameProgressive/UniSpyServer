using System;
using System.Linq;
using System.Net;
using UniSpyServer.Servers.NatNegotiation.Abstraction.BaseClass;
using UniSpyServer.Servers.NatNegotiation.Entity.Contract;
using UniSpyServer.Servers.NatNegotiation.Entity.Enumerate;
using UniSpyServer.UniSpyLib.Encryption;
using UniSpyServer.UniSpyLib.MiscMethod;

namespace UniSpyServer.Servers.NatNegotiation.Entity.Structure.Request
{
    [RequestContract(RequestType.Init)]
    public sealed class InitRequest : RequestBase
    {
        public string GameName { get; private set; }
        [Newtonsoft.Json.JsonConverter(typeof(IPEndPointConverter))]
        public IPEndPoint LocalIPEndPoint { get; private set; }
        public InitRequest(byte[] rawRequest) : base(rawRequest)
        {
        }
        public override void Parse()
        {
            base.Parse();
            var ipBytes = RawRequest.Skip(15).Take(4).Reverse().ToArray();
            var portBytes = RawRequest.Skip(19).Take(2).Reverse().ToArray();
            var port = BitConverter.ToUInt16(portBytes);
            LocalIPEndPoint = new IPEndPoint(new IPAddress(ipBytes), port);
            if (RawRequest.Length > 21)
            {
                if (RawRequest[RawRequest.Length - 1] == 0)
                {
                    var gameNameBytes = RawRequest.Skip(21).ToArray();
                    GameName = UniSpyEncoding.GetString(gameNameBytes);
                }
            }
        }
    }
}
