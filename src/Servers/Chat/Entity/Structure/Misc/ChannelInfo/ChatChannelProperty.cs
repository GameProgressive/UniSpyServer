using Chat.Entity.Structure.Request;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using UniSpyLib.Logging;

namespace Chat.Entity.Structure.Misc.ChannelInfo
{
    internal sealed class ChatChannelProperty
    {
        public string ChannelName { get; private set; }
        public uint MaxNumberUser { get; private set; }
        public ChatChannelMode ChannelMode { get; set; }
        public DateTime ChannelCreatedTime { get; private set; }
        public ConcurrentBag<ChatChannelUser> BanList { get; set; }
        public ConcurrentBag<ChatChannelUser> ChannelUsers { get; set; }
        public string Password { get; set; }
        public Dictionary<string, string> ChannelKeyValue { get; private set; }
        public string ChannelTopic { get; set; }
        public bool IsPeerServer { get; set; }

        public ChatChannelProperty()
        {
            ChannelCreatedTime = DateTime.Now;
            ChannelMode = new ChatChannelMode();
            ChannelKeyValue = new Dictionary<string, string>();
            BanList = new ConcurrentBag<ChatChannelUser>();
            ChannelUsers = new ConcurrentBag<ChatChannelUser>();
        }

        public void SetDefaultProperties(ChatChannelUser creator, JoinRequest cmd)
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
        public void SetProperties(ChatChannelUser changer, ModeRequest cmd)
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

        private void AddChannelUserLimits(ModeRequest cmd)
        {
            MaxNumberUser = cmd.LimitNumber;
        }

        private void RemoveChannelUserLimits(ModeRequest cmd)
        {
            MaxNumberUser = 200;
        }
        private void AddBanOnUser(ModeRequest cmd)
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
        private void RemoveBanOnUser(ModeRequest cmd)
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
                LogWriter.Error($"Multiple user with same nick name in channel {ChannelName}");
            }
        }
        private void AddChannelPassword(ModeRequest cmd)
        {
            if (Password == null)
            {
                Password = cmd.Password;
            }
        }
        private void RemoveChannelPassword(ModeRequest cmd)
        {
            if (Password == cmd.Password)
            {
                Password = null;
            }
        }
        private void AddChannelOperator(ModeRequest cmd)
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
            user.IsChannelOperator = true;
        }

        private void RemoveChannelOperator(ModeRequest cmd)
        {
            var result = ChannelUsers.Where(u => u.UserInfo.UserName == cmd.UserName);
            if (result.Count() != 1)
            {
                return;
            }
            ChatChannelUser user = result.First();

            if (user.IsChannelCreator)
            {
                user.IsChannelCreator = false;
            }
        }

        private void EnableUserVoicePermission(ModeRequest cmd)
        {
            var result = ChannelUsers.Where(u => u.UserInfo.UserName == cmd.UserName);
            if (result.Count() != 1)
            {
                return;
            }

            ChatChannelUser user = result.First();

            if (user.IsVoiceable)
            {
                user.IsVoiceable = true;
            }

        }
        private void DisableUserVoicePermission(ModeRequest cmd)
        {
            var result = ChannelUsers.Where(u => u.UserInfo.UserName == cmd.UserName);
            if (result.Count() != 1)
            {
                return;
            }

            var user = result.First();
            if (user.IsVoiceable)
            {
                user.IsVoiceable = false;
            }
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
