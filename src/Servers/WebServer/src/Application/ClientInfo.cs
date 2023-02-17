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
        public const string PeerKeyPrivate = "0000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000";
        public const string PeerKeyPublicModulus = "0000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000";
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