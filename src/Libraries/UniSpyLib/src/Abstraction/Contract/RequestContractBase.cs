namespace UniSpyServer.UniSpyLib.Abstraction.Contract
{
    /// <summary>
    /// Represents a discription for request data.
    /// </summary>
    public abstract class RequestContractBase : CmdContractBase
    {
        public RequestContractBase(object name) : base(name)
        {
        }
    }
}