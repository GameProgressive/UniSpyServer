namespace UniSpyServer.Servers.NatNegotiation.Entity.Structure
{
    public class GameInfo
    {
        public int ID;
        public string Name;
        public string Secretkey;
        public ushort QueryPort;
        public ushort BackendFlags;
        public uint ServicesDisabled;
        public KeyData PushKeys;
        public byte NumPushKeys; //sb protocol sends as a byte so max of 255
    }

    public class KeyData
    {
        public byte Name;
        public byte Type;
    }
}
