using System.Collections.Generic;
using UniSpyLib.Abstraction.Interface;

namespace ServerBrowser.Handler.CommandSwitcher
{
    public class SBRequestSerializer
    {
        public static List<IRequest> Serialize(ISession session, byte[] rawRequest)
        {
            List<IRequest> requests = new List<IRequest>();

            return requests;
        }

        public static IRequest GenerateRequest(byte[] rawRequest)
        {


            return null;
        }
    }
}
