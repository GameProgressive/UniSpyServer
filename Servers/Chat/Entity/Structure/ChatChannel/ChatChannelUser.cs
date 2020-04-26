using Chat.Entity.Structure.ChatUser;
using Chat.Server;

namespace Chat.Entity.Structure.ChatChannel
{
    public class ChatChannelUser
    {
        public ChatChannelUser(ChatSession session)
        {
            Session = session;
            UserInfo = session.UserInfo;
        }
        public bool IsVoiceable { get; protected set; }
        public bool IsChannelCreator { get; protected set; }
        public bool IsChannelOperator { get; protected set; }
        public ChatSession Session { get; protected set; }
        public ChatUserInfo UserInfo { get; protected set; }

        public void SetDefaultProperties()
        {
            IsVoiceable = true;
            IsChannelCreator = false;
            IsChannelOperator = false;
        }

        public void SetCreatorProperties()
        {
            IsVoiceable = true;
            IsChannelCreator = true;
            IsChannelOperator = true;
        }

        public ChatChannelUser SetVoicePermission(bool flag = true)
        {
            IsVoiceable = flag;
            return this;
        }
        public ChatChannelUser SetChannelCreator(bool flag = false)
        {
            IsChannelCreator = flag;
            return this;
        }
        public ChatChannelUser SetChannelOperator(bool flag = false)
        {
            IsChannelOperator = flag;
            return this;
        }
    }
}
