import unittest

from backends.protocols.gamespy.web_services.handlers import LoginRemoteAuthHandler
from backends.protocols.gamespy.web_services.requests import LoginRemoteAuthRequest


class HandlerTests(unittest.TestCase):
    def test_sdk_login_remote_auth(self):
        raw = {"raw_request": "<?xml version=\"1.0\" encoding=\"UTF-8\"?><SOAP-ENV:Envelope xmlns:SOAP-ENV=\"http://schemas.xmlsoap.org/soap/envelope/\" xmlns:SOAP-ENC=\"http://schemas.xmlsoap.org/soap/encoding/\" xmlns:xsi=\"http://www.w3.org/2001/XMLSchema-instance\" xmlns:xsd=\"http://www.w3.org/2001/XMLSchema\" xmlns:ns1=\"http://gamespy.net/AuthService/\"><SOAP-ENV:Body><ns1:LoginRemoteAuthWithGameId><ns1:version>1</ns1:version><ns1:gameid>0</ns1:gameid><ns1:partnercode>0</ns1:partnercode><ns1:namespaceid>0</ns1:namespaceid><ns1:authtoken>GMTy13lsJmiY7L19ojyN3XTM08ll0C4EWWijwmJyq3ttiZmoDUQJ0OSnar9nQCu5MpOGvi4Z0EcC2uNaS4yKrUA+h+tTDDoJHF7ZjoWKOTj00yNOEdzWyG08cKdVQwFRkF+h8oG/Jd+Ik3sWviXq/+5bhZQ7iXxTbbDwNL6Lagp/pLZ9czLnYPhY7VEcoQlx9oO</ns1:authtoken><ns1:challenge>LH8c.DLe</ns1:challenge></ns1:LoginRemoteAuthWithGameId></SOAP-ENV:Body></SOAP-ENV:Envelope>",
               "version": 1, "partner_code": 0, "namespace_id": 0, "auth_token": "GMTy13lsJmiY7L19ojyN3XTM08ll0C4EWWijwmJyq3ttiZmoDUQJ0OSnar9nQCu5MpOGvi4Z0EcC2uNaS4yKrUA+h+tTDDoJHF7ZjoWKOTj00yNOEdzWyG08cKdVQwFRkF+h8oG/Jd+Ik3sWviXq/+5bhZQ7iXxTbbDwNL6Lagp/pLZ9czLnYPhY7VEcoQlx9oO", "challenge": "LH8c.DLe", "game_id": 0, "client_ip": "172.19.0.4", "server_id": "950b7638-a90d-469b-ac1f-861e63c8c613", "client_port": 57502}
        request = LoginRemoteAuthRequest.model_validate(raw)
        handler = LoginRemoteAuthHandler(request)
        handler.handle()
        handler.response

    def test_sdk_login_uniquenick(self):
        raw = {}
