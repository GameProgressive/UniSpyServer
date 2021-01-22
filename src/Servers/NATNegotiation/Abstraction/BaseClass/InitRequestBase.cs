using NATNegotiation.Abstraction.BaseClass;
using NATNegotiation.Entity.Enumerate;
using System.Net;
using UniSpyLib.Extensions;

namespace NATNegotiation.Entity.Structure.Request
{
    internal abstract class InitRequestBase : NNRequestBase
    {
        public static new readonly int Size = NNRequestBase.Size + 9;

        public NatPortType PortType { get; protected set; }
        public byte ClientIndex { get; protected set; }
        public byte UseGamePort { get; protected set; }
        public string LocalIP { get; protected set; }
        public ushort LocalPort { get; protected set; }

        public InitRequestBase(byte[] rawRequest) : base(rawRequest)
        {
        }



        public override void Parse()
        {
            base.Parse();
            if (ErrorCode != NNErrorCode.NoError)
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
        }
    }
}
