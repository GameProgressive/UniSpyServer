using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using UniSpyServer.Servers.Chat.Entity.Structure.Request.Channel;
using UniSpyServer.Servers.Chat.Network;
using UniSpyServer.UniSpyLib.Abstraction.Interface;
using UniSpyServer.UniSpyLib.Logging;

namespace UniSpyServer.Servers.Chat.Entity.Structure.Misc.ChannelInfo
{
    public sealed class Channel
    {
        /// <summary>
        /// Channel name
        /// </summary>
        /// <value></value>
        public string Name { get; private set; }
        /// <summary>
        /// The maximum number of users that can be in the channel
        /// </summary>
        /// <value></value>
        public int MaxNumberUser { get; private set; }
        public ChannelMode Mode { get; set; }
        public DateTime CreateTime { get; private set; }
        /// <summary>
        /// | key -> Nickname | value -> ChannelUser|
        /// </summary>
        /// <value></value>
        public ConcurrentDictionary<string, ChannelUser> BanList { get; set; }
        /// <summary>
        /// | key -> Nickname | value -> ChannelUser|
        /// </summary>
        /// <value></value>
        public ConcurrentDictionary<string, ChannelUser> Users { get; set; }
        public string Password { get; set; }
        public Dictionary<string, string> ChannelKeyValue { get; private set; }
        public string Topic { get; set; }
        public bool IsPeerServer { get; set; }
        public ChannelUser Creator { get; private set; }
        public Channel(string name, ChannelUser creator = null)
        {
            CreateTime = DateTime.Now;
            Mode = new ChannelMode();
            ChannelKeyValue = new Dictionary<string, string>();
            BanList = new ConcurrentDictionary<string, ChannelUser>();
            Users = new ConcurrentDictionary<string, ChannelUser>();
            MaxNumberUser = 200;
            Name = name;
            Mode.SetDefaultModes();
            Creator = creator;
        }

        /// <summary>
        /// Send message to all users in this channel
        /// except the sender
        /// </summary>
        /// <returns></returns>
        public bool MultiCast(IUniSpyResponse message)
        {
            foreach (var kv in Users)
            {
                kv.Value.UserInfo.Session.Send(message);
            }
            LogWriter.LogNetworkMultiCast((string)message.SendingBuffer);
            return true;
        }
        public bool MultiCastExceptSender(ChannelUser sender, IUniSpyResponse message)
        {
            foreach (var kv in Users)
            {
                if (kv.Value.UserInfo.Session.Id == sender.UserInfo.Session.Id)
                {
                    continue;
                }
                kv.Value.UserInfo.Session.Send(message);
            }

            return true;
        }
        public string GetAllUsersNickString()
        {
            string nicks = "";
            foreach (var kv in Users)
            {
                if (kv.Value.IsChannelCreator)
                {
                    nicks += "@" + kv.Value.UserInfo.NickName + " ";
                }
                else
                {
                    nicks += kv.Value.UserInfo.NickName + " ";
                }
            }
            //if user equals last user in channel we do not add space after it
            nicks = nicks.Substring(0, nicks.Length - 1);
            return nicks;
        }
        public void AddBindOnUserAndChannel(ChannelUser joiner)
        {
            // !! we can not directly use the Contains() method that ConcurrentDictionary or 
            // !! ConcurrentBag provide because it will not work properly.
            if (!Users.Keys.Contains(joiner.UserInfo.NickName))
            {
                Users.TryAdd(joiner.UserInfo.NickName, joiner);
            }

            if (!joiner.UserInfo.JoinedChannels.Keys.Contains(this.Name))
            {
                joiner.UserInfo.JoinedChannels.TryAdd(this.Name, this);
            }

        }
        public void RemoveBindOnUserAndChannel(ChannelUser leaver)
        {
            //!! we should use ConcurrentDictionary here
            //!! FIXME: when removing user from channel, 
            //!! we should do more checks on user not only just TryTake()
            if (Users.Keys.Contains(leaver.UserInfo.NickName))
            // !! we takeout wrong user from channel
            {
                var kv = new KeyValuePair<string, ChannelUser>(
                    leaver.UserInfo.NickName,
                    Users[leaver.UserInfo.NickName]);
                Users.TryRemove(kv);
            }

            if (leaver.UserInfo.JoinedChannels.Keys.Contains(this.Name))
            {
                var kv = new KeyValuePair<string, Channel>(this.Name, this);
                leaver.UserInfo.JoinedChannels.TryRemove(kv);
            }

        }

        public ChannelUser GetChannelUserBySession(Session session)
        {
            return Users.Values.Where(u => u.UserInfo.Session.Id == session.Id).FirstOrDefault();
        }
        public bool IsUserBanned(ChannelUser user)
        {
            if (!BanList.Keys.Contains(user.UserInfo.NickName))
            {
                return false;
            }
            if (BanList[user.UserInfo.NickName].UserInfo.Session.Id != user.UserInfo.Session.Id)
            {
                return false;
            }
            return true;
        }
        public bool IsUserBanned(Session session)
        {
            if (BanList.Keys.Contains(session.UserInfo.NickName))
            {
                var resultUser = BanList[session.UserInfo.NickName];
                if (resultUser.UserInfo.Session.Id == session.Id)
                {
                    return true;
                }
            }

            return false;
        }
        public bool IsUserExisted(ChannelUser user) => Users.Keys.Contains(user.UserInfo.NickName);

        public ChannelUser GetChannelUserByNickName(string nickName)
        {
            if (Users.Keys.Contains(nickName))
            {
                return Users[nickName];
            }
            else
            {
                return null;
            }
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
            // todo check permission of each operation
            foreach (var op in request.ModeOperations)
            {
                switch (op)
                {
                    case ModeOperationType.AddChannelUserLimits:
                        AddChannelUserLimits(request);
                        break;
                    case ModeOperationType.RemoveChannelUserLimits:
                        RemoveChannelUserLimits(request);
                        break;
                    case ModeOperationType.AddBanOnUser:
                        AddBanOnUser(request);
                        break;
                    case ModeOperationType.RemoveBanOnUser:
                        RemoveBanOnUser(request);
                        break;
                    case ModeOperationType.AddChannelPassword:
                        AddChannelPassword(request);
                        break;
                    case ModeOperationType.RemoveChannelPassword:
                        RemoveChannelPassword(request);
                        break;
                    case ModeOperationType.AddChannelOperator:
                        AddChannelOperator(request);
                        break;
                    case ModeOperationType.RemoveChannelOperator:
                        RemoveChannelOperator(request);
                        break;
                    case ModeOperationType.EnableUserVoicePermission:
                        EnableUserVoicePermission(request);
                        break;
                    case ModeOperationType.DisableUserVoicePermission:
                        DisableUserVoicePermission(request);
                        break;
                    case ModeOperationType.SetChannelModesWithUserLimit:
                        AddChannelUserLimits(request);
                        goto default;
                    default:
                        Mode.ChangeModes(request);
                        break;
                }
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
            var result = Users.Values.Where(u => u.UserInfo.NickName == request.NickName);
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
                LogWriter.Error($"Multiple user with same nick name in channel {Name}");
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
            var result = Users.Where(u => u.Value.UserInfo.UserName == request.UserName);
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
            var result = Users.Where(u => u.Value.UserInfo.UserName == request.UserName);
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
            var result = Users.Where(u => u.Value.UserInfo.UserName == request.UserName);
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
            var result = Users.Where(u => u.Value.UserInfo.UserName == request.UserName);
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
