using GameSpyLib.Encryption;
using GameSpyLib.Extensions;
using NatNegotiation.Entity.Enumerator;
using System;
using System.Collections.Generic;
using System.Text;

namespace NatNegotiation.Entity.Structure.Packet
{
    public class ReportPacket : BasePacket
    {
        public new static readonly int Size = BasePacket.Size + 61;

        public byte PortType;
        public byte ClientIndex;
        public byte NegResult;
        public NatNegotiationType NatType; //int
        public NatNegotiationMappingScheme MappingScheme; //int
        public string GameName;

        public new void Parse(byte[] recv)
        {
            base.Parse(recv);
            PortType = recv[13];
            ClientIndex = recv[14];
            NegResult = recv[15];

            NatType = (NatNegotiationType)BitConverter.ToInt32(ByteTools.SubBytes(recv, 17, sizeof(int)));
            MappingScheme = (NatNegotiationMappingScheme)BitConverter.ToInt32(ByteTools.SubBytes(recv, 19, sizeof(int)));
            GameName = Encoding.ASCII.GetString(ByteTools.SubBytes(recv, 23, recv.Length - 23));
        }

        public override byte[] GenerateResponse(NatPacketType packetType)
        {
            List<byte> data = new List<byte>();

            data.AddRange(base.GenerateResponse(packetType));

            data.Add(PortType);
            data.Add(ClientIndex);
            data.Add(NegResult);
            data.AddRange(BitConverter.GetBytes((int)NatType));
            data.AddRange(BitConverter.GetBytes((int)MappingScheme));
            data.AddRange(Encoding.ASCII.GetBytes(GameName));

            return data.ToArray();
        }
    }
}
