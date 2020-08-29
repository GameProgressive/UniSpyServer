using Chat.Entity.Structure.ChatCommand;
using Chat.Handler.CommandHandler;
using Chat.Handler.CommandHandler.ChatBasicCommandHandler;
using Chat.Handler.CommandHandler.ChatChannel.ChatChannelKey;
using GameSpyLib.Common.Entity.Interface;
using GameSpyLib.Logging;
using Serilog.Events;
using System;
using System.Collections.Generic;

namespace Chat.Handler.SystemHandler.ChatCommandManage
{
    public class ChatCommandManager
    {
        public Dictionary<string, Type> AvailableCommands;

        public ChatCommandManager()
        {
            AvailableCommands = new Dictionary<string, Type>();
        }

        public void Start()
        {
            ////ChatChannelKey
            //AddCommand(new GETCHANKEY(), typeof(GETCHANKEYHandler));
            //AddCommand(new SETCHANKEY(), typeof(SETCHANKEYHandler));
            //AddCommand(new GETCKEY(), typeof(GETCKEYHandler));
            //AddCommand(new SETCKEY(), typeof(SETCKEYHandler));
            //AddCommand(new GETKEY(), typeof(GETKEYHandler));
            //AddCommand(new SETKEY(), typeof(SETKEYHandler));

            ////ChatChannelMessage
            //AddCommand(new ATM(), typeof(ATMHandler));
            //AddCommand(new NOTICE(), typeof(NOTICEHandler));
            //AddCommand(new PRIVMSG(), typeof(PRIVMSGHandler));
            //AddCommand(new UTM(), typeof(UTMHandler));

            ////ChatChannel
            //AddCommand(new JOIN(), typeof(JOINHandler));
            //AddCommand(new KICK(), typeof(KICKHandler));
            //AddCommand(new MODE(), typeof(MODEHandler));
            //AddCommand(new PART(), typeof(PARTHandler));
            //AddCommand(new TOPIC(), typeof(TOPICHandler));

            ////ChatLoggedIn
            ////AddCommand(new GETUDPRELAY(), typeof(GETUDPRELAYHandler));
            ////AddCommand(new LIST(), typeof(LISTHandler));
            //AddCommand(new NAMES(), typeof(NAMESHandler));
            //AddCommand(new PING(), typeof(PINGHandler));
            //AddCommand(new QUIT(), typeof(QUITHandler));
            //AddCommand(new USER(), typeof(USERHandler));
            //AddCommand(new USRIP(), typeof(USRIPHandler));
            //AddCommand(new WHO(), typeof(WHOHandler));
            //AddCommand(new WHOIS(), typeof(WHOISHandler));

            ////ChatNotLoggedIn
            //AddCommand(new CRYPT(), typeof(CRYPTHandler));
            //AddCommand(new LOGIN(), typeof(LOGINHandler));
            //AddCommand(new NICK(), typeof(NICKHandler));
        }

        public ChatCommandManager AddCommand(ChatRequestBase cmd, Type cmdHandlerType)
        {
            AvailableCommands.TryAdd(cmd.CmdName, cmdHandlerType);
            return this;
        }

        public bool HandleCommands(ISession session, List<ChatRequestBase> requests)
        {
            foreach (var reqeust in requests)
            {
                if (AvailableCommands.ContainsKey(reqeust.CmdName))
                {
                    Type handlerType;
                    AvailableCommands.TryGetValue(reqeust.CmdName, out handlerType);
                    ChatCommandHandlerBase handler =
                        (ChatCommandHandlerBase)Activator.CreateInstance(handlerType, session, reqeust);
                    handler.Handle();
                }
                else
                {
                    LogWriter.ToLog(LogEventLevel.Error, $"{reqeust.CmdName}Handler not implemented!");
                    continue;
                }
            }
            return true;
        }
    }
}
