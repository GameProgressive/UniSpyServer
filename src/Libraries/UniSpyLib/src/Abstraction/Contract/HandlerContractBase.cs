namespace UniSpyServer.UniSpyLib.Abstraction.Contract
{
    /// <summary>
    /// Represents a discription for handler.
    /// </summary>
    public abstract class HandlerContractBase : CmdContractBase
    {
        public HandlerContractBase(object name) : base(name)
        {
        }
    }
}