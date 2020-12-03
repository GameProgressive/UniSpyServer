using UniSpyLib.Extensions;
using NATNegotiation.Abstraction.BaseClass;
using NATNegotiation.Entity.Enumerate;
using System.Collections.Generic;
using System.Net;

namespace NATNegotiation.Entity.Structure.Request
{
    public class InitRequest : NNRequestBase
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



        public override object Parse()
        {
            if (!(bool)base.Parse())
            {
                return false;
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

            return true;
        }

        public override byte[] BuildResponse()
        {

            List<byte> data = new List<byte>();

            data.AddRange(base.BuildResponse());

            data.Add((byte)PortType);
            data.Add(ClientIndex);
            data.Add(UseGamePort);

            data.AddRange(HtonsExtensions.IPStringToBytes(LocalIP));
            data.AddRange(HtonsExtensions.UshortPortToBytes(LocalPort));

            return data.ToArray();
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
