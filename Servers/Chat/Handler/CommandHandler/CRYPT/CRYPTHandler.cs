using Chat.Application;
using Chat.Entity.Structure;
using Chat.Handler.SystemHandler.Encryption;
using GameSpyLib.Common;
using GameSpyLib.Extensions;

namespace Chat.Handler.CommandHandler.CRYPT
{
    public class CRYPTHandler : CommandHandlerBase
    {
        string _secretKey;
        public CRYPTHandler() : base()
        {
        }

        public override void CheckRequest(ChatSession session, string[] recv)
        {
            base.CheckRequest(session, recv);
            // CRYPT des 1 gamename
            session.UserInfo.GameName = recv[3];

        }

        public override void DataOperation(ChatSession session, string[] recv)
        {
            base.DataOperation(session, recv);
            if (DataOperationExtensions.GetSecretKey(recv[3], out _secretKey) || _secretKey == null)
            {
                _sendingBuffer = ChatServer.GenerateChatCommand(ChatError.MoreParameters, "CRYPT :Secret key not found!");
                return;
            }
        }

        public override void ConstructResponse(ChatSession session, string[] recv)
        {
            base.ConstructResponse(session, recv);

            // 2. Prepare two keys
            ChatCrypt.Init(session.UserInfo.ClientCTX, ChatServer.ClientKey, _secretKey);
            ChatCrypt.Init(session.UserInfo.ServerCTX, ChatServer.ServerKey, _secretKey);

            // 3. Response the crypt command
            _sendingBuffer = ChatServer.GenerateChatCommand(ChatRPL.SecureKey, "* "+ChatServer.ClientKey +" "+ ChatServer.ServerKey);
        }

        public override void Response(ChatSession session, string[] recv)
        {
            base.Response(session, recv);
            //set use encryption flag to true
            session.UserInfo.useEncryption = true;
        }
    }
}
