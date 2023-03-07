namespace UniSpy.Server.Chat.Abstraction.BaseClass.Message
{
    public abstract class MessageResultBase : ResultBase
    {
        public string UserIRCPrefix { get; set; }
        public string TargetName { get; set; }
        protected MessageResultBase(){ }
    }
}