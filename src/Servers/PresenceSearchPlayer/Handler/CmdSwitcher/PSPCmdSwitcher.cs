using PresenceSearchPlayer.Entity.Enumerate;
using PresenceSearchPlayer.Handler.CmdHandler.Error;
using Serilog.Events;
using System;
using UniSpyLib.Abstraction.BaseClass;
using UniSpyLib.Abstraction.Interface;
using UniSpyLib.Logging;

namespace PresenceSearchPlayer.Handler.CmdSwitcher
{
    internal class PSPCmdSwitcher : UniSpyCmdSwitcherBase
    {
        private new string _rawRequest => (string)base._rawRequest;
        public PSPCmdSwitcher(IUniSpySession session, string rawRequest) : base(session, rawRequest)
        {
        }

        protected override void SerializeCommandHandlers()
        {
            foreach (var request in _requests)
            {
                var handler = new PSPCmdHandlerSerializer(_session, request).Serialize();
                if (handler == null)
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
                request.Parse();
                if ((GPErrorCode)request.ErrorCode != GPErrorCode.NoError)
                {
                    _session.SendAsync(
                        ErrorMsg.BuildGPErrorMsg(
                            (GPErrorCode)request.ErrorCode));
                    continue;
                }
                _requests.Add(request);
            }
        }
    }
}
