namespace UniSpy.Server.PresenceConnectionManager.Abstraction.BaseClass
{
    public abstract class ResponseBase : UniSpy.Server.Core.Abstraction.BaseClass.ResponseBase
    {
        protected new ResultBase _result => (ResultBase)base._result;
        protected new RequestBase _request => (RequestBase)base._request;
        public new string SendingBuffer{ get => (string)base.SendingBuffer;
            protected set => base.SendingBuffer = value; }

        public ResponseBase(RequestBase request, ResultBase result) : base(request, result)
        {
        }
    }
}
