import hashlib
from typing import TYPE_CHECKING, cast
from frontends.gamespy.library.network.http_handler import HttpData
import frontends.gamespy.protocols.web_services.abstractions.contracts as lib
from frontends.gamespy.protocols.web_services.aggregations.soap_envelop import SoapEnvelop
from frontends.gamespy.protocols.web_services.applications.client import ClientInfo
from frontends.gamespy.protocols.web_services.modules.auth.aggregates.enums import AuthCode, CommandName
from frontends.gamespy.protocols.web_services.modules.auth.aggregates.exceptions import ParseException
import datetime
from cryptography.hazmat.primitives import hashes, serialization
from cryptography.hazmat.backends import default_backend
from cryptography.hazmat.primitives.asymmetric import padding
from cryptography.hazmat.primitives.asymmetric.rsa import RSAPrivateKey
from cryptography.hazmat.primitives.asymmetric import utils as asym_utils
NAMESPACE = "http://gamespy.net/AuthService/"


def decrypt_rsa(cipher: str) -> str:
    private_key = serialization.load_pem_private_key(
        ClientInfo.PASSWORD_ENCRYPT_PRIVATE_KEY.encode(),
        password=None,  # Set this if your key is encrypted
        backend=default_backend())
    if TYPE_CHECKING:
        private_key = cast(RSAPrivateKey, private_key)
    plaintext = private_key.decrypt(bytes.fromhex(cipher), padding.PKCS1v15())
    assert isinstance(plaintext, bytes)
    plaintext_hex = bytes.hex(plaintext)
    return plaintext_hex


def sign_rsa(plan_text: bytes) -> str:
    private_key = serialization.load_pem_private_key(
        ClientInfo.PASSWORD_ENCRYPT_PRIVATE_KEY.encode(),
        password=None,  # Set this if your key is encrypted
        backend=default_backend())
    if TYPE_CHECKING:
        private_key = cast(RSAPrivateKey, private_key)
    sig = private_key.sign(plan_text,
                           padding.PKCS1v15(),
                           algorithm=hashes.MD5())
    assert isinstance(sig, bytes)
    sig_hex = bytes.hex(sig)
    return sig_hex


class LoginRequestBase(lib.RequestBase):
    version: int
    partner_code: int
    namespace_id: int
    command_name: CommandName

    def parse(self) -> None:
        super().parse()
        self.version = self._get_int("version")
        self.partner_code = self._get_int("partnercode")
        self.namespace_id = self._get_int("namespaceid")
        self.command_name = CommandName(self.command_name)

    def _get_int(self, attr_name: str) -> int:
        try:
            result = super()._get_int(attr_name)
        except:
            raise ParseException(f"{attr_name} is missing",
                                 self.command_name)
        return result

    def _get_str(self, attr_name: str) -> str:
        try:
            result = super()._get_str(attr_name)
        except:
            raise ParseException(f"{attr_name} is missing",
                                 self.command_name)
        return result

    def _get_value(self, attr_name: str) -> object:
        value = super()._get_value(attr_name)
        if value is None:
            raise ParseException(f"{attr_name} is missing",
                                 self.command_name)
        return value

    def _parse_password(self):
        password_dict = self._get_value("password")
        assert isinstance(password_dict, dict)
        enc_password = password_dict['Value']
        password = decrypt_rsa(enc_password)
        import hashlib
        hash_obj = hashlib.sha256()
        hash_obj.update(password.encode("utf-8"))
        hash_hex = hash_obj.hexdigest()
        return hash_hex


class ResultBase(lib.ResultBase):
    response_code: int = 0


class LoginResultBase(ResultBase):
    response_code: int = 0
    length: int = 303
    user_id: int
    profile_id: int
    profile_nick: str
    unique_nick: str
    version: int
    namespace_id: int
    partner_code: int
    cdkey_hash: str | None = None
    session_token: str
    """
    c# version data, equal to pcm login_ticket
    """


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
        body = str(self._content)
        headers = {
            "SessionToken": self._result.session_token,
            "Content-Length": len(body)
        }
        self.sending_buffer = HttpData(headers=headers, body=body)

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
        # signature = ClientInfo.SIGNATURE_PREFIX + hash_str
        signature = sign_rsa(hash_str).upper()
        # sig_reverse = int.from_bytes(bytes.fromhex(signature)).to_bytes(len(bytes.fromhex(signature)),"big").hex()
        #! dotnet sdk donot verify signature
        #! c sdk verify signature
        self._content.add("signature", signature)
        self._content.go_to_content_element()
        self._content.add("peerkeyprivate", ClientInfo.PEER_KEY_PRIVATE)
        """
        This peer private key is using for p2p key exchange, not for client to web server authentication
        """

    def __compute_hash(self) -> bytes:
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
        data_to_hash.extend(int(ClientInfo.PEER_KEY_EXPONENT, 16).to_bytes(3))
        # server data should be convert to bytes[128] then added to list
        data_to_hash.extend(bytes.fromhex(ClientInfo.SERVER_DATA))

        return bytes(data_to_hash)
