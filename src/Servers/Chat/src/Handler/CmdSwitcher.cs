using System;
using System.Collections.Generic;
using System.Linq;

using UniSpyServer.Servers.Chat.Entity.Structure.Request.Channel;
using UniSpyServer.Servers.Chat.Entity.Structure.Request.General;
using UniSpyServer.Servers.Chat.Entity.Structure.Request.Message;
using UniSpyServer.Servers.Chat.Handler.CmdHandler.Channel;
using UniSpyServer.Servers.Chat.Handler.CmdHandler.General;
using UniSpyServer.Servers.Chat.Handler.CmdHandler.Message;
using UniSpyServer.UniSpyLib.Abstraction.BaseClass;
using UniSpyServer.UniSpyLib.Abstraction.Interface;
using UniSpyServer.UniSpyLib.Encryption;

namespace UniSpyServer.Servers.Chat.Handler
{
    /// <summary>
    /// Process request to Commands
    /// </summary>
    public sealed class CmdSwitcher : CmdSwitcherBase
    {
        private new string _rawRequest => UniSpyEncoding.GetString((byte[])base._rawRequest);
        public CmdSwitcher(IClient client, object rawRequest) : base(client, rawRequest) { }

        protected override void ProcessRawRequest()
        {
            string[] splitedRawRequests = _rawRequest.Replace("\r", "")
                .Split("\n", StringSplitOptions.RemoveEmptyEntries);
            foreach (var rawRequest in splitedRawRequests)
            {
                var name = rawRequest.Trim(' ').Split(' ', StringSplitOptions.RemoveEmptyEntries).ToList().First();
                _requests.Add(new KeyValuePair<object, object>(name, rawRequest));
            }
        }

        protected override IHandler CreateCmdHandlers(object name, object rawRequest)
        {
            switch ((string)name)
            {
                # region General
                case "CRYPT":
                    return new CryptHandler(_client, new CryptRequest((string)rawRequest));
                case "CDKEY":
                    return new CDKeyHandler(_client, new CDKeyRequest((string)rawRequest));
                case "GETKEY":
                    return new GetKeyHandler(_client, new GetKeyRequest((string)rawRequest));
                case "LIST":
                    return new ListHandler(_client, new ListRequest((string)rawRequest));
                case "LOGIN":
                    return new LoginHandler(_client, new LoginRequest((string)rawRequest));
                case "NICK":
                    return new NickHandler(_client, new NickRequest((string)rawRequest));
                case "PING":
                    return new PingHandler(_client, new PingRequest((string)rawRequest));
                case "QUIT":
                    return new QuitHandler(_client, new QuitRequest((string)rawRequest));
                case "SETKEY":
                    return new SetKeyHandler(_client, new SetKeyRequest((string)rawRequest));
                case "USER":
                    return new UserHandler(_client, new UserRequest((string)rawRequest));
                case "USRIP":
                    return new UserIPHandler(_client, new UserIPRequest((string)rawRequest));
                case "WHO":
                    return new WhoHandler(_client, new WhoRequest((string)rawRequest));
                case "WHOIS":
                    return new WhoIsHandler(_client, new WhoIsRequest((string)rawRequest));
                #endregion

                #region Channel
                case "GETCHANKEY":
                    return new GetChannelKeyHandler(_client, new GetChannelKeyRequest((string)rawRequest));
                case "GETCKEY":
                    return new GetCKeyHandler(_client, new GetCKeyRequest((string)rawRequest));
                case "JOIN":
                    return new JoinHandler(_client, new JoinRequest((string)rawRequest));
                case "KICK":
                    return new KickHandler(_client, new KickRequest((string)rawRequest));
                case "MODE":
                    return new ModeHandler(_client, new ModeRequest((string)rawRequest));
                case "NAMES":
                    return new NamesHandler(_client, new NamesRequest((string)rawRequest));
                case "PART":
                    return new PartHandler(_client, new PartRequest((string)rawRequest));
                case "SETCHANKEY":
                    return new SetChannelKeyHandler(_client, new SetChannelKeyRequest((string)rawRequest));
                case "SETCKEY":
                    return new SetCKeyHandler(_client, new SetCKeyRequest((string)rawRequest));
                case "TOPIC":
                    return new TopicHandler(_client, new TopicRequest((string)rawRequest));
                #endregion
                #region  Message
                case "ATM":
                    return new AboveTheTableMsgHandler(_client, new AboveTheTableMsgRequest((string)rawRequest));
                case "NOTICE":
                    return new NoticeHandler(_client, new NoticeRequest((string)rawRequest));
                case "PRIVMSG":
                    return new PrivateMsgHandler(_client, new PrivateMsgRequest((string)rawRequest));
                case "UTM":
                    return new UnderTheTableMsgHandler(_client, new UnderTheTableMsgRequest((string)rawRequest));
                default:
                    return null;
                    #endregion
            }
        }
    }
}