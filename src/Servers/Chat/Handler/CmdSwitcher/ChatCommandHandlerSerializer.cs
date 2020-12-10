using System;
using System.Linq;
using Chat.Abstraction.BaseClass;
using Serilog.Events;
using UniSpyLib.Abstraction.BaseClass;
using UniSpyLib.Abstraction.Interface;
using UniSpyLib.Logging;

namespace Chat.Handler.CommandSwitcher
{
    public class ChatCommandHandlerSerializer:UniSpyCmdHandlerSerializerBase
    {
        public ChatCommandHandlerSerializer(IUniSpySession session, IUniSpyRequest request) : base(session, request)
        {
        }

        public override IUniSpyHandler Serialize()
        {
            string cmdName = ((ChatRequestBase)_request).CommandName;
            Type handlerType = AppDomain.CurrentDomain
                    .GetAssemblies()
                    .SelectMany(x => x.GetTypes())
                    .FirstOrDefault(t => t.Name == cmdName + "Handler");

            if (handlerType != null)
            {
                object[] args = { _session, _request };
                var handler = Activator.CreateInstance(handlerType, args);
                if (handler != null)
                {
                    return (IUniSpyHandler)handler;
                }
                else
                {
                    LogWriter.ToLog(LogEventLevel.Error, $"Unknown command {cmdName}!");
                    return null;
                }
            }
            else
            {
                LogWriter.ToLog(LogEventLevel.Error, $"{cmdName}Handler not implemented!");
                return null;
            }
        }
    }
}
