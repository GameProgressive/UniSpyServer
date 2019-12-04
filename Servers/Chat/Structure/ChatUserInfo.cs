using System;
using System.Collections.Generic;
using System.Text;

namespace Chat.Structure
{
    public class ChatUserInfo
    {
        public string gameName = "";
        public string nickname = "";
        public string serverIP = "";
        public string username = "";

        // secure connection

        public GSPeerChatCTX ClientCTX = new GSPeerChatCTX();
        public GSPeerChatCTX ServerCTX = new GSPeerChatCTX();
        public bool encrypted = false;
    }
}
