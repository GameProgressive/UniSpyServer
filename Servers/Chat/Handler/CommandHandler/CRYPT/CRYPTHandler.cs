using Chat.Entity.Structure;
using Chat.Handler.SystemHandler.Encryption;
using GameSpyLib.Common.Entity.Interface;
using GameSpyLib.Extensions;

namespace Chat.Handler.CommandHandler.CRYPT
{
    public class CRYPTHandler : CommandHandlerBase
    {
        string _secretKey;

        public CRYPTHandler(IClient client, string[] recv) : base(client, recv)
        {
        }

        public override void CheckRequest()
        {
            base.CheckRequest();
            // CRYPT des 1 gamename
            _session.UserInfo.GameName = _recv[3];
        }

        public override void DataOperation()
        {
            base.DataOperation();
            if (!DataOperationExtensions.GetSecretKey(_recv[3], out _secretKey) || _secretKey == null)
            {
                _sendingBuffer = ChatServer.GenerateChatCommand(ChatError.MoreParameters, "CRYPT :Secret key not found!");
                return;
            }
        }

        public override void ConstructResponse()
        {
            base.ConstructResponse();

            // 2. Prepare two keys
            ChatCrypt.Init(_session.UserInfo.ClientCTX, ChatServer.ClientKey, _secretKey);
            ChatCrypt.Init(_session.UserInfo.ServerCTX, ChatServer.ServerKey, _secretKey);

            // 3. Response the crypt command
            _sendingBuffer = ChatServer.GenerateChatCommand(ChatRPL.SecureKey, "* "+ChatServer.ClientKey +" "+ ChatServer.ServerKey);
        }

        public override void Response()
        {
            base.Response();
            //set use encryption flag to true
            _session.UserInfo.useEncryption = true;
        }
    }
}
