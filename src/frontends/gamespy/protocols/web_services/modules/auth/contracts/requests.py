from frontends.gamespy.library.network.http_data import HttpData
from frontends.gamespy.protocols.web_services.modules.auth.abstractions.contracts import (
    NAMESPACE,
    RequestBase,
)
from frontends.gamespy.protocols.web_services.modules.auth.aggregates.enums import (
    CommandName,
)
from frontends.gamespy.protocols.web_services.modules.auth.aggregates.exceptions import (
    ParseException,
)


class LoginProfileRequest(RequestBase):
    email: str
    nick: str
    password: str
    cdkey: str | None
    command_name: CommandName

    @staticmethod
    def gs_parse(raw_request: HttpData) -> "LoginProfileRequest":
        data = RequestBase.gs_parse(raw_request)
        data_dict = data.model_dump()
        data_dict["email"] = data.str_mapping["email"]
        data_dict["nick"] = data.str_mapping["profilenick"]
        data_dict["password"] = data._parse_password()

        data_dict["cdkey"] = data.str_mapping.get("cdkey")
        data = LoginProfileRequest(**data_dict)
        return data


class LoginPs3CertRequest(RequestBase):
    ps3_cert: str
    npticket: str

    @staticmethod
    def gs_parse(raw_request: HttpData) -> "LoginPs3CertRequest":
        data = RequestBase.gs_parse(raw_request)
        data_dict = data.model_dump()
        data_dict["ps3_cert"] = data.str_mapping["ps3sert"]
        data_dict["npticket"] = data.str_mapping["npticket"]
        data = LoginPs3CertRequest(**data_dict)
        return data


class LoginRemoteAuthRequest(RequestBase):
    auth_token: str
    challenge: str

    @staticmethod
    def gs_parse(raw_request: HttpData) -> "LoginRemoteAuthRequest":
        data = RequestBase.gs_parse(raw_request)
        data_dict = data.model_dump()
        data_dict["auth_token"] = data.str_mapping["authtoken"]
        data_dict["challenge"] = data.str_mapping["challenge"]

        data = LoginRemoteAuthRequest(**data_dict)
        return data


class LoginUniqueNickRequest(RequestBase):
    uniquenick: str
    password: str

    @staticmethod
    def gs_parse(raw_request: HttpData) -> "LoginUniqueNickRequest":
        data = RequestBase.gs_parse(raw_request)
        data_dict = data.model_dump()
        data_dict["uniquenick"] = data.str_mapping["uniquenick"]
        data_dict["password"] = data._parse_password()
        data = LoginUniqueNickRequest(**data_dict)
        return data


class CreateUserAccountRequest(RequestBase):
    email: str
    nick: str
    uniquenick: str
    password: str

    @staticmethod
    def gs_parse(raw_request: HttpData) -> "CreateUserAccountRequest":
        data = RequestBase.gs_parse(raw_request)
        data_dict = data.model_dump()
        data_dict["email"] = data.str_mapping["email"]
        data_dict["nick"] = data.str_mapping["profilenick"]
        data_dict["uniquenick"] = data.str_mapping["uniquenick"]
        data_dict["password"] = data._parse_password()
        data = CreateUserAccountRequest(**data_dict)
        return data


if __name__ == "__main__":
    data = """<?xml version="1.0" encoding="UTF-8"?><SOAP-ENV:Envelope xmlns:SOAP-ENV="http://schemas.xmlsoap.org/soap/envelope/"
    xmlns:SOAP-ENC="http://schemas.xmlsoap.org/soap/encoding/" xmlns:xsi="http://www.w3.org/2001/XMLSchema-instance"
    xmlns:xsd="http://www.w3.org/2001/XMLSchema" xmlns:ns1="http://gamespy.net/AuthService/"><SOAP-ENV:Body>
        <ns1:LoginProfileWithGameId>
            <ns1:version>1</ns1:version>
            <ns1:gameid>0</ns1:gameid>
            <ns1:partnercode>0</ns1:partnercode>
            <ns1:namespaceid>1</ns1:namespaceid>
            <ns1:email>spyguy@gamespy.com</ns1:email>
            <ns1:profilenick>spyguy</ns1:profilenick>
            <ns1:password>
                <ns1:Value>
                    00026cfb61b75553fb113e8a158a1ce8c88dcb5415a405efeeb4ba605c315bb9204c09a53e23fb63081d208f44f813f5967c4da7d85f55ef66d4d39a4c89a9800771ee4f742bb808f35bced93fe7fa289eb4bdc94a44fa6f9121ef0eba4d3827d0cacaaeafdc85b64e4d9dc34814f6d4178363303f543a2fd8d8af0030303030
                </ns1:Value>
            </ns1:password>
        </ns1:LoginProfileWithGameId>
    </SOAP-ENV:Body>
    </SOAP-ENV:Envelope>"""
    h = HttpData(path=None, headers=None, body=data)

    req = LoginProfileRequest.gs_parse(h)
