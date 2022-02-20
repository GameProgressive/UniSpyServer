namespace UniSpyServer.Servers.Chat.Abstraction.BaseClass
{
    public abstract class ResponseBase : UniSpyLib.Abstraction.BaseClass.ResponseBase
    {
        public new string SendingBuffer{ get => (string)base.SendingBuffer;
            protected set => base.SendingBuffer = value; }
        protected new ResultBase _result => (ResultBase)base._result;
        protected new RequestBase _request => (RequestBase)base._request;
        protected ResponseBase(UniSpyLib.Abstraction.BaseClass.RequestBase request, UniSpyLib.Abstraction.BaseClass.ResultBase result) : base(request, result)
        {
        }
    }
}
