using UniSpyLib.Abstraction.BaseClass;

namespace ServerBrowser.Abstraction.BaseClass
{

    /// <summary>
    /// SB always need to response to client even there are no server or error occured
    /// </summary>
    internal abstract class SBResponseBase : UniSpyResponseBase
    {
        protected new RequestBase _request => (RequestBase)base._request;
        protected new SBResultBase _result => (SBResultBase)base._result;
        public new byte[] SendingBuffer
        {
            get => (byte[])base.SendingBuffer;
            set => base.SendingBuffer = value;
        }
        protected SBResponseBase(UniSpyRequestBase request, UniSpyResultBase result) : base(request, result)
        {
        }

    }
}
