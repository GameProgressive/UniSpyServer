using Chat.Application;
using Chat.Entity.Structure;
using Chat.Handler.SystemHandler.Encryption;

namespace Chat.Handler.CommandHandler.CRYPT
{
    public class CRYPTHandler:CommandHandlerBase
    {
        string _secretKey;
        public CRYPTHandler(ChatSession session, string[] recv) : base(session, recv)
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
            _secretKey = CRYPTQuery.GetSecretKeyFromGame(recv[3]);

            if (_secretKey == null)
            {
                _sendingBuffer = ChatServer.GenerateChatCommand(ChatError.MoreParameters, "CRYPT :Secret key not found!");
                return;
            }

        }

        public override void ConstructResponse(ChatSession session, string[] recv)
        {
            base.ConstructResponse(session, recv);

            string Info =
                $"{ServerManager.ServerName} Elevating security for user " +
                $"{session.Socket.RemoteEndPoint} " +
                $"with game {session.UserInfo.GameName}";

            session.ToLog(Serilog.Events.LogEventLevel.Information, Info);

            // 2. Prepare two keys
            ChatCrypt.Init(session.UserInfo.ClientCTX, ChatServer.ClientKey, _secretKey);
            ChatCrypt.Init(session.UserInfo.ServerCTX, ChatServer.ServerKey, _secretKey);

            // 3. Response the crypt command
            _sendingBuffer = ChatServer.GenerateChatCommand(ChatRPL.SecureKey, $"* {ChatServer.ClientKey} { ChatServer.ServerKey}");

            //set use encryption flag to true
            session.UserInfo.useEncryption = true;
        }

    }
}
