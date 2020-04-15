using CDKey.Handler.CommandHandler.SKey;
using GameSpyLib.Logging;
using GameSpyLib.MiscMethod;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;

namespace CDKey.Handler.CommandSwitcher
{
    public class CommandSwitcher
    {
        public static void Switch(CDKeyServer server, EndPoint endPoint, string message)
        {
            message.Replace(@"\r\n", "").Replace("\0", "");
            string[] keyValueArray = message.TrimStart('\\').Split('\\');
            Dictionary<string, string> recv = GameSpyUtils.ConvertRequestToKeyValue(keyValueArray);

            try
            {
                switch (recv.Keys.First())
                {
                    //keep client alive request, we skip this
                    case "ka":
                        Console.WriteLine("Received keep alive command");
                        break;
                    case "auth":
                        break;
                    case "resp":
                        break;
                    case "skey":
                        SKeyHandler.IsCDKeyValid(server, endPoint, recv);
                        break;
                    case "disc"://disconnect from server
                        break;
                    case "uon":
                        break;

                    default:
                        LogWriter.UnknownDataRecieved(message);
                        break;
                }
            }
            catch (Exception e)
            {
                LogWriter.ToLog(e);
            }
        }
    }
}
