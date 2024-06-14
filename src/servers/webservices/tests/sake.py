import unittest

from servers.webservices.modules.auth.contracts.requests import LoginUniqueNickRequest


class Auth(unittest.TestCase):
    def test_login_unique_nick(self) -> None:
        raw = """
                <?xml version="1.0" encoding="UTF-8"?>
                    <SOAP-ENV:Envelope
                        xmlns:SOAP-ENV="http://schemas.xmlsoap.org/soap/envelope/"
                        xmlns:SOAP-ENC="http://schemas.xmlsoap.org/soap/encoding/"
                        xmlns:xsi="http://www.w3.org/2001/XMLSchema-instance"
                        xmlns:xsd="http://www.w3.org/2001/XMLSchema"
                        xmlns:ns1="http://gamespy.net/AuthService/">
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
                    </SOAP-ENV:Envelope>
                """
        r = LoginUniqueNickRequest(raw)
        r.parse()
        