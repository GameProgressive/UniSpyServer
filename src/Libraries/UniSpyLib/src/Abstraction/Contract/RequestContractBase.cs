namespace UniSpyServer.UniSpyLib.Abstraction.Contract
{
    /// <summary>
    /// Represents a discription for request data.
    /// </summary>
    public abstract class RequestContractBase : CmdContractBase
    {
        protected RequestContractBase(object name) : base(name)
        {
        }
    }
}