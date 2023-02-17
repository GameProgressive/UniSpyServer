namespace UniSpy.Server.ServerBrowser.V2.Abstraction.BaseClass
{

    /// <summary>
    /// SB always need to response to client even there are no server or error occured
    /// </summary>
    public abstract class ResponseBase : UniSpy.Server.Core.Abstraction.BaseClass.ResponseBase
    {
        protected new RequestBase _request => (RequestBase)base._request;
        protected new ResultBase _result => (ResultBase)base._result;
        public new byte[] SendingBuffer { get => (byte[])base.SendingBuffer; set => base.SendingBuffer = value; }
        protected ResponseBase(RequestBase request, ResultBase result) : base(request, result)
        {
        }

    }
}
