using Chat.Abstraction.BaseClass;
using UniSpyLib.Abstraction.Interface;
using UniSpyLib.Logging;
using Serilog.Events;
using System;
using System.Linq;

namespace Chat.Handler.CommandSwitcher
{
    /// <summary>
    /// Process request to Commands
    /// </summary>
    public class ChatCommandSwitcher
    {
        public static void Switch(ISession session, string recv)
        {
            var requestList = ChatRequestSerializer.Serialize(recv);

            #region Handle specific request
            foreach (var request in requestList)
            {
                string cmdName = ((ChatRequestBase)request).CmdName;
                Type handlerType = AppDomain.CurrentDomain
                        .GetAssemblies()
                        .SelectMany(x => x.GetTypes())
                        .FirstOrDefault(t => t.Name == cmdName + "Handler");

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
                        LogWriter.ToLog(LogEventLevel.Error, $"Unknown command {cmdName}!");
                    }
                }
                else
                {
                    LogWriter.ToLog(LogEventLevel.Error, $"{cmdName}Handler not implemented!");
                }
            }
            #endregion
        }
    }
}
