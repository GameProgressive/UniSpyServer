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
        public ChatSession ChannelCreator { get; set; }
        public ConcurrentBag<ChatSession> ChannelOperators { get; set; }
        public ConcurrentBag<ChatSession> BanList { get; set; }
        public ConcurrentBag<ChatSession> MuteUserList { get; set; }
        public ConcurrentBag<ChatSession> ChannelUsers { get; set; }
        public string Password { get; set; }


        public ChatChannelProperty()
        {
            ChannelCreatedTime = DateTime.Now;
            ChannelMode = new ChatChannelMode();
            ChannelOperators = new ConcurrentBag<ChatSession>();
            BanList = new ConcurrentBag<ChatSession>();
            MuteUserList = new ConcurrentBag<ChatSession>();
            ChannelUsers = new ConcurrentBag<ChatSession>();
        }

        public void SetDefaultProperties(ChatSession creator, JOIN cmd)
        {
            MaxNumberUser = 200;
            ChannelCreator = creator;
            ChannelName = cmd.ChannelName;
            ChannelMode.SetDefaultModes();
        }

        public void SetProperties(ChatSession changer,MODE cmd)
        {
            ChannelCreator = changer;
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
