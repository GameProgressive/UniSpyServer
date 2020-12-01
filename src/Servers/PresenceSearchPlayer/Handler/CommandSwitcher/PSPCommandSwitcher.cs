using UniSpyLib.Logging;
using UniSpyLib.Abstraction.BaseClass;
using UniSpyLib.Abstraction.Interface;
using Serilog.Events;
using System;
using PresenceSearchPlayer.Handler.CommandHandler.Error;
using PresenceSearchPlayer.Entity.Enumerate;

namespace PresenceSearchPlayer.Handler.CommandSwitcher
{
    public class PSPCommandSwitcher : CommandSwitcherBase
    {
        protected new string _rawRequest;
        public PSPCommandSwitcher(ISession session, object rawRequest) : base(session, rawRequest)
        {
        }

        protected override void SerializeCommands()
        {
            throw new System.NotImplementedException();
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
