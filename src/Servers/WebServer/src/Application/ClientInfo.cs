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
        /// <summary>
        /// RandomNumber
        /// </summary>
        public const string PeerKeyExponent = "010001";
        public const string PeerKeyModulus = "aefb5064bbd1eb632fa8d57aab1c49366ce0ee3161cbef19f2b7971b63b811790ecbf6a47b34c55f65a0766b40c261c5d69c394cd320842dd2bccba883d30eae8fdba5d03b21b09bfc600dcb30b1b2f3fbe8077630b006dcb54c4254f14891762f72e7bbfe743eb8baf65f9e8c8d11ebe46f6b59e986b4c394cfbc2c8606e29f";
        /// <summary>
        /// should be 256 characters
        /// </summary>
        public const string ServerData =
        "0000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000";
        public const int ExpireTime = 0;
        public const string SignaturePreFix = "0001FFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFF003020300C06082A864886F70D020505000410";
        public ClientInfo()
        {
        }
    }
}