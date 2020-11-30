using Chat.Abstraction.BaseClass;
using UniSpyLib.Abstraction.Interface;
using UniSpyLib.Logging;
using Serilog.Events;
using System;
using System.Linq;
using UniSpyLib.Abstraction.BaseClass;

namespace Chat.Handler.CommandSwitcher
{
    /// <summary>
    /// Process request to Commands
    /// </summary>
    public class ChatCommandSwitcher : CommandSerializerBase
    {
        protected new string _rawRequest;
        public ChatCommandSwitcher(ISession session, object rawRequest) : base(session, rawRequest)
        {
        }

        public override void Serialize()
        {
            var requestList = ChatRequestSerializer.Serialize(_rawRequest);

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
                    object[] args = { _session, request };
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
