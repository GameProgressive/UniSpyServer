using Chat.Entity.Structure.ChatCommand;
using Chat.Handler.CommandHandler;
using GameSpyLib.Common.BaseClass;
using GameSpyLib.Common.Entity.Interface;
using GameSpyLib.Logging;
using Serilog.Events;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Chat.Handler.CommandSwitcher
{
    /// <summary>
    /// Process request to Commands
    /// </summary>
    public class ChatCommandSwitcher : CommandSwitcherBase
    {
        public void Switch(ISession session, string recv)
        {
            #region Process request to our defined class
            List<ChatRequestBase> requestList = new List<ChatRequestBase>();

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
                    var specificRequest  = Activator.CreateInstance(requestType,generalRequest.RawRequest);
                    if (specificRequest == null)
                    {
                        LogWriter.ToLog(LogEventLevel.Error, $"Unknown request {generalRequest.CmdName}!");
                        return;
                    }
                    requestList.Add(generalRequest);
                }
                else
                {
                    LogWriter.ToLog(LogEventLevel.Error, $"Request: {generalRequest.CmdName} not implemented!");
                }
            }
            #endregion

            #region Handle specific request
            foreach (var request in requestList)
            {
                Type handlerType = AppDomain.CurrentDomain
                        .GetAssemblies()
                        .SelectMany(x => x.GetTypes())
                        .FirstOrDefault(t => t.Name == request.CmdName + "Handler");

                if (handlerType != null)
                {
                    object[] args = { session, request };
                    var handler = Activator.CreateInstance(handlerType, args);
                    if (handler != null)
                    {
                        ((ChatCommandHandlerBase)handler).Handle();
                    }
                    else
                    {
                        LogWriter.ToLog(LogEventLevel.Error, $"Unknown command {request.CmdName}!");
                    }
                }
                else
                {
                    LogWriter.ToLog(LogEventLevel.Error, $"{request.CmdName}Handler not implemented!");
                }
            }
            #endregion
        }
    }
}
