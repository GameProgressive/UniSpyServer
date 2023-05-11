using System.Collections.Generic;
using UniSpy.Server.Core.Encryption;
using UniSpy.Server.ServerBrowser.V1.Abstraction.BaseClass;

namespace UniSpy.Server.ServerBrowser.V1.Aggregate
{

    public class Enctype2 : EnctypeBase
    {
        public const int HeaderSize = 8;
        public byte[] GameSecreteKey { get; private set; }
        public Enctype2(string gameSecretKey)
        {
            GameSecreteKey = UniSpyEncoding.GetBytes(gameSecretKey);
        }

        public override byte[] Encrypt(byte[] data)
        {
            var encData = new byte[326];
            var buffer = new List<byte>();
            buffer.AddRange(new byte[8] { 0, 0, 0, 0, 0, 0, 0, 0 });
            buffer.AddRange(encData);



            // at last
            data[0] ^= 0xec;
            throw new System.NotImplementedException();
        }
    }
}