using System;
using System.Security.Cryptography;
using UniSpy.Server.Core.Extension;
using UniSpy.Server.Core.Logging;
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
                var publicKey = rsa.ExportParameters(false);
                var d = privateKey.D;
                var modulus = privateKey.Modulus;
                var exponent = privateKey.Exponent;
                
                Console.WriteLine(StringExtensions.ConvertByteToHexString(d));
                Console.WriteLine(StringExtensions.ConvertByteToHexString(modulus));
                Console.WriteLine(StringExtensions.ConvertByteToHexString(exponent));


                var newPublicParam = new RSAParameters
                {
                    Modulus = modulus,
                    Exponent = exponent
                };
                var newPrivateParam = new RSAParameters
                {
                    Modulus = modulus,
                    Exponent = exponent,
                    D = d,
                };
            }
        }
    }
}