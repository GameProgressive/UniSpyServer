using Chat.Entity.Structure.ChatCommand;
using System.Collections.Generic;

namespace Chat.Entity.Structure.ChannelInfo
{
    public class ChatChannelMode
    {

        //i - toggle the invite-only channel flag;
        public bool IsInviteOnly { get; protected set; }
        //p - toggle the private channel flag;
        public bool IsPrivateChannel { get; protected set; }
        //s - toggle the secret channel flag;
        public bool IsSecretChannel { get; protected set; }
        //m - toggle the moderated channel;
        public bool IsModeratedChannel { get; protected set; }
        //n - toggle the no messages to channel from clients on the outside;
        public bool IsAllowExternalMessage { get; protected set; }
        //t - toggle the topic settable by channel operator only flag;
        public bool IsTopicOnlySetByChannelOperator { get; protected set; }

        /// <summary>
        /// default constructor
        /// </summary>
        public ChatChannelMode()
        {

        }

        public void SetDefaultModes()
        {
            IsModeratedChannel = true;
            IsTopicOnlySetByChannelOperator = true;
        }

        public void ChangeModes(MODERequest cmd)
        {
            switch (cmd.RequestType)
            {
                case ModeRequestType.SetChannelModes:
                    SetChannelModes(cmd);
                    break;
                case ModeRequestType.SetChannelModesWithUserLimit:
                    SetChannelModes(cmd);
                    break;
            }
        }

        private void SetChannelModes(MODERequest cmd)
        {
            List<string> flags = new List<string>();

            if (cmd.ModeFlag == null)
            {
                return;
            }
            for (int i = 0; i < cmd.ModeFlag.Length; i += 2)
            {
                flags.Add($"{cmd.ModeFlag[i]}{cmd.ModeFlag[i + 1]}");
            }

            foreach (var f in flags)
            {
                SetModeByFlag(f);
            }
        }

        private void SetModeByFlag(string flag)
        {
            //XiXpXsXmXnXtXlXe
            switch (flag)
            {
                case "+i":
                    IsInviteOnly = true;
                    break;
                case "-i":
                    IsInviteOnly = false;
                    break;
                case "+p":
                    IsPrivateChannel = true;
                    break;
                case "-p":
                    IsPrivateChannel = false;
                    break;
                case "+s":
                    IsSecretChannel = true;
                    break;
                case "-s":
                    IsSecretChannel = false;
                    break;
                case "+m":
                    IsModeratedChannel = true;
                    break;
                case "-m":
                    IsModeratedChannel = false;
                    break;
                case "+n":
                    IsAllowExternalMessage = true;
                    break;
                case "-n":
                    IsAllowExternalMessage = false;
                    break;
                case "+t":
                    IsTopicOnlySetByChannelOperator = true;
                    break;
                case "-t":
                    IsTopicOnlySetByChannelOperator = false;
                    break;
                case "+e":
                case "-e":
                    break;
            }
        }

        public string GetChannelMode()
        {
            string buffer = "+";
            if (IsInviteOnly)
            {
                buffer += "i";
            }
            if (IsPrivateChannel)
            {
                buffer += "p";
            }
            if (IsSecretChannel)
            {
                buffer += "s";
            }
            if (IsModeratedChannel)
            {
                buffer += "m";
            }
            if (IsAllowExternalMessage)
            {
                buffer += "n";
            }
            if (IsTopicOnlySetByChannelOperator)
            {
                buffer += "t";
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
