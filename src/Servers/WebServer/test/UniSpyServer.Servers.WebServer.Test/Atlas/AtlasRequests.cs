namespace UniSpyServer.Servers.WebServer.Test.Atlas
{
    public class AtlasRequests
    {
        public const string CreateMatchlessSession =
            @"<?xml version=""1.0"" encoding=""UTF-8""?>
            <SOAP-ENV:Envelope xmlns:SOAP-ENV=""http://schemas.xmlsoap.org/soap/envelope/""
                xmlns:SOAP-ENC=""http://schemas.xmlsoap.org/soap/encoding/""
                xmlns:xsi=""http://www.w3.org/2001/XMLSchema-instance""
                xmlns:xsd=""http://www.w3.org/2001/XMLSchema""
                xmlns:ns1=""http://gamespy.net/sake"">
                <SOAP-ENV:Body>
                    <ns1:CreateMatchlessSession>
                        <ns1:certificate>XXXXXX</ns1:certificate>
                        <ns1:proof>XXXXXX</ns1:proof>
                        <ns1:gameid>0</ns1:gameid>
                    </ns1:CreateMatchlessSession>
                </SOAP-ENV:Body>
            </SOAP-ENV:Envelope>";

        public const string CreateSession =
            @"<?xml version=""1.0"" encoding=""UTF-8""?>
            <SOAP-ENV:Envelope xmlns:SOAP-ENV=""http://schemas.xmlsoap.org/soap/envelope/""
                xmlns:SOAP-ENC=""http://schemas.xmlsoap.org/soap/encoding/""
                xmlns:xsi=""http://www.w3.org/2001/XMLSchema-instance""
                xmlns:xsd=""http://www.w3.org/2001/XMLSchema""
                xmlns:ns1=""http://gamespy.net/sake"">
                <SOAP-ENV:Body>
                    <ns1:CreateSession>
                        <ns1:certificate>XXXXXX</ns1:certificate>
                        <ns1:proof>XXXXXX</ns1:proof>
                        <ns1:gameid>0</ns1:gameid>
                    </ns1:CreateSession>
                </SOAP-ENV:Body>
            </SOAP-ENV:Envelope>";

        public const string SetReportIntention =
            @"<?xml version=""1.0"" encoding=""UTF-8""?>
            <SOAP-ENV:Envelope xmlns:SOAP-ENV=""http://schemas.xmlsoap.org/soap/envelope/""
                xmlns:SOAP-ENC=""http://schemas.xmlsoap.org/soap/encoding/""
                xmlns:xsi=""http://www.w3.org/2001/XMLSchema-instance""
                xmlns:xsd=""http://www.w3.org/2001/XMLSchema""
                xmlns:ns1=""http://gamespy.net/sake"">
                <SOAP-ENV:Body>
                    <ns1:SetReportIntention>
                        <ns1:certificate>XXXXXX</ns1:certificate>
                        <ns1:proof>XXXXXX</ns1:proof>
                        <ns1:csid>0</ns1:csid>
                        <ns1:ccid>0</ns1:ccid>
                        <ns1:gameid>0</ns1:gameid>
                        <ns1:authoritative>XXXXXX</ns1:authoritative>
                    </ns1:SetReportIntention>
                </SOAP-ENV:Body>
            </SOAP-ENV:Envelope>";

        public const string SubmitReport =
            @"<?xml version=""1.0"" encoding=""UTF-8""?>
            <SOAP-ENV:Envelope xmlns:SOAP-ENV=""http://schemas.xmlsoap.org/soap/envelope/""
                xmlns:SOAP-ENC=""http://schemas.xmlsoap.org/soap/encoding/""
                xmlns:xsi=""http://www.w3.org/2001/XMLSchema-instance""
                xmlns:xsd=""http://www.w3.org/2001/XMLSchema""
                xmlns:ns1=""http://gamespy.net/sake"">
                <SOAP-ENV:Body>
                    <ns1:SubmitReport>
                        <ns1:certificate>XXXXXX</ns1:certificate>
                        <ns1:proof>XXXXXX</ns1:proof>
                        <ns1:csid>0</ns1:csid>
                        <ns1:ccid>0</ns1:ccid>
                        <ns1:gameid>0</ns1:gameid>
                        <ns1:authoritative>XXXXXX</ns1:authoritative>
                    </ns1:SubmitReport>
                </SOAP-ENV:Body>
            </SOAP-ENV:Envelope>";
    }
}
