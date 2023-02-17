namespace UniSpy.Server.Chat.Abstraction.BaseClass
{
    public abstract class ResultBase : UniSpy.Server.Core.Abstraction.BaseClass.ResultBase
    {
        public string IRCErrorCode { get; set; }

        public ResultBase(){ }
    }
}
