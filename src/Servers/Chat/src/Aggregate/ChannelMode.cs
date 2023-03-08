using System.Text;
using UniSpy.Server.Chat.Contract.Request.Channel;

namespace UniSpy.Server.Chat.Aggregate.Misc.ChannelInfo
{
    public sealed class ChannelMode
    {
        //i - toggle the invite-only channel flag;
        public bool IsInviteOnly { get; private set; } = false;
        //p - toggle the private channel flag;
        public bool IsPrivateChannel { get; private set; } = false;
        //s - toggle the secret channel flag;
        public bool IsSecretChannel { get; private set; } = false;
        //m - toggle the moderated channel;
        public bool IsModeratedChannel { get; private set; } = false;
        //n - toggle the no messages to channel from clients on the outside;
        public bool IsAllowExternalMessage { get; private set; } = false;
        //t - toggle the topic settable by channel operator only flag;
        public bool IsTopicOnlySetByChannelOperator { get; private set; } = true;
        // e - toggle the operator allow channel limits flag;
        public bool IsOperatorAbeyChannelLimits { get; private set; } = true;
        /// <summary>
        /// default constructor
        /// </summary>
        public ChannelMode()
        {
        }

        public void SetChannelModes(ModeOperationType operation)
        {
            switch (operation)
            {
                case ModeOperationType.SetOperatorAbeyChannelLimits:
                    IsOperatorAbeyChannelLimits = true;
                    break;
                case ModeOperationType.RemoveOperatorAbeyChannelLimits:
                    IsOperatorAbeyChannelLimits = false;
                    break;
                case ModeOperationType.SetInvitedOnly:
                    IsInviteOnly = true;
                    break;
                case ModeOperationType.RemoveInvitedOnly:
                    IsInviteOnly = false;
                    break;
                case ModeOperationType.SetPrivateChannelFlag:
                    IsPrivateChannel = true;
                    break;
                case ModeOperationType.RemovePrivateChannelFlag:
                    IsPrivateChannel = false;
                    break;
                case ModeOperationType.SetSecretChannelFlag:
                    IsSecretChannel = true;
                    break;
                case ModeOperationType.RemoveSecretChannelFlag:
                    IsSecretChannel = false;
                    break;
                case ModeOperationType.SetModeratedChannelFlag:
                    IsModeratedChannel = true;
                    break;
                case ModeOperationType.RemoveModeratedChannelFlag:
                    IsModeratedChannel = false;
                    break;
                case ModeOperationType.EnableExternalMessagesFlag:
                    IsAllowExternalMessage = true;
                    break;
                case ModeOperationType.DisableExternalMessagesFlag:
                    IsAllowExternalMessage = false;
                    break;
                case ModeOperationType.SetTopicChangeByOperatorFlag:
                    IsTopicOnlySetByChannelOperator = true;
                    break;
                case ModeOperationType.RemoveTopicChangeByOperatorFlag:
                    IsTopicOnlySetByChannelOperator = false;
                    break;
            }
        }

        public override string ToString()
        {

            var buffer = new StringBuilder();

            buffer.Append("+");
            if (IsInviteOnly)
            {
                buffer.Append("i");
            }
            if (IsPrivateChannel)
            {
                buffer.Append("p");
            }
            if (IsSecretChannel)
            {
                buffer.Append("s");
            }
            if (IsModeratedChannel)
            {
                buffer.Append("m");
            }
            if (IsAllowExternalMessage)
            {
                buffer.Append("n");
            }
            if (IsTopicOnlySetByChannelOperator)
            {
                buffer.Append("t");
            }

            //response is like +nt
            return buffer.ToString();
        }
    }
}
