using UniSpyLib.Logging;
using UniSpyLib.Abstraction.BaseClass;
using UniSpyLib.Abstraction.Interface;
using Serilog.Events;
using System;
using PresenceSearchPlayer.Handler.CmdHandler.Error;
using PresenceSearchPlayer.Entity.Enumerate;

namespace PresenceSearchPlayer.Handler.CmdSwitcher
{
    public class PSPCmdSwitcher : UniSpyCmdSwitcherBase
    {
        protected new string _rawRequest;
        public PSPCmdSwitcher(IUniSpySession session, string rawRequest) : base(session, rawRequest)
        {
            _rawRequest = rawRequest;
        }

        protected override void SerializeCommandHandlers()
        {
            foreach(var request in _requests)
            {
            var handler = new PSPCmdHandlerSerializer(_session,request).Serialize();
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

            string[] rawRequests =
            _rawRequest.Split("\\final\\", StringSplitOptions.RemoveEmptyEntries);
            foreach (var rawRequest in rawRequests)
            {
                var request = new PSPRequestSerializer(rawRequest).Serialize();
                if (request == null)
                {
                    continue;
                }
                var flag = (GPErrorCode)request.Parse();
                if (flag != GPErrorCode.NoError)
                {
                    _session.SendAsync(ErrorMsg.BuildGPErrorMsg(flag));
                    continue;
                }
                _requests.Add(request);
            }
        }
    }
}
