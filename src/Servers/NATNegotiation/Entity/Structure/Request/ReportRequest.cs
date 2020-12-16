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
        public NatPortType PortType;
        public byte ClientIndex;
        public NATNegotiationResult NatResult;
        public NATNegotiationType NatType;
        public NATNegotiationMappingScheme MappingScheme;
        public string GameName;

        public ReportRequest(byte[] rawRequest) : base(rawRequest)
        {
        }

        public override void Parse()
        {
            base.Parse();
            if (!ErrorCode)
            {
                return;
            }
            PortType = (NatPortType)RawRequest[13];
            ClientIndex = RawRequest[14];

            NatResult = (NATNegotiationResult)RawRequest[15];

            NatType = (NATNegotiationType)BitConverter.ToInt32(
                ByteTools.SubBytes(RawRequest, 17, sizeof(int)));
            MappingScheme = (NATNegotiationMappingScheme)BitConverter.ToInt32(
                ByteTools.SubBytes(RawRequest, 19, sizeof(int)));
            GameName = Encoding.ASCII.GetString(
                ByteTools.SubBytes(RawRequest, 23, RawRequest.Length - 23));

            ErrorCode = true;
        }

        public override byte[] BuildResponse()
        {
            List<byte> data = new List<byte>();
            CommandName = NatPacketType.ReportAck;
            data.AddRange(base.BuildResponse());

            data.Add((byte)PortType);
            data.Add(ClientIndex);
            data.Add((byte)NatResult);
            data.AddRange(BitConverter.GetBytes((int)NatType));
            data.AddRange(BitConverter.GetBytes((int)MappingScheme));
            data.AddRange(Encoding.ASCII.GetBytes(GameName));

            return data.ToArray();
        }
    }
}
