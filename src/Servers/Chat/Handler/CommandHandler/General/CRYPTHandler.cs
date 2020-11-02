using Chat.Abstraction.BaseClass;
using Chat.Entity.Structure;
using Chat.Entity.Structure.ChatCommand;
using Chat.Entity.Structure.Response.General;
using Chat.Handler.SystemHandler.Encryption;
using Chat.Server;
using UniSpyLib.Abstraction.Interface;
using UniSpyLib.Extensions;
using UniSpyLib.Logging;
using Serilog.Events;

namespace Chat.Handler.CommandHandler.ChatGeneralCommandHandler
{
    public class CRYPTHandler : ChatCommandHandlerBase
    {
        new CRYPTRequest _request;
        // CRYPT des 1 gamename
        public CRYPTHandler(ISession session, ChatRequestBase request) : base(session, request)
        {
            _request = (CRYPTRequest)request;
        }

        protected override void DataOperation()
        {
            base.DataOperation();

            string secretKey;
            if (!DataOperationExtensions.GetSecretKey(_request.GameName, out secretKey)
                || secretKey == null)
            {
                LogWriter.ToLog(LogEventLevel.Error, "secret key not found!");
                _errorCode = ChatError.UnSupportedGame;
                return;
            }
            _session.UserInfo.SetGameSecretKey(secretKey);
            _session.UserInfo.SetGameName(_request.GameName);
            // 2. Prepare two keys
            ChatCrypt.Init(_session.UserInfo.ClientCTX, ChatServer.ClientKey, _session.UserInfo.GameSecretKey);
            ChatCrypt.Init(_session.UserInfo.ServerCTX, ChatServer.ServerKey, _session.UserInfo.GameSecretKey);

        }

        protected override void BuildNormalResponse()
        {
            base.BuildNormalResponse();

            // 3. Response the crypt command
            _sendingBuffer = CRYPTReply.BuildCryptReply(ChatServer.ClientKey, ChatServer.ServerKey);
        }

        protected override void Response()
        {
            base.Response();
            _session.UserInfo.SetUseEncryptionFlag(true);
        }
    }
}
