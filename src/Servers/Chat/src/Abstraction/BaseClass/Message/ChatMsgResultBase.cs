namespace UniSpyServer.Servers.Chat.Abstraction.BaseClass.Message
{
    public abstract class MsgResultBase : ResultBase
    {
        public string UserIRCPrefix { get; set; }
        public string TargetName { get; set; }
        protected MsgResultBase(){ }
    }
}