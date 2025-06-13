from typing import Optional
from frontends.gamespy.library.abstractions.switcher import SwitcherBase
from frontends.gamespy.protocols.game_traffic_relay.applications.client import Client
from frontends.gamespy.protocols.game_traffic_relay.applications.handlers import (
    PingHandler)
from frontends.gamespy.protocols.natneg.aggregations.enums import RequestType
from frontends.gamespy.protocols.natneg.contracts.requests import PingRequest


class Switcher(SwitcherBase):
    _raw_request: bytes
    _client: Client

    def __init__(self, client: Client, raw_request: bytes) -> None:
        super().__init__(client, raw_request)
        assert issubclass(type(client), Client)
        assert isinstance(raw_request, bytes)

    def _process_raw_request(self) -> None:
        name = self._raw_request[7]
        if name not in RequestType:
            self._client.log_debug(f"Request: {name} is not a valid request.")
            return
        self._requests.append((RequestType(name), self._raw_request))

    def _create_cmd_handlers(
        self, name: RequestType, raw_request: bytes
    ) -> Optional[PingHandler | None]:
        assert isinstance(name, RequestType)
        assert isinstance(raw_request, bytes)
        match name:
            case RequestType.PING:
                return PingHandler(self._client, PingRequest(raw_request))
            case _:
                return None
