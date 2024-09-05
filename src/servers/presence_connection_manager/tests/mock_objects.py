
from library.src.unispy_server_config import CONFIG
from library.tests.mock_objects.general import ConnectionMock, LogMock, RequestHandlerMock
from servers.presence_connection_manager.src.applications.client import Client


class ClientMock(Client):
    pass


def create_client():
    handler = RequestHandlerMock()
    logger = LogMock()
    conn = ConnectionMock(
        handler=handler,
        config=CONFIG.servers["PresenceConnectionManager"], t_client=ClientMock,
        logger=logger)

    return conn._client
