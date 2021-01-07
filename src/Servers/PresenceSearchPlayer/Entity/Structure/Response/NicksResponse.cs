using PresenceSearchPlayer.Abstraction.BaseClass;
using PresenceSearchPlayer.Entity.Structure.Result;

namespace PresenceSearchPlayer.Entity.Structure.Response
{
    public class NicksResponse : PSPResponseBase
    {
        protected new NicksResult _result
        {
            get { return (NicksResult)base._result; }
        }
        public NicksResponse(PSPResultBase result) : base(result)
        {
        }

        protected override void BuildNormalResponse()
        {
            SendingBuffer = @"\nr\";
            foreach (var info in _result.DataBaseResults)
            {
                SendingBuffer += @"\nick\";
                SendingBuffer += info.NickName;
                if (_result.IsRequireUniqueNicks)
                {
                    SendingBuffer += @"\uniquenick\";
                    SendingBuffer += info.UniqueNick;
                }
                SendingBuffer += @"\ndone\final\";
            }
        }
    }
}
