using System;
using Chat.Entity.Structure.ChatChannel;
using Chat.Entity.Structure.ChatCommand;
using Chat.Handler.SystemHandler.ChannelManage;
using GameSpyLib.Common.Entity.Interface;

namespace Chat.Handler.CommandHandler
{
    public class SETCKEYHandler : ChatCommandHandlerBase
    {
        ChatChannelUser _user;
        SETCKEY _setckeyCmd;
        ChatChannelBase _channel;
        public SETCKEYHandler(ISession client, ChatCommandBase cmd) : base(client, cmd)
        {
            _setckeyCmd = (SETCKEY)cmd;
        }

        public override void CheckRequest()
        {
            base.CheckRequest();

            if (_session.UserInfo.NickName != _setckeyCmd.NickName)
            {
                _errorCode = Entity.Structure.ChatError.Parse;
                return;
            }

            if (!_session.UserInfo.IsJoinedChannel(_setckeyCmd.ChannelName))
            {
                _errorCode = Entity.Structure.ChatError.Parse;
                return;
            }

            if (!ChatChannelManager.GetChannel(_setckeyCmd.ChannelName, out _channel))
            {
                _errorCode = Entity.Structure.ChatError.Parse;
                return;
            }

            if (!_channel.GetChannelUserBySession(_session, out _user))
            {
                _errorCode = Entity.Structure.ChatError.Parse;
                return;
            }
        }

        public override void DataOperation()
        {
            base.DataOperation();
            if (_setckeyCmd.NickName != _user.UserInfo.NickName)
            {
                _errorCode = Entity.Structure.ChatError.DataOperation;
                return;
            }
            _user.SetUserKeyValue(_setckeyCmd.KeyValues);
        }

        public override void ConstructResponse()
        {
            base.ConstructResponse();
            if (_errorCode > Entity.Structure.ChatError.NoError)
            {
                //TODO
                //build error response here
                //_sendingBuffer = ChatCommandBase.BuildErrorRPL(_errorCode,"")
            }
        }
    }
}
