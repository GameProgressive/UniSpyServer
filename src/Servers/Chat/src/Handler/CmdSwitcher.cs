using System;
using System.Collections.Generic;
using System.Linq;
using UniSpy.Server.Chat.Contract.Request.Channel;
using UniSpy.Server.Chat.Contract.Request.General;
using UniSpy.Server.Chat.Contract.Request.Message;
using UniSpy.Server.Chat.Handler.CmdHandler.Channel;
using UniSpy.Server.Chat.Handler.CmdHandler.General;
using UniSpy.Server.Chat.Handler.CmdHandler.Message;
using UniSpy.Server.Core.Abstraction.BaseClass;
using UniSpy.Server.Core.Abstraction.Interface;
using UniSpy.Server.Core.Encryption;

namespace UniSpy.Server.Chat.Handler
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
            var request = (string)rawRequest;
            switch ((string)name)
            {
                # region General
                case "CRYPT":
                    return new CryptHandler(_client, new CryptRequest(request));
                case "CDKEY":
                    return new CDKeyHandler(_client, new CDKeyRequest(request));
                case "GETKEY":
                    return new GetKeyHandler(_client, new GetKeyRequest(request));
                case "LIST":
                    return new ListHandler(_client, new ListRequest(request));
                case "LOGIN":
                    return new LoginHandler(_client, new LoginRequest(request));
                case "NICK":
                    return new NickHandler(_client, new NickRequest(request));
                case "PING":
                    return new PingHandler(_client, new PingRequest(request));
                case "QUIT":
                    return new QuitHandler(_client, new QuitRequest(request));
                case "SETKEY":
                    return new SetKeyHandler(_client, new SetKeyRequest(request));
                case "USER":
                    return new UserHandler(_client, new UserRequest(request));
                case "USRIP":
                    return new UserIPHandler(_client, new UserIPRequest(request));
                case "WHO":
                    return new WhoHandler(_client, new WhoRequest(request));
                case "WHOIS":
                    return new WhoIsHandler(_client, new WhoIsRequest(request));
                #endregion

                #region Channel
                case "GETCHANKEY":
                    return new GetChannelKeyHandler(_client, new GetChannelKeyRequest(request));
                case "GETCKEY":
                    return new GetCKeyHandler(_client, new GetCKeyRequest(request));
                case "JOIN":
                    return new JoinHandler(_client, new JoinRequest(request));
                case "KICK":
                    return new KickHandler(_client, new KickRequest(request));
                case "MODE":
                    return new ModeHandler(_client, new ModeRequest(request));
                case "NAMES":
                    return new NamesHandler(_client, new NamesRequest(request));
                case "PART":
                    return new PartHandler(_client, new PartRequest(request));
                case "SETCHANKEY":
                    return new SetChannelKeyHandler(_client, new SetChannelKeyRequest(request));
                case "SETCKEY":
                    return new SetCKeyHandler(_client, new SetCKeyRequest(request));
                case "TOPIC":
                    return new TopicHandler(_client, new TopicRequest(request));
                #endregion
                #region  Message
                case "ATM":
                    return new AboveTheTableMsgHandler(_client, new AboveTheTableMsgRequest(request));
                case "NOTICE":
                    return new NoticeHandler(_client, new NoticeRequest(request));
                case "PRIVMSG":
                    return new PrivateMsgHandler(_client, new PrivateMsgRequest(request));
                case "UTM":
                    return new UnderTheTableMsgHandler(_client, new UnderTheTableMsgRequest(request));
                default:
                    return null;
                    #endregion
            }
        }
    }
}