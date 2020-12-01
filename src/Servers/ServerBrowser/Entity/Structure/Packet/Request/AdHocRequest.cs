using UniSpyLib.Extensions;
using System;
using System.Net;
using UniSpyLib.Abstraction.Interface;
using UniSpyLib.Abstraction.BaseClass;

namespace ServerBrowser.Entity.Structure.Packet.Request
{
    public class AdHocRequest : RequestBase
    {
        /// <summary>
        /// The game server client search for
        /// </summary>
        public string TargetServerIP { get { return TargetEndPoint.Address.ToString(); } }
        public string TargetServerHostPort { get { return TargetEndPoint.Port.ToString(); } }
        public IPEndPoint TargetEndPoint { get; protected set; }
        public new byte[] RawRequest { get; protected set; }

        public AdHocRequest(byte[] rawRequest)
        {
            RawRequest = rawRequest;
        }

        public override object Parse()
        {
            ushort length = ByteTools.ToUInt16(ByteTools.SubBytes(RawRequest, 0, 2), true);

            //if(recv.Length<length)
            //{
            //    return false;
            //}

            byte[] ip = ByteTools.SubBytes(RawRequest, 3, 4);
            byte[] port = ByteTools.SubBytes(RawRequest, 7, 2);
            Array.Reverse(port);

            //TODO fix for gbrome!!!!!!!!!!!!!!!!!!!
            TargetEndPoint = ByteTools.GetIPEndPoint(ip, port);
            return true;
        }
    }
}
