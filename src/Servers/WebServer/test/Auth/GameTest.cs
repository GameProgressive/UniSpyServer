using Xunit;

namespace UniSpy.Server.WebServer.Test.Auth;

public class GameTest
{
    [Fact]
    public void Crysis2Auth()
    {
        var correct = @"<?xml version='1.0' encoding='utf8'?>
                            <SOAP-ENV:Envelope
                                xmlns:SOAP-ENV=""http://schemas.xmlsoap.org/soap/envelope/""
                                xmlns:ns1=""http://gamespy.net/AuthService/"">
                                <SOAP-ENV:Body>
                                    <ns1:LoginUniqueNickResult>
                                        <ns1:responseCode>0</ns1:responseCode>
                                        <ns1:peerkeyprivate>0529e96f6f937c4ccb05427782412d6febba9f8cd983f9245ce34a1446d711bceed0da10748a5420c290ffad9e959de906b2498698dc66ca2b63e2e6b02d344ba9f936b44571eed3f91bcec3154d3dbb258510d13b89ecea7be33a5690b51d6e35acc0fe04f1a41b3f0fd4dd35d9166d8c9d3921142fdc92f9967e927bc8c991</ns1:peerkeyprivate>
                                        <ns1:certificate>
                                            <ns1:length>0</ns1:length>
                                            <ns1:version>1</ns1:version>
                                            <ns1:partnercode>95</ns1:partnercode>
                                            <ns1:namespaceid>95</ns1:namespaceid>
                                            <ns1:userid>67436</ns1:userid>
                                            <ns1:profileid>10040172</ns1:profileid>
                                            <ns1:expiretime>1687112622</ns1:expiretime>
                                            <ns1:profilenick>MyCrysis</ns1:profilenick>
                                            <ns1:uniquenick>asdf19</ns1:uniquenick>
                                            <ns1:cdkeyhash>D41D8CD98F00B204E9800998ECF8427E</ns1:cdkeyhash>
                                            <ns1:peerkeymodulus>aefb5064bbd1eb632fa8d57aab1c49366ce0ee3161cbef19f2b7971b63b811790ecbf6a47b34c55f65a0766b40c261c5d69c394cd320842dd2bccba883d30eae8fdba5d03b21b09bfc600dcb30b1b2f3fbe8077630b006dcb54c4254f14891762f72e7bbfe743eb8baf65f9e8c8d11ebe46f6b59e986b4c394cfbc2c8606e29f</ns1:peerkeymodulus>
                                            <ns1:peerkeyexponent>010001</ns1:peerkeyexponent>
                                            <ns1:serverdata>a75fbef2b50236c991e438efe81f0941a645a23d00e3f9bb409681e6c9b37e92a97c88afe65663081642c494c06860ee5dd45f9adab4d9caac3ed766717027eb76f8e6c6a30cbd32637abacfad0dcaf0e96948d20182766099a5133e89b92c498bc3470d1bab666d549e8af51edc2f009d63ecdff84551ed04138b6129adfd9f</ns1:serverdata>
                                            <ns1:signature>5F3E3EB96D4301D21E4F8375BB6FE4800F5CB27CD5B3D7DE49CB46B0B3E75A63A3D534AF7DA0FAB21B8C7A4EF56A316FACEECEAB153AD313876FC2A1895A151982897359328D06258B0125789FEBF928D117B7C8A342F8C40135708A17A65A3B189D3A0B7EBFEF21E1C435A409B1DA9449D6EF17D69BAAD844FADFFA30B227A8</ns1:signature>
                                        </ns1:certificate>
                                    </ns1:LoginUniqueNickResult>
                                </SOAP-ENV:Body>
                            </SOAP-ENV:Envelope>";
        // Given
        var raw = @"<?xml version=""1.0"" encoding=""UTF-8""?>
                        <SOAP-ENV:Envelope
                            xmlns:SOAP-ENV=""http://schemas.xmlsoap.org/soap/envelope/""
                            xmlns:SOAP-ENC=""http://schemas.xmlsoap.org/soap/encoding/""
                            xmlns:xsi=""http://www.w3.org/2001/XMLSchema-instance""
                            xmlns:xsd=""http://www.w3.org/2001/XMLSchema""
                            xmlns:ns1=""http://gamespy.net/AuthService/"">
                            <SOAP-ENV:Body>
                                <ns1:LoginUniqueNick>
                                    <ns1:version>1</ns1:version>
                                    <ns1:partnercode>95</ns1:partnercode>
                                    <ns1:namespaceid>95</ns1:namespaceid>
                                    <ns1:uniquenick>spyguy</ns1:uniquenick>
                                    <ns1:password>
                                        <ns1:Value>0000</ns1:Value>
                                    </ns1:password>
                                </ns1:LoginUniqueNick>
                            </SOAP-ENV:Body>
                        </SOAP-ENV:Envelope>";
        // When
        var request = new Module.Auth.Contract.Request.LoginUniqueNickRequest(raw);
        request.Parse();
        var handler = new Module.Auth.Handler.LoginUniqueNickHandler(MokeObject.Client,request);
        handler.Handle();
        // Then
    }
}