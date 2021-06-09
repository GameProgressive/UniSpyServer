using QueryReport.Entity.Structure.Response;
using QueryReport.Entity.Structure.Result;
using QueryReport.Network;
using UniSpyLib.Abstraction.BaseClass;
using UniSpyLib.Abstraction.Interface;

namespace QueryReport.Abstraction.BaseClass
{
    internal abstract class QRCmdHandlerBase : UniSpyCmdHandlerBase
    {
        protected new QRRequestBase _request => (QRRequestBase)base._request;
        protected new QRSession _session => (QRSession)base._session;
        protected new QRResultBase _result
        {
            get => (QRResultBase)base._result;
            set => base._result = value;
        }
        protected QRCmdHandlerBase(IUniSpySession session, IUniSpyRequest request) : base(session, request)
        {
            _result = new QRDefaultResult();
        }

        public override void Handle()
        {
            RequestCheck();
            DataOperation();
            ResponseConstruct();
            Response();
        }

        protected override void RequestCheck() { }
        protected override void DataOperation() { }
        protected override void ResponseConstruct()
        {
            _response = new QRDefaultResponse(_request, _result);
        }
    }
}
