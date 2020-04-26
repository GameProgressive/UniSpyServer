using System;
using System.Collections.Concurrent;
using Chat.Entity.Structure.ChatCommand;
using Chat.Server;
using System.Linq;
using GameSpyLib.Logging;

namespace Chat.Entity.Structure.ChatChannel
{
    public class ChatChannelProperty
    {
        public string ChannelName { get; set; }
        public uint MaxNumberUser { get; set; }
        public ChatChannelMode ChannelMode { get; set; }
        public DateTime ChannelCreatedTime { get; set; }
        public ConcurrentBag<ChatChannelUser> BanList { get; set; }
        public ConcurrentBag<ChatChannelUser> ChannelUsers { get; set; }
        public string Password { get; set; }


        public ChatChannelProperty()
        {
            ChannelCreatedTime = DateTime.Now;
            ChannelMode = new ChatChannelMode();
            BanList = new ConcurrentBag<ChatChannelUser>();
            ChannelUsers = new ConcurrentBag<ChatChannelUser>();
        }

        public void SetDefaultProperties(ChatChannelUser creator, JOIN cmd)
        {
            MaxNumberUser = 200;
            ChannelName = cmd.ChannelName;
            ChannelMode.SetDefaultModes();
        }

        /// <summary>
        /// We only care about how to set mode in this channel
        /// we do not need to care about if the user is legal
        /// because MODEHandler will check for us
        /// </summary>
        /// <param name="changer"></param>
        /// <param name="cmd"></param>
        public void SetProperties(ChatChannelUser changer, MODE cmd)
        {
            switch (cmd.RequestType)
            {
                case ModeRequestType.AddChannelUserLimits:
                    AddChannelUserLimits(cmd);
                    break;
                case ModeRequestType.RemoveChannelUserLimits:
                    RemoveChannelUserLimits(cmd);
                    break;
                case ModeRequestType.AddBanOnUser:
                    AddBanOnUser(cmd);
                    break;
                case ModeRequestType.RemoveBanOnUser:
                    RemoveBanOnUser(cmd);
                    break;
                case ModeRequestType.AddChannelPassword:
                    AddChannelPassword(cmd);
                    break;
                case ModeRequestType.RemoveChannelPassword:
                    RemoveChannelPassword(cmd);
                    break;
                case ModeRequestType.AddChannelOperator:
                    AddChannelOperator(cmd);
                    break;
                case ModeRequestType.RemoveChannelOperator:
                    RemoveChannelOperator(cmd);
                    break;
                case ModeRequestType.EnableUserVoicePermission:
                    EnableUserVoicePermission(cmd);
                    break;
                case ModeRequestType.DisableUserVoicePermission:
                    DisableUserVoicePermission(cmd);
                    break;
                case ModeRequestType.EnableUserQuietFlag:
                    break;
                case ModeRequestType.DisableUserQuietFlag:
                    break;
                case ModeRequestType.SetChannelModesWithUserLimit:
                    AddChannelUserLimits(cmd);
                    goto default;
                default:
                    ChannelMode.ChangeModes(cmd);
                    break;
            }
        }
        private void AddChannelUserLimits(MODE cmd)
        {
            MaxNumberUser = cmd.LimitNumber;
        }
        private void RemoveChannelUserLimits(MODE cmd)
        {
            MaxNumberUser = 200;
        }
        private void AddBanOnUser(MODE cmd)
        {
            var result = ChannelUsers.Where(u => u.UserInfo.NickName == cmd.NickName);
            if (result.Count() != 1)
            {
                return;
            }
            ChatChannelUser user = result.First();

            if (BanList.Where(u => u.UserInfo.NickName == cmd.NickName).Count() == 1)
            {
                return;
            }

            BanList.Add(user);
        }
        private void RemoveBanOnUser(MODE cmd)
        {
            var result = BanList.Where(u => u.UserInfo.NickName == cmd.NickName);
            if (result.Count() == 1)
            {
                ChatChannelUser user = result.First();
                BanList.TryTake(out user);
                return;
            }
            if (result.Count() > 1)
            {
                LogWriter.ToLog(Serilog.Events.LogEventLevel.Error,
                    $"Multiple user with same nick name in channel {ChannelName}");
            }
        }
        private void AddChannelPassword(MODE cmd)
        {
            if (Password == null)
            {
                Password = cmd.Password;
            }
        }
        private void RemoveChannelPassword(MODE cmd)
        {
            if (Password == cmd.Password)
            {
                Password = null;
            }
        }
        private void AddChannelOperator(MODE cmd)
        {
            // check whether this user is in this channel
            var result = ChannelUsers.Where(u => u.UserInfo.UserName == cmd.UserName);
            if (result.Count() != 1)
            {
                return;
            }
            ChatChannelUser user = result.First();

            //if this user is already in operator we do not add it
            if (user.IsChannelOperator)
            {
                return;
            }
            user.SetChannelOperator(true);
        }
        private void RemoveChannelOperator(MODE cmd)
        {
            var result = ChannelUsers.Where(u => u.UserInfo.UserName == cmd.UserName);
            if (result.Count() != 1)
            {
                return;
            }
            ChatChannelUser user = result.First();

            if (user.IsChannelCreator)
            {
                user.SetChannelCreator(false);
            }
        }
        private void EnableUserVoicePermission(MODE cmd)
        {
            var result = ChannelUsers.Where(u => u.UserInfo.UserName == cmd.UserName);
            if (result.Count() != 1)
            {
                return;
            }

            ChatChannelUser user = result.First();

            if (user.IsVoiceable)
            {
                user.SetVoicePermission(true);
            }

        }
        private void DisableUserVoicePermission(MODE cmd)
        {
            var result = ChannelUsers.Where(u => u.UserInfo.UserName == cmd.UserName);
            if (result.Count() != 1)
            {
                return;
            }

            ChatChannelUser user = result.First();
            if (user.IsVoiceable)
            {
                user.SetVoicePermission(false);
            }
        }
    }
}
