from socket import inet_ntoa
import frontends.gamespy.library.abstractions.contracts as lib

from frontends.gamespy.library.extentions.bytes_extentions import ip_to_4_bytes
from frontends.gamespy.library.extentions.encoding import get_bytes
from frontends.gamespy.protocols.query_report.aggregates.game_server_info import (
    GameServerInfo,
)
from frontends.gamespy.protocols.server_browser.v2.aggregations.encryption import (
    SERVER_CHALLENGE,
)
from frontends.gamespy.protocols.server_browser.v2.aggregations.string_flags import (
    STRING_SPLITER,
)
from frontends.gamespy.protocols.server_browser.v2.aggregations.enums import (
    DataKeyType,
    GameServerFlags,
    RequestType,
    ServerListUpdateOption,
)

QUERY_REPORT_DEFAULT_PORT: int = int(6500)


class RequestBase(lib.RequestBase):
    request_length: int
    raw_request: bytes
    command_name: RequestType

    def __init__(self, raw_request: bytes) -> None:
        assert isinstance(raw_request, bytes)
        super().__init__(raw_request)

    def parse(self) -> None:
        self.request_length = int.from_bytes(
            self.raw_request[:2], byteorder="little")
        self.command_name = RequestType(self.raw_request[2])


class ResultBase(lib.ResultBase):
    pass


class ResponseBase(lib.ResponseBase):
    _result: ResultBase
    sending_buffer: bytes

    def __init__(self, result: ResultBase) -> None:
        assert issubclass(type(result), ResultBase)
        super().__init__(result)


class ServerListUpdateOptionRequestBase(RequestBase):
    request_version: int
    protocol_version: int
    encoding_version: int
    game_version: int
    query_options: int
    dev_game_name: str
    game_name: str
    client_challenge: str
    update_option: ServerListUpdateOption
    keys: list[str]
    filter: str | None
    source_ip: str
    max_servers: int

    def __init__(self, raw_request: bytes):
        assert isinstance(raw_request, bytes)
        super().__init__(raw_request)
        self.filter = None


class ServerListUpdateOptionResultBase(ResultBase):
    client_remote_ip: str
    flag: GameServerFlags
    game_secret_key: str
    keys: list[str]


class ServerListUpdateOptionResponseBase(ResponseBase):
    _result: ServerListUpdateOptionResultBase
    _buffer: bytearray

    def __init__(
        self,
        result: ServerListUpdateOptionResultBase,
    ) -> None:
        assert issubclass(type(result), ServerListUpdateOptionResultBase)
        super().__init__(result)
        self._buffer = bytearray()

    def build(self) -> None:
        crypt_header = self.build_crypt_header()
        self._buffer.extend(crypt_header)
        self._buffer.extend(
            ip_to_4_bytes(self._result.client_remote_ip))
        self._buffer.extend(
            QUERY_REPORT_DEFAULT_PORT.to_bytes(2))
        assert len(self._buffer) == 20

    def build_crypt_header(self) -> bytearray:
        # cryptHeader have 14 bytes, when we encrypt data we need skip the first 14 bytes
        crypt_header = bytearray()
        crypt_header.append(2 ^ 0xEC)
        crypt_header.extend([0, 0])  # message length?
        crypt_header.append(len(SERVER_CHALLENGE) ^ 0xEA)
        crypt_header.extend(get_bytes(SERVER_CHALLENGE))
        assert len(crypt_header) == 14
        return crypt_header

    def build_server_keys(self) -> None:
        # we add the total number of the requested keys
        self._buffer.append(len(self._result.keys))
        # then we add the keys
        for key in self._result.keys:
            self._buffer.append(DataKeyType.STRING)
            self._buffer.extend(get_bytes(key))
            self._buffer.append(STRING_SPLITER)

    def build_unique_value(self):
        self._buffer.append(0)


class AdHocResultBase(ResultBase):
    game_server_info: GameServerInfo


class AdHocRequestBase(RequestBase):
    game_server_public_ip: str
    game_server_public_port: int

    def parse(self) -> None:
        super().parse()
        self.game_server_public_ip = inet_ntoa(self.raw_request[3:7])
        self.game_server_public_port = int.from_bytes(
            self.raw_request[7:9], byteorder="big"
        )


class AdHocResponseBase(ResponseBase):
    _result: AdHocResultBase
    _buffer: bytearray

    def __init__(self, result: ResultBase) -> None:
        super().__init__(result)
        self._buffer = bytearray()
