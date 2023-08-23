public interface IEnctype1Test : IEnctypeShareTest
{
    void EncryptByEnc0Key(byte[] data, int size, byte[] crypt);
    void CreateEnc0Key(byte[] data, int len, byte[] buff);
    void CreateEnc1Key(byte[] validateKey, byte[] enc1Key);
    int Func5(int cnt, byte[] id, ref int n1, ref int n2, byte[] encKey);
    void EncryptByEnc1Key(byte[] data, int len, byte[] enc1Key);
    int SubstituteEnc1key(int len, byte[] enc1Key);
    void Func8(byte[] data, int len, byte[] enctype1_data);
    byte[] Enc0Key { get; }
    byte[] Enc1Key { get; }
    byte[] ValidateKey { get; }
}

public interface IEnctypeShareTest
{
    void Encshare1(uint[] tbuff, byte[] datap, int datapIndex, int len);
    void Encshare2(uint[] tbuff, uint tbuffp, int len);
    void EncShare3(uint[] data, int n1 = 0, int n2 = 0);
    void EncShare4(byte[] src, int size, uint[] dest);
    byte[] ConvertUintToBytes(uint[] input);
    uint[] ConvertBytesToUint(byte[] input);
}

public interface IEnctype2Test : IEnctypeShareTest
{
    void Encoder(byte[] key, byte[] data, int size);

}