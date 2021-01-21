using System;
using System.Net;
using ServerBrowser.Abstraction.BaseClass;
using ServerBrowser.Entity.Enumerate;
using UniSpyLib.Extensions;

namespace ServerBrowser.Entity.Structure.Request
{
    internal sealed class AdHocRequest : SBRequestBase
    {
        /// <summary>
        /// The game server client search for
        /// </summary>
        public string TargetServerIP => TargetEndPoint.Address.ToString();
        public string TargetServerHostPort => TargetEndPoint.Port.ToString();
        public IPEndPoint TargetEndPoint { get; private set; }

        public AdHocRequest(byte[] rawRequest) : base(rawRequest)
        {
            ErrorCode = SBErrorCode.NoError;
        }

        public override void Parse()
        {
            base.Parse();
            //if(recv.Length<length)
            //{
            //    return false;
            //}
            CommandName = (SBClientRequestType)RawRequest[2];

            byte[] ip = ByteTools.SubBytes(RawRequest, 3, 4);
            byte[] port = ByteTools.SubBytes(RawRequest, 7, 2);
            Array.Reverse(port);

            //TODO fix for gbrome!!!!!!!!!!!!!!!!!!!
            TargetEndPoint = ByteTools.GetIPEndPoint(ip, port);
        }
    }
}
