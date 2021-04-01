using Chat.Abstraction.BaseClass;
using Chat.Entity.Structure;
using Chat.Entity.Structure.Misc;
using Chat.Entity.Structure.Request.General;
using Chat.Entity.Structure.Response.General;
using Chat.Entity.Structure.Result.General;
using Chat.Network;
using Serilog.Events;
using UniSpyLib.Abstraction.Interface;
using UniSpyLib.Extensions;
using UniSpyLib.Logging;

namespace Chat.Handler.CmdHandler.General
{
    internal sealed class CRYPTHandler : ChatCmdHandlerBase
    {
        private new CRYPTRequest _request
        {
            get { return (CRYPTRequest)base._request; }
        }
        private new CRYPTResult _result
        {
            get { return (CRYPTResult)base._result; }
            set { base._result = value; }
        }
        // CRYPT des 1 gamename
        public CRYPTHandler(IUniSpySession session, IUniSpyRequest request) : base(session, request)
        {
            _result = new CRYPTResult();
        }

        protected override void DataOperation()
        {
            string secretKey = DataOperationExtensions.GetSecretKey(_request.GameName);
            if (secretKey == null)
            {
                LogWriter.ToLog(LogEventLevel.Error, "secret key not found!");
                _result.ErrorCode = ChatErrorCode.UnSupportedGame;
                _session.Disconnect();
                return;
            }
            _session.UserInfo.GameSecretKey = secretKey;
            _session.UserInfo.GameName = _request.GameName;
            // 2. Prepare two keys
            ChatCrypt.Init(_session.UserInfo.ClientCTX, ChatConstants.ClientKey, _session.UserInfo.GameSecretKey);
            ChatCrypt.Init(_session.UserInfo.ServerCTX, ChatConstants.ServerKey, _session.UserInfo.GameSecretKey);
        }
        protected override void ResponseConstruct()
        {
            _response = new CRYPTResponse(_request, _result);
        }
        protected override void Response()
        {
            base.Response();
            _session.UserInfo.IsUsingEncryption = true;
        }
    }
}
