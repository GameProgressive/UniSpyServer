using Chat.Entity.Structure;
using Chat.Entity.Structure.ChatCommand;
using Chat.Entity.Structure.ChatResponse;
using Chat.Handler.SystemHandler.Encryption;
using Chat.Server;
using GameSpyLib.Common.Entity.Interface;
using GameSpyLib.Extensions;
using GameSpyLib.Logging;
using Serilog.Events;

namespace Chat.Handler.CommandHandler.ChatGeneralCommandHandler
{
    public class CRYPTHandler : ChatCommandHandlerBase
    {
        new CRYPTRequest _request;

        public CRYPTHandler(ISession session, ChatRequestBase request) : base(session, request)
        {
            _request = new CRYPTRequest(request.RawRequest);
        }

        protected override void CheckRequest()
        {
            base.CheckRequest();
            if (!_request.Parse())
            {
                _errorCode = ChatError.Parse;
                return;
            }
            // CRYPT des 1 gamename
            _session.UserInfo.SetGameName(_request.GameName);
        }

        protected override void DataOperation()
        {
            base.DataOperation();
            string secretKey;
            if (!DataOperationExtensions.GetSecretKey(_session.UserInfo.GameName, out secretKey)
                || secretKey == null)
            {
                LogWriter.ToLog(LogEventLevel.Error, "secret key not found!");
                return;
            }
            _session.UserInfo.SetGameSecretKey(secretKey);
        }

        protected override void ConstructResponse()
        {
            base.ConstructResponse();

            // 2. Prepare two keys
            ChatCrypt.Init(_session.UserInfo.ClientCTX, ChatServer.ClientKey, _session.UserInfo.GameSecretKey);
            ChatCrypt.Init(_session.UserInfo.ServerCTX, ChatServer.ServerKey, _session.UserInfo.GameSecretKey);

            // 3. Response the crypt command
            _sendingBuffer =
               ChatReply.
               BuildCryptReply(ChatServer.ClientKey, ChatServer.ServerKey);
        }

        protected override void Response()
        {
            //set use encryption flag to true
            _session.SendAsync(_sendingBuffer);
            _session.UserInfo.SetUseEncryptionFlag(true);
        }
    }
}
