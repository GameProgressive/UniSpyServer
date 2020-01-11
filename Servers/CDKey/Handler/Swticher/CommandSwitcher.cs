using CDKey.Handler.CommandHandler.SKey;
using GameSpyLib.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;

namespace CDKey.Handler.CommandHandler
{
    public class CommandSwitcher
    {
        public static void Switch(CDKeyServer server, EndPoint endPoint, Dictionary<string, string> recv)
        {
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
                        server.UnknownDataRecived(recv);
                        break;
                }
            }
            catch (Exception e)
            {
                LogWriter.Log.WriteException(e);
            }
        }
    }
}
