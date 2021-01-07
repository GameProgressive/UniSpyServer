using PresenceSearchPlayer.Abstraction.BaseClass;
using PresenceSearchPlayer.Entity.Structure.Result;

namespace PresenceSearchPlayer.Entity.Structure.Response
{
    public class OthersListResponse : PSPResponseBase
    {
        protected new OthersListResult _result => (OthersListResult)base._result;

        public OthersListResponse(PSPResultBase result) : base(result)
        {
        }

        protected override void BuildNormalResponse()
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
