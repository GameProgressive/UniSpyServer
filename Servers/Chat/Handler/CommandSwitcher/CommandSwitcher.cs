using System;
using System.Collections.Generic;
using Chat.Entity.Structure.ChatCommand;
using Chat.Server;
using GameSpyLib.Common.BaseClass;
using GameSpyLib.Common.Entity.Interface;
using GameSpyLib.Logging;

namespace Chat.Handler.CommandSwitcher
{
    /// <summary>
    /// Process request to Commands
    /// </summary>
    public class ChatCommandSwitcher : CommandSwitcherBase
    {
        public void Switch(IClient client, string buffer)
        {
            List<ChatCommandBase> cmds = new List<ChatCommandBase>();

            string[] requests = buffer.Split("\r\n", StringSplitOptions.RemoveEmptyEntries);
            foreach (var r in requests)
            {
                ChatBasicCommand basicCommand = new ChatBasicCommand(r);
                //TODO Generate accordingly command
                Type cmdType = Type.GetType(basicCommand.GetType().Namespace + "." + basicCommand.CommandName);

                if (cmdType == null)
                {
                    LogWriter.ToLog($"{basicCommand.CommandName} Not implemented yet");
                    continue;
                }

                ChatCommandBase cmd = (ChatCommandBase)Activator.CreateInstance(cmdType,r);
                cmds.Add(cmd);
            }
            if (cmds.Count == 0)
            {
                return;
            }
            ChatServer.CommandManager.HandleCommands(client,cmds);
        }
    }
}
