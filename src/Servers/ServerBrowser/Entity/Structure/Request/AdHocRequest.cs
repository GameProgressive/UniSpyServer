using UniSpyLib.Extensions;
using System;
using System.Net;
using UniSpyLib.Abstraction.Interface;
using UniSpyLib.Abstraction.BaseClass;
using ServerBrowser.Entity.Enumerate;
using System.Linq;
using NATNegotiation.Abstraction.BaseClass;

namespace ServerBrowser.Entity.Structure.Request
{
    public class AdHocRequest : UniSpyRequestBase
    {
        /// <summary>
        /// The game server client search for
        /// </summary>
        public string TargetServerIP { get { return TargetEndPoint.Address.ToString(); } }
        public string TargetServerHostPort { get { return TargetEndPoint.Port.ToString(); } }
        public IPEndPoint TargetEndPoint { get; protected set; }
        public new byte[] RawRequest { get; protected set; }
        public new SBClientRequestType CommandName { get; protected set; }

        public AdHocRequest(byte[] rawRequest):base(rawRequest)
        {
            RawRequest = rawRequest;
            CommandName = (SBClientRequestType)RawRequest[2];
        }
        public AdHocRequest(byte[] rawRequest, SBClientRequestType commandName) : base(rawRequest)
        {
            RawRequest = rawRequest;
            CommandName = commandName;
        }

        public override object Parse()
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
            return true;
        }
    }
}
