using Chat.Entity.Structure.ChatCommand;
using System;
namespace Chat.Entity.Structure.ChatChannel
{
    public class ChatChannelMode
    {
        public bool IsSupportChannelMode { get; protected set; }
        public bool IsStandardChannel { get; protected set; }
        public bool IsQuietChannel { get; protected set; }
        public bool IsModerate { get; protected set; }
        public bool IsAllowOutsideMessage { get; protected set; }
        public bool IsInvitedOnly { get; protected set; }
        public bool IsAnonymous { get; protected set; }
        public bool IsPrivateChannel { get; protected set; }
        public bool IsSecretChannel { get; protected set; }
        public bool IsReop { get; protected set; }

        public ChatChannelMode(bool supportMode = true)
        {
            IsSupportChannelMode = supportMode;
        }

        public bool SetModes(ChatCommandBase cmd)
        {
            throw new NotImplementedException();
        }

        public ChatChannelMode SetSupportChannelMode(bool flag = true)
        {
            IsSupportChannelMode = flag;
            return this;
        }
        public ChatChannelMode SetStandardChannel(bool flag = true)
        {
            IsStandardChannel = flag;
            return this;
        }
        public ChatChannelMode SetQuit(bool flag = true)
        {
            IsQuietChannel = flag;
            return this;
        }

        public ChatChannelMode SetModerate(bool flag = true)
        {
            IsModerate = flag;
            return this;
        }

        public ChatChannelMode SetAllowOutsideMsg(bool flag = true)
        {
            IsAllowOutsideMessage = flag;
            return this;
        }

        public ChatChannelMode SetInvitedOnly(bool flag = true)
        {
            IsInvitedOnly = flag;
            return this;
        }

        public ChatChannelMode SetAnonymous(bool flag = true)
        {
            IsAnonymous = flag;
            return this;
        }

        public ChatChannelMode SetPrivate(bool flag = true)
        {
            IsPrivateChannel = flag;
            return this;
        }
        public ChatChannelMode SetSecretChannel(bool flag = true)
        {
            IsSecretChannel = flag;
            return this;
        }
    }
}
