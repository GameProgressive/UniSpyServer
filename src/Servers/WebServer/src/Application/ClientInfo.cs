using System;
using UniSpy.Server.Core.Abstraction.BaseClass;

namespace UniSpy.Server.WebServer.Application
{
    /// <summary>
    /// Note!! the public exponent on SDK must set to 000001
    /// because the multiple inverse of 1 is 1
    /// data^1 mod n = data
    /// enc^1 mod n = data^1^1 mod n = data 
    /// I do not want let our server to compute the rsa encryption so I made this trick
    /// I am very glad that my major is cryptography during my master degree
    /// </summary>
    public sealed class ClientInfo : ClientInfoBase
    {
        public const string PeerKeyExponent = "000001";
        public const string PeerKeyModulus = "aefb5064bbd1eb632fa8d57aab1c49366ce0ee3161cbef19f2b7971b63b811790ecbf6a47b34c55f65a0766b40c261c5d69c394cd320842dd2bccba883d30eae8fdba5d03b21b09bfc600dcb30b1b2f3fbe8077630b006dcb54c4254f14891762f72e7bbfe743eb8baf65f9e8c8d11ebe46f6b59e986b4c394cfbc2c8606e29f";
        /// <summary>
        /// The exponent generated for user
        /// exponent is 000001
        /// </summary>
        public static readonly byte PeerKeyExponentByte = 1;
        /// <summary>
        /// The modulus generated for user aefb5064bbd1eb632fa8d57aab1c49366ce0ee3161cbef19f2b7971b63b811790ecbf6a47b34c55f65a0766b40c261c5d69c394cd320842dd2bccba883d30eae8fdba5d03b21b09bfc600dcb30b1b2f3fbe8077630b006dcb54c4254f14891762f72e7bbfe743eb8baf65f9e8c8d11ebe46f6b59e986b4c394cfbc2c8606e29f
        /// </summary>
        public static readonly byte[] PeerKeyModulusBytes = { 0xAE, 0xFB, 0x50, 0x64, 0xBB, 0xD1, 0xEB, 0x63, 0x2F, 0xA8, 0xD5, 0x7A, 0xAB, 0x1C, 0x49, 0x36, 0x6C, 0xE0, 0xEE, 0x31, 0x61, 0xCB, 0xEF, 0x19, 0xF2, 0xB7, 0x97, 0x1B, 0x63, 0xB8, 0x11, 0x79, 0x0E, 0xCB, 0xF6, 0xA4, 0x7B, 0x34, 0xC5, 0x5F, 0x65, 0xA0, 0x76, 0x6B, 0x40, 0xC2, 0x61, 0xC5, 0xD6, 0x9C, 0x39, 0x4C, 0xD3, 0x20, 0x84, 0x2D, 0xD2, 0xBC, 0xCB, 0xA8, 0x83, 0xD3, 0x0E, 0xAE, 0x8F, 0xDB, 0xA5, 0xD0, 0x3B, 0x21, 0xB0, 0x9B, 0xFC, 0x60, 0x0D, 0xCB, 0x30, 0xB1, 0xB2, 0xF3, 0xFB, 0xE8, 0x07, 0x76, 0x30, 0xB0, 0x06, 0xDC, 0xB5, 0x4C, 0x42, 0x54, 0xF1, 0x48, 0x91, 0x76, 0x2F, 0x72, 0xE7, 0xBB, 0xFE, 0x74, 0x3E, 0xB8, 0xBA, 0xF6, 0x5F, 0x9E, 0x8C, 0x8D, 0x11, 0xEB, 0xE4, 0x6F, 0x6B, 0x59, 0xE9, 0x86, 0xB4, 0xC3, 0x94, 0xCF, 0xBC, 0x2C, 0x86, 0x06, 0xE2, 0x9F };
        /// <summary>
        /// should be 256 characters
        /// </summary>
        public const string ServerData =
        "0000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000";
        public static int ExpireTime => (int)((DateTimeOffset)DateTime.UtcNow + TimeSpan.FromMinutes(5)).ToUnixTimeSeconds();
        public const string SignaturePreFix = "0001FFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFF003020300C06082A864886F70D020505000410";
        public ClientInfo()
        {
        }
    }
}