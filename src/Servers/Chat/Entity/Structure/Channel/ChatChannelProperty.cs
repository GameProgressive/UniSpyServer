using Chat.Entity.Structure.ChatCommand;
using UniSpyLib.Logging;
using Serilog.Events;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;

namespace Chat.Entity.Structure.ChatChannel
{
    public class ChatChannelProperty
    {
        public string ChannelName { get; protected set; }
        public uint MaxNumberUser { get; protected set; }
        public ChatChannelMode ChannelMode { get; set; }
        public DateTime ChannelCreatedTime { get; protected set; }
        public ConcurrentBag<ChatChannelUser> BanList { get; set; }
        public ConcurrentBag<ChatChannelUser> ChannelUsers { get; set; }
        public string Password { get; set; }
        public Dictionary<string, string> ChannelKeyValue { get; protected set; }
        public string ChannelTopic { get; protected set; }
        public bool IsPeerServer { get; protected set; }
        public ChatChannelProperty SetPeerServerFlag(bool flag)
        {
            IsPeerServer = flag;
            return this;
        }

        public ChatChannelProperty()
        {
            ChannelCreatedTime = DateTime.Now;
            ChannelMode = new ChatChannelMode();
            BanList = new ConcurrentBag<ChatChannelUser>();
            ChannelUsers = new ConcurrentBag<ChatChannelUser>();
            ChannelKeyValue = new Dictionary<string, string>();
        }

        public void SetDefaultProperties(ChatChannelUser creator, JOINRequest cmd)
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
        public void SetProperties(ChatChannelUser changer, MODERequest cmd)
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
                case ModeRequestType.SetChannelModesWithUserLimit:
                    AddChannelUserLimits(cmd);
                    goto default;
                default:
                    ChannelMode.ChangeModes(cmd);
                    break;
            }
        }
        private void AddChannelUserLimits(MODERequest cmd)
        {
            MaxNumberUser = cmd.LimitNumber;
        }
        private void RemoveChannelUserLimits(MODERequest cmd)
        {
            MaxNumberUser = 200;
        }
        private void AddBanOnUser(MODERequest cmd)
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
        private void RemoveBanOnUser(MODERequest cmd)
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
                LogWriter.ToLog(LogEventLevel.Error,
                    $"Multiple user with same nick name in channel {ChannelName}");
            }
        }
        private void AddChannelPassword(MODERequest cmd)
        {
            if (Password == null)
            {
                Password = cmd.Password;
            }
        }
        private void RemoveChannelPassword(MODERequest cmd)
        {
            if (Password == cmd.Password)
            {
                Password = null;
            }
        }
        private void AddChannelOperator(MODERequest cmd)
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
        private void RemoveChannelOperator(MODERequest cmd)
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
        private void EnableUserVoicePermission(MODERequest cmd)
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
        private void DisableUserVoicePermission(MODERequest cmd)
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

        public void SetChannelTopic(string topic)
        {
            ChannelTopic = topic;
        }
        public void SetChannelKeyValue(Dictionary<string, string> keyValue)
        {
            foreach (var kv in keyValue)
            {
                if (ChannelKeyValue.ContainsKey(kv.Key))
                {
                    ChannelKeyValue[kv.Key] = kv.Value;
                }
                else
                {
                    ChannelKeyValue.Add(kv.Key, kv.Value);
                }
            }
        }

        public string GetChannelValueString(List<string> keys)
        {
            string values = "";
            foreach (var key in keys)
            {
                if (ChannelKeyValue.ContainsKey(key))
                {
                    values += @"\" + ChannelKeyValue[key];
                }
            }
            return values;
        }
    }
}
