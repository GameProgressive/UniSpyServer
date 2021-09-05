namespace Chat.Abstraction.BaseClass.Message
{
    internal abstract class ChatMsgResultBase : ChatResultBase
    {
        public string UserIRCPrefix { get; set; }
        public string TargetName { get; set; }
        protected ChatMsgResultBase()
        {
        }
    }
}