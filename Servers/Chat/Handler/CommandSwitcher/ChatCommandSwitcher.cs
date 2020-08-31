using Chat.Entity.Structure;
using Chat.Entity.Structure.ChatCommand;
using Chat.Handler.CommandHandler.ChatChannelCommandHandler;
using Chat.Handler.CommandHandler.ChatGeneralCommandHandler;
using Chat.Handler.CommandHandler.ChatMessageCommandHandler;
using GameSpyLib.Common.BaseClass;
using GameSpyLib.Common.Entity.Interface;
using GameSpyLib.Logging;
using Serilog.Events;
using System;
using System.Collections.Generic;

namespace Chat.Handler.CommandSwitcher
{
    /// <summary>
    /// Process request to Commands
    /// </summary>
    public class ChatCommandSwitcher : CommandSwitcherBase
    {
        public void Switch(ISession session, string recv)
        {

            List<ChatRequestBase> requestList = new List<ChatRequestBase>();

            string[] rawRequests = recv.Replace("\r", "")
                .Split("\n", StringSplitOptions.RemoveEmptyEntries);
            // first we convert request into our ChatCommand class
            // next we handle each command
            foreach (var rawRequest in rawRequests)
            {
                ChatRequestBase request = new ChatRequestBase(rawRequest);

                if (!request.Parse())
                {
                    LogWriter.ToLog(LogEventLevel.Error, "Invalid request!");
                    continue;
                }

                requestList.Add(request);
            }

            foreach (var request in requestList)
            {
                switch (request.CmdName)
                {
                    case ChatCmdName.CKEY:
                        break;
                    case ChatCmdName.CRYPT:
                        new CRYPTHandler(session, request).Handle();
                        break;
                    case ChatCmdName.USER:
                        new USERHandler(session, request).Handle();
                        break;
                    case ChatCmdName.NICK:
                        new NICKHandler(session, request).Handle();
                        break;
                    case ChatCmdName.INVITE:
                        goto default;

                    case ChatCmdName.LIST:
                        new LISTHandler(session, request).Handle();
                        break;
                    case ChatCmdName.LISTLIMIT:
                        goto default;

                    case ChatCmdName.LOGIN:
                        new LOGINHandler(session, request).Handle();
                        break;
                    case ChatCmdName.LOGINPREAUTH:
                        goto default;

                    case ChatCmdName.MODE:
                        new MODEHandler(session, request).Handle();
                        break;
                    case ChatCmdName.NAMES:
                        new NAMESHandler(session, request).Handle();
                        break;

                    case ChatCmdName.PING:
                        new PINGHandler(session, request).Handle();
                        break;
                    case ChatCmdName.PONG:
                        goto default;
                    case ChatCmdName.QUIT:
                        new QUITHandler(session, request).Handle();
                        break;
                    case ChatCmdName.REGISTERNICK:
                        goto default;
                    case ChatCmdName.USRIP:
                        new USRIPHandler(session, request).Handle();
                        break;

                    case ChatCmdName.WHO:
                        new WHOHandler(session, request).Handle();
                        break;
                    case ChatCmdName.WHOIS:
                        new WHOISHandler(session, request).Handle();
                        break;

                    case ChatCmdName.GETCHANKEY:
                        new GETCHANKEYHandler(session, request).Handle();
                        break;
                    case ChatCmdName.GETCKEY:
                        new GETCKEYHandler(session, request).Handle();
                        break;
                    case ChatCmdName.GETKEY:
                        new GETKEYHandler(session, request).Handle();
                        break;
                    case ChatCmdName.SETCHANKEY:
                        new SETCHANKEYHandler(session, request).Handle();
                        break;
                    case ChatCmdName.SETCKEY:
                        new SETCKEYHandler(session, request).Handle();
                        break;
                    case ChatCmdName.SETKEY:
                        new SETKEYHandler(session, request).Handle();
                        break;

                    case ChatCmdName.GETUDPRELAY: goto default;
                    case ChatCmdName.SETGROUP: goto default;
                    case ChatCmdName.JOIN:
                        new JOINHandler(session, request).Handle();
                        break;
                    case ChatCmdName.KICK:
                        new KICKHandler(session, request).Handle();
                        break;
                    case ChatCmdName.PART:
                        new PARTHandler(session, request).Handle();
                        break;
                    case ChatCmdName.TOPIC:
                        new TOPICHandler(session, request).Handle();
                        break;

                    case ChatCmdName.ATM:
                        new ATMHandler(session, request).Handle();
                        break;
                    case ChatCmdName.UTM:
                        new UTMHandler(session, request).Handle();
                        break;
                    case ChatCmdName.NOTICE:
                        new NOTICEHandler(session, request).Handle();
                        break;
                    case ChatCmdName.PRIVMSG:
                        new PRIVMSGHandler(session, request).Handle();
                        break;

                    default:
                        LogWriter.ToLog(LogEventLevel.Error, $"{request.CmdName}Handler Not implemented yet");
                        break;
                }
            }
        }
    }
}
