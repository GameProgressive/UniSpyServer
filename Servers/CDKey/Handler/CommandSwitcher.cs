using CDKey.Handler.SKey;
using GameSpyLib.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;

namespace CDKey.Handler
{
    public class CommandSwitcher
    {
        public static void Switch(CDKeyServer server, EndPoint endPoint, Dictionary<string, string> recv)
        {
            try
            {
                switch (recv.Values.First())
                {
                    //keep client alive request, we skip this
                    case "ka":
                        break;
                    case "auth":
                        break;
                    case "resp":
                        break;
                    case "skey":
                        SKeyHandler.IsCDKeyValid(server, endPoint, recv);
                        break;
                    case "disc":
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
