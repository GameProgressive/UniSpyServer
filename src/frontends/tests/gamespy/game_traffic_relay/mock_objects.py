from typing import cast
from frontends.gamespy.library.configs import CONFIG
from frontends.gamespy.protocols.game_traffic_relay.applications.client import Client
from frontends.gamespy.protocols.game_traffic_relay.applications.handlers import (
    PingHandler,
)
from frontends.tests.gamespy.library.mock_objects import (
    ConnectionMock,
    LogMock,
    RequestHandlerMock,
    create_mock_url,
)


class ClientMock(Client):
    pass


def create_client(client_address: tuple = ("192.168.0.1", 0)) -> Client:
    CONFIG.unittest.is_raise_except = True
    handler = RequestHandlerMock(client_address)
    logger = LogMock()
    conn = ConnectionMock(
        handler=handler,
        config=CONFIG.servers["GameTrafficRelay"],
        t_client=ClientMock,
        logger=logger,
    )

    config = CONFIG.servers["GameTrafficRelay"]
    create_mock_url(config, PingHandler, {"message": "ok"})
    return cast(Client, conn._client)
