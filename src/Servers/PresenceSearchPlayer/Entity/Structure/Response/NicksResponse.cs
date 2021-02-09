using PresenceSearchPlayer.Abstraction.BaseClass;
using PresenceSearchPlayer.Entity.Structure.Request;
using PresenceSearchPlayer.Entity.Structure.Result;
using UniSpyLib.Abstraction.BaseClass;

namespace PresenceSearchPlayer.Entity.Structure.Response
{
    internal sealed class NicksResponse : PSPResponseBase
    {
        private new NicksResult _result=> (NicksResult)base._result;
        private new NicksRequest _request => (NicksRequest)base._request;

        internal NicksResponse(PSPRequestBase request, UniSpyResultBase result) : base(request, result)
        {
        }

        protected override void BuildNormalResponse()
        {
            SendingBuffer = @"\nr\";
            foreach (var info in _result.DataBaseResults)
            {
                SendingBuffer += @"\nick\";
                SendingBuffer += info.NickName;
                if (_request.IsRequireUniqueNicks)
                {
                    SendingBuffer += @"\uniquenick\";
                    SendingBuffer += info.UniqueNick;
                }
            }
            SendingBuffer += @"\ndone\final\";
        }
    }
}
