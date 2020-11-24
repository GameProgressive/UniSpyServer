using UniSpyLib.Abstraction.Interface;
using UniSpyLib.Logging;


namespace CDKey.Handler.CommandSwitcher
{
    public class CDKeyCommandSwitcher
    {
        public static void Switch(ISession session, string rawRequest)
        {
            var requests = CDKeyRequestSerializer.Serialize(session, rawRequest);

            foreach (var request in requests)
            {
                switch (request.CommandName)
                {
                    //keep client alive request, we skip this
                    case "ka":
                        break;
                    case "auth":
                        break;
                    case "resp":
                        break;
                    case "skey":
                        break;
                    case "disc"://disconnect from server
                        break;
                    case "uon":
                        break;
                    default:
                        LogWriter.UnknownDataRecieved(rawRequest);
                        break;
                }
            }
        }
    }
}
