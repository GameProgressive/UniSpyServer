from socket import inet_ntoa
from typing import TYPE_CHECKING
import library.src.abstractions.contracts

from library.src.extentions.bytes_extentions import ip_to_4_bytes
from library.src.extentions.encoding import get_bytes
from servers.query_report.src.aggregates.game_server_info import GameServerInfo
from servers.server_browser.src.v2.aggregations.encryption import SERVER_CHALLENGE
from servers.server_browser.src.v2.aggregations.string_flags import STRING_SPLITER
from servers.server_browser.src.v2.aggregations.enums import (
    DataKeyType,
    GameServerFlags,
    RequestType,
    ServerListUpdateOption,
)

QUERY_REPORT_DEFAULT_PORT: int = int(6500)


class RequestBase(library.src.abstractions.contracts.RequestBase):
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


class ResultBase(library.src.abstractions.contracts.ResultBase):
    pass


class ResponseBase(library.src.abstractions.contracts.ResponseBase):
    _request: RequestBase
    _result: ResultBase
    sending_buffer: bytes

    def __init__(self, request: RequestBase, result: ResultBase) -> None:
        assert issubclass(type(request), RequestBase)
        assert issubclass(type(result), ResultBase)
        super().__init__(request, result)


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
    filter: str
    source_ip: str
    max_servers: int

    def __init__(self, raw_request: bytes):
        assert isinstance(raw_request, bytes)
        super().__init__(raw_request)


class ServerListUpdateOptionResultBase(ResultBase):
    client_remote_ip: str
    flag: GameServerFlags
    game_secret_key: str


class ServerListUpdateOptionResponseBase(ResponseBase):
    _request: ServerListUpdateOptionRequestBase
    _result: ServerListUpdateOptionResultBase
    _servers_info_buffers: bytearray

    def __init__(
        self,
        request: ServerListUpdateOptionRequestBase,
        result: ServerListUpdateOptionResultBase,
    ) -> None:
        assert issubclass(type(request), ServerListUpdateOptionRequestBase)
        assert issubclass(type(result), ServerListUpdateOptionResultBase)
        super().__init__(request, result)
        self._servers_info_buffers = bytearray()

    def build(self) -> None:
        crypt_header = self.build_crypt_header()
        self._servers_info_buffers.extend(crypt_header)
        self._servers_info_buffers.extend(
            ip_to_4_bytes(self._result.client_remote_ip))
        self._servers_info_buffers.extend(
            QUERY_REPORT_DEFAULT_PORT.to_bytes(4))

    def build_crypt_header(self) -> list:
        # cryptHeader have 14 bytes, when we encrypt data we need skip the first 14 bytes
        crypt_header = []
        crypt_header.append(2 ^ 0xEC)
        crypt_header.extend([0, 0])  # message length?
        crypt_header.append(len(SERVER_CHALLENGE) ^ 0xEA)
        crypt_header.extend(get_bytes(SERVER_CHALLENGE))
        return crypt_header

    def build_server_keys(self) -> None:
        # we add the total number of the requested keys
        self._servers_info_buffers.append(len(self._request.keys))
        # then we add the keys
        for key in self._request.keys:
            self._servers_info_buffers.append(DataKeyType.STRING)
            self._servers_info_buffers.extend(get_bytes(key))
            self._servers_info_buffers.extend(STRING_SPLITER)

    def build_unique_value(self):
        self._servers_info_buffers.append(0)


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

    def __init__(self, request: RequestBase, result: ResultBase) -> None:
        super().__init__(request, result)
        self._buffer = bytearray()
