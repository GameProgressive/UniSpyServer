using System.Collections.Generic;
using Chat.Abstraction.BaseClass;
using Chat.Entity.Structure.Misc;
using Chat.Entity.Structure.Result;
using UniSpyLib.Abstraction.BaseClass;

namespace Chat.Entity.Structure.Response.General
{
    internal class QUITResponse : ChatResponseBase
    {
        private new QUITResult _result => (QUITResult)base._result;
        public QUITResponse(UniSpyRequestBase request, UniSpyResultBase result) : base(request, result)
        {
        }

        protected override void BuildNormalResponse()
        {
            foreach (var channelInfo in _result.ChannelInfos)
            {
                channelInfo.LeaveReplySendingBuffer =
                    ChatIRCReplyBuilder.Build(
                        _result.QuiterPrefix,
                        ChatReplyName.PART,
                        channelInfo.ChannelName,
                        _result.Message);
                if (channelInfo.IsPeerServer && channelInfo.IsChannelCreator)
                { 
                    channelInfo.KickReplySendingBuffer = 
                }
            }
        }
    }
}