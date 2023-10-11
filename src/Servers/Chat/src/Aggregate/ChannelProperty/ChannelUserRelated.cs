using Newtonsoft.Json;
using System.Collections.Concurrent;
using System.Linq;
using UniSpy.Server.Chat.Abstraction.Interface;
using UniSpy.Server.Chat.Contract.Request.Channel;
using UniSpy.Server.Chat.Error.IRC.General;
using UniSpy.Server.Core.Abstraction.Interface;

namespace UniSpy.Server.Chat.Aggregate
{
    public partial class Channel
    {
        /// <summary>
        /// | key -> Nickname | value -> ChannelUser|
        /// </summary>
        [JsonProperty]
        public ConcurrentDictionary<string, ChannelUser> BanList { get; private set; } = new();
        /// <summary>
        /// | key -> Nickname | value -> ChannelUser|
        /// </summary>
        [JsonProperty]
        public ConcurrentDictionary<string, ChannelUser> Users { get; private set; } = new();
        [JsonProperty]
        public string _creatorNickName { get; private set; }
        [JsonIgnore]
        public ChannelUser Creator
        {
            get
            {
                if (Users.Values.Where(u => u.Client.Info.NickName == _creatorNickName).Count() == 1)
                {
                    return Users[_creatorNickName];
                }
                else
                {
                    return null;
                }
            }
        }
        private void BanUser(ModeRequest request)
        {
            var result = Users.Values.Where(u => u.Client.Info.NickName == request.NickName);
            if (result.Count() != 1)
            {
                return;
            }
            ChannelUser user = result.First();

            if (BanList.Values.Where(u => u.Client.Info.NickName == request.NickName).Count() == 1)
            {
                return;
            }

            BanList.TryAdd(user.Client.Info.NickName, user);
        }
        private void UnBanUser(ModeRequest request)
        {
            var result = BanList.Where(u => u.Value.Client.Info.NickName == request.NickName);
            if (result.Count() == 1)
            {
                var keyValue = result.First();
                BanList.TryRemove(keyValue);
                return;
            }
            if (result.Count() > 1)
            {
                throw new ErrOneUSNickNameException("Multiple user with same nick name in channel {Name}");
            }
        }

        private void AddChannelOperator(ModeRequest request)
        {
            //check whether this user is in this channel
            var result = Users.Where(u => u.Value.Client.Info.UserName == request.UserName);
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
            var result = Users.Where(u => u.Value.Client.Info.UserName == request.UserName);
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
            var result = Users.Where(u => u.Value.Client.Info.UserName == request.UserName);
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
            var result = Users.Where(u => u.Value.Client.Info.UserName == request.UserName);
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
        public ChannelUser GetUser(string nickName) => Users.ContainsKey(nickName) == true ? Users[nickName] : null;
        public ChannelUser GetUser(IShareClient client) => Users.Values.FirstOrDefault(u => u.Client.Connection.RemoteIPEndPoint == client.Connection.RemoteIPEndPoint);
        public ChannelUser AddUser(IShareClient client, string password = null, bool isChannelCreator = false, bool isChannelOperator = false)
        {
            Validation(client, password);
            var user = new ChannelUser(client, this);
            switch (RoomType)
            {
                case PeerRoomType.Normal:
                case PeerRoomType.Staging:
                    // user created room
                    user.IsChannelCreator = isChannelCreator;
                    user.IsChannelOperator = isChannelOperator;
                    break;
            }
            AddBindOnUserAndChannel(user);
            return user;
        }

        public void RemoveUser(ChannelUser user)
        {
            user.Client.Info.PreviousJoinedChannel = Name;
            RemoveBindOnUserAndChannel(user);
        }

        public bool IsUserExisted(ChannelUser user) => IsUserExisted(user.Client);
        public bool IsUserExisted(IShareClient client) => Users.ContainsKey(client.Info.NickName);
        public bool IsUserBanned(ChannelUser user) => IsUserBanned(user.Client);
        private bool IsUserBanned(IShareClient client)
        {
            if (!BanList.ContainsKey(client.Info.NickName))
            {
                return false;
            }
            if (BanList[client.Info.NickName].Client.Connection.RemoteIPEndPoint != client.Connection.RemoteIPEndPoint)
            {
                return false;
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
                    nicks += $"@{user.Client.Info.NickName}";
                }
                else
                {
                    nicks += user.Client.Info.NickName;
                }

                if (!user.Equals(Users.Values.Last()))
                {
                    nicks += " ";
                }
            }
            return nicks;
        }
        public static void AddBindOnUserAndChannel(ChannelUser joiner)
        {
            joiner.Channel.Users.TryAdd(joiner.Client.Info.NickName, joiner);
            joiner.Client.Info.JoinedChannels.TryAdd(joiner.Channel.Name, joiner.Channel);
        }
        public static void RemoveBindOnUserAndChannel(ChannelUser leaver)
        {
            leaver.Channel.Users.TryRemove(leaver.Client.Info.NickName, out _);
            leaver.Client.Info.JoinedChannels.Remove(leaver.Channel.Name, out _);
        }
        /// <summary>
        /// Send message to all users in this channel
        /// except the sender
        /// </summary>
        public void MultiCast(IClient sender, IResponse message, bool isSkipSender = false)
        {
            var users = Users.Values.Where(u => !u.IsRemoteClient).ToList();
            foreach (var user in users)
            {
                if (isSkipSender
                && sender.Connection.RemoteIPEndPoint.Equals(user.RemoteIPEndPoint))
                {
                    continue;
                }
                user.Client.Send(message);
            }
        }
    }
}