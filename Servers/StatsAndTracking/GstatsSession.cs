using System;
using System.Collections.Generic;
using System.Text;
using GameSpyLib.Common;
using GameSpyLib.Extensions;
using GameSpyLib.Network;
using GameSpyLib.Logging;

namespace StatsAndTracking
{
    public class GstatsSession : TemplateTcpSession
    {
        public GstatsSession(TemplateTcpServer server) : base(server)
        {
            DisconnectAfterSend = true;
        }

        protected override void OnReceived(string message)
        {
            message = message.Replace(@"\final\", "");
            string decodedmsg = Enctypex.XorEncoding(message, 1);
            if (decodedmsg[0] != '\\')
            {
                return;
            }
            string[] recieved = decodedmsg.TrimStart('\\').Split('\\');
            Dictionary<string, string> dict = GameSpyUtils.ConvertGPResponseToKeyValue(recieved);

            CommandSwitcher.Switch(this, dict);
        }

        public override bool SendAsync(string text)
        {
            text = Enctypex.XorEncoding(text, 1);
            text += @"\final\";
            return base.SendAsync(text);
        }

    }
}
