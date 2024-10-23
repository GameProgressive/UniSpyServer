from typing import cast
from library.src.configs import CONFIG
from library.tests.mock_objects.general import ConnectionMock, LogMock, RequestHandlerMock
from servers.presence_search_player.src.applications.client import Client


class ClientMock(Client):

    pass


def create_client() -> Client:
    handler = RequestHandlerMock()
    logger = LogMock()
    conn = ConnectionMock(
        handler=handler,
        config=CONFIG.servers["PresenceSearchPlayer"], t_client=ClientMock,
        logger=logger)

    return cast(Client, conn._client)
