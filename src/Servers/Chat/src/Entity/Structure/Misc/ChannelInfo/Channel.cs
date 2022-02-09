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
        public ChannelMode Mode { get; private set; }
        public DateTime CreateTime { get; private set; }
        /// <summary>
        /// | key -> Nickname | value -> ChannelUser|
        /// </summary>
        /// <value></value>
        public IDictionary<string, ChannelUser> BanList { get; private set; }
        /// <summary>
        /// | key -> Nickname | value -> ChannelUser|
        /// </summary>
        /// <value></value>
        public IDictionary<string, ChannelUser> Users { get; private set; }
        public IDictionary<string, string> ChannelKeyValue { get; private set; }
        public ChannelUser Creator { get; private set; }
        public bool IsPeerServer { get; set; }
        public string Password { get; private set; }
        public string Topic { get; set; }
        public Channel(string name, ChannelUser creator = null)
        {
            CreateTime = DateTime.Now;
            Mode = new ChannelMode();
            ChannelKeyValue = new ConcurrentDictionary<string, string>();
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
        public bool MultiCast(IResponse message)
        {
            foreach (var kv in Users)
            {
                kv.Value.UserInfo.Session.Send(message);
            }
            LogWriter.LogNetworkMultiCast((string)message.SendingBuffer);
            return true;
        }
        public bool MultiCastExceptSender(ChannelUser sender, IResponse message)
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
            if (!Users.ContainsKey(joiner.UserInfo.NickName))
            {
                Users.TryAdd(joiner.UserInfo.NickName, joiner);
            }

            if (!joiner.UserInfo.JoinedChannels.ContainsKey(this.Name))
            {
                joiner.UserInfo.JoinedChannels.TryAdd(this.Name, this);
            }

        }
        public void RemoveBindOnUserAndChannel(ChannelUser leaver)
        {
            //!! we should use ConcurrentDictionary here
            //!! FIXME: when removing user from channel, 
            //!! we should do more checks on user not only just TryTake()
            if (Users.ContainsKey(leaver.UserInfo.NickName))
            // !! we takeout wrong user from channel
            {
                var kv = new KeyValuePair<string, ChannelUser>(
                    leaver.UserInfo.NickName,
                    Users[leaver.UserInfo.NickName]);
                Users.Remove(kv);
            }

            if (leaver.UserInfo.JoinedChannels.ContainsKey(this.Name))
            {
                var kv = new KeyValuePair<string, Channel>(this.Name, this);
                leaver.UserInfo.JoinedChannels.Remove(kv);
            }

        }

        public ChannelUser GetChannelUserBySession(Session session)
        {
            return Users.Values.Where(u => u.UserInfo.Session.Id == session.Id).FirstOrDefault();
        }
        public bool IsUserBanned(ChannelUser user)
        {
            if (!BanList.ContainsKey(user.UserInfo.NickName))
            {
                return false;
            }
            if (BanList[user.UserInfo.NickName].UserInfo.Session.Id != user.UserInfo.Session.Id)
            {
                return false;
            }
            return true;
        }
        public bool IsUserExisted(ChannelUser user) => Users.ContainsKey(user.UserInfo.NickName);
        public ChannelUser GetChannelUserByNickName(string nickName) => Users.ContainsKey(nickName) == true ? Users[nickName] : null;

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
                        MaxNumberUser = request.LimitNumber;
                        break;
                    case ModeOperationType.RemoveChannelUserLimits:
                        MaxNumberUser = 200;
                        break;
                    case ModeOperationType.AddBanOnUser:
                        AddBanOnUser(request);
                        break;
                    case ModeOperationType.RemoveBanOnUser:
                        RemoveBanOnUser(request);
                        break;
                    case ModeOperationType.AddChannelPassword:
                        Password = request.Password;
                        break;
                    case ModeOperationType.RemoveChannelPassword:
                        Password = null;
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
                    default:
                        Mode.SetChannelModes(op);
                        break;
                }
            }
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
                BanList.Remove(keyValue);
                return;
            }
            if (result.Count() > 1)
            {
                LogWriter.Error($"Multiple user with same nick name in channel {Name}");
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
