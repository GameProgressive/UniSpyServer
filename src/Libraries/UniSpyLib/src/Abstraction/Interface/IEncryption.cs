namespace UniSpyServer.UniSpyLib.Abstraction.Interface
{
    public interface IEncryption
    {
        byte[] Encrypt(byte[] data);
        byte[] Decrypt(byte[] data);
    }
}