using System;

namespace UniSpyLib.Abstraction.BaseClass.Contract
{
    /// <summary>
    /// Represents a discription for handler.
    /// </summary>
    public abstract class HandlerContractBase : CmdContractBase
    {
        protected HandlerContractBase(object name) : base(name)
        {
        }
    }
}