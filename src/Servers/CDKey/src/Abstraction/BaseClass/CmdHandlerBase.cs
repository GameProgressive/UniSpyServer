using UniSpy.Server.Core.Abstraction.Interface;

namespace UniSpy.Server.CDKey.Abstraction.BaseClass
{
    public abstract class CmdHandlerBase : UniSpy.Server.Core.Abstraction.BaseClass.CmdHandlerBase
    {
        protected new RequestBase _request => (RequestBase)base._request;
        protected new ResponseBase _response => (ResponseBase)base._response;
        protected new ResultBase _result { get => (ResultBase)base._result; set => base._result = value; }
        public CmdHandlerBase(IClient client, IRequest request) : base(client, request)
        {
        }
    }
}
