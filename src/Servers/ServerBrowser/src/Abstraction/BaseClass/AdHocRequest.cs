using System;
using System.Linq;
using System.Net;
using UniSpy.Server.ServerBrowser.Abstraction.BaseClass;

namespace UniSpy.Server.ServerBrowser.Abstraction
{
    public abstract class AdHocRequest : RequestBase
    {
        /// <summary>
        /// The game server client search for
        /// </summary>
        public IPEndPoint GameServerPublicIPEndPoint { get; private set; }
        public AdHocRequest(byte[] rawRequest) : base(rawRequest)
        {
        }

        public override void Parse()
        {
            base.Parse();
            byte[] ip = RawRequest.Skip(3).Take(4).ToArray();
            // raw request is in big endian we need to convert it to little endian
            byte[] port = RawRequest.Skip(7).Take(2).Reverse().ToArray();
            //TODO fix for gbrome!!!!!!!!!!!!!!!!!!!
            GameServerPublicIPEndPoint = new IPEndPoint(new IPAddress(ip), BitConverter.ToUInt16(port));
        }
    }
}
