namespace UniSpyServer.UniSpyLib.Abstraction.Interface
{
    public interface ICryptography
    {
        byte[] Encrypt(byte[] data);
        byte[] Decrypt(byte[] data);
    }
}