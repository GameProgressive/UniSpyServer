using System;
using System.Linq;
using System.Net;
using System.Numerics;
using UniSpyServer.UniSpyLib.Abstraction.BaseClass;
using UniSpyServer.UniSpyLib.Extensions;

namespace UniSpyServer.Servers.WebServer.Entity.Structure
{
    public class ClientInfo : ClientInfoBase
    {
        public static string ExponentHexStr => PeerKeyPublicExponent.ToByteArray().ToString().Replace("-", string.Empty);
        public static string ModulusHexStr => PeerKeyModulus.ToByteArray().ToString().Replace("-", string.Empty);
        public static BigInteger PeerKeyPublicExponent = BigInteger.Parse("010001", System.Globalization.NumberStyles.AllowHexSpecifier);
        public static BigInteger PeerKeyModulus = BigInteger.Parse("00af12efb486a5f594f4b86d153cef694fba59bde5005411e271ad9e53ae41bd3183b3b06459de85907bfdcee256180bd450f7f547dd1c81f57e14b477a48cef415f957de5ea723a0050be386fd2c1369761340f23ed43aa4299926107e3c56845ea32685a7ced12a32bfd6b6a2aefe8b8b9fdf0893f486342f36fd6000d691ee1", System.Globalization.NumberStyles.AllowHexSpecifier);
        public static BigInteger PeerKeyPrivateExponent = BigInteger.Parse("00e2201247fcb3ef29e45e842eee4a1b072ae59c115de6f4e0bb857b4e9282b2ee2b6ce1aef46a5eea7ad7bd0a5a4a969a186e2dd1e379e4a27a0e2120f49da702d4a3b892f5c776fee869d218f145b6e32f32b71063a0222addc256e8fdc977b2324a71370777295d45240f4f5fdf7cd7ab9c2393fdce0781c5118b9e1e905537", System.Globalization.NumberStyles.AllowHexSpecifier);
        public const string ServerData = "908EA21B9109C45591A1A011BF84A18940D22E032601A1B2DD235E278A9EF131404E6B07F7E2BE8BF4A658E2CB2DDE27E09354B7127C8A05D10BB4298837F96518CCB412497BE01ABA8969F9F46D23EBDE7CC9BE6268F0E6ED8209AD79727BC8E0274F6725A67CAB91AC87022E5871040BF856E541A76BB57C07F4B9BE4C6316";
        public const string ExpireTime = "U3VuZGF5LCBPY3RvYmVyIDE4LCAyMDA5IDE6MTk6NTMgQU0=";
        public const string SignaturePreFix = "0001FFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFF003020300C06082A864886F70D020505000410";
        
        public ClientInfo()
        {
        }
    }
}