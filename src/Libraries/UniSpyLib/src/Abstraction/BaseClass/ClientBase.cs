using UniSpyServer.UniSpyLib.Abstraction.Interface;

namespace UniSpyServer.UniSpyLib.Abstraction.BaseClass
{
    public abstract class ClientBase
    {
        public ISession Session { get; private set; }
        public ICryptography Encryption { get; private set; }
        public UserInfoBase UserInfo { get; protected set; }
        public ClientBase(ISession session)
        {
            Session = session;
        }
        public ClientBase(ISession session, ICryptography encryption)
        {
            Session = session;
            Encryption = encryption;
        }
    }
}