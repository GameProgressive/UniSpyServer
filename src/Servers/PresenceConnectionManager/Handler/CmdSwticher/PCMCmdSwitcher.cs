using System;
using PresenceConnectionManager.Handler.CmdSwticher;
using PresenceSearchPlayer.Entity.Enumerate;
using PresenceSearchPlayer.Handler.CmdHandler.Error;
using Serilog.Events;
using UniSpyLib.Abstraction.BaseClass;
using UniSpyLib.Abstraction.Interface;
using UniSpyLib.Logging;

namespace PresenceConnectionManager.Handler.CommandSwitcher
{
    public class PCMCmdSwitcher : UniSpyCmdSwitcherBase
    {
        protected new string _rawRequest { get { return (string)base._rawRequest; } }

        public PCMCmdSwitcher(IUniSpySession session, object rawRequest) : base(session, rawRequest)
        {
        }

        protected override void SerializeCommandHandlers()
        {
            foreach (var request in _requests)
            {
                var handler = new PCMCmdHandlerSerializer(_session, request).Serialize();
                if(handler == null)
                {
                    return;
                }
                _handlers.Add(handler);
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