namespace UniSpy.Server.Chat.Abstraction.BaseClass
{
    public abstract class ResponseBase : UniSpy.Server.Core.Abstraction.BaseClass.ResponseBase
    {
        public new string SendingBuffer{ get => (string)base.SendingBuffer;
            protected set => base.SendingBuffer = value; }
        protected new ResultBase _result => (ResultBase)base._result;
        protected new RequestBase _request => (RequestBase)base._request;
        protected ResponseBase(UniSpy.Server.Core.Abstraction.BaseClass.RequestBase request, UniSpy.Server.Core.Abstraction.BaseClass.ResultBase result) : base(request, result){ }
    }
}
