using Chat.Entity.Structure.ChatCommand;
using GameSpyLib.Logging;
using System.Collections.Generic;
using System.Linq;

namespace Chat.Entity.Structure.ChatChannel
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

        public void SetModes(MODE cmd)
        {
            switch (cmd.RequestType)
            {

            }
            ////when we really recieved mode command we do folloing
            //if (cmd.CommandName == "MODE")
            //{
            //    MODE modeCmd = (MODE)cmd;
            //    List<string> setMode = modeCmd.Modes.Where(m => m.Contains('+') || m.Contains('-')).ToList();
            //    foreach (var m in setMode)
            //    {
            //        ChangeMode(m);
            //    }
            //}

        }

        public void ChangeMode(string cmd)
        {
            
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
