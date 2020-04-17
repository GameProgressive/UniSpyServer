using System;
using System.Collections.Generic;
using Chat.Entity.Structure.ChatCommand;
using Chat.Handler.CommandHandler;
using GameSpyLib.Common.Entity.Interface;
using GameSpyLib.Logging;

namespace Chat.Handler.CommandSwitcher
{
    public class ChatCommandBaseManager
    {
        public Dictionary<string, Type> AvailableCommands;

        public ChatCommandBaseManager()
        {
            AvailableCommands = new Dictionary<string, Type>();
        }

        public void AddCommand(ChatCommandBase cmd,Type cmdHandlerType)
        {
            AvailableCommands.TryAdd(cmd.CommandName, cmdHandlerType);
        }

        public bool HandleCommands(IClient client,List<ChatCommandBase> cmds)
        {
            string sendingBuffer = "";
            foreach (var cmd in cmds)
            {
                if (AvailableCommands.ContainsKey(cmd.CommandName))
                {
                    Type handlerType;
                    AvailableCommands.TryGetValue(cmd.CommandName, out handlerType);
                    ChatCommandHandlerBase handler =
                        (ChatCommandHandlerBase)Activator.CreateInstance(handlerType, cmd, sendingBuffer);
                    handler.Handle();
                }
                else
                {
                    throw new NotImplementedException("Not implemented chat command!");
                }
            }
            //if there are multiple commands in one request,
            //we build wrap all response to one and send it back
            if (sendingBuffer == "" || sendingBuffer.Length < 3)
            {
                return false;
            }
            client.SendAsync(sendingBuffer);

            return true;
        }
    }
}
