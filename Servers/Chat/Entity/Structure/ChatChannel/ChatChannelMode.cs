using Chat.Entity.Structure.ChatCommand;
using GameSpyLib.Logging;
using System.Collections.Generic;
using System.Linq;

namespace Chat.Entity.Structure.ChatChannel
{
    public class ChatChannelMode
    {
        public Dictionary<char, char> ModesKV;

        /// <summary>
        /// default constructor
        /// </summary>
        public ChatChannelMode()
        {
            ModesKV = new Dictionary<char, char>();
            SetDefaultModes();
            //SetStandardChannel();
            //SetModerate();
            //SetAllowOutsideMsg();
        }
        public void SetDefaultModes()
        {
            //    O - give "channel creator" status;
            //    o - give / take channel operator privilege;
            //    v - give / take the voice privilege;
            //    a - toggle the anonymous channel flag;
            //    i - toggle the invite-only channel flag;
            //    m - toggle the moderated channel;
            //    n - toggle the no messages to channel from clients on the
            //        outside;
            //    q - toggle the quiet channel flag;
            //    p - toggle the private channel flag;
            //    s - toggle the secret channel flag;
            //    r - toggle the server reop channel flag;
            //    t - toggle the topic settable by channel operator only flag;
            //    k - set/remove the channel key(password);
            //    l - set/remove the user limit to channel;
            //    b - set/remove ban mask to keep users out;
            //    e - set/remove an exception mask to override a ban mask;
            //    I - set/remove an invitation mask to automatically override the invite-only flag;

            ModesKV.Add('a', '+');
            ModesKV.Add('i', '-');
            ModesKV.Add('m', '+');
            ModesKV.Add('n', '+');
            ModesKV.Add('q', '-');
            ModesKV.Add('p', '-');
            ModesKV.Add('s', '-');
            ModesKV.Add('r', '-');
            ModesKV.Add('t', '+');
            ModesKV.Add('I', '-');
        }

        public void SetModes(ChatCommandBase cmd)
        {
            //when we really recieved mode command we do folloing
            if (cmd.CommandName == "MODE")
            {
                MODE modeCmd = (MODE)cmd;
                List<string> setMode = modeCmd.Modes.Where(m => m.Contains('+') || m.Contains('-')).ToList();
                foreach (var m in setMode)
                {
                    ChangeMode(m);
                }
            }

        }

        public void ChangeMode(string cmd)
        {
            foreach (var m in ModesKV.Keys)
            {
                if (cmd.Contains(m))
                {
                    if (!cmd.Contains(ModesKV[m]))
                    {
                        //first char is the flag
                        ModesKV[m] = cmd[0];
                    }
                }
                else
                {
                    LogWriter.ToLog($"Unsupported channel mode: {m}");
                }
            }
        }

        public string GetChannelMode()
        {
            var mode = ModesKV.Where(c => c.Value == '+').Select(c=>c.Key);
            string buffer="+";
            foreach (var k in mode)
            {
                buffer += k;
            }
            //response is like +nt
            return buffer;
        }


        public static bool ConvertModeFlagToBool(string cmd)
        {
            if (cmd.Contains('+'))
            {
                return true;
            }
            else
            {
                return false;
            }
        }
    }
}
