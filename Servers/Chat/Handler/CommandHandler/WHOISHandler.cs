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
        public WHOISHandler(IClient client, ChatCommandBase cmd) : base(client, cmd)
        {
            _whoisCmd = (WHOIS)cmd;
        }

        public override void DataOperation()
        {
            base.DataOperation();
            var result = from s in ChatSessionManager.Sessions.Values
                         where s.ClientInfo.NickName == _whoisCmd.NickName
                         select new
                         {
                             nickName = s.ClientInfo.NickName,
                             name = s.ClientInfo.Name,
                             userName = s.ClientInfo.UserName,
                             address = ((IPEndPoint)s.Socket.RemoteEndPoint).Address,
                             joinedChannel = s.ClientInfo.JoinedChannels.Select(c => c.Property.ChannelName)
                         };

            if (result.Count() != 1)
            {
                _errorCode = Entity.Structure.ChatError.DataOperation;
                return;
            }
            var info = result.FirstOrDefault();
            _sendingBuffer = ChatCommandBase.BuildNormalRPL(
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

                _sendingBuffer += ChatCommandBase.BuildNormalRPL(
                    ChatServer.ServerDomain,
                    Entity.Structure.ChatResponseType.WhoIsChannels,
                    $"{info.nickName} {info.name}",
                    channels
                    );
                _sendingBuffer += ChatCommandBase.BuildNormalRPL(
                        ChatServer.ServerDomain,
                        Entity.Structure.ChatResponseType.EndOfWhoIs,
                        $"{info.nickName} {info.name}", "End of /WHOIS list."
                        );
            }
            else
            {
                _errorCode = Entity.Structure.ChatError.NoSuchNick;
            }
        }

        public override void ConstructResponse()
        {
            base.ConstructResponse();
            if (_errorCode == Entity.Structure.ChatError.NoSuchNick)
            {
                _sendingBuffer = ChatCommandBase.BuildErrorRPL(ChatServer.ServerDomain,
                    _errorCode,
                    $"{_session.ClientInfo.NickName} {_whoisCmd.NickName}", "No such nick.");
            }
            else
            {
                _errorCode = Entity.Structure.ChatError.ConstructResponse;
                return;
            }
        }

    }
}
