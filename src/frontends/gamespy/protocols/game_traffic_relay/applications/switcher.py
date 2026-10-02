from frontends.gamespy.library.abstractions.handler import CmdHandlerBase
from frontends.gamespy.library.abstractions.switcher import SwitcherBase
from frontends.gamespy.library.exceptions.general import UniSpyException
from frontends.gamespy.protocols.game_traffic_relay.applications.client import Client
from frontends.gamespy.protocols.game_traffic_relay.applications.connection import (
    ConnectionListener,
    ConnectStatus,
)
from frontends.gamespy.protocols.game_traffic_relay.applications.handlers import (
    MessageRelayHandler,
    PingHandler,
)
from frontends.gamespy.protocols.game_traffic_relay.contracts.general import (
    MessageRelayRequest,
)
from frontends.gamespy.protocols.natneg.aggregations.enums import CommandName
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
        if name == CommandName.PING.value:
            self._requests.append((CommandName.PING, self._raw_request))
        else:
            self._requests.append((CommandName.RELAY_MSG, self._raw_request))

    def _create_cmd_handlers(
        self, name: CommandName, raw_request: bytes
    ) -> CmdHandlerBase:
        assert isinstance(name, CommandName)
        assert isinstance(raw_request, bytes)
        saved_client = ConnectionListener.get_client_by_ip(
            self._client.connection.ip_endpoint
        )
        if saved_client is None:
            client = self._client
        else:
            client = saved_client
        match name:
            case CommandName.PING:
                return PingHandler(client, PingRequest.gs_parse(raw_request))
            case CommandName.RELAY_MSG:
                return MessageRelayHandler(
                    client, MessageRelayRequest.gs_parse(raw_request)
                )
            case _:
                raise UniSpyException("unable to handle the message")
