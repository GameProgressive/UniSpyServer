using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using UniSpyServer.Servers.Chat.Entity.Exception;
using UniSpyServer.Servers.Chat.Entity.Structure.Request.Channel;
using UniSpyServer.UniSpyLib.Abstraction.Interface;
using UniSpyServer.UniSpyLib.Encryption;
using UniSpyServer.UniSpyLib.Logging;

namespace UniSpyServer.Servers.Chat.Entity.Structure.Misc.ChannelInfo
{
    public enum PeerRoomType
    {
        /// <summary>
        /// the first channel that a connected user joined at first time
        /// </summary>
        Title,
        /// <summary>
        /// User created room for gaming
        /// </summary>
        Staging,
        /// <summary>
        /// User created room which can be seperated by categories
        /// </summary>
        Group
    }
    public sealed class Channel
    {
        /// <summary>
        /// When game connects to server, the player will enter the default channel for communicating with other players.
        /// </summary>
        public const string TitleRoomPrefix = "#GSP";
        /// <summary>
        /// When a player creates their own game and is waiting for others to join they are placed in a separate chat room called the "staging room"
        /// Staging room have two title seperator like #GSP!xxxx!xxxx
        /// </summary>
        public const string StagingRoomPrefix = "#GSP";
        /// <summary>
        /// group rooms is used split the list of games into categories (by gametype, skill, region, etc.). In this case, when entering the title room, the user would get a list of group rooms instead of a list of games
        /// Group room have one title seperator like #GPG!xxxxxx
        /// </summary>
        public const string GroupRoomPrefix = "#GPG";
        public const char TitleSeperator = '!';
        public static Func<string, PeerRoomType> GetRoomType = (channelName) =>
         {
             if (IsStagingRoom(channelName))
             {
                 return PeerRoomType.Staging;
             }
             else if (IsGroupRoom(channelName))
             {
                 return PeerRoomType.Group;
             }
             else if (IsTitleRoom(channelName))
             {
                 return PeerRoomType.Title;
             }
             throw new ChatException("Invalid channel name");
         };
        private static Func<string, bool> IsStagingRoom = (channelName) =>
        {
            var a = channelName.Count(c => c == TitleSeperator) == 2 ? true : false;
            var b = channelName.StartsWith(StagingRoomPrefix) ? true : false;
            return a && b;
        };
        private static Func<string, bool> IsTitleRoom = (channelName) =>
        {
            var a = channelName.Count(c => c == TitleSeperator) == 1 ? true : false;
            var b = channelName.StartsWith(TitleRoomPrefix) ? true : false;
            return a && b;
        };
        private static Func<string, bool> IsGroupRoom = (channelName) =>
        {
            var a = channelName.Count(c => c == TitleSeperator) == 0 ? true : false;
            var b = channelName.StartsWith(GroupRoomPrefix) ? true : false;
            return a && b;
        };
        /// <summary>
        /// Channel name
        /// </summary>
        /// <value></value>
        public string Name { get; private set; }
        /// <summary>
        /// The maximum number of users that can be in the channel
        /// </summary>
        /// <value></value>
        public int MaxNumberUser { get; private set; } = 200;
        public ChannelMode Mode { get; private set; } = new ChannelMode();
        public DateTime CreateTime { get; private set; } = DateTime.Now;
        /// <summary>
        /// | key -> Nickname | value -> ChannelUser|
        /// </summary>
        /// <value></value>
        public IDictionary<string, ChannelUser> BanList { get; private set; } = new ConcurrentDictionary<string, ChannelUser>();
        /// <summary>
        /// | key -> Nickname | value -> ChannelUser|
        /// </summary>
        /// <value></value>
        public IDictionary<string, ChannelUser> Users { get; private set; } = new ConcurrentDictionary<string, ChannelUser>();
        public IDictionary<string, string> ChannelKeyValue { get; private set; } =
        new ConcurrentDictionary<string, string>();
        public ChannelUser Creator { get; private set; }
        public bool IsPeerServer { get; set; }
        public string Password { get; private set; }
        public string Topic { get; set; }
        public Channel(string name, ChannelUser creator = null)
        {
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
            foreach (var user in Users.Values)
            {
                user.ClientRef.Send(message);
            }
            LogWriter.LogNetworkMultiCast((string)message.SendingBuffer);
            return true;
        }
        public bool MultiCastExceptSender(ChannelUser sender, IResponse message)
        {
            message.Build();
            foreach (var user in Users.Values)
            {
                if (user.ClientRef.Session.RemoteIPEndPoint == sender.ClientRef.Session.RemoteIPEndPoint)
                {
                    continue;
                }
                user.ClientRef.Send(message);
            }

            return true;
        }
        public string GetAllUsersNickString()
        {
            string nicks = "";
            foreach (var user in Users.Values)
            {
                if (user.IsChannelCreator)
                {
                    nicks += "@" + user.ClientRef.Info.NickName + " ";
                }
                else
                {
                    nicks += user.ClientRef.Info.NickName + " ";
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
            if (!Users.ContainsKey(joiner.Info.NickName))
            {
                Users.TryAdd(joiner.Info.NickName, joiner);
            }

            if (!joiner.Info.JoinedChannels.ContainsKey(this.Name))
            {
                joiner.Info.JoinedChannels.TryAdd(this.Name, this);
            }

        }
        public void RemoveBindOnUserAndChannel(ChannelUser leaver)
        {
            //!! we should use ConcurrentDictionary here
            //!! FIXME: when removing user from channel, 
            //!! we should do more checks on user not only just TryTake()
            if (Users.ContainsKey(leaver.Info.NickName))
            // !! we takeout wrong user from channel
            {
                var kv = new KeyValuePair<string, ChannelUser>(
                    leaver.Info.NickName,
                    Users[leaver.Info.NickName]);
                Users.Remove(kv);
            }

            if (leaver.Info.JoinedChannels.ContainsKey(this.Name))
            {
                var kv = new KeyValuePair<string, Channel>(this.Name, this);
                leaver.Info.JoinedChannels.Remove(kv);
            }

        }

        public ChannelUser GetChannelUser(IClient client)
        {
            return Users.Values.Where(u => u.Session.RemoteIPEndPoint == client.Session.RemoteIPEndPoint).FirstOrDefault();
        }
        public bool IsUserBanned(ChannelUser user)
        {
            if (!BanList.ContainsKey(user.Info.NickName))
            {
                return false;
            }
            if (BanList[user.Info.NickName].Session.RemoteIPEndPoint != user.Session.RemoteIPEndPoint)
            {
                return false;
            }
            return true;
        }
        public bool IsUserExisted(ChannelUser user) => Users.ContainsKey(user.Info.NickName);
        public ChannelUser GetChannelUser(string nickName) => Users.ContainsKey(nickName) == true ? Users[nickName] : null;

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
            var result = Users.Values.Where(u => u.Info.NickName == request.NickName);
            if (result.Count() != 1)
            {
                return;
            }
            ChannelUser user = result.First();

            if (BanList.Values.Where(u => u.Info.NickName == request.NickName).Count() == 1)
            {
                return;
            }

            BanList.TryAdd(user.Info.NickName, user);
        }
        private void RemoveBanOnUser(ModeRequest request)
        {
            var result = BanList.Where(u => u.Value.Info.NickName == request.NickName);
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
            var result = Users.Where(u => u.Value.Info.UserName == request.UserName);
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
            var result = Users.Where(u => u.Value.Info.UserName == request.UserName);
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
            var result = Users.Where(u => u.Value.Info.UserName == request.UserName);
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
            var result = Users.Where(u => u.Value.Info.UserName == request.UserName);
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
