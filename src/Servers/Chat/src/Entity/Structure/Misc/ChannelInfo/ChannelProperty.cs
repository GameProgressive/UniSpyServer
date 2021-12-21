using UniSpyServer.Servers.Chat.Entity.Structure.Request;
using UniSpyServer.Servers.Chat.Entity.Structure.Request.Channel;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using UniSpyServer.UniSpyLib.Logging;

namespace UniSpyServer.Servers.Chat.Entity.Structure.Misc.ChannelInfo
{
    public sealed class ChannelProperty
    {
        public string ChannelName { get; private set; }
        public uint MaxNumberUser { get; private set; }
        public ChannelMode ChannelMode { get; set; }
        public DateTime ChannelCreatedTime { get; private set; }
        /// <summary>
        /// | key -> Nickname | value -> ChannelUser|
        /// </summary>
        /// <value></value>
        public ConcurrentDictionary<string, ChannelUser> BanList { get; set; }
        /// <summary>
        /// | key -> Nickname | value -> ChannelUser|
        /// </summary>
        /// <value></value>
        public ConcurrentDictionary<string, ChannelUser> ChannelUsers { get; set; }
        public string Password { get; set; }
        public Dictionary<string, string> ChannelKeyValue { get; private set; }
        public string ChannelTopic { get; set; }
        public bool IsPeerServer { get; set; }

        public ChannelProperty()
        {
            ChannelCreatedTime = DateTime.Now;
            ChannelMode = new ChannelMode();
            ChannelKeyValue = new Dictionary<string, string>();
            BanList = new ConcurrentDictionary<string, ChannelUser>();
            ChannelUsers = new ConcurrentDictionary<string, ChannelUser>();
        }

        public void SetDefaultProperties(ChannelUser creator, JoinRequest request)
        {
            MaxNumberUser = 200;
            ChannelName = request.ChannelName;
            ChannelMode.SetDefaultModes();
        }

        /// <summary>
        /// We only care about how to set mode in this channel
        /// we do not need to care about if the user is legal
        /// because MODEHandler will check for us
        /// </summary>
        /// <param name="changer"></param>
        /// <param name="cmd"></param>
        public void SetProperties(ChannelUser changer, ModeRequest request)
        {
            switch (request.RequestType)
            {
                case ModeRequestType.AddChannelUserLimits:
                    AddChannelUserLimits(request);
                    break;
                case ModeRequestType.RemoveChannelUserLimits:
                    RemoveChannelUserLimits(request);
                    break;
                case ModeRequestType.AddBanOnUser:
                    AddBanOnUser(request);
                    break;
                case ModeRequestType.RemoveBanOnUser:
                    RemoveBanOnUser(request);
                    break;
                case ModeRequestType.AddChannelPassword:
                    AddChannelPassword(request);
                    break;
                case ModeRequestType.RemoveChannelPassword:
                    RemoveChannelPassword(request);
                    break;
                case ModeRequestType.AddChannelOperator:
                    AddChannelOperator(request);
                    break;
                case ModeRequestType.RemoveChannelOperator:
                    RemoveChannelOperator(request);
                    break;
                case ModeRequestType.EnableUserVoicePermission:
                    EnableUserVoicePermission(request);
                    break;
                case ModeRequestType.DisableUserVoicePermission:
                    DisableUserVoicePermission(request);
                    break;
                case ModeRequestType.SetChannelModesWithUserLimit:
                    AddChannelUserLimits(request);
                    goto default;
                default:
                    ChannelMode.ChangeModes(request);
                    break;
            }
        }

        private void AddChannelUserLimits(ModeRequest request)
        {
            MaxNumberUser = request.LimitNumber;
        }

        private void RemoveChannelUserLimits(ModeRequest request)
        {
            MaxNumberUser = 200;
        }
        private void AddBanOnUser(ModeRequest request)
        {
            var result = ChannelUsers.Values.Where(u => u.UserInfo.NickName == request.NickName);
            if (result.Count() != 1)
            {
                return;
            }
            ChannelUser user = result.First();

            if (BanList.Values.Where(u => u.UserInfo.NickName == request.NickName).Count() == 1)
            {
                return;
            }

            BanList.TryAdd(user.UserInfo.NickName, user);
        }
        private void RemoveBanOnUser(ModeRequest request)
        {
            var result = BanList.Where(u => u.Value.UserInfo.NickName == request.NickName);
            if (result.Count() == 1)
            {
                var keyValue = result.First();
                BanList.TryRemove(keyValue);
                return;
            }
            if (result.Count() > 1)
            {
                LogWriter.Error($"Multiple user with same nick name in channel {ChannelName}");
            }
        }
        private void AddChannelPassword(ModeRequest request)
        {
            if (Password == null)
            {
                Password = request.Password;
            }
        }
        private void RemoveChannelPassword(ModeRequest request)
        {
            if (Password == request.Password)
            {
                Password = null;
            }
        }
        private void AddChannelOperator(ModeRequest request)
        {
            // check whether this user is in this channel
            var result = ChannelUsers.Where(u => u.Value.UserInfo.UserName == request.UserName);
            if (result.Count() != 1)
            {
                return;
            }
            var kv = result.First();

            //if this user is already in operator we do not add it
            if (kv.Value.IsChannelOperator)
            {
                return;
            }
            kv.Value.IsChannelOperator = true;
        }

        private void RemoveChannelOperator(ModeRequest request)
        {
            var result = ChannelUsers.Where(u => u.Value.UserInfo.UserName == request.UserName);
            if (result.Count() != 1)
            {
                return;
            }
            var keyValue = result.First();

            if (keyValue.Value.IsChannelCreator)
            {
                keyValue.Value.IsChannelCreator = false;
            }
        }

        private void EnableUserVoicePermission(ModeRequest request)
        {
            var result = ChannelUsers.Where(u => u.Value.UserInfo.UserName == request.UserName);
            if (result.Count() != 1)
            {
                return;
            }

            var kv = result.First();

            if (kv.Value.IsVoiceable)
            {
                kv.Value.IsVoiceable = true;
            }

        }
        private void DisableUserVoicePermission(ModeRequest request)
        {
            var result = ChannelUsers.Where(u => u.Value.UserInfo.UserName == request.UserName);
            if (result.Count() != 1)
            {
                return;
            }

            var kv = result.First();
            if (kv.Value.IsVoiceable)
            {
                kv.Value.IsVoiceable = false;
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
