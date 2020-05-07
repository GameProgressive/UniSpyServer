using System;
using System.Linq;
using System.Net;
using Chat.Entity.Structure.ChatCommand;
using Chat.Handler.SystemHandler.ChatSessionManage;
using Chat.Server;
using GameSpyLib.Common.Entity.Interface;

namespace Chat.Handler.CommandHandler
{
    public class WHOISHandler : ChatCommandHandlerBase
    {
        WHOIS _whoisCmd;
        public WHOISHandler(ISession client, ChatCommandBase cmd) : base(client, cmd)
        {
            _whoisCmd = (WHOIS)cmd;
        }

        public override void DataOperation()
        {
            base.DataOperation();
            var result = from s in ChatSessionManager.Sessions.Values
                         where s.UserInfo.NickName == _whoisCmd.NickName
                         select new
                         {
                             nickName = s.UserInfo.NickName,
                             name = s.UserInfo.Name,
                             userName = s.UserInfo.UserName,
                             address = ((IPEndPoint)s.Socket.RemoteEndPoint).Address,
                             joinedChannel = s.UserInfo.JoinedChannels.Select(c => c.Property.ChannelName)
                         };

            if (result.Count() != 1)
            {
                _errorCode = Entity.Structure.ChatError.DataOperation;
                return;
            }

            var info = result.FirstOrDefault();
            _sendingBuffer = ChatCommandBase.BuildNumericRPL(
                ChatServer.ServerDomain,
                Entity.Structure.ChatResponseType.WhoIsUser,
                $"{info.nickName} {info.name} {info.userName} {info.address} *",
                info.userName);

            if (info.joinedChannel.Count() != 0)
            {
                string channels = "";
                //todo remove last space
                foreach (var c in info.joinedChannel)
                {
                    channels += $"@{c} ";
                }


                _sendingBuffer += ChatCommandBase.BuildNumericRPL(
                    ChatServer.ServerDomain,
                    Entity.Structure.ChatResponseType.WhoIsChannels,
                    $"{info.nickName} {info.name}",
                    channels
                    );
            }
            _sendingBuffer += ChatCommandBase.BuildNumericRPL(
                    ChatServer.ServerDomain,
                    Entity.Structure.ChatResponseType.EndOfWhoIs,
                    $"{info.nickName} {info.name}", "End of /WHOIS list."
                    );
        }

        public override void ConstructResponse()
        {
            base.ConstructResponse();
            if (_errorCode == Entity.Structure.ChatError.NoSuchNick)
            {
                _sendingBuffer = ChatCommandBase.BuildNumericErrorRPL(ChatServer.ServerDomain,
                    _errorCode,
                    $"{_session.UserInfo.NickName} {_whoisCmd.NickName}", "No such nick.");
            }
        }

    }
}
