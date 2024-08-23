from socket import inet_ntoa
from typing import List, Optional
import library.src.abstractions.contracts
import abc

from library.src.extentions.encoding import get_bytes
from servers.server_browser.src.v2.aggregations.encryption import SERVER_CHALLENGE
from servers.server_browser.src.v2.aggregations.string_flags import STRING_SPLITER
from servers.server_browser.src.v2.contracts.results import AdHocResult
from servers.server_browser.src.v2.enums.general import (
    DataKeyType,
    GameServerFlags,
    RequestType,
    ServerListUpdateOption,
)

QUERY_REPORT_DEFAULT_PORT = 6500


class RequestBase(library.src.abstractions.contracts.RequestBase):
    request_length: int
    raw_request: bytes
    command_name: RequestType

    def __init__(self, raw_request: bytes) -> None:
        assert isinstance(raw_request, bytes)
        super().__init__(raw_request)

    def parse(self) -> None:
        self.request_length = int.from_bytes(self.raw_request[:2], byteorder="little")
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
    def __init__(self):
        self.request_version: Optional[int] = None
        self.protocol_version: Optional[int] = None
        self.encoding_version: Optional[int] = None
        self.game_version: Optional[int] = None
        self.query_options: Optional[int] = None
        self.dev_game_name: Optional[str] = None
        self.game_name: Optional[str] = None
        self.client_challenge: Optional[str] = None
        self.update_option: Optional[ServerListUpdateOption] = None
        self.keys: Optional[List[str]] = None
        self.filter: Optional[str] = None
        self.source_ip: str
        self.max_servers: Optional[int] = None

    def __init__(self, raw_request: bytes):
        assert isinstance(raw_request, bytes)
        super().__init__(raw_request)


class ServerListUpdateOptionResultBase(ResultBase):
    client_remote_ip: bytes
    flag: GameServerFlags
    game_secret_key: str


class ServerListUpdateOptionResponseBase(ResponseBase):
    _request: ServerListUpdateOptionRequestBase
    _result: ServerListUpdateOptionResultBase
    _servers_info_buffers: list = []

    def __init__(
        self,
        request: ServerListUpdateOptionRequestBase,
        result: ServerListUpdateOptionResultBase,
    ) -> None:
        assert issubclass(type(request), ServerListUpdateOptionRequestBase)
        assert issubclass(type(result), ServerListUpdateOptionResultBase)
        super().__init__(request, result)

    def build(self) -> None:
        crypt_header = self.build_crypt_header()
        self._servers_info_buffers.extend(crypt_header)
        self._servers_info_buffers.extend(self._result.client_remote_ip)
        self._servers_info_buffers.extend(bytes(6500))

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
            self._servers_info_buffers.append(STRING_SPLITER)

    def build_unique_value(self):
        self._servers_info_buffers.append(0)


class AdHocRequestBase(RequestBase):
    game_server_public_ip: bytes
    game_server_public_port: bytes

    def parse(self) -> None:
        super().parse()
        self.game_server_public_ip = inet_ntoa(self.raw_request[3:7])
        self.game_server_public_port = int.from_bytes(
            self.raw_request[7:9], byteorder="big"
        )


class AdHocResponseBase(ResponseBase):
    _result: AdHocResult
    _buffer: list[int] = []
