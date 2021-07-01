using System;
using UniSpyLib.Abstraction.BaseClass;
using UniSpyLib.Abstraction.BaseClass.Factory;
using UniSpyLib.Abstraction.Interface;

namespace NatNegotiation.Handler.CmdSwitcher
{
    internal sealed class NNCmdSwitcher : UniSpyCmdSwitcher
    {
        private new byte[] _rawRequest => (byte[])base._rawRequest;
        public NNCmdSwitcher(IUniSpySession session, object rawRequest) : base(session, rawRequest)
        {
        }

        protected override void DeserializeRequests()
        {
            var key = _rawRequest[7];
            var request = (IUniSpyRequest)Activator.CreateInstance(
                                    UniSpyServerFactory.RequestMapping[key],
                                    _rawRequest);
            request.Parse();
            _requests.Add(request);
        }
    }
}
