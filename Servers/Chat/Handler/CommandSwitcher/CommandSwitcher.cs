using Chat.Entity.Structure.ChatCommand;
using Chat.Server;
using GameSpyLib.Common.BaseClass;
using GameSpyLib.Common.Entity.Interface;
using GameSpyLib.Logging;
using System;
using System.Collections.Generic;

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
            // first we convert request into our ChatCommand class
            // next we handle each command
            foreach (var request in requests)
            {
                ChatCommandBase basicCmd = new ChatCommandBase();

                if (!basicCmd.Parse(request))
                {
                    LogWriter.ToLog(Serilog.Events.LogEventLevel.Error,"Invalid request!");
                    continue;
                }

                //TODO create command according to its name
                Type cmdType =
                    Type.GetType(basicCmd.GetType().Namespace + "." + basicCmd.CommandName);

                if (cmdType == null)
                {
                    LogWriter.ToLog($"{basicCmd.CommandName} Not implemented yet");
                    continue;
                }

                ChatCommandBase cmd = (ChatCommandBase)Activator.CreateInstance(cmdType);

                if (!cmd.Parse(request))
                {
                    continue;
                }
                cmds.Add(cmd);
            }

            if (cmds.Count == 0)
            {
                return;
            }

            ChatServer.CommandManager.HandleCommands(client, cmds);
        }
    }
}
