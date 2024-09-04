import hashlib
import servers.web_services.src.abstractions.contracts as lib
from servers.web_services.src.aggregations.soap_envelop import SoapEnvelop
from servers.web_services.src.applications.client import ClientInfo
from servers.web_services.src.modules.auth.exceptions.general import AuthException
import datetime

NAMESPACE = "http://gamespy.net/AuthService/"


class LoginRequestBase(lib.RequestBase):
    version: int
    partner_code: int
    namespace_id: int

    def parse(self) -> None:
        super().parse()
        version_node = self._content_element.find(
            f".//{{{NAMESPACE}}}version")
        if version_node is None or version_node.text is None:
            raise AuthException("version is missing from the request.")
        self.version = int(version_node.text)
        partner_id_node = self._content_element.find(
            f".//{{{NAMESPACE}}}partnercode")
        if partner_id_node is None or partner_id_node.text is None:
            raise AuthException("partner id is missing from the request.")
        self.partner_code = int(partner_id_node.text)
        namespace_id_node = self._content_element.find(
            f".//{{{NAMESPACE}}}namespaceid")
        if namespace_id_node is None or namespace_id_node.text is None:
            raise AuthException("namespace id is missing from the request.")
        self.namespace_id = int(namespace_id_node.text)


class LoginResultBase(lib.ResultBase):
    response_code: int = 0
    length: int = 303
    user_id: int
    profile_id: int
    profile_nick: str
    unique_nick: str
    cdkey_hash: str


class LoginResponseBase(lib.ResponseBase):
    _request: LoginRequestBase
    _result: LoginResultBase
    _content: SoapEnvelop = SoapEnvelop("http://gamespy.net/AuthService/")
    _expiretime: int = int(
        (datetime.datetime.now() + datetime.timedelta(days=1)).timestamp()
    )

    def __init__(self, request: LoginRequestBase, result: LoginResultBase) -> None:
        assert isinstance(request, LoginRequestBase)
        assert isinstance(result, LoginResultBase)
        super().__init__(request, result)

    def build(self) -> None:
        self._build_context()
        super().build()

    def _build_context(self):
        self._content.add("responseCode", "h")
        self._content.add("certificate")
        self._content.add("length", self._result.length)
        self._content.add("version", self._request.version)
        self._content.add("partnercode", self._request.partner_code)
        self._content.add("namespaceid", self._request.namespace_id)
        self._content.add("userid", self._result.user_id)
        self._content.add("profileid", self._result.profile_id)
        self._content.add("expiretime", self._expiretime)
        self._content.add("profilenick", self._result.profile_nick)
        self._content.add("uniquenick", self._result.unique_nick)
        self._content.add("cdkeyhash", self._result.cdkey_hash)
        self._content.add("peerkeymodulus", ClientInfo.PEER_KEY_MODULUS)
        self._content.add("peerkeyexponent", ClientInfo.PEER_KEY_EXPONENT)
        self._content.add("serverdata", ClientInfo.SERVER_DATA)
        hash_str = self.__compute_hash()
        self._content.add("signature", ClientInfo.SIGNATURE_PREFIX + hash_str)
        self._content.back_to_parent_element()
        self._content.add("peerkeyprivate", ClientInfo.PEER_KEY_EXPONENT)

    def __compute_hash(self) -> str:
        """return md5 str"""
        data_to_hash = bytearray()
        data_to_hash.extend(
            self._result.length.to_bytes(4, byteorder="little"))
        data_to_hash.extend(
            self._request.version.to_bytes(4, byteorder="little"))
        data_to_hash.extend(
            self._request.partner_code.to_bytes(4, byteorder="little"))
        data_to_hash.extend(
            self._request.namespace_id.to_bytes(4, byteorder="little"))
        data_to_hash.extend(
            self._result.user_id.to_bytes(4, byteorder="little"))
        data_to_hash.extend(
            self._result.profile_id.to_bytes(4, byteorder="little"))
        data_to_hash.extend(self._expiretime.to_bytes(4, byteorder="little"))
        data_to_hash.extend(self._result.profile_nick.encode("ascii"))
        data_to_hash.extend(self._result.unique_nick.encode("ascii"))
        data_to_hash.extend(self._result.cdkey_hash.encode("ascii"))

        data_to_hash.extend(bytes.fromhex(ClientInfo.PEER_KEY_MODULUS))
        data_to_hash.append(0x01)

        # server data should be convert to bytes[128] then added to list
        data_to_hash.extend(bytes.fromhex(ClientInfo.SERVER_DATA))

        hash_object = hashlib.md5()
        hash_object.update(data_to_hash)
        hash_string = hash_object.hexdigest()
        return hash_string
