using UniSpy.Server.PresenceSearchPlayer.Abstraction.BaseClass;
using UniSpy.Server.PresenceSearchPlayer.Contract.Result;

namespace UniSpy.Server.PresenceSearchPlayer.Contract.Response
{
    public sealed class OthersResponse : ResponseBase
    {
        private new OthersResult _result => (OthersResult)base._result;
        public OthersResponse(RequestBase request, UniSpy.Server.Core.Abstraction.BaseClass.ResultBase result) : base(request, result)
        {
        }

        public override void Build()
        {
            SendingBuffer = @"\others\";

            foreach (var info in _result.DatabaseResults)
            {
                SendingBuffer += @"\o\" + info.ProfileId;
                SendingBuffer += @"\nick\" + info.Nick;
                SendingBuffer += @"\uniquenick\" + info.Uniquenick;
                SendingBuffer += @"\first\" + info.Firstname;
                SendingBuffer += @"\last\" + info.Lastname;
                SendingBuffer += @"\email\" + info.Email;
            }
            SendingBuffer += @"\odone\final\";
        }
    }
}
