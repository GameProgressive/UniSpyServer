using UniSpy.Server.Chat.Abstraction.Interface;
using UniSpy.Server.Chat.Aggregate.Misc;
using UniSpy.Server.Chat.Error.IRC.Channel;

namespace UniSpy.Server.Chat.Aggregate
{
    public partial class Channel
    {
        public void VerifyPassword(string pass)
        {
            if (Password != pass)
            {
                throw new Chat.Exception("Password is not correct");
            }
        }
        private void Validation(IChatClient client, string password)
        {
            if (Mode.IsInviteOnly)
            {
                //invited only
                throw new IRCChannelException("This is an invited only channel.", IRCErrorCode.InviteOnlyChan, Name);
            }
            if (IsUserBanned(client))
            {
                throw new BannedFromChanException($"You are banned from this channel:{Name}.", Name);
            }
            if (IsUserExisted(client))
            {
                throw new Chat.Exception($"{client.Info.NickName} is already in channel {Name}");
            }
            if (client.Info.IsJoinedChannel(Name))
            {
                // we do not send anything to this user and users in this channel
                throw new Chat.Exception($"User: {client.Info.NickName} is already joined the channel: {Name}");
            }
            if (Password is not null)
            {
                if (password is null)
                {
                    throw new Chat.Exception("You must input password to join this channel.");
                }
                if (Password != password)
                {
                    throw new Chat.Exception("Password is not correct");
                }
            }

            if (Mode.IsInviteOnly)
            {
                if (!Mode.InviteNickNames.Contains(client.Info.NickName))
                {
                    throw new InviteOnlyChanException("You must invited to this channel", Name);
                }
                else
                {
                    // user is already join, we remove him from list
                    Mode.InviteNickNames.Remove(client.Info.NickName);
                }
            }
        }
    }
}