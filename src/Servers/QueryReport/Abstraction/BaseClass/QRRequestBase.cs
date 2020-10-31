using GameSpyLib.Abstraction.Interface;
using GameSpyLib.Extensions;
using QueryReport.Entity.Enumerate;
using System;
using System.Collections.Generic;

namespace QueryReport.Abstraction.BaseClass
{
    public class QRRequestBase : IRequest
    {
        public static readonly byte[] MagicData = { 0xFE, 0XFD };
        public QRPacketType PacketType { get; protected set; }
        public int InstantKey { get; protected set; }
        public byte[] _rawRequest { get; protected set; }

        public QRRequestBase(byte[] rawRequest)
        {
            _rawRequest = rawRequest;
        }

        public QRRequestBase(QRPacketType packetType, int instantKey)
        {
            PacketType = packetType;
            InstantKey = instantKey;
        }

        public virtual bool Parse()
        {
            if (_rawRequest.Length < 3)
                return false;

            PacketType = (QRPacketType)_rawRequest[0];
            InstantKey = BitConverter.ToInt32(ByteTools.SubBytes(_rawRequest, 1, 4));
            return true;
        }

        object IRequest.Parse()
        {
            return Parse();
        }

        public object GetInstance()
        {
            return this;
        }
    }
}
