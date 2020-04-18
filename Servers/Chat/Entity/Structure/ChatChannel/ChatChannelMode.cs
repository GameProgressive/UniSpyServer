namespace Chat.Entity.Structure.ChatChannel
{
    public class ChatChannelMode
    {
        public bool IsSupportChannelMode;
        public bool IsStandardChannel;
        public bool IsQuietChannel;
        public bool IsModerate;
        public bool IsAllowOutsideMessage;
        public bool IsInvitedOnly;
        public bool IsAnonymous;
        public bool IsPrivateChannel;
        public bool IsSecretChannel;
        public bool IsReop;

        public ChatChannelMode(bool supportMode)
        {
            IsSupportChannelMode = supportMode;
        }
        public ChatChannelMode SetIsSupportChannelMode()
        {
            IsSupportChannelMode = true;
            return this;
        }

    }
}
