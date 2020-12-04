using UniSpyLib.Abstraction.Interface;
using UniSpyLib.Extensions;
using QueryReport.Entity.Enumerate;
using System;
using UniSpyLib.Abstraction.BaseClass;

namespace QueryReport.Abstraction.BaseClass
{
    public class QRRequestBase : UniSpyRequestBase
    {
        public static readonly byte[] MagicData = { 0xFE, 0XFD };
        public QRPacketType PacketType { get; protected set; }
        public int InstantKey { get; protected set; }
        public new byte[] RawRequest { get; protected set; }


        public QRRequestBase(byte[] rawRequest):base(rawRequest)
        {
            RawRequest = rawRequest;
        }

        public override object Parse()
        {
            if (RawRequest.Length < 3)
            {
                return false;
            }
            PacketType = (QRPacketType)RawRequest[0];
            InstantKey = BitConverter.ToInt32(ByteTools.SubBytes(RawRequest, 1, 4));
            return true;
        }
    }
}
