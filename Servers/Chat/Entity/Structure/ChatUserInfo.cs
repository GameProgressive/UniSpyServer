using System;
using System.Collections.Generic;
using System.Text;

namespace Chat.Entity.Structure
{
    public class ChatUserInfo
    {
        public string gameName = "";
        public string nickname = "";
        public string serverIP = "";
        public string username = "";
        public int namespaceID = 0;
        public string uniqueNickname = "";

        // secure connection

        public GSPeerChatCTX ClientCTX = new GSPeerChatCTX();
        public GSPeerChatCTX ServerCTX = new GSPeerChatCTX();
        public bool encrypted = false;
    }
}
