using Chat.Abstraction.BaseClass;
using Chat.Entity.Structure.Misc;
using Chat.Entity.Structure.Result.General;
using System.Linq;
using UniSpyLib.Abstraction.BaseClass;

namespace Chat.Entity.Structure.Response.General
{
    internal sealed class WhoIsResponse : ResponseBase
    {
        private new WhoIsResult _result => (WhoIsResult)base._result;
        public WhoIsResponse(UniSpyRequestBase request, UniSpyResultBase result) : base(request, result)
        {
        }
        public override void Build()
        {
            SendingBuffer = IRCReplyBuilder.Build(
                ResponseName.WhoIsUser,
                cmdParams: $"{_result.NickName} {_result.Name} {_result.UserName} {_result.PublicIPAddress} *",
                _result.UserName);

            if (_result.JoinedChannelName.Count() != 0)
            {
                string channelNames = "";
                //todo remove last space
                foreach (var name in _result.JoinedChannelName)
                {
                    channelNames += $"@{name} ";
                }

                SendingBuffer += IRCReplyBuilder.Build(
                    ResponseName.WhoIsChannels,
                    $"{_result.NickName} {_result.Name}",
                    channelNames
                    );

                SendingBuffer += IRCReplyBuilder.Build(
                    ResponseName.EndOfWhoIs,
                    $"{_result.NickName} {_result.Name}",
                    "End of /WHOIS list."
                    );
            }
        }
    }
}
