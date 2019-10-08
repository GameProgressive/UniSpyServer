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

        protected override void OnReceived(byte[] buffer, long offset, long size)
        {
            if (size > 2048)
            {
                LogInfo("Client spam, ignored!");
                return;
            }

            string message = Encoding.UTF8.GetString(buffer, (int)offset, (int)size);
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
        protected override void OnDisconnected()
        {

            LogInfo($"Id [{Id}] disconnected!");
        }
        protected override void OnConnected()
        {
            LogInfo($"Id [{Id}] connected!");
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

        protected void LogInfo(string message)
        {
            LogWriter.Log.Write(LogLevel.Info, "{0} {1}", ServerName, message);
        }

    }
}
