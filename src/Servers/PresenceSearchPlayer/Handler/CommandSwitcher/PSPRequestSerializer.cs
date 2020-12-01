using System;
using System.Collections.Generic;
using System.Linq;
using PresenceSearchPlayer.Entity.Enumerate;
using PresenceSearchPlayer.Entity.Structure;
using PresenceSearchPlayer.Entity.Structure.Request;
using PresenceSearchPlayer.Handler.CommandHandler.Error;
using Serilog.Events;
using UniSpyLib.Abstraction.Interface;
using UniSpyLib.Logging;
using UniSpyLib.MiscMethod;

namespace PresenceSearchPlayer.Handler.CommandSwitcher
{
    public class PSPRequestSerializer
    {
        public static List<IRequest> Serialize(ISession session, string rawRequest)
        {
            List<IRequest> requestList = new List<IRequest>();
            if (rawRequest[0] != '\\')
            {
                LogWriter.ToLog(LogEventLevel.Error, "Invalid request recieved!");
                return null;
            }
            string[] commands = rawRequest.Split("\\final\\", StringSplitOptions.RemoveEmptyEntries);
            foreach (var command in commands)
            {
                IRequest request = GenerateRequest(command);
                if (request == null)
                {
                    continue;
                }
                var flag = (GPErrorCode)request.Parse();
                if (flag != GPErrorCode.NoError)
                {
                    session.SendAsync(ErrorMsg.BuildGPErrorMsg(flag));
                    continue;
                }
                requestList.Add(request);
            }

            return requestList;
        }

        private static IRequest GenerateRequest(string rawCommand)
        {
            if (rawCommand.Length < 1)
            {
                return null;
            }
            // Read client message, and parse it into key value pairs
            string[] recieved = rawCommand.TrimStart('\\').Split('\\');
            Dictionary<string, string> keyValue = GameSpyUtils.ConvertToKeyValue(recieved);

            switch (keyValue.Keys.First())
            {
                case PSPRequestName.Search:
                    return new SearchRequest(keyValue);
                case PSPRequestName.Valid:
                    return new SearchRequest(keyValue);
                case PSPRequestName.Nicks:
                    return new SearchRequest(keyValue);
                case PSPRequestName.PMatch:
                case PSPRequestName.Check:
                    return new SearchRequest(keyValue);
                case PSPRequestName.NewUser:
                    return new SearchRequest(keyValue);
                case PSPRequestName.SearchUnique:
                    return new SearchRequest(keyValue);
                case PSPRequestName.Others:
                    return new SearchRequest(keyValue);
                case PSPRequestName.OtherList:
                    return new SearchRequest(keyValue);
                case PSPRequestName.UniqueSearch:
                    return new SearchRequest(keyValue);
                default:
                    LogWriter.UnknownDataRecieved(rawCommand);
                    return null;
            }
        }
    }
}
