using UniSpyLib.Extensions;
using NATNegotiation.Abstraction.BaseClass;
using NATNegotiation.Entity.Enumerate;
using System;
using System.Collections.Generic;
using System.Text;

namespace NATNegotiation.Entity.Structure.Request
{
    public class ReportRequest : NNRequestBase
    {
        public byte PortType;
        public byte ClientIndex;
        public NATNegotiationResult NatResult;
        public NATNegotiationType NatType; //int
        public NATNegotiationMappingScheme MappingScheme; //int
        public string GameName;

        public ReportRequest(byte[] rawRequest) : base(rawRequest)
        {
        }

        public override bool Parse()
        {
            if (!base.Parse())
            {
                return false;
            }
            PortType = RawRequest[13];
            ClientIndex = RawRequest[14];

            NatResult = (NATNegotiationResult)RawRequest[15];

            NatType = (NATNegotiationType)BitConverter.ToInt32(
                ByteTools.SubBytes(RawRequest, 17, sizeof(int)));
            MappingScheme = (NATNegotiationMappingScheme)BitConverter.ToInt32(
                ByteTools.SubBytes(RawRequest, 19, sizeof(int)));
            GameName = Encoding.ASCII.GetString(
                ByteTools.SubBytes(RawRequest, 23, RawRequest.Length - 23));

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
