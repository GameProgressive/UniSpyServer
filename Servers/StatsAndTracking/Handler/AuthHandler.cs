using GameSpyLib.Common;
using GameSpyLib.Extensions;
using System;
using System.Collections.Generic;
using System.Text;

namespace StatsAndTracking.Handler
{
    public class AuthHandler
    {
        public static void SendSessionKey(GStatsClient client, Dictionary<string, string> dict)
        {
            string sendingBuffer = string.Format(@"\sesskey\{0}",dict["response"]);
            sendingBuffer = Enctypex.XorEncoding(sendingBuffer,1);
            sendingBuffer += @"\final\";
            client.Stream.SendAsync(sendingBuffer);

        }
    }
}
