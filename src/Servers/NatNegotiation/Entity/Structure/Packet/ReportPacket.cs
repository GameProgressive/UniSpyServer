using GameSpyLib.Extensions;
using NatNegotiation.Entity.Enumerate;
using System;
using System.Collections.Generic;
using System.Text;

namespace NatNegotiation.Entity.Structure.Packet
{
    public class ReportPacket : BasePacket
    {
        public byte PortType;
        public byte ClientIndex;
        public NatNegotiationResult NatResult;
        public NatNegotiationType NatType; //int
        public NatNegotiationMappingScheme MappingScheme; //int
        public string GameName;

        public override bool Parse(byte[] recv)
        {
            if (!base.Parse(recv))
            {
                return false;
            }
            PortType = recv[13];
            ClientIndex = recv[14];

            NatResult = (NatNegotiationResult)recv[15];

            NatType = (NatNegotiationType)BitConverter.ToInt32(ByteTools.SubBytes(recv, 17, sizeof(int)));
            MappingScheme = (NatNegotiationMappingScheme)BitConverter.ToInt32(ByteTools.SubBytes(recv, 19, sizeof(int)));
            GameName = Encoding.ASCII.GetString(ByteTools.SubBytes(recv, 23, recv.Length - 23));

            return true;
        }

        public override byte[] BuildResponse()
        {
            List<byte> data = new List<byte>();
            PacketType = NatPacketType.ReportAck;
            data.AddRange(base.BuildResponse());

            data.Add(PortType);
            data.Add(ClientIndex);
            data.Add((byte)NatResult);
            data.AddRange(BitConverter.GetBytes((int)NatType));
            data.AddRange(BitConverter.GetBytes((int)MappingScheme));
            data.AddRange(Encoding.ASCII.GetBytes(GameName));

            return data.ToArray();
        }
    }
}
