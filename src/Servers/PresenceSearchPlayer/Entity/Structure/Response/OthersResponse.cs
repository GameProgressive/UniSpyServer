using PresenceSearchPlayer.Abstraction.BaseClass;
using PresenceSearchPlayer.Entity.Structure.Result;

namespace PresenceSearchPlayer.Entity.Structure.Response
{
    public class OthersResponse:PSPResponseBase
    {
        protected new OthersResult _result => (OthersResult)base._result;
        public OthersResponse(PSPResultBase result) : base(result)
        {
        }

        protected override void BuildNormalResponse()
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
