using Chat.Entity.Structure;
using Chat.Entity.Structure.ChatCommand;
using Chat.Entity.Structure.Enumerator.Request;
using Chat.Handler.SystemHandler.Encryption;
using Chat.Server;
using GameSpyLib.Common.Entity.Interface;
using GameSpyLib.Extensions;

namespace Chat.Handler.CommandHandler.CRYPT
{
    public class CRYPTHandler : ChatCommandHandlerBase
    {
        string _secretKey;
        Entity.Structure.ChatCommand.CRYPT _cryptCmd;
        public CRYPTHandler(IClient client, ChatCommandBase cmd, string response) : base(client, cmd, response)
        {
            _cryptCmd = (Entity.Structure.ChatCommand.CRYPT)_cmd;
        }

        public override void CheckRequest()
        {
            base.CheckRequest();

            // CRYPT des 1 gamename
            //_clientInfo.GameName = _recv[3];
            _session.ClientInfo.GameName = _cryptCmd.GameName;
        }

        public override void DataOperation()
        {
            base.DataOperation();
            if (!DataOperationExtensions.GetSecretKey(_session.ClientInfo.GameName, out _session.ClientInfo.GameSecretKey)
                || _session.ClientInfo.GameSecretKey == null)
            {
                _sendingBuffer = ChatCommandBase.BuildCommandString(ChatError.MoreParameters, $"{CommandName} :Secret key not found!");
                return;
            }
        }

        public override void ConstructResponse()
        {
            base.ConstructResponse();

            // 2. Prepare two keys
            ChatCrypt.Init(_session.ClientInfo.ClientCTX, ChatServer.ClientKey, _secretKey);
            ChatCrypt.Init(_session.ClientInfo.ServerCTX, ChatServer.ServerKey, _secretKey);

            // 3. Response the crypt command
            _sendingBuffer = ChatCommandBase.BuildCommandString(ChatResponse.SecureKey, "* " + ChatServer.ClientKey + " " + ChatServer.ServerKey);
        }

        public override void Response()
        {
            base.Response();
            //set use encryption flag to true
            _client.SendAsync(_sendingBuffer);
            _session.ClientInfo.UseEncryption = true;
            //we do not want send again so we make sendingbuffer as null;
            _sendingBuffer = "";
        }

        public override void SetCommandName()
        {
            CommandName = ChatRequest.CRYPT.ToString();
        }
    }
}
