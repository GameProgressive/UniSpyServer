using UniSpy.Server.Chat.Abstraction.BaseClass;
using UniSpy.Server.Chat.Aggregate.Misc;
using UniSpy.Server.Chat.Contract.Result.General;
using System.Linq;

namespace UniSpy.Server.Chat.Contract.Response.General
{
    public sealed class WhoIsResponse : ResponseBase
    {
        private new WhoIsResult _result => (WhoIsResult)base._result;
        public WhoIsResponse(RequestBase request, ResultBase result) : base(request, result) { }
        public override void Build()
        {
            SendingBuffer = $":{ServerDomain} {ResponseName.WhoIsUser} {_result.NickName} {_result.Name} {_result.UserName} {_result.PublicIPAddress} * :{_result.UserName}\r\n";

            if (_result.JoinedChannelName.Count() != 0)
            {
                string channelNames = "";
                //todo remove last space
                foreach (var name in _result.JoinedChannelName)
                {
                    channelNames += $"@{name} ";
                }

                SendingBuffer += $":{ServerDomain} {ResponseName.WhoIsChannels} {_result.NickName} {_result.Name} :{channelNames}\r\n";

                SendingBuffer += $":{ServerDomain} {ResponseName.EndOfWhoIs} {_result.NickName} {_result.Name} :End of WHOIS list.\r\n";
            }
        }
    }
}
