using UniSpyServer.UniSpyLib.Abstraction.BaseClass;

namespace UniSpyServer.Servers.CDkey.Abstraction.BaseClass
{
    public abstract class ResponseBase : UniSpyResponseBase
    {
        protected new RequestBase _request => (RequestBase)base._request;
        protected new ResultBase _result => (ResultBase)base._result;
        public ResponseBase(UniSpyRequestBase request, UniSpyResultBase result) : base(request, result)
        {
        }
    }
}
