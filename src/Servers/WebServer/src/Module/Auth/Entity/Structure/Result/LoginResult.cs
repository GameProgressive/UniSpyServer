using UniSpyServer.Servers.WebServer.Abstraction;

namespace UniSpyServer.Servers.WebServer.Module.Auth.Entity.Structure.Result
{
    public record LoginCertificate
    {
        public int Length { get; set; }
        public int UserId { get; set; }
        public int ProfileId { get; set; }
        public string ProfileNick { get; set; }
        public string UniqueNick { get; set; }
        public string CdKeyHash { get; set; }
        /// <summary>
        /// 256 bytes 
        /// </summary>
        /// <value>Hex string</value>
        public string PeerKeyModulus { get; private set; }
        public string PeerKeyExponent { get; private set; }
        /// <summary>
        /// 256 bytes 
        /// </summary>
        /// <value>Hex string</value>
        public string ServerData { get; private set; }
        /// <summary>
        /// 256 bytes 
        /// </summary>
        /// <value>Hex string</value>
        public string Signature { get; private set; }
        public string ExpireTime { get; private set; }
        public LoginCertificate()
        {
            Length = 303;
            PeerKeyExponent = "010001";
            ServerData = "908EA21B9109C45591A1A011BF84A18940D22E032601A1B2DD235E278A9EF131404E6B07F7E2BE8BF4A658E2CB2DDE27E09354B7127C8A05D10BB4298837F96518CCB412497BE01ABA8969F9F46D23EBDE7CC9BE6268F0E6ED8209AD79727BC8E0274F6725A67CAB91AC87022E5871040BF856E541A76BB57C07F4B9BE4C6316";
            Signature = "181A4E679AC27D83543CECB8E1398243113EF6322D630923C6CD26860F265FC031C2C61D4F9D86046C07BBBF9CF86894903BD867E3CB59A0D9EFDADCB34A7FB3CC8BC7650B48E8913D327C38BB31E0EEB06E1FC1ACA2CFC52569BE8C48840627783D7FFC4A506B1D23A1C4AEAF12724DEB12B5036E0189E48A0FCB2832E1FB00";
            ExpireTime = "U3VuZGF5LCBPY3RvYmVyIDE4LCAyMDA5IDE6MTk6NTMgQU0=";
        }
    }
    public class LoginResult : ResultBase
    {
        public int? ResponseCode { get; set; }
        public LoginCertificate Certificate { get; set; }
        public string PeerKeyPrivate { get; private set; }

        public LoginResult()
        {
            ResponseCode = 0;
            PeerKeyPrivate = "8818DA2AC0E0956E0C67CA8D785CFAF3A11A9404D1ED9A6E580EA8569E087B75316B85D77B2208916BE2E0D37C7D7FD18EFD6B2E77C11CDA6E1B689BF460A40BBAF861D800497822004880024B4E7F98A020B1896F536D7219E67AB24B17D60A7BDD7D42E3501BB2FA50BB071EF7A80F29870FFD7C409C0B7BB7A8F70489D04D";
        }
    }
}