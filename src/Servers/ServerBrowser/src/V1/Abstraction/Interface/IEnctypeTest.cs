public interface IEnctype1Test : IEnctypeShareTest
{
    void Func1(byte[] data);
    void Func2(byte[] data, int size, byte[] crypt);
    void Func3(byte[] data, int len, byte[] buff);
    void Func4(byte[] id);
    int Func5(int cnt, byte[] id, ref int n1, ref int n2);
    void Func6(byte[] data, int len);
    int Func7(int len);
    void Func8(byte[] data, int len, byte[] enctype1_data);
    byte[] EncKey { get; }
}

public interface IEnctypeShareTest
{
    void Encshare1(uint[] tbuff, int tbuffIndex, byte[] datap, int datapIndex, int len);
    void Encshare2(uint[] tbuff, uint tbuffp, int len);
    void EncShare3(uint[] data, int n1 = 0, int n2 = 0);
    void EncShare4(byte[] src, int size, uint[] dest);
    byte[] ConvertUintToBytes(uint[] input);
    uint[] ConvertBytesToUint(byte[] input);
}

public interface IEnctype2Test : IEnctypeShareTest
{
    int Encoder(byte[] key, byte[] data, int size);

}