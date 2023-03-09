namespace UniSpy.Server.Chat.Abstraction.BaseClass
{
    public abstract class ResponseBase : UniSpy.Server.Core.Abstraction.BaseClass.ResponseBase
    {
        public const string ServerDomain = "unispy.net";
        public new string SendingBuffer{ get => (string)base.SendingBuffer;
            protected set => base.SendingBuffer = value; }
        protected new ResultBase _result => (ResultBase)base._result;
        protected new RequestBase _request => (RequestBase)base._request;
        public ResponseBase(RequestBase request, ResultBase result) : base(request, result){ }
    }
}
