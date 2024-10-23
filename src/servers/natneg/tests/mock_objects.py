from library.tests.mock_objects.general import ConnectionMock, LogMock, RequestHandlerMock
from library.src.configs import CONFIG
from servers.natneg.src.applications.client import Client
from typing import cast


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
