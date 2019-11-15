using GameSpyLib.Logging;
using GameSpyLib.Network;
using System.Collections.Generic;

namespace Chat
{
    public class ChatSession : TemplateTcpSession
    {
        public Dictionary<string, string> _recv;
        public ChatSession(TemplateTcpServer server) : base(server)
        {
        }

        protected override void OnReceived(string message)
        {
            //LogWriter.Log.Write("[CHAT] Recv " + message, LogLevel.Info);
            //Stream.SendAsync("PING capricorn.goes.here :123456");
            //ChatHandler.Crypt(this, message);
         
            if (message[message.Length - 1] == ' ')
            {
                message = message.Substring(0, message.Length - 2);
            }
            string[] request = message.Trim(' ').Split(' ');
            _recv = new Dictionary<string, string>();
            _recv.Add("command",request[0]);
            _recv.Add("enctype", request[1]);
            _recv.Add("value", request[2]);
            _recv.Add("gamename", request[3]);


            CommandSwitcher.Switch();
        }

    }
}

