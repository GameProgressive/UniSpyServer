using UniSpyLib.Abstraction.BaseClass;

namespace PresenceSearchPlayer.Abstraction.BaseClass
{
    public abstract class PSPResponseBase : UniSpyResponse
    {
        protected new PSPResultBase _result
        {
            get => (PSPResultBase)base._result;
        }
        protected new PSPRequestBase _request
        {
            get => (PSPRequestBase)base._request;
        }

        public new string SendingBuffer
        {
            get => (string)base.SendingBuffer;
            protected set => base.SendingBuffer = value;
        }
        protected PSPResponseBase(PSPRequestBase request, UniSpyResult result) : base(request, result)
        {
        }
    }
}
