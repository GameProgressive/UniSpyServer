namespace Chat.Abstraction.BaseClass.Message
{
    internal abstract class MsgResultBase : ResultBase
    {
        public string UserIRCPrefix { get; set; }
        public string TargetName { get; set; }
        protected MsgResultBase()
        {
        }
    }
}