using UniSpyServer.PresenceSearchPlayer.Abstraction.BaseClass;
using UniSpyServer.PresenceSearchPlayer.Entity.Structure.Request;
using UniSpyServer.PresenceSearchPlayer.Entity.Structure.Result;
using UniSpyServer.UniSpyLib.Abstraction.BaseClass;

namespace UniSpyServer.PresenceSearchPlayer.Entity.Structure.Response
{
    public sealed class NicksResponse : ResponseBase
    {
        private new NicksResult _result => (NicksResult)base._result;
        private new NicksRequest _request => (NicksRequest)base._request;

        public NicksResponse(RequestBase request, UniSpyResultBase result) : base(request, result)
        {
        }

        public override void Build()
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
