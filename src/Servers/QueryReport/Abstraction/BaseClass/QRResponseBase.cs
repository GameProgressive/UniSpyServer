using System;
using System.Collections.Generic;
using QueryReport.Entity.Enumerate;

namespace QueryReport.Abstraction.BaseClass
{
    public class QRResponseBase
    {
        public QRPacketType PacketType { get; protected set; }
        public int InstantKey { get; protected set; }

        public QRResponseBase(QRRequestBase request)
        {
            PacketType = request.CommandName;
            InstantKey = request.InstantKey;
        }

        public QRResponseBase(QRPacketType packetType, int instantKey)
        {
            PacketType = packetType;
            InstantKey = instantKey;
        }

        public QRResponseBase()
        {
        }

        public virtual byte[] BuildResponse()
        {
            List<byte> data = new List<byte>();
            data.AddRange(QRRequestBase.MagicData);
            data.Add((byte)PacketType);
            data.AddRange(BitConverter.GetBytes(InstantKey));

            return data.ToArray();
        }
    }
}
