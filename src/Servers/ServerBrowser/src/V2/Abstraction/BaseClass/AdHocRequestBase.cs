using System;
using System.Linq;
using System.Net;
using UniSpy.Server.ServerBrowser.V2.Abstraction.BaseClass;

namespace UniSpy.Server.ServerBrowser.V2.Abstraction
{
    public abstract class AdHocRequestBase : RequestBase
    {
        /// <summary>
        /// The game server client search for
        /// </summary>
        public IPEndPoint GameServerPublicIPEndPoint { get; private set; }
        public AdHocRequestBase(byte[] rawRequest) : base(rawRequest)
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
