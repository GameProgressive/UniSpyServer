using GameSpyLib.Common;
using GameSpyLib.Extensions;
using GameSpyLib.Logging;
using GameSpyLib.Network;
using PresenceSearchPlayer.Enumerator;
using System.Collections.Generic;
using System.Text;

namespace PresenceSearchPlayer
{
    public class GPSPSession : TemplateTcpSession
    {
        public GPSPSession(GPSPServer server) : base(server)
        {
            DisconnectAfterSend = true;
        }

        protected override void OnReceived(byte[] buffer, long offset, long size)
        {
            if (size > 2048)
            {
                ToLog("[Spam] client spam we ignored!");
                return;
            }

            base.OnReceived(buffer, offset, size);

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

    }
}
