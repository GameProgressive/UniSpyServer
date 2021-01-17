using NATNegotiation.Abstraction.BaseClass;
using ServerBrowser.Entity.Enumerate;
using System;
using System.Linq;
using System.Net;
using UniSpyLib.Abstraction.BaseClass;
using UniSpyLib.Extensions;

namespace ServerBrowser.Entity.Structure.Request
{
    public class AdHocRequest : UniSpyRequestBase
    {
        /// <summary>
        /// The game server client search for
        /// </summary>
        public string TargetServerIP => TargetEndPoint.Address.ToString();
        public string TargetServerHostPort => TargetEndPoint.Port.ToString();
        public IPEndPoint TargetEndPoint { get; protected set; }
        public new byte[] RawRequest
        {
            get { return (byte[])base.RawRequest; }
            protected set { base.RawRequest = value; }
        }
        public new SBClientRequestType CommandName
        {
            get { return (SBClientRequestType)base.CommandName; }
            protected set { base.CommandName = value; }
        }

        public AdHocRequest(byte[] rawRequest) : base(rawRequest)
        {
            //TODO check this
            CommandName = (SBClientRequestType)RawRequest[2];
        }

        public override void Parse()
        {
            ushort length = ByteTools.ToUInt16(ByteTools.SubBytes(RawRequest, 0, 2), true);

            //if(recv.Length<length)
            //{
            //    return false;
            //}
            if (RawRequest.Take(6).SequenceEqual(NNRequestBase.MagicData))
            {
                CommandName = SBClientRequestType.NatNegRequest;
            }
            else
            {
                CommandName = (SBClientRequestType)RawRequest[2];
            }
            byte[] ip = ByteTools.SubBytes(RawRequest, 3, 4);
            byte[] port = ByteTools.SubBytes(RawRequest, 7, 2);
            Array.Reverse(port);

            //TODO fix for gbrome!!!!!!!!!!!!!!!!!!!
            TargetEndPoint = ByteTools.GetIPEndPoint(ip, port);
            ErrorCode = true;
        }
    }
}
