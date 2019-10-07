using GameSpyLib.Common;
using GameSpyLib.Extensions;
using GameSpyLib.Network;
using PresenceSearchPlayer.Enumerator;
using System.Collections.Generic;
using System.Text;

namespace PresenceSearchPlayer
{
    public class GPSPSession : TcpSession
    {

        public GPSPSession(TcpServer server) : base(server)
        {
            DisconnectAfterSend = true;
        }

        protected override void OnReceived(byte[] buffer, long offset, long size)
        {
            if (size > 2048)
            {
                Server.ToLog("Client spam, ignored!");
                return;
            }

            string message = Encoding.UTF8.GetString(buffer, (int)offset, (int)size);
            message = RequstFormatConversion(message);
            if (message[0] != '\\')
            {
                GameSpyUtils.SendGPError(this, GPErrorCode.Parse, "An invalid request was sended.");
                return;
            }

            string[] commands = message.Split("\\final\\");

            foreach (string command in commands)
            {
                if (command.Length < 1)
                    continue;

                // Read client message, and parse it into key value pairs
                string[] recieved = command.TrimStart('\\').Split('\\');
                Dictionary<string, string> dict = GameSpyUtils.ConvertGPResponseToKeyValue(recieved);

                CommandSwitcher.Switch(this, dict);
            }
        }

        protected override void OnDisconnected()
        {
            
           Server.ToLog($"[Disc] Id [{Id}]");
        }
        protected override void OnConnected()
        {
            Server.ToLog($"[Conn] Id [{Id}]");
        }

        public string RequstFormatConversion(string message)
        {
            message = message.Replace(@"\-", @"\");
            message = message.Replace('-', '\\');

            int pos = message.IndexesOf("\\")[1];
            string temp = message.Substring(pos, 2);

            if (message.Substring(pos, 2) != "\\\\")
            {
                message = message.Insert(pos, "\\");
            }
            return message;
        }
    }
}
