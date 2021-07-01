using Chat.Abstraction.BaseClass;
using Chat.Entity.Structure.Misc;
using Chat.Entity.Structure.Result.General;
using System.Linq;
using UniSpyLib.Abstraction.BaseClass;

namespace Chat.Entity.Structure.Response.General
{
    internal sealed class WHOISResponse : ChatResponseBase
    {
        private new WHOISResult _result => (WHOISResult)base._result;
        public WHOISResponse(UniSpyRequest request, UniSpyResult result) : base(request, result)
        {
        }
        public override void Build()
        {
            SendingBuffer = ChatIRCReplyBuilder.Build(
                ChatReplyName.WhoIsUser,
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

                SendingBuffer += ChatIRCReplyBuilder.Build(
                    ChatReplyName.WhoIsChannels,
                    $"{_result.NickName} {_result.Name}",
                    channelNames
                    );

                SendingBuffer += ChatIRCReplyBuilder.Build(
                    ChatReplyName.EndOfWhoIs,
                    $"{_result.NickName} {_result.Name}",
                    "End of /WHOIS list."
                    );
            }
        }
    }
}
