using Serilog.Events;
using System;
using System.Linq;
using UniSpyLib.Abstraction.BaseClass;
using UniSpyLib.Abstraction.Interface;
using UniSpyLib.Logging;

namespace Chat.Handler.CommandSwitcher
{
    internal sealed class ChatCmdHandlerFactory : UniSpyCmdHandlerFactoryBase
    {
        public ChatCmdHandlerFactory(IUniSpySession session, IUniSpyRequest request) : base(session, request)
        {
        }

        public override IUniSpyHandler Serialize()
        {

            Type handlerType = AppDomain.CurrentDomain
                    .GetAssemblies()
                    .SelectMany(x => x.GetTypes())
                    .FirstOrDefault(t => t.Name == $"{(string)_request.CommandName}Handler");

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
                    LogWriter.ToLog(LogEventLevel.Error, $"Unknown command {(string)_request.CommandName}!");
                    return null;
                }
            }
            else
            {
                LogWriter.ToLog(LogEventLevel.Error, $"{(string)_request.CommandName}Handler not implemented!");
                return null;
            }
        }
    }
}
