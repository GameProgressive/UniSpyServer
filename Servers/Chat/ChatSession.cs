using GameSpyLib.Logging;
using GameSpyLib.Network;
using System.Collections.Generic;

namespace Chat
{
    public class ChatSession : TemplateTcpSession
    {
        public Dictionary<string, string> _recv;
        public bool IsEncrypt = false;
        public ChatSession(TemplateTcpServer server) : base(server)
        {
        }

        protected override void OnReceived(string data)
        {
            string[] messages = data.Split("\r\n");

            foreach (string message in messages)
            {
                if (message.Length < 1)
                    continue;

                string[] request = message.Trim(' ').Split(' ');
                CommandSwitcher.Switch(this, request);
            }
        }

    }
}

