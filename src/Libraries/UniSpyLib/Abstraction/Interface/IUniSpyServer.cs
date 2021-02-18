using System;
namespace UniSpyLib.Abstraction.Interface
{
    public interface IUniSpyServer
    {
        public Guid ServerID { get; }
        public bool Start();
    }
}
