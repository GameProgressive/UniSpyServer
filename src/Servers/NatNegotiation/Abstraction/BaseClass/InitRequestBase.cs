using NatNegotiation.Abstraction.BaseClass;
using NatNegotiation.Entity.Enumerate;
using UniSpyLib.Extensions;

namespace NatNegotiation.Entity.Structure.Request
{
    internal abstract class InitRequestBase : RequestBase
    {
        public static new readonly int Size = RequestBase.Size + 9;

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

            PortType = (NatPortType)RawRequest[RequestBase.Size];//
            ClientIndex = RawRequest[RequestBase.Size + 1];//00
            UseGamePort = RawRequest[RequestBase.Size + 2];//00

            LocalIP = HtonsExtensions.
                BytesToIPString(
                ByteTools.SubBytes(RawRequest, RequestBase.Size + 3, 4));

            LocalPort = HtonsExtensions.
                BytesToUshortPort(
                ByteTools.SubBytes(RawRequest, RequestBase.Size + 7, 2));
        }
    }
}
