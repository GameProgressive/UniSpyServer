﻿using Chat.Abstraction.BaseClass;
using Chat.Entity.Structure.Misc;
using Chat.Entity.Structure.Result.General;
using UniSpyLib.Abstraction.BaseClass;

namespace Chat.Entity.Structure.Response.General
{
    internal sealed class ListResponse : ResponseBase
    {
        private new ListResult _result => (ListResult)base._result;
        public ListResponse(UniSpyRequestBase request, UniSpyResultBase result) : base(request, result)
        {
        }
        public override void Build()
        {
            SendingBuffer = "";
            foreach (var info in _result.ChannelInfoList)
            {
                var cmdParams = $"param1 {info.ChannelName} {info.TotalChannelUsers} {info.ChannelTopic}";
                SendingBuffer += ChatIRCReplyBuilder.Build(
                    _result.UserIRCPrefix,
                    ChatReplyName.ListStart,
                    cmdParams,
                    null);
            }
            SendingBuffer += ChatIRCReplyBuilder.Build(_result.UserIRCPrefix, ChatReplyName.ListEnd, null, null);
        }
    }
}