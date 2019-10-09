using GameSpyLib.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;

namespace CDKey
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
                    case "auth":
                    case "resp":
                    case "skey":
                        CDKeyHandler.IsCDKeyValid(server, endPoint, recv);
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
