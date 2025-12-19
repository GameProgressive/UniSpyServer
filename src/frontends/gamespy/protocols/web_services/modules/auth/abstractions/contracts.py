import hashlib
import frontends.gamespy.protocols.web_services.abstractions.contracts as lib
from frontends.gamespy.protocols.web_services.aggregations.soap_envelop import SoapEnvelop
from frontends.gamespy.protocols.web_services.applications.client import ClientInfo
from frontends.gamespy.protocols.web_services.modules.auth.aggregates.enums import AuthCode, ResponseName
from frontends.gamespy.protocols.web_services.modules.auth.aggregates.exceptions import ParseException
import datetime

NAMESPACE = "http://gamespy.net/AuthService/"


class LoginRequestBase(lib.RequestBase):
    version: int
    partner_code: int
    namespace_id: int
    response_name: ResponseName

    def parse(self) -> None:
        super().parse()
        self.version = self._get_int("version")
        self.partner_code = self._get_int("partnercode")
        self.namespace_id = self._get_int("namespaceid")

    def _get_int(self, attr_name: str) -> int:
        try:
            result = super()._get_int(attr_name)
        except:
            raise ParseException(f"{attr_name} is missing",
                                 self.response_name)
        return result

    def _get_str(self, attr_name: str) -> str:
        try:
            result = super()._get_str(attr_name)
        except:
            raise ParseException(f"{attr_name} is missing",
                                 self.response_name)
        return result

    def _get_value(self, attr_name: str) -> object:
        value = super()._get_value(attr_name)
        if value is None:
            raise ParseException(f"{attr_name} is missing",
                                 self.response_name)
        return value

    def _parse_password(self):
        password = self._get_value("password")
        assert isinstance(password, dict)
        return password['Value']


class LoginResultBase(lib.ResultBase):
    response_code: int = 0
    length: int = 303
    user_id: int
    profile_id: int
    profile_nick: str
    unique_nick: str
    cdkey_hash: str | None = None
    version: int
    namespace_id: int
    partner_code: int


class LoginResponseBase(lib.ResponseBase):
    _result: LoginResultBase
    _content: SoapEnvelop
    _expiretime: int

    def __init__(self, result: LoginResultBase) -> None:
        assert isinstance(result, LoginResultBase)
        super().__init__(result)
        self._expiretime = int(
            (datetime.datetime.now() + datetime.timedelta(days=1)).timestamp()
        )

    def build(self) -> None:
        self._build_context()
        super().build()

    def _build_context(self):
        self._content.add("responseCode", AuthCode.SUCCESS.value)
        self._content.add("certificate")
        self._content.add("length", self._result.length)
        self._content.add("version", self._result.version)
        self._content.add("partnercode", self._result.partner_code)
        self._content.add("namespaceid", self._result.namespace_id)
        self._content.add("userid", self._result.user_id)
        self._content.add("profileid", self._result.profile_id)
        self._content.add("expiretime", self._expiretime)
        self._content.add("profilenick", self._result.profile_nick)
        self._content.add("uniquenick", self._result.unique_nick)
        if self._result.cdkey_hash is not None:
            self._content.add("cdkeyhash", self._result.cdkey_hash)
        self._content.add("peerkeymodulus", ClientInfo.PEER_KEY_MODULUS)
        self._content.add("peerkeyexponent", ClientInfo.PEER_KEY_EXPONENT)
        self._content.add("serverdata", ClientInfo.SERVER_DATA)
        hash_str = self.__compute_hash()
        self._content.add("signature", ClientInfo.SIGNATURE_PREFIX + hash_str)
        self._content.go_to_content_element()
        self._content.add("peerkeyprivate", ClientInfo.PEER_KEY_EXPONENT)

    def __compute_hash(self) -> str:
        """return md5 str"""
        data_to_hash = bytearray()
        data_to_hash.extend(
            self._result.length.to_bytes(4, byteorder="little"))
        data_to_hash.extend(
            self._result.version.to_bytes(4, byteorder="little"))
        data_to_hash.extend(
            self._result.partner_code.to_bytes(4, byteorder="little"))
        data_to_hash.extend(
            self._result.namespace_id.to_bytes(4, byteorder="little"))
        data_to_hash.extend(
            self._result.user_id.to_bytes(4, byteorder="little"))
        data_to_hash.extend(
            self._result.profile_id.to_bytes(4, byteorder="little"))
        data_to_hash.extend(self._expiretime.to_bytes(4, byteorder="little"))
        data_to_hash.extend(self._result.profile_nick.encode("ascii"))
        data_to_hash.extend(self._result.unique_nick.encode("ascii"))
        if self._result.cdkey_hash is not None:
            data_to_hash.extend(self._result.cdkey_hash.encode("ascii"))

        data_to_hash.extend(bytes.fromhex(ClientInfo.PEER_KEY_MODULUS))
        data_to_hash.append(0x01)

        # server data should be convert to bytes[128] then added to list
        data_to_hash.extend(bytes.fromhex(ClientInfo.SERVER_DATA))

        hash_object = hashlib.md5()
        hash_object.update(data_to_hash)
        hash_string = hash_object.hexdigest()
        return hash_string
