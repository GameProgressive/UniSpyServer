using GameSpyLib.Common;
using GameSpyLib.Network;
using PresenceSearchPlayer.Enumerator;
using PresenceSearchPlayer.Handler.CommandHandler;
using System.Collections.Generic;

namespace PresenceSearchPlayer
{
    public class GPSPSession : TemplateTcpSession
    {
        public uint OperationID;
        public GPSPSession(GPSPServer server) : base(server)
        {
        }

        protected override void OnReceived(string message)
        {
            if (message[0] != '\\')
            {
                GameSpyUtils.SendGPError(this, GPErrorCode.Parse, "An invalid request was sended.");
                return;
            }
            message =@"\newuser\\email\fridij2007@icloud.com\nick\FridiNaTor\passwordenc\VZ6bkUCsg95I\productid\10618\gamename\fsw10hpc\namespaceid\0\uniquenick\\cdkeyenc\RrTCqwaRq]88Ii7eauCVR[leSA__\id\1\final\";

             string[] commands = message.Split("\\final\\",System.StringSplitOptions.RemoveEmptyEntries);

            foreach (string command in commands)
            {
                if (command.Length < 1)
                    continue;

                // Read client message, and parse it into key value pairs
                string[] recieved = command.TrimStart('\\').Split('\\');
                Dictionary<string, string> dict = GameSpyUtils.ConvertRequestToKeyValue(recieved);

                CommandSwitcher.Switch(this, dict);
            }
        }

    }
}
