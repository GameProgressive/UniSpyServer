using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using Chat.Entity.Structure.ChatCommand;
using Chat.Server;

namespace Chat.Entity.Structure.ChatChannel
{
    public class ChatChannelProperty
    {
        public string ChannelName { get; set; }
        public uint MaxNumberUser { get; set; }
        public ChatChannelMode ChannelMode { get; set; }
        public DateTime ChannelCreatedTime { get; set; }
        public List<ChatSession> ChannelOperators { get; set; }
        public ChatSession ChannelCreator { get; set; }
        public List<ChatSession> BanList { get; set; }
        public List<ChatSession> MuteUserList { get; set; }
        public List<ChatSession> ChannelUsers { get; set; }
        public string Password { get; set; }


        public ChatChannelProperty()
        {
            MaxNumberUser = 200;
            ChannelCreatedTime = DateTime.Now;
            ChannelMode = new ChatChannelMode();
            ChannelOperators = new List<ChatSession>();
            BanList = new List<ChatSession>();
            MuteUserList = new List<ChatSession>();
            ChannelUsers = new List<ChatSession>();
            
        }

        public void SetProperties(ChatSession creator,ChatCommandBase cmd)
        {
            ChannelCreator = creator;
            ChannelName = ((JOIN)cmd).ChannelName;
            ChannelMode.SetModes(cmd);
            //at here we do some mode command
            //password
            //MODE modeCmd = (MODE)cmd;
            /*
            foreach (var c in modeCmd.Modes)
            {
                throw new NotImplementedException();
                if (c.Contains("o"))
                {
                    if (ChatChannelMode.ConvertModeFlagToBool(c))
                    {
                        //give/take channel operator privilege;
                    }
                }
                else if (c.Contains("v"))
                {
                    //give/take the voice privilege;
                }
                else if (c.Contains("k"))
                {
                    //set / remove the channel key(password);
                }
                else if (c.Contains("l"))
                {
                    //set / remove the user limit to channel;
                }
                else if (c.Contains("b"))
                { }
                else if (c.Contains("b"))
                { }
                else if (c.Contains("e"))
                { }
            }
            */

        }
    }
}
