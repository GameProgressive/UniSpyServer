using UniSpyLib.Extensions;
using NATNegotiation.Abstraction.BaseClass;
using NATNegotiation.Entity.Enumerate;
using System.Collections.Generic;
using System.Net;

namespace NATNegotiation.Entity.Structure.Request
{
    internal class InitRequest : NNRequestBase
    {
        public static new readonly int Size = NNRequestBase.Size + 9;

        public NatPortType PortType { get; protected set; }
        public byte ClientIndex { get; protected set; }
        public byte UseGamePort { get; protected set; }
        public string LocalIP { get; protected set; }
        public ushort LocalPort { get; protected set; }

        public InitRequest(byte[] rawRequest) : base(rawRequest)
        {
        }



        public override void Parse()
        {
            base.Parse();
            if (!ErrorCode)
            {
                return;
            }
            PortType = (NatPortType)RawRequest[NNRequestBase.Size];//
            ClientIndex = RawRequest[NNRequestBase.Size + 1];//00
            UseGamePort = RawRequest[NNRequestBase.Size + 2];//00

            LocalIP = HtonsExtensions.
                BytesToIPString(
                ByteTools.SubBytes(RawRequest, NNRequestBase.Size + 3, 4));

            LocalPort = HtonsExtensions.
                BytesToUshortPort(
                ByteTools.SubBytes(RawRequest, NNRequestBase.Size + 7, 2));

            ErrorCode = true;
        }

        public InitRequest SetIPAndPortForResponse(EndPoint end)
        {
            LocalIP = ((IPEndPoint)end).Address.ToString();
            LocalPort = (ushort)((IPEndPoint)end).Port;
            return this;
        }

        public InitRequest SetPortType(NatPortType type)
        {
            PortType = type;
            return this;
        }
    }
}
