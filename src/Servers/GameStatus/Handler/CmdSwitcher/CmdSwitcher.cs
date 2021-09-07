using System.Linq;
using UniSpyLib.Abstraction.BaseClass;
using UniSpyLib.Abstraction.Interface;
using UniSpyLib.MiscMethod;

namespace GameStatus.Handler.CmdSwitcher
{
    internal sealed class CmdSwitcher : UniSpyCmdSwitcherBase
    {
        private new string _message => (string)base._message;

        public CmdSwitcher(IUniSpySession session, object rawRequest) : base(session, rawRequest)
        {
        }
        protected override void ProcessRawRequest()
        {
            if (_message[0] != '\\')
            {
                throw new UniSpyException("Invalid request");
            }
            string[] splitedRawRequests = _message.TrimStart('\\').Split('\\');
            foreach (var rawRequest in splitedRawRequests)
            {
                var name = GameSpyUtils.ConvertToKeyValue(rawRequest).Keys.First();
                _rawRequests.Add(name, rawRequest);
            }

        }
    }
}
