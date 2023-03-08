using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using UniSpy.Server.Chat.Abstraction.Interface;
using UniSpy.Server.Chat.Contract.Request.Channel;
using UniSpy.Server.Chat.Exception;
using UniSpy.Server.Chat.Exception.IRC.Channel;
using UniSpy.Server.Core.Abstraction.Interface;
using UniSpy.Server.Core.Logging;

namespace UniSpy.Server.Chat.Aggregate.Misc.ChannelInfo
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
        public int MaxNumberUser { get; private set; } = 200;
        public ChannelMode Mode { get; private set; } = new ChannelMode();
        public DateTime CreateTime { get; private set; } = DateTime.Now;
        /// <summary>
        /// | key -> Nickname | value -> ChannelUser|
        /// </summary>
        public IDictionary<string, ChannelUser> BanList { get; private set; } = new ConcurrentDictionary<string, ChannelUser>();
        /// <summary>
        /// | key -> Nickname | value -> ChannelUser|
        /// </summary>
        public ConcurrentDictionary<string, ChannelUser> Users { get; private set; } = new ConcurrentDictionary<string, ChannelUser>();
        /// <summary>
        /// Channel key values
        /// </summary>
        public KeyValueManager KeyValues { get; private set; } = new KeyValueManager();
        public ChannelUser Creator { get; private set; }
        public PeerRoomType RoomType { get; private set; }
        public string Password { get; private set; }
        public string Topic { get; set; }
        public Redis.ChatMessageChannel MessageBroker { get; private set; }
        public Channel(string name, IChatClient client, string password = null)
        {
            Name = name;
            Password = password;
            RoomType = PeerRoom.GetRoomType(Name);
            switch (RoomType)
            {
                case PeerRoomType.Normal:
                case PeerRoomType.Staging:
                    // user created room
                    Creator = AddUser(client, password, true, true);
                    break;
                case PeerRoomType.Title:
                case PeerRoomType.Group:
                    // official room can not create by user
                    AddUser(client, password);
                    break;
            }
            MessageBroker = new Redis.ChatMessageChannel(Name);
        }

        /// <summary>
        /// Send message to all users in this channel
        /// except the sender
        /// </summary>
        /// <returns></returns>
        public void MultiCast(IClient sender, IResponse message, bool isSkipSender = false)
        {
            foreach (var user in Users.Values)
            {
                if (user.IsRemoteUser)
                {
                    continue;
                }
                if (isSkipSender)
                {
                    if (user.RemoteIPEndPoint.Equals(sender.Connection.RemoteIPEndPoint))
                    {
                        continue;
                    }
                }
                user.ClientRef.Send(message);
            }
        }
        public string GetAllUsersNickString()
        {
            string nicks = "";
            foreach (var user in Users.Values)
            {
                if (user.IsChannelCreator)
                {
                    nicks += $"@{user.ClientRef.Info.NickName}";
                }
                else
                {
                    nicks += user.ClientRef.Info.NickName;
                }
                if (!user.Equals(Users.Values.Last()))
                {
                    nicks += " ";
                }
            }
            return nicks;
        }
        private void AddBindOnUserAndChannel(ChannelUser joiner)
        {
            Users.TryAdd(joiner.Info.NickName, joiner);
            joiner.Info.JoinedChannels.TryAdd(this.Name, this);
        }
        private void RemoveBindOnUserAndChannel(ChannelUser leaver)
        {
            Users.TryRemove(leaver.Info.NickName, out _);
            leaver.Info.JoinedChannels.TryRemove(Name, out _);
        }

        public ChannelUser GetChannelUser(IChatClient client) => Users.Values.FirstOrDefault(u => u.Connection.RemoteIPEndPoint == client.Connection.RemoteIPEndPoint);
        public bool IsUserBanned(ChannelUser user) => IsUserBanned(user.ClientRef);
        private bool IsUserBanned(IChatClient client)
        {
            if (!BanList.ContainsKey(client.Info.NickName))
            {
                return false;
            }
            if (BanList[client.Info.NickName].Connection.RemoteIPEndPoint != client.Connection.RemoteIPEndPoint)
            {
                return false;
            }
            return true;
        }
        public bool IsUserExisted(ChannelUser user) => IsUserExisted(user.ClientRef);
        public bool IsUserExisted(IChatClient client) => Users.ContainsKey(client.Info.NickName);
        private void Validation(IChatClient client, string password)
        {
            if (Mode.IsInviteOnly)
            {
                //invited only
                throw new IRCChannelException("This is an invited only channel.", IRCErrorCode.InviteOnlyChan, Name);
            }
            if (IsUserBanned(client))
            {
                throw new IRCBannedFromChanException($"You are banned from this channel:{Name}.", Name);
            }
            if (IsUserExisted(client))
            {
                throw new ChatException($"{client.Info.NickName} is already in channel {Name}");
            }
            if (client.Info.IsJoinedChannel(Name))
            {
                // we do not send anything to this user and users in this channel
                throw new ChatException($"User: {client.Info.NickName} is already joined the channel: {Name}");
            }
            if (Password is not null)
            {
                if (password is null)
                {
                    throw new ChatException("You must input password to join this channel.");
                }
                if (Password != password)
                {
                    throw new ChatException("Password is not correct");
                }
            }
        }
        public ChannelUser AddUser(IChatClient client, string password = null, bool IsChannelCreator = false, bool IsChannelOperator = false)
        {
            Validation(client, password);
            var user = new ChannelUser(client, this);
            user.SetDefaultProperties(IsChannelCreator, IsChannelOperator);
            AddBindOnUserAndChannel(user);
            return user;
        }
        public void VerifyPassword(string pass)
        {
            if (Password != pass)
            {
                throw new ChatException("Password is not correct");
            }
        }
        public void RemoveUser(IChatClient client)
        {
            var user = GetChannelUser(client);

            if (user is not null)
            {
                RemoveUser(user);
            }
        }
        public void RemoveUser(ChannelUser user)
        {
            RemoveBindOnUserAndChannel(user);
        }
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
                        BanUser(request);
                        break;
                    case ModeOperationType.RemoveBanOnUser:
                        UnBanUser(request);
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
        private void BanUser(ModeRequest request)
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
        private void UnBanUser(ModeRequest request)
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
                LogWriter.LogError($"Multiple user with same nick name in channel {Name}");
            }
        }

        private void AddChannelOperator(ModeRequest request)
        {
            //check whether this user is in this channel
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
    }
}
