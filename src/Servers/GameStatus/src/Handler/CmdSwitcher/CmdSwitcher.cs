using System.Linq;
using UniSpyServer.UniSpyLib.Abstraction.BaseClass;
using UniSpyServer.UniSpyLib.Abstraction.Interface;
using UniSpyServer.UniSpyLib.MiscMethod;

namespace UniSpyServer.GameStatus.Handler.CmdSwitcher
{
    public sealed class CmdSwitcher : UniSpyCmdSwitcherBase
    {
        private new string _rawRequest => (string)base._rawRequest;

        public CmdSwitcher(IUniSpySession session, object rawRequest) : base(session, rawRequest)
        {
        }
        protected override void ProcessRawRequest()
        {
            if (_rawRequest[0] != '\\')
            {
                throw new UniSpyException("Invalid request");
            }
            string[] splitedRawRequests = _rawRequest.TrimStart('\\').Split('\\');
            foreach (var rawRequest in splitedRawRequests)
            {
                var name = GameSpyUtils.ConvertToKeyValue(rawRequest).Keys.First();
                _cmdMapping.Add(name, rawRequest);
            }

        }
    }
}
