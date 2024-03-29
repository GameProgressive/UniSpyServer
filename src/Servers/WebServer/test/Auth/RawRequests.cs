namespace UniSpy.Server.WebServer.Test.Auth
{
    public class RawRequests
    {
        public const string LoginProfile =
            @"<?xml version=""1.0"" encoding=""UTF-8""?>
            <SOAP-ENV:Envelope xmlns:SOAP-ENV=""http://schemas.xmlsoap.org/soap/envelope/""
                xmlns:SOAP-ENC=""http://schemas.xmlsoap.org/soap/encoding/""
                xmlns:xsi=""http://www.w3.org/2001/XMLSchema-instance""
                xmlns:xsd=""http://www.w3.org/2001/XMLSchema""
                xmlns:ns1=""http://gamespy.net/sake"">
                <SOAP-ENV:Body>
                    <ns1:LoginProfile>
                        <ns1:version>1</ns1:version>
                        <ns1:gameid>0</ns1:gameid>
                        <ns1:partnercode>0</ns1:partnercode>
                        <ns1:namespaceid>0</ns1:namespaceid>
                        <ns1:email>spyguy@unispy.org</ns1:email>
                        <ns1:uniquenick>spyguy</ns1:uniquenick>
                        <ns1:cdkey>XXXXXXXXXXX</ns1:cdkey>
                        <ns1:password>
                            <ns1:Value>XXXXXXXXXXX</ns1:Value>
                        </ns1:password>
                    </ns1:LoginProfile>
                </SOAP-ENV:Body>
            </SOAP-ENV:Envelope>";

        public const string LoginPs3Cert =
            @"<?xml version=""1.0"" encoding=""UTF-8""?>
            <SOAP-ENV:Envelope xmlns:SOAP-ENV=""http://schemas.xmlsoap.org/soap/envelope/""
                xmlns:SOAP-ENC=""http://schemas.xmlsoap.org/soap/encoding/""
                xmlns:xsi=""http://www.w3.org/2001/XMLSchema-instance""
                xmlns:xsd=""http://www.w3.org/2001/XMLSchema""
                xmlns:ns1=""http://gamespy.net/sake"">
                <SOAP-ENV:Body>
                    <ns1:LoginPs3Cert>
                        <ns1:version>0</ns1:version>
                        <ns1:gameid>0</ns1:gameid>
                        <ns1:namespaceid>0</ns1:namespaceid>
                        <ns1:partnercode>0001</ns1:partnercode>
                        <ns1:ps3sert>0</ns1:ps3sert>
                        <ns1:npticket>0001</ns1:npticket>
                    </ns1:LoginPs3Cert>
                </SOAP-ENV:Body>
            </SOAP-ENV:Envelope>";

        public const string LoginRemoteAuth =
            @"<?xml version=""1.0"" encoding=""UTF-8""?>
            <SOAP-ENV:Envelope xmlns:SOAP-ENV=""http://schemas.xmlsoap.org/soap/envelope/""
                xmlns:SOAP-ENC=""http://schemas.xmlsoap.org/soap/encoding/""
                xmlns:xsi=""http://www.w3.org/2001/XMLSchema-instance""
                xmlns:xsd=""http://www.w3.org/2001/XMLSchema""
                xmlns:ns1=""http://gamespy.net/sake"">
                <SOAP-ENV:Body>
                    <ns1:LoginRemoteAuth>
                        <ns1:version>1</ns1:version>
                        <ns1:gameid>0</ns1:gameid>
                        <ns1:partnercode>0</ns1:partnercode>
                        <ns1:namespaceid>0</ns1:namespaceid>
                        <ns1:authtoken>XXXXXXXXXXX</ns1:authtoken>
                        <ns1:challenge>XXXXXXXXXXX</ns1:challenge>
                    </ns1:LoginRemoteAuth>
                </SOAP-ENV:Body>
            </SOAP-ENV:Envelope>";

        public const string LoginUniqueNick =
            @"<?xml version=""1.0"" encoding=""UTF-8""?>
            <SOAP-ENV:Envelope xmlns:SOAP-ENV=""http://schemas.xmlsoap.org/soap/envelope/""
                xmlns:SOAP-ENC=""http://schemas.xmlsoap.org/soap/encoding/""
                xmlns:xsi=""http://www.w3.org/2001/XMLSchema-instance""
                xmlns:xsd=""http://www.w3.org/2001/XMLSchema""
                xmlns:ns1=""http://gamespy.net/sake"">
                <SOAP-ENV:Body>
                    <ns1:LoginUniqueNick>
                        <ns1:version>1</ns1:version>
                        <ns1:partnercode>0</ns1:partnercode>
                        <ns1:namespaceid>0</ns1:namespaceid>
                        <ns1:uniquenick>spyguy</ns1:uniquenick>
                        <ns1:password>
                            <ns1:Value>XXXXXXXXXXX</ns1:Value>
                        </ns1:password>
                    </ns1:LoginUniqueNick>
                </SOAP-ENV:Body>
            </SOAP-ENV:Envelope>";
    }
}
