using System;
using System.Collections.Generic;
using System.Linq;
using CDKey.Entity.Structure;
using UniSpyLib.Abstraction.Interface;
using UniSpyLib.Logging;
using UniSpyLib.MiscMethod;

namespace CDKey.Handler.CommandSwitcher
{
    public class CDKeyRequestSerializer
    {
        public static List<IRequest> Serialize(ISession session, string rawRequest)
        {
            List<IRequest> requests = new List<IRequest>();
            var request = rawRequest;
            //request.Replace(@"\r\n", "").Replace("\0", "");
            string[] commands = rawRequest.Split(@"\r\n", StringSplitOptions.RemoveEmptyEntries);
            foreach (var command in commands)
            {
                requests.Add(GenerateRequest(command));
            }
            return requests;
        }

        private static IRequest GenerateRequest(string command)
        {
            var kv = GameSpyUtils.ConvertToKeyValue(command);

            switch (kv.Keys.First())
            {
                //keep client alive request, we skip this
                case RequestName.KA:
                    return null;
                case RequestName.Auth:
                    return null;
                case RequestName.Resp:
                    return null;
                case RequestName.SKey:
                    return null;
                case RequestName.Disc://disconnect from server
                    return null;
                case RequestName.UON:
                    return null;
                default:
                    LogWriter.UnknownDataRecieved(command);
                    return null;
            }
        }

    }
}
