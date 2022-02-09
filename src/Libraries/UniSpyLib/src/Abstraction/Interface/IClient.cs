namespace UniSpyServer.UniSpyLib.Abstraction.Interface
{
    public interface IClient
    {
        // we store client info here
        ISession Session { get; }
        IEncryption Encryption { get; }
    }
}