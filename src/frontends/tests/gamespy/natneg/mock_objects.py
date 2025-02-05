from frontends.tests.gamespy.library.mock_objects import ConnectionMock, LogMock, RequestHandlerMock, create_mock_url
from frontends.gamespy.library.configs import CONFIG
from frontends.gamespy.protocols.natneg.applications.client import Client
from typing import cast

from frontends.gamespy.protocols.natneg.applications.handlers import AddressCheckHandler, ErtAckHandler, InitHandler, NatifyHandler


class ClientMock(Client):

    pass


def create_client() -> Client:
    handler = RequestHandlerMock()
    logger = LogMock()
    conn = ConnectionMock(
        handler=handler,
        config=CONFIG.servers["NatNegotiation"], t_client=ClientMock,
        logger=logger)

    config = CONFIG.servers["NatNegotiation"]
    create_mock_url(config, InitHandler, {"message": "ok"})
    create_mock_url(config, AddressCheckHandler, {"message": "ok"})
    create_mock_url(config, AddressCheckHandler, {"message": "ok"})
    create_mock_url(config, NatifyHandler, {"message": "ok"})
    create_mock_url(config, ErtAckHandler, {"message": "ok"})
    return cast(Client, conn._client)
