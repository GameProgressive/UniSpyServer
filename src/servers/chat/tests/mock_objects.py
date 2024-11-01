from typing import TYPE_CHECKING, cast
from library.src.configs import CONFIG
from library.tests.mock_objects import ConnectionMock, LogMock, RequestHandlerMock, create_mock_url
from servers.chat.src.applications.client import Client
from servers.chat.src.applications.handlers import UserIPHandler


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
    create_mock_url(config, UserIPHandler, {"message": "ok"})
    
    return conn._client
