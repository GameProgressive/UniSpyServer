namespace Chat.Entity.Structure
{
    public class ChatUserInfo
    {
        public string GameName = "";
        public string NickName = "";
        public string ServerIP = "";
        public string UserName = "";
        public int NameSpaceID = 0;
        public string UniqueNickName = "";

        // secure connection

        public GSPeerChatCTX ClientCTX = new GSPeerChatCTX();
        public GSPeerChatCTX ServerCTX = new GSPeerChatCTX();
        public bool useEncryption = false;
    }
}
