using Chat.Entity.Structure;
using Chat.Entity.Structure.ChatCommand;
using Chat.Handler.SystemHandler.Encryption;
using Chat.Server;
using GameSpyLib.Common.Entity.Interface;
using GameSpyLib.Extensions;
using GameSpyLib.Logging;

namespace Chat.Handler.CommandHandler
{
    public class CRYPTHandler : ChatCommandHandlerBase
    {
        CRYPT _cryptCmd;

        public CRYPTHandler(IClient client, ChatCommandBase cmd) : base(client, cmd)
        {
            _cryptCmd = (CRYPT)_cmd;
        }

        public override void CheckRequest()
        {
            base.CheckRequest();

            // CRYPT des 1 gamename
            //_clientInfo.GameName = _recv[3];
            _session.UserInfo.GameName = _cryptCmd.GameName;
        }

        public override void DataOperation()
        {
            base.DataOperation();
            string secretKey;
            if (!DataOperationExtensions.GetSecretKey(_session.UserInfo.GameName, out secretKey)
                || secretKey== null)
            {
                LogWriter.ToLog(Serilog.Events.LogEventLevel.Error, "secret key not found!");
                return;
            }
            _session.UserInfo.GameSecretKey = secretKey;
        }

        public override void ConstructResponse()
        {
            base.ConstructResponse();

            // 2. Prepare two keys
            ChatCrypt.Init(_session.UserInfo.ClientCTX, ChatServer.ClientKey, _session.UserInfo.GameSecretKey);
            ChatCrypt.Init(_session.UserInfo.ServerCTX, ChatServer.ServerKey, _session.UserInfo.GameSecretKey);

            // 3. Response the crypt command
            _sendingBuffer = _cryptCmd.BuildResponse(ChatServer.ClientKey, ChatServer.ServerKey);
        }

        public override void Response()
        {
            //set use encryption flag to true
            _client.SendAsync(_sendingBuffer);
            _session.UserInfo.UseEncryption = true;
        }
    }
}
