using UniSpyServer.UniSpyLib.Abstraction.BaseClass;

namespace UniSpyServer.ServerBrowser.Abstraction.BaseClass
{

    /// <summary>
    /// SB always need to response to client even there are no server or error occured
    /// </summary>
    public abstract class ResponseBase : UniSpyResponseBase
    {
        protected new RequestBase _request => (RequestBase)base._request;
        protected new ResultBase _result => (ResultBase)base._result;
        public new byte[] SendingBuffer
        {
            get => (byte[])base.SendingBuffer;
            set => base.SendingBuffer = value;
        }
        protected ResponseBase(UniSpyRequestBase request, UniSpyResultBase result) : base(request, result)
        {
        }

    }
}
