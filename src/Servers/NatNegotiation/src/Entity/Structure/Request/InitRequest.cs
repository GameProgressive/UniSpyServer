using System;
using System.Linq;
using System.Net;
using UniSpy.Server.NatNegotiation.Abstraction.BaseClass;
using UniSpy.Server.Core.Encryption;
using UniSpy.Server.Core.MiscMethod;

namespace UniSpy.Server.NatNegotiation.Entity.Structure.Request
{

    public sealed class InitRequest : CommonRequestBase
    {
        public string GameName { get; private set; }
        /// <summary>
        /// GamePort (GP) private ip address and port, using for NAT negotiation
        /// </summary>
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

            port = (ushort)BitConverter.ToUInt16(portBytes);
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
