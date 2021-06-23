using PresenceSearchPlayer.Entity.Exception.General;
using System;
using System.Linq;
using UniSpyLib.Abstraction.BaseClass;
using UniSpyLib.Abstraction.Interface;
using UniSpyLib.Logging;
using UniSpyLib.MiscMethod;

namespace PresenceConnectionManager.Handler.CommandSwitcher
{
    internal sealed class PCMCmdSwitcher : UniSpyCmdSwitcherBase
    {
        private new string _rawRequest => (string)base._rawRequest;
        public PCMCmdSwitcher(IUniSpySession session, object rawRequest) : base(session, rawRequest)
        {
        }
        protected override void DeserializeRequests()
        {
            try
            {
                if (_rawRequest[0] != '\\')
                {
                    throw new GPParseException("Request format is invalid");
                }
                var rawRequests = _rawRequest.Split(@"\final\", StringSplitOptions.RemoveEmptyEntries);

                foreach (var rawRequest in rawRequests)
                {
                    // Read client message, and parse it into key value pairs
                    var keyValues = GameSpyUtils.ConvertToKeyValue(rawRequest);
                    
                    if (keyValues.Keys.Count < 1)
                        throw new GPException("Request command not found"); // malformed query

                    var key = keyValues.Keys.First();

                    if (!UniSpyServerFactoryBase.RequestMapping.ContainsKey(key))
                    {
                        LogWriter.LogUnkownRequest(_rawRequest);
                        throw new GPParseException($"Unknown request: {key}");
                    }
                    var request = (IUniSpyRequest)Activator.CreateInstance(
                        UniSpyServerFactoryBase.RequestMapping[key],
                        _rawRequest);

                    request.Parse();
                    _requests.Add(request);
                }
            }
            catch (GPParseException e)
            {
                _session.SendAsync(e.ErrorResponse);
                throw e;
            }
        }
    }
}