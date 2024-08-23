import unittest

import responses

from servers.webservices.src.modules.auth.contracts.requests import LoginProfileWithGameIdRequest, LoginPs3CertRequest, LoginRemoteAuthRequest, LoginUniqueNickRequest

LOGIN_PROFILE = """<?xml version="1.0" encoding="UTF-8"?>
            <SOAP-ENV:Envelope xmlns:SOAP-ENV="http://schemas.xmlsoap.org/soap/envelope/"
                xmlns:SOAP-ENC="http://schemas.xmlsoap.org/soap/encoding/"
                xmlns:xsi="http://www.w3.org/2001/XMLSchema-instance"
                xmlns:xsd="http://www.w3.org/2001/XMLSchema"
                xmlns:ns1="http://gamespy.net/AuthService/">
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
            </SOAP-ENV:Envelope>"""
LOGIN_PS3_CERT = """<?xml version="1.0" encoding="UTF-8"?>
            <SOAP-ENV:Envelope xmlns:SOAP-ENV="http://schemas.xmlsoap.org/soap/envelope/"
                xmlns:SOAP-ENC="http://schemas.xmlsoap.org/soap/encoding/"
                xmlns:xsi="http://www.w3.org/2001/XMLSchema-instance"
                xmlns:xsd="http://www.w3.org/2001/XMLSchema"
                xmlns:ns1="http://gamespy.net/AuthService/">
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
            </SOAP-ENV:Envelope>"""

LOGIN_REMOTE_AUTH = """<?xml version="1.0" encoding="UTF-8"?>
            <SOAP-ENV:Envelope xmlns:SOAP-ENV="http://schemas.xmlsoap.org/soap/envelope/"
                xmlns:SOAP-ENC="http://schemas.xmlsoap.org/soap/encoding/"
                xmlns:xsi="http://www.w3.org/2001/XMLSchema-instance"
                xmlns:xsd="http://www.w3.org/2001/XMLSchema"
                xmlns:ns1="http://gamespy.net/AuthService/">
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
            </SOAP-ENV:Envelope>"""

LOGIN_UNIQUENICK = """<?xml version="1.0" encoding="UTF-8"?>
            <SOAP-ENV:Envelope xmlns:SOAP-ENV="http://schemas.xmlsoap.org/soap/envelope/"
                xmlns:SOAP-ENC="http://schemas.xmlsoap.org/soap/encoding/"
                xmlns:xsi="http://www.w3.org/2001/XMLSchema-instance"
                xmlns:xsd="http://www.w3.org/2001/XMLSchema"
                xmlns:ns1="http://gamespy.net/AuthService/">
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
            </SOAP-ENV:Envelope>"""

CRYSIS = """<?xml version="1.0" encoding="UTF-8"?>
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
                        </SOAP-ENV:Envelope>"""


class AuthTests(unittest.TestCase):
    @responses.activate
    def test_crysis_auth(self):

        request = LoginUniqueNickRequest(CRYSIS)
        request.parse()

    @responses.activate
    def test_login_profile(self):
        request = LoginProfileWithGameIdRequest(LOGIN_PROFILE)
        request.parse()
        self.assertEqual(1, request.version)
        self.assertEqual(0, request.game_id)
        self.assertEqual(0, request.partner_code)
        self.assertEqual(0, request.namespace_id)
        self.assertEqual("spyguy@unispy.org", request.email)
        self.assertEqual("spyguy", request.uniquenick)
        self.assertEqual("XXXXXXXXXXX", request.cdkey)
        self.assertEqual("XXXXXXXXXXX", request.password)

        # handler = LoginProfileWithGameIdHandler(request)
        # handler.handle()

    def test_login_ps3_cert(self):
        request = LoginPs3CertRequest(LOGIN_PS3_CERT)
        request.parse()
        self.assertEqual(0, request.version)
        self.assertEqual(0, request.game_id)
        self.assertEqual(1, request.partner_code)
        self.assertEqual(0, request.namespace_id)
        self.assertEqual("0", request.ps3_cert)
        self.assertEqual("0001", request.npticket)

    def test_remote_auth(self):
        request = LoginRemoteAuthRequest(LOGIN_REMOTE_AUTH)
        request.parse()
        self.assertEqual(1, request.version)
        self.assertEqual(0, request.game_id)
        self.assertEqual(0, request.partner_code)
        self.assertEqual(0, request.namespace_id)
        self.assertEqual("XXXXXXXXXXX", request.auth_token)
        self.assertEqual("XXXXXXXXXXX", request.challenge)

    def test_login_uniquenick(self):
        request = LoginUniqueNickRequest(LOGIN_UNIQUENICK)
        request.parse()
        self.assertEqual(1, request.version)
        self.assertEqual(0, request.partner_code)
        self.assertEqual(0, request.namespace_id)
        self.assertEqual("spyguy", request.uniquenick)
        self.assertEqual("XXXXXXXXXXX", request.password)


if __name__ == "__main__":
    unittest.main()
