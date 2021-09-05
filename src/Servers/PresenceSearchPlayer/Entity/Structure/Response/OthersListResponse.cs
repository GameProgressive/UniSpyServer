using PresenceSearchPlayer.Abstraction.BaseClass;
using PresenceSearchPlayer.Entity.Structure.Result;
using UniSpyLib.Abstraction.BaseClass;

namespace PresenceSearchPlayer.Entity.Structure.Response
{
    internal sealed class OthersListResponse : PSPResponseBase
    {
        private new OthersListResult _result => (OthersListResult)base._result;
        public OthersListResponse(PSPRequestBase request, UniSpyResultBase result) : base(request, result)
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
