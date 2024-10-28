from library.tests.mock_objects import ConnectionMock, LogMock, RequestHandlerMock, create_mock_url
from library.src.configs import CONFIG
from servers.natneg.src.applications.client import Client
from typing import cast

from servers.natneg.src.applications.handlers import AddressCheckHandler, InitHandler, NatifyHandler


class ClientMock(Client):

    pass


def create_client() -> Client:
    handler = RequestHandlerMock()
    logger = LogMock()
    conn = ConnectionMock(
        handler=handler,
        config=CONFIG.servers["NatNegotiation"], t_client=ClientMock,
        logger=logger)

    return cast(Client, conn._client)


config = CONFIG.servers["NatNegotiation"]
create_mock_url(config, InitHandler, {"message": "ok"})
create_mock_url(config, AddressCheckHandler, {"message": "ok"})
create_mock_url(config, AddressCheckHandler, {"message": "ok"})
create_mock_url(config, NatifyHandler, {"message": "ok"})
