using PresenceSearchPlayer.Abstraction.BaseClass;
using PresenceSearchPlayer.Entity.Structure.Result;
using UniSpyLib.Abstraction.BaseClass;

namespace PresenceSearchPlayer.Entity.Structure.Response
{
    internal sealed class OthersResponse : PSPResponseBase
    {
        private new OthersResult _result => (OthersResult)base._result;
        public OthersResponse(PSPRequestBase request, UniSpyResult result) : base(request, result)
        {
        }

        public override void Build()
        {
            SendingBuffer = @"\others\";

            foreach (var info in _result.DatabaseResults)
            {
                SendingBuffer += @"\o\" + info.Profileid;
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
