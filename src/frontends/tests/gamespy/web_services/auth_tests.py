import unittest

import responses

from frontends.gamespy.library.network.http_handler import HttpData
from frontends.gamespy.protocols.web_services.modules.auth.contracts.requests import CreateUserAccountRequest, LoginProfileWithGameIdRequest, LoginPs3CertRequest, LoginRemoteAuthRequest, LoginUniqueNickRequest

LOGIN_PROFILE = """<?xml version="1.0" encoding="UTF-8"?><SOAP-ENV:Envelope xmlns:SOAP-ENV="http://schemas.xmlsoap.org/soap/envelope/"
    xmlns:SOAP-ENC="http://schemas.xmlsoap.org/soap/encoding/" xmlns:xsi="http://www.w3.org/2001/XMLSchema-instance"
    xmlns:xsd="http://www.w3.org/2001/XMLSchema" xmlns:ns1="http://gamespy.net/AuthService/"><SOAP-ENV:Body>
        <ns1:LoginProfileWithGameId>
            <ns1:version>1</ns1:version>
            <ns1:gameid>0</ns1:gameid>
            <ns1:partnercode>0</ns1:partnercode>
            <ns1:namespaceid>0</ns1:namespaceid>
            <ns1:email>spyguy@gamespy.com</ns1:email>
            <ns1:profilenick>spyguy</ns1:profilenick>
            <ns1:password>
                <ns1:Value>
                    00026cfb61b75553fb113e8a158a1ce8c88dcb5415a405efeeb4ba605c315bb9204c09a53e23fb63081d208f44f813f5967c4da7d85f55ef66d4d39a4c89a9800771ee4f742bb808f35bced93fe7fa289eb4bdc94a44fa6f9121ef0eba4d3827d0cacaaeafdc85b64e4d9dc34814f6d4178363303f543a2fd8d8af0030303030
                </ns1:Value>
            </ns1:password>
        </ns1:LoginProfileWithGameId>
    </SOAP-ENV:Body></SOAP-ENV:Envelope>"""
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

CREATE_USER_ACCOUNT = """<?xml version="1.0" encoding="UTF-8"?><SOAP-ENV:Envelope xmlns:SOAP-ENV="http://schemas.xmlsoap.org/soap/envelope/"
    xmlns:SOAP-ENC="http://schemas.xmlsoap.org/soap/encoding/" xmlns:xsi="http://www.w3.org/2001/XMLSchema-instance"
    xmlns:xsd="http://www.w3.org/2001/XMLSchema" xmlns:ns1="http://gamespy.net/AuthService/"><SOAP-ENV:Body>
        <ns1:CreateUserAccount>
            <ns1:version>1</ns1:version>
            <ns1:partnercode>99</ns1:partnercode>
            <ns1:namespaceid>102</ns1:namespaceid>
            <ns1:email>4C84AA7017F21B0A5B83B56D6D06C86D22E93662@example.com</ns1:email>
            <ns1:profilenick>4C84AA7017F21B0A5B83</ns1:profilenick>
            <ns1:uniquenick>4C84AA7017F21B0A5B83B56D6D06C86D22E93662</ns1:uniquenick>
            <ns1:password>
                <ns1:Value>411CDA6489C3223A4FB6CA7BD6521F961679CFA5E04C6C95A988995658975B4B99D2C1E91629354F8422E470BDA5EC06614E42576E2E812A1792E75589EC9BDA0CF3B9D1D60BF1847A424717950AA1C81DD93CEF029F931A33359208D6EC2F0A8AEF3C49F1048E9D585D55AFA0A9E2D545EAF2DB0460B3DE2965EB55E5FF88A2</ns1:Value>
            </ns1:password>
        </ns1:CreateUserAccount>
    </SOAP-ENV:Body></SOAP-ENV:Envelope>"""


class AuthTests(unittest.TestCase):
    @responses.activate
    def test_create_user_account(self):
        request = CreateUserAccountRequest(
            str(HttpData(body=CREATE_USER_ACCOUNT)))
        request.parse()
        pass

    @unittest.skip("old request password encryption problem")
    @responses.activate
    def test_crysis_auth(self):
        request = LoginUniqueNickRequest(str(HttpData(body=CRYSIS)))
        request.parse()

    @unittest.skip("old request password encryption problem")
    @responses.activate
    def test_login_profile(self):
        request = LoginProfileWithGameIdRequest(
            str(HttpData(body=LOGIN_PROFILE)))
        request.parse()
        self.assertEqual(1, request.version)
        self.assertEqual(0, request.game_id)
        self.assertEqual(0, request.partner_code)
        self.assertEqual(0, request.namespace_id)
        self.assertEqual("spyguy@gamespy.com", request.email)
        self.assertEqual("spyguy", request.nick)
        self.assertEqual("00026cfb61b75553fb113e8a158a1ce8c88dcb5415a405efeeb4ba605c315bb9204c09a53e23fb63081d208f44f813f5967c4da7d85f55ef66d4d39a4c89a9800771ee4f742bb808f35bced93fe7fa289eb4bdc94a44fa6f9121ef0eba4d3827d0cacaaeafdc85b64e4d9dc34814f6d4178363303f543a2fd8d8af0030303030", request.password)

        # handler = LoginProfileWithGameIdHandler(request)
        # handler.handle()

    def test_login_ps3_cert(self):
        request = LoginPs3CertRequest(str(HttpData(body=LOGIN_PS3_CERT)))
        request.parse()
        self.assertEqual(0, request.version)
        self.assertEqual(0, request.game_id)
        self.assertEqual(1, request.partner_code)
        self.assertEqual(0, request.namespace_id)
        self.assertEqual("0", request.ps3_cert)
        self.assertEqual("0001", request.npticket)

    def test_remote_auth(self):
        request = LoginRemoteAuthRequest(str(HttpData(body=LOGIN_REMOTE_AUTH)))
        request.parse()
        self.assertEqual(1, request.version)
        self.assertEqual(0, request.game_id)
        self.assertEqual(0, request.partner_code)
        self.assertEqual(0, request.namespace_id)
        self.assertEqual("XXXXXXXXXXX", request.auth_token)
        self.assertEqual("XXXXXXXXXXX", request.challenge)

    @unittest.skip("old request password encryption problem")
    def test_login_uniquenick(self):
        request = LoginUniqueNickRequest(str(HttpData(body=LOGIN_UNIQUENICK)))
        request.parse()
        self.assertEqual(1, request.version)
        self.assertEqual(0, request.partner_code)
        self.assertEqual(0, request.namespace_id)
        self.assertEqual("spyguy", request.uniquenick)
        self.assertEqual("XXXXXXXXXXX", request.password)


if __name__ == "__main__":
    unittest.main()
