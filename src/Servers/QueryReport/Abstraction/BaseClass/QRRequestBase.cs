using UniSpyLib.Abstraction.Interface;
using UniSpyLib.Extensions;
using QueryReport.Entity.Enumerate;
using System;

namespace QueryReport.Abstraction.BaseClass
{
    public class QRRequestBase : IRequest
    {
        public static readonly byte[] MagicData = { 0xFE, 0XFD };
        public QRPacketType PacketType { get; protected set; }
        public int InstantKey { get; protected set; }
        public byte[] RawRequest { get; protected set; }

        object IRequest.CommandName => PacketType;

        public QRRequestBase(byte[] rawRequest)
        {
            RawRequest = rawRequest;
        }

        public virtual bool Parse()
        {
            if (RawRequest.Length < 3)
            {
                return false;
            }
            PacketType = (QRPacketType)RawRequest[0];
            InstantKey = BitConverter.ToInt32(ByteTools.SubBytes(RawRequest, 1, 4));
            return true;
        }

        object IRequest.Parse()
        {
            return Parse();
        }
    }
}
