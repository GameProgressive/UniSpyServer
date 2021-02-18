﻿using Chat.Abstraction.BaseClass;
using Chat.Entity.Structure;
using Chat.Entity.Structure.Request;
using Chat.Entity.Structure.Response;
using Chat.Entity.Structure.Response.Channel;
using Chat.Entity.Structure.Result.Channel;
using UniSpyLib.Abstraction.Interface;

namespace Chat.Handler.CmdHandler.Channel
{
    // Sets channel key/values.
    // If user is NULL or "", the keys will be set on the channel.
    // Otherwise, they will be set on the user,
    // Only ops can set channel keys on other users.
    // Set a value to NULL or "" to clear that key.
    internal sealed class SETCHANKEYHandler : ChatChannelHandlerBase
    {
        private new SETCHANKEYRequest _request=>(SETCHANKEYRequest)base._request;
        private new SETCHANKEYResult _result
        {
            get => (SETCHANKEYResult)base._result;
            set => base._result = value;
        }
        public SETCHANKEYHandler(IUniSpySession session, IUniSpyRequest request) : base(session, request)
        {
            _result = new SETCHANKEYResult();
        }

        protected override void DataOperation()
        {
            if (!_user.IsChannelOperator)
            {
                _result.ErrorCode = ChatErrorCode.NotChannelOperator;
                return;
            }
            _channel.Property.SetChannelKeyValue(_request.KeyValue);
            _result.ChannelName = _result.ChannelName;
            _result.ChannelUserIRCPrefix = _user.UserInfo.IRCPrefix;
            
        }
        protected override void ResponseConstruct()
        {
            _response = new SETCHANKEYResponse(_request, _result);
        }
    }
}