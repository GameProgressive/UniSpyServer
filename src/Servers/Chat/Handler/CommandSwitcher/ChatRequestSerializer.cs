using Chat.Abstraction.BaseClass;
using UniSpyLib.Logging;
using Serilog.Events;
using System;
using System.Collections.Generic;
using System.Linq;
using UniSpyLib.Abstraction.Interface;

namespace Chat.Handler.CommandSwitcher
{
    public class ChatRequestSerializer
    {
        public static List<object> Serialize(string recv)
        {
            List<object> requestList = new List<object>();

            string[] rawRequests = recv.Replace("\r", "")
                .Split("\n", StringSplitOptions.RemoveEmptyEntries);
            // first we convert request into our ChatCommand class
            // next we handle each command
            foreach (var rawRequest in rawRequests)
            {
                ChatRequestBase generalRequest = new ChatRequestBase(rawRequest);
                if (!generalRequest.Parse())
                {
                    LogWriter.ToLog(LogEventLevel.Error, "Invalid request!");
                    continue;
                }

                Type requestType = AppDomain.CurrentDomain
                        .GetAssemblies()
                        .SelectMany(x => x.GetTypes())
                        .FirstOrDefault(t => t.Name == generalRequest.CmdName + "Request");
                if (requestType != null)
                {
                    var specificRequest = Activator.CreateInstance(requestType, generalRequest.RawRequest);
                    if (specificRequest == null)
                    {
                        LogWriter.ToLog(LogEventLevel.Error, $"Unknown request {generalRequest.CmdName}!");
                        continue;
                    }
                    if (!((ChatRequestBase)specificRequest).Parse())
                    {
                        LogWriter.ToLog(LogEventLevel.Error, "Invalid request!");
                        continue;
                    }
                    requestList.Add(specificRequest);
                }
                else
                {
                    LogWriter.ToLog(LogEventLevel.Error, $"Request: {generalRequest.CmdName} not implemented!");
                }
            }
            return requestList;
        }

    }
}
