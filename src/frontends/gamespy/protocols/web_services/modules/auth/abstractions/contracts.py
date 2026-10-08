import datetime
from typing import TYPE_CHECKING, cast

from cryptography.hazmat.backends import default_backend
from cryptography.hazmat.primitives import hashes, serialization
from cryptography.hazmat.primitives.asymmetric import padding
from cryptography.hazmat.primitives.asymmetric.rsa import RSAPrivateKey

import frontends.gamespy.protocols.web_services.abstractions.contracts as lib
from frontends.gamespy.library.network.http_data import HttpData
from frontends.gamespy.protocols.web_services.aggregations.soap_envelop import (
    SoapEnvelop,
)
from frontends.gamespy.protocols.web_services.modules.auth.aggregates.enums import (
    AuthCode,
    CommandName,
)
from frontends.gamespy.protocols.web_services.modules.auth.aggregates.exceptions import (
    ParseException,
)

NAMESPACE = "http://gamespy.net/AuthService/"
"""
Note!! the public exponent on SDK must set to 000001\n
because the multiple inverse of 1 is 1\n
data^1 mod n = data\n
enc^1 mod n = data^1^1 mod n = data\n
I do not want let our server to compute the rsa encryption so I made this trick\n
"""

PEER_KEY_PRIVATE = "AACAB1208809F171A08E6274AD685ACF657F4742FC1F51C3025BA6D97337F039F1B3D9B25858964E8D813F8657BCE89ED0A1A9C4720C383661A742C90582546BCDF6DD47E4B1D5A8D87876FBDD7EC1EC7F866BCAC6953BF3B6CDEE4D0A64EB3C3DE86532A0E4C037B2DCBD746A2F1A1B082DE1CC6BEE3A7C2B63F21800007D95"
PEER_KEY_EXPONENT = "010001"
PEER_KEY_MODULUS = "D4536886309F2BDCFCC54F1B41B90D9C4934540F895DD312A22D9AD53E013B12F8F6658364679F7EBAAD8D8F088C4908E0D2392AB072085AFE48B7B7D1F10105D0BBA0662D17ECCAF52E6AEAFB0BA70728A3E8716306B3D2E6DBE9447EF7B6187388DE70CE0A83941D55BF2F686EC4521BAB419A615E3F06DF83AEF72BB3EAD1"
SERVER_DATA = "95980bf5011ce73f2866b995a272420c36f1e8b4ac946f0b5bfe87c9fef0811036da00cfa85e77e00af11c924d425ec06b1dd052feab1250376155272904cbf9da831b0ce3d52964424c0a426b869e2c0ad11ffa3e70496e27ea250adb707a96b3496bff190eafc0b6b9c99db75b02c2a822bb1b5b3d954e7b2c0f9b1487e3e1"
"""
server data can be arbitrary hex string
"""
SIGNATURE_PREFIX = "0001FFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFF003020300C06082A864886F70D020505000410"
"""
using to append the length of signature
"""
PASSWORD_ENCRYPT_PRIVATE_KEY = """
-----BEGIN PRIVATE KEY-----
MIICdwIBADANBgkqhkiG9w0BAQEFAASCAmEwggJdAgEAAoGBANRTaIYwnyvc/MVP
G0G5DZxJNFQPiV3TEqItmtU+ATsS+PZlg2Rnn366rY2PCIxJCODSOSqwcgha/ki3
t9HxAQXQu6BmLRfsyvUuaur7C6cHKKPocWMGs9Lm2+lEfve2GHOI3nDOCoOUHVW/
L2huxFIbq0GaYV4/Bt+Drvcrs+rRAgMBAAECgYEAqsqxIIgJ8XGgjmJ0rWhaz2V/
R0L8H1HDAlum2XM38Dnxs9myWFiWTo2BP4ZXvOie0KGpxHIMODZhp0LJBYJUa832
3UfksdWo2Hh2+91+wex/hmvKxpU787bN7k0KZOs8PehlMqDkwDey3L10ai8aGwgt
4cxr7jp8K2PyGAAAfZUCQQD7bIXmN1nYpouymYstYVIViRB5iNBGP4/KQ7ElSccX
88X3LItlq2MQAFJk0cXNcBfjJQiMtrZxPMofb87yF/z3AkEA2DC2dcqVJ4oEF1yr
nqA/nBNmS0h12R2jkydS3B37k391JI2Z8/B/zjUlWCLUVby9evTYaQkqvgKKmSqU
/yxMdwJBALPSbX43jmoWzAmEKffeCFBgxMi34oarxVLb0WIi/2ORNcDQOi8QQnza
ThPPuRJzHpKWFSRXNeuNl96eIDwkjgcCQG1MK2Lf2YqU1z6sZkObBq20jRnwd0we
FO23iseoDOFkJegmArh2VVb+PXQSn8D829rG4IYx0T8g78tB4PQlBD8CQApJLxS/
kE+fo4XxwPJxK3Kdlm9RzHHnBR2fVMHeoHxYN0PQjc+ZG2pyzTd9ECWyEsaTCDbw
r7pFFeFkd/3ynww=
-----END PRIVATE KEY-----
"""


def decrypt_rsa(cipher: str) -> str:
    private_key = serialization.load_pem_private_key(
        PASSWORD_ENCRYPT_PRIVATE_KEY.encode(),
        password=None,  # Set this if your key is encrypted
        backend=default_backend(),
    )
    if TYPE_CHECKING:
        private_key = cast(RSAPrivateKey, private_key)
    plaintext = private_key.decrypt(bytes.fromhex(cipher), padding.PKCS1v15())
    assert isinstance(plaintext, bytes)
    plaintext_hex = bytes.hex(plaintext)
    return plaintext_hex


def sign_rsa(plan_text: bytes) -> str:
    private_key = serialization.load_pem_private_key(
        PASSWORD_ENCRYPT_PRIVATE_KEY.encode(),
        password=None,  # Set this if your key is encrypted
        backend=default_backend(),
    )
    if TYPE_CHECKING:
        private_key = cast(RSAPrivateKey, private_key)
    sig = private_key.sign(plan_text, padding.PKCS1v15(), algorithm=hashes.MD5())
    assert isinstance(sig, bytes)
    sig_hex = bytes.hex(sig)
    return sig_hex


class RequestBase(lib.RequestBase):
    version: int
    partner_code: int
    namespace_id: int
    game_id: int | None
    command_name: CommandName

    @staticmethod
    def gs_parse(raw_request: HttpData) -> "RequestBase":
        data = lib.RequestBase.gs_parse(raw_request)
        data_dict = data.model_dump()
        data_dict["version"] = int(data.str_mapping["version"])
        data_dict["partner_code"] = int(data.str_mapping["partnercode"])
        data_dict["namespace_id"] = int(data.str_mapping["namespaceid"])
        data_dict["game_id"] = data.int_mapping.get("gameid")
        data = RequestBase(**data_dict)
        return data

    def _parse_password(self):
        password_dict = self.request_dict["password"]
        assert isinstance(password_dict, dict)
        enc_password = password_dict["Value"]
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
    """
    the length of certificate tag and its childen\n
    currently hard coded, no effect on anything
    """
    user_id: int
    profile_id: int
    profile_nick: str
    unique_nick: str
    version: int
    namespace_id: int
    partner_code: int
    cdkey_hash: str | None = None
    session_token: str
    command_name: CommandName
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
            (
                datetime.datetime.now(datetime.timezone.utc)
                + datetime.timedelta(days=1)
            ).timestamp()
        )

    def build(self) -> None:
        self._build_context()
        body = str(self._content)
        headers = {
            "SessionToken": self._result.session_token,
            "Content-Length": len(body),
        }
        self.sending_buffer = HttpData(path=None, headers=headers, body=body)

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
        self._content.add("peerkeymodulus", PEER_KEY_MODULUS)
        self._content.add("peerkeyexponent", PEER_KEY_EXPONENT)
        self._content.add("serverdata", SERVER_DATA)
        hash_str = self.__compute_hash()
        # signature = ClientInfo.SIGNATURE_PREFIX + hash_str
        signature = sign_rsa(hash_str).upper()
        # sig_reverse = int.from_bytes(bytes.fromhex(signature)).to_bytes(len(bytes.fromhex(signature)),"big").hex()
        #! dotnet sdk donot verify signature
        #! c sdk verify signature
        self._content.add("signature", signature)
        self._content.go_to_content_element()
        self._content.add("peerkeyprivate", PEER_KEY_PRIVATE)
        """
        This peer private key is using for p2p key exchange, not for client to web server authentication
        """

    def __compute_hash(self) -> bytes:
        """return md5 str"""
        data_to_hash = bytearray()
        data_to_hash.extend(self._result.length.to_bytes(4, byteorder="little"))
        data_to_hash.extend(self._result.version.to_bytes(4, byteorder="little"))
        data_to_hash.extend(self._result.partner_code.to_bytes(4, byteorder="little"))
        data_to_hash.extend(self._result.namespace_id.to_bytes(4, byteorder="little"))
        data_to_hash.extend(self._result.user_id.to_bytes(4, byteorder="little"))
        data_to_hash.extend(self._result.profile_id.to_bytes(4, byteorder="little"))
        data_to_hash.extend(self._expiretime.to_bytes(4, byteorder="little"))
        data_to_hash.extend(self._result.profile_nick.encode("ascii"))
        data_to_hash.extend(self._result.unique_nick.encode("ascii"))
        if self._result.cdkey_hash is not None:
            data_to_hash.extend(self._result.cdkey_hash.encode("ascii"))

        data_to_hash.extend(bytes.fromhex(PEER_KEY_MODULUS))
        data_to_hash.extend(int(PEER_KEY_EXPONENT, 16).to_bytes(3))
        # server data should be convert to bytes[128] then added to list
        data_to_hash.extend(bytes.fromhex(SERVER_DATA))

        return bytes(data_to_hash)
