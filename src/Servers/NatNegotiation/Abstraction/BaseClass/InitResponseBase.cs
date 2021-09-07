using NatNegotiation.Entity.Structure.Request;
using UniSpyLib.Abstraction.BaseClass;

namespace NatNegotiation.Abstraction.BaseClass
{
    public abstract class InitResponseBase : ResponseBase
    {
        private new InitRequestBase _request => (InitRequestBase)base._request;
        private new InitResultBase _result => (InitResultBase)base._result;
        public InitResponseBase(UniSpyRequestBase request, UniSpyResultBase result) : base(request, result)
        {
        }
        public override void Build()
        {
            base.Build();
            List<byte> data = new List<byte>();
            data.AddRange(SendingBuffer);
            data.Add((byte)_request.PortType);
            data.Add(_request.ClientIndex);
            data.Add(_request.UseGamePort);
            data.AddRange(HtonsExtensions.IPStringToBytes(_result.LocalIP));
            data.AddRange(HtonsExtensions.UshortPortToBytes(_result.LocalPort));
            SendingBuffer = data.ToArray();
        }
    }
}