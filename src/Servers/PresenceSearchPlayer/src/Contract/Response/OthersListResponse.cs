using UniSpy.Server.PresenceSearchPlayer.Abstraction.BaseClass;
using UniSpy.Server.PresenceSearchPlayer.Contract.Result;

namespace UniSpy.Server.PresenceSearchPlayer.Contract.Response
{
    public sealed class OthersListResponse : ResponseBase
    {
        private new OthersListResult _result => (OthersListResult)base._result;
        public OthersListResponse(RequestBase request, ResultBase result) : base(request, result)
        {
        }

        public override void Build()
        {
            SendingBuffer = @"\otherslist\";
            foreach (var info in _result.DatabaseResults)
            {
                SendingBuffer += $@"\o\{info.ProfileId}";
                SendingBuffer += $@"\uniquenick\{info.Uniquenick}";
            }

            SendingBuffer += @"oldone";
        }
    }
}
