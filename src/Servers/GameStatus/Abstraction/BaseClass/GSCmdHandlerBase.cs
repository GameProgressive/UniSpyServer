using System.Text;
using GameStatus.Entity.Enumerate;
using GameStatus.Entity.Structure.Misc;
using GameStatus.Entity.Structure.Response;
using GameStatus.Entity.Structure.Result;
using GameStatus.Handler.SystemHandler;
using GameStatus.Network;
using Serilog.Events;
using UniSpyLib.Abstraction.BaseClass;
using UniSpyLib.Abstraction.Interface;
using UniSpyLib.Encryption;
using UniSpyLib.Extensions;
using UniSpyLib.Logging;

namespace GameStatus.Abstraction.BaseClass
{
    /// <summary>
    /// we only use selfdefine error code here
    /// so we do not need to send it to client
    /// </summary>
    internal abstract class GSCmdHandlerBase : UniSpyCmdHandlerBase
    {
        protected new GSSession _session => (GSSession)base._session;
        protected new GSRequestBase _request => (GSRequestBase)base._request;
        protected new GSResultBase _result
        {
            get { return (GSResultBase)base._result; }
            set { base._result = value; }
        }
        protected GSCmdHandlerBase(IUniSpySession session, IUniSpyRequest request) : base(session, request)
        {
            _result = new GSDefaultResult();
        }

        public override void Handle()
        {
            RequestCheck();
            if (_result.ErrorCode != GSErrorCode.NoError)
            {
                ResponseConstruct();
                Response();
                return;
            }

            DataOperation();

            if (_result.ErrorCode == GSErrorCode.Database)
            {
                ResponseConstruct();
                Response();
                return;
            }

            ResponseConstruct();
            Response();
        }
        protected override void RequestCheck() { }
        protected override void ResponseConstruct()
        {
            _response = new GSDefaultResponse(_request, _result);
        }
    }
}
