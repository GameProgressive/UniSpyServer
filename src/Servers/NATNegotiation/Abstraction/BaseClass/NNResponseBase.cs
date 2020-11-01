using System;
using System.Collections.Generic;
using NATNegotiation.Entity.Enumerate;

namespace NATNegotiation.Abstraction.BaseClass
{
    public class NNResponseBase
    {
        public byte Version { get; protected set; }
        public NatPacketType PacketType { get; protected set; }
        public uint Cookie { get; protected set; }

        public NNResponseBase(NNRequestBase request)
        {
            Version = request.Version;
            Cookie = request.Cookie;
        }

        public NNResponseBase(byte version, uint cookie)
        {
            Version = version;
            Cookie = cookie;
        }

        public virtual byte[] BuildResponse()
        {
            List<byte> data = new List<byte>();
            data.AddRange(NNRequestBase.MagicData);
            data.Add(Version);
            data.Add((byte)PacketType);
            data.AddRange(BitConverter.GetBytes(Cookie));

            return data.ToArray();
        }
    }
}
