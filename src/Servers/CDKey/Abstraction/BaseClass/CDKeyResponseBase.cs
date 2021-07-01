using UniSpyLib.Abstraction.BaseClass;

namespace CDKey.Abstraction.BaseClass
{
    internal abstract class CDKeyResponseBase : UniSpyResponse
    {
        protected new CDKeyRequestBase _request => (CDKeyRequestBase)base._request;
        protected new CDKeyResultBase _result => (CDKeyResultBase)base._result;
        public CDKeyResponseBase(UniSpyRequest request, UniSpyResult result) : base(request, result)
        {
        }
    }
}
