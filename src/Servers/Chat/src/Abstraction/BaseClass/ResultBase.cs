namespace UniSpyServer.Servers.Chat.Abstraction.BaseClass
{
    public abstract class ResultBase : UniSpyLib.Abstraction.BaseClass.ResultBase
    {
        public string IRCErrorCode { get; set; }

        public ResultBase(){ }
    }
}
