using UniSpyLib.Abstraction.Interface;
using UniSpyLib.Logging;
using System;
using UniSpyLib.Abstraction.BaseClass;
using PresenceConnectionManager.Handler.CommandSwticher;
using Serilog.Events;
using PresenceSearchPlayer.Entity.Enumerate;
using PresenceSearchPlayer.Handler.CommandHandler.Error;

namespace PresenceConnectionManager.Handler.CommandSwitcher
{
    public class PCMCommandSwitcher : CommandSwitcherBase
    {
        protected new string _rawRequest;
        public PCMCommandSwitcher(ISession session, object rawRequest) : base(session, rawRequest)
        {
        }

        protected override void SerializeCommands()
        {
            foreach (var request in _requests)
            {
                _handlers.Add(new PCMCommandHandlerSerializer(_session, request).Serialize());
            }
        }

        protected override void SerializeRequests()
        {
            if (_rawRequest[0] != '\\')
            {
                LogWriter.ToLog(LogEventLevel.Error, "Invalid request recieved!");
                return;
            }
            string[] rawRequests = _rawRequest.Split("\\final\\", StringSplitOptions.RemoveEmptyEntries);

            foreach (var rawRequest in rawRequests)
            {
                var request = new PCMRequestSerializer(rawRequest).Serialize();
                var errorCode = (GPErrorCode)request.Parse();
                if (errorCode != GPErrorCode.NoError)
                {
                    _session.SendAsync(ErrorMsg.BuildGPErrorMsg(errorCode));
                    continue;
                }
                _requests.Add(request);
            }
        }
    }
}