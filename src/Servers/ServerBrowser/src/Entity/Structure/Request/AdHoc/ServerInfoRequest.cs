using System;
using System.Linq;
using System.Net;
using UniSpyServer.Servers.ServerBrowser.Abstraction.BaseClass;
using UniSpyServer.Servers.ServerBrowser.Entity.Contract;
using UniSpyServer.Servers.ServerBrowser.Entity.Enumerate;

namespace UniSpyServer.Servers.ServerBrowser.Entity.Structure.Request
{
    [RequestContract(RequestType.ServerInfoRequest)]
    public sealed class ServerInfoRequest : RequestBase
    {
        /// <summary>
        /// The game server client search for
        /// </summary>
        public IPEndPoint TargetIPEndPoint { get; private set; }

        public ServerInfoRequest(byte[] rawRequest) : base(rawRequest)
        {
        }

        public override void Parse()
        {
            base.Parse();
            //if(recv.Length<length)
            //{
            //    return false;
            //}
            CommandName = (RequestType)RawRequest[2];

            byte[] ip = RawRequest.Skip(3).Take(4).ToArray();
            // raw request is in big endian we need to convert it to little endian
            // byte[] port = RawRequest.Skip(7).Take(2).Reverse().ToArray();

            byte[] port = RawRequest.Skip(7).Take(2).Reverse().ToArray();
            //TODO fix for gbrome!!!!!!!!!!!!!!!!!!!
            TargetIPEndPoint = new IPEndPoint(new IPAddress(ip), BitConverter.ToUInt16(port));
        }
    }
}
