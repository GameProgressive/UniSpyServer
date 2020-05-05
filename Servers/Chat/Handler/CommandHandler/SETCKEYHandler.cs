using System;
using System.Linq;
using Chat.Entity.Structure;
using Chat.Entity.Structure.ChatChannel;
using Chat.Entity.Structure.ChatCommand;
using Chat.Handler.SystemHandler.ChannelManage;
using Chat.Server;
using GameSpyLib.Common.Entity.Interface;

namespace Chat.Handler.CommandHandler
{
    public class SETCKEYHandler : ChatCommandHandlerBase
    {
        ChatChannelUser _user;
        new SETCKEY _cmd;
        ChatChannelBase _channel;
        public SETCKEYHandler(ISession client, ChatCommandBase cmd) : base(client, cmd)
        {
            _cmd = (SETCKEY)cmd;
        }

        public override void CheckRequest()
        {
            base.CheckRequest();

            if (_session.UserInfo.NickName != _cmd.NickName)
            {
                _errorCode = ChatError.Parse;
                return;
            }

            if (!_session.UserInfo.IsJoinedChannel(_cmd.ChannelName))
            {
                _errorCode = ChatError.Parse;
                return;
            }

            if (!ChatChannelManager.GetChannel(_cmd.ChannelName, out _channel))
            {
                _errorCode = ChatError.Parse;
                return;
            }

            if (!_channel.GetChannelUserBySession(_session, out _user))
            {
                _errorCode = ChatError.Parse;
                return;
            }
        }

        public override void DataOperation()
        {
            base.DataOperation();
            if (_cmd.NickName != _user.UserInfo.NickName)
            {
                _errorCode = ChatError.DataOperation;
                return;
            }
            _user.SetUserKeyValue(_cmd.KeyValues);
        }

        public override void ConstructResponse()
        {
            base.ConstructResponse();
            if (_errorCode > ChatError.NoError)
            {
                //TODO
                //build error response here
                //_sendingBuffer = ChatCommandBase.BuildErrorRPL(_errorCode,"")
            }
        }

        public override void Response()
        {
            foreach (var user in _channel.Property.ChannelUsers)
            {

                string flags = "";
                foreach (var dic in _cmd.KeyValues)
                {
                    flags += @"\" + dic.Key + @"\" + dic.Value;
                }

                //todo check the paramemter 
                string buffer =
                ChatCommandBase.BuildNumericRPL(ChatServer.ServerDomain,
                      ChatResponseType.GetCKey,
                      $"* {_channel.Property.ChannelName} {_user.UserInfo.NickName} BCAST {flags}", "");

                user.Session.SendAsync(buffer);
            }
        }
    }
}
