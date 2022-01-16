using UniSpyServer.Servers.ServerBrowser.Abstraction.BaseClass;
using UniSpyServer.Servers.ServerBrowser.Entity.Enumerate;
using System;
using System.Net;
using UniSpyServer.UniSpyLib.Extensions;
using System.Linq;

namespace UniSpyServer.Servers.ServerBrowser.Entity.Structure.Request
{
    public sealed class AdHocRequest : RequestBase
    {
        /// <summary>
        /// The game server client search for
        /// </summary>
        public string TargetServerIP => TargetIPEndPoint.Address.ToString();
        public string TargetServerHostPort => TargetIPEndPoint.Port.ToString();
        public IPEndPoint TargetIPEndPoint { get; private set; }

        public AdHocRequest(byte[] rawRequest) : base(rawRequest)
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
            byte[] port = RawRequest.Skip(7).Take(2).Reverse().ToArray();
            //TODO fix for gbrome!!!!!!!!!!!!!!!!!!!
            TargetIPEndPoint = new IPEndPoint(new IPAddress(ip), BitConverter.ToInt16(port));
        }
    }
}
