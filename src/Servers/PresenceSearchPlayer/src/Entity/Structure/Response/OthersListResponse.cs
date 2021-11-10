using UniSpyServer.Servers.PresenceSearchPlayer.Abstraction.BaseClass;
using UniSpyServer.Servers.PresenceSearchPlayer.Entity.Structure.Result;
using UniSpyServer.UniSpyLib.Abstraction.BaseClass;

namespace UniSpyServer.Servers.PresenceSearchPlayer.Entity.Structure.Response
{
    public sealed class OthersListResponse : ResponseBase
    {
        private new OthersListResult _result => (OthersListResult)base._result;
        public OthersListResponse(RequestBase request, UniSpyResultBase result) : base(request, result)
        {
        }

        public override void Build()
        {
            SendingBuffer = @"\otherslist\";
            foreach (var info in _result.DatabaseResults)
            {
                SendingBuffer += $@"\o\{info.ProfileID}";
                SendingBuffer += $@"\uniquenick\{info.Uniquenick}";
            }

            SendingBuffer += @"oldone";
        }
    }
}
