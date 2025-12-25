from http.server import BaseHTTPRequestHandler
from typing import cast

from frontends.gamespy.library.abstractions.client import ClientBase, ClientInfoBase
from frontends.gamespy.library.abstractions.switcher import SwitcherBase
from frontends.gamespy.library.exceptions.general import UniSpyException
from frontends.gamespy.library.log.log_manager import LogWriter
from frontends.gamespy.library.network.http_handler import HttpConnection, HttpData
from frontends.gamespy.library.configs import ServerConfig


class ClientInfo(ClientInfoBase):
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


class Client(ClientBase):
    info: ClientInfo
    client_pool: dict[str, "Client"] = {}

    def __init__(self, connection: HttpConnection, server_config: ServerConfig, logger: LogWriter):
        super().__init__(connection, server_config, logger)
        self.info = ClientInfo()
        self.is_log_raw = False

    def on_received(self, buffer: bytes) -> None:
        """
        http server do not have custom encryption
        """
        if not isinstance(buffer, bytes):
            raise UniSpyException("buffer type is invalid")
        self.log_network_receving(buffer)
        switcher = self._create_switcher(buffer)
        if switcher is not None:
            switcher.handle()

    def _create_switcher(self, buffer: bytes) -> SwitcherBase | None:
        """
        this function overide is different than super class \n
        http request need check route, if route url is not in our process list \n
        we log error and return None
        """
        import frontends.gamespy.protocols.web_services.modules.altas.applications.switcher as altas
        import frontends.gamespy.protocols.web_services.modules.auth.applications.switcher as auth
        import frontends.gamespy.protocols.web_services.modules.sake.applications.switcher as sake
        http_h = cast(BaseHTTPRequestHandler, self.connection.handler)
        data = HttpData(path=http_h.path, headers=dict(
            http_h.headers), body=buffer.decode())
        if "sakefileserver/uploadstream.aspx" in http_h.path \
                or "SakeFileServer/download.aspx" in http_h.path\
                or "SakeStorageServer/StorageServer.asmx" in http_h.path:
            return sake.Switcher(self, data)
        elif "SakeStorageServer/Public/StorageServer.asmx" in http_h.path:
            return sake.Switcher(self, data)
        elif "AuthService/AuthService.asmx" in http_h.path:
            return auth.Switcher(self, data)
        elif "/CompetitionService/CompetitionService.asmx" in http_h.path \
                or "/AtlasDataServices/GameConfig.asmx" in http_h.path:
            return altas.Switcher(self, data)
        else:
            self.log_error(f"unsupported url:{http_h.requestline}")
            return None
