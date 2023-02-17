namespace UniSpy.Server.PresenceConnectionManager.Abstraction.BaseClass
{
    public abstract class ResponseBase : UniSpy.Server.Core.Abstraction.BaseClass.ResponseBase
    {
        protected new ResultBase _result => (ResultBase)base._result;
        protected new RequestBase _request => (RequestBase)base._request;
        public new string SendingBuffer{ get => (string)base.SendingBuffer;
            protected set => base.SendingBuffer = value; }

        protected ResponseBase(UniSpy.Server.Core.Abstraction.BaseClass.RequestBase request, UniSpy.Server.Core.Abstraction.BaseClass.ResultBase result) : base(request, result)
        {
        }
    }
}
