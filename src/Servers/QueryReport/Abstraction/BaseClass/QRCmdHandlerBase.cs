using QueryReport.Entity.Enumerate;
using QueryReport.Network;
using UniSpyLib.Abstraction.BaseClass;
using UniSpyLib.Abstraction.Interface;
using UniSpyLib.Extensions;

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
        }

        public override void Handle()
        {
            RequestCheck();

            if (_result.ErrorCode != QRErrorCode.NoError)
            {
                ResponseConstruct();
                Response();
                return;
            }

            DataOperation();

            if (_result.ErrorCode == QRErrorCode.Database)
            {
                ResponseConstruct();
                Response();
                return;
            }

            ResponseConstruct();

            if (_result.ErrorCode != QRErrorCode.NoError)
            {
                ResponseConstruct();
                Response();
                return;
            }

            Response();
        }


        protected override void Response()
        {
            if (_response == null)
            {
                return;
            }
            _response.Build();
            if (!StringExtensions.CheckResponseValidation((byte[])_response.SendingBuffer))
            {
                return;
            }
            _session.Send((byte[])_response.SendingBuffer);
        }
    }
}
