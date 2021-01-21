using Chat.Abstraction.BaseClass;
using Chat.Entity.Structure;
using Chat.Entity.Structure.Request;
using Chat.Entity.Structure.Response.General;
using Chat.Entity.Structure.Result;
using Chat.Handler.SystemHandler.Encryption;
using Chat.Network;
using Serilog.Events;
using UniSpyLib.Abstraction.Interface;
using UniSpyLib.Extensions;
using UniSpyLib.Logging;

namespace Chat.Handler.CmdHandler.General
{
    public class CRYPTHandler : ChatCmdHandlerBase
    {
        protected new CRYPTRequest _request
        {
            get { return (CRYPTRequest)base._request; }
        }
        protected new CRYPTResult _result
        {
            get { return (CRYPTResult)base._result; }
            set { base._result = value; }
        }
        // CRYPT des 1 gamename
        public CRYPTHandler(IUniSpySession session, IUniSpyRequest request) : base(session, request)
        {
        }

        protected override void DataOperation()
        {
            string secretKey;
            if (!DataOperationExtensions.GetSecretKey(_request.GameName, out secretKey)
                || secretKey == null)
            {
                LogWriter.ToLog(LogEventLevel.Error, "secret key not found!");
                _errorCode = ChatErrorCode.UnSupportedGame;
                _session.Disconnect();
                return;
            }
            _session.UserInfo.GameSecretKey = secretKey;
            _session.UserInfo.GameName = _request.GameName;
            // 2. Prepare two keys
            ChatCrypt.Init(_session.UserInfo.ClientCTX, ChatServer.ClientKey, _session.UserInfo.GameSecretKey);
            ChatCrypt.Init(_session.UserInfo.ServerCTX, ChatServer.ServerKey, _session.UserInfo.GameSecretKey);
            _result = new CRYPTResult(ChatServer.ClientKey, ChatServer.ServerKey);
            _response = new CRYPTResponse(_result);
        }

        protected override void BuildNormalResponse()
        {
            base.BuildNormalResponse();

            // 3. Response the crypt command
            _sendingBuffer = CRYPTResponse.BuildCryptReply(ChatServer.ClientKey, ChatServer.ServerKey);
        }

        protected override void Response()
        {
            base.Response();
            _session.UserInfo.IsUsingEncryption = true;
        }
    }
}
