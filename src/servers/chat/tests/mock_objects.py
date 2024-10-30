from typing import TYPE_CHECKING, cast
from library.src.configs import CONFIG
from library.tests.mock_objects import ConnectionMock, LogMock, RequestHandlerMock
from servers.chat.src.applications.client import Client


class ClientMock(Client):
    pass


def create_client() -> Client:
    handler = RequestHandlerMock()
    logger = LogMock()
    config = CONFIG.servers["Chat"]
    conn = ConnectionMock(
        handler=handler,
        config=config, t_client=ClientMock,
        logger=logger)
    if TYPE_CHECKING:
        conn._client = cast(Client, conn._client)
    return conn._client
