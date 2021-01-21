﻿using Chat.Abstraction.BaseClass;
using Chat.Entity.Structure;
using Chat.Entity.Structure.Request;
using Chat.Entity.Structure.Misc;
using Chat.Entity.Structure.Misc.ChannelInfo;
using Chat.Handler.SystemHandler.ChannelManage;
using UniSpyLib.Abstraction.Interface;

namespace Chat.Handler.CmdHandler.General
{
    public class NAMESHandler : ChatCmdHandlerBase
    {
        protected new NAMESRequest _request { get { return (NAMESRequest)base._request; } }
        private ChatChannel _channel;
        private ChatChannelUser _user;
        public NAMESHandler(IUniSpySession session, IUniSpyRequest request) : base(session, request)
        {
        }

        protected override void RequestCheck()
        {
            base.RequestCheck();

            if (!ChatChannelManager.GetChannel(_request.ChannelName, out _channel))
            {
                _errorCode = ChatErrorCode.NoSuchChannel;
                _sendingBuffer = ChatIRCErrorCode.BuildNoSuchChannelError(_request.ChannelName);
            }

            //can not find any user
            if (!_channel.GetChannelUserBySession(_session, out _user))
            {
                _errorCode = ChatErrorCode.NoSuchNick;
                _sendingBuffer = ChatIRCErrorCode.BuildNoSuchNickError();
                return;
            }

        }

        protected override void Response()
        {
            _channel.SendChannelUsersToJoiner(_user);
        }
    }
}
