using System;

namespace UniSpyServer.UniSpyLib.Abstraction.BaseClass.Contract
{
    public abstract class CmdContractBase : Attribute
    {
        public object Name { get; }
        public CmdContractBase(object name)
        {
            Name = name;
        }
    }
}
