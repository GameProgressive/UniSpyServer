using System.Security.Cryptography;
using Xunit;

namespace UniSpy.Server.WebServer.Test.Auth
{
    public class RSATest
    {
        [Fact]
        public void KeyGen()
        {
            using (var rsa = new RSACryptoServiceProvider(1024))
            {
                var privateKey = rsa.ExportParameters(true);

                // The public key is used for verifying the signature
                var publicKey = rsa.ExportParameters(false);
            }
        }
    }
}