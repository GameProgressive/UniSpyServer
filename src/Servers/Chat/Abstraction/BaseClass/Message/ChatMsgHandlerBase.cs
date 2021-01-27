using System.Runtime.CompilerServices;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using Chat.Application;
using Chat.Entity.Structure;
using Chat.Entity.Structure.Misc;
using Chat.Network;
using UniSpyLib.Abstraction.Interface;

namespace Chat.Abstraction.BaseClass
{
    internal abstract class ChatMsgHandlerBase : ChatChannelHandlerBase
    {
        protected new ChatMsgRequestBase _request => (ChatMsgRequestBase)base._request;
 
        public ChatMsgHandlerBase(IUniSpySession session, IUniSpyRequest request) : base(session, request)
        {
        }

        protected override void RequestCheck()
        {
            switch (_request.RequestType)
            {
                case ChatMessageType.ChannelMessage:
                    base.RequestCheck();
                    break;
                case ChatMessageType.UserMessage:

                    if (_request.RequestType == ChatMessageType.UserMessage)
                    {
                        var user = _channel.GetChannelUserByNickName(_request.NickName);
                        if (user == null)
                        {
                            _result.ErrorCode = ChatErrorCode.IRCError;
                            _result.IRCErrorCode = ChatIRCErrorCode.NoSuchNick;
                        }
                    }
                    break;
                default:
                    _result.ErrorCode = ChatErrorCode.Parse;
                    break;
            }
        }

        protected override void Response()
        {
            if (_response == null)
            {
                return;
            }
            _response.Build();
            switch (_request.RequestType)
            {
                case ChatMessageType.ChannelMessage:
                    _channel.MultiCastExceptSender(_user,(string)_response.SendingBuffer);
                    break;
                case ChatMessageType.UserMessage:
                    _user.UserInfo.Session.SendAsync((string)_response.SendingBuffer);
                    break;
            }

        }
    }
}
