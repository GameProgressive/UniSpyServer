namespace UniSpy.Server.QueryReport.V1.Abstraction.BaseClass
{
    public abstract class ResponseBase : UniSpy.Server.Core.Abstraction.BaseClass.ResponseBase
    {
        public new string SendingBuffer { get => (string)base.SendingBuffer; protected set => base.SendingBuffer = value; }
        public ResponseBase(RequestBase request, ResultBase result) : base(request, result)
        {
        }
    }
}