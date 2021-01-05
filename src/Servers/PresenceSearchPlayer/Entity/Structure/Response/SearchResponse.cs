using PresenceSearchPlayer.Abstraction.BaseClass;
using PresenceSearchPlayer.Entity.Structure.Result;

namespace PresenceSearchPlayer.Entity.Structure.Response
{
    public class SearchResponse : PSPResponseBase
    {
        public SearchResponse(PSPResultBase result) : base(result)
        {
        }

        protected new SearchResult _result => (SearchResult)base._result;

        protected override void BuildNormalResponse()
        {
            SendingBuffer = @"\bsr\";
            foreach (var info in _result.DataBaseResults)
            {
                SendingBuffer += info.Profileid.ToString();
                SendingBuffer += @"\nick\" + info.Nick;
                SendingBuffer += @"\uniquenick\" + info.Uniquenick;
                SendingBuffer += @"\namespaceid\" + info.NamespaceID;
                SendingBuffer += @"\firstname\" + info.Firstname;
                SendingBuffer += @"\lastname\" + info.Lastname;
                SendingBuffer += @"\email\" + info.Email;
            }

            SendingBuffer += @"\bsrdone\\more\0\final\";
        }
    }
}
