namespace UniSpyServer.UniSpyLib.Abstraction.Interface
{
    public interface IClient
    {
        // we store client info here
        ISession Session { get; }
        ICryptography Crypto { get; }
    }
}