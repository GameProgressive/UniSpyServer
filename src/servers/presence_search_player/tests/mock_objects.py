from library.src.unispy_server_config import CONFIG
from library.tests.mock_objects.general import ConnectionMock, LogMock, RequestHandlerMock
from servers.presence_search_player.src.applications.client import Client


class ClientMock(Client):

    pass


def create_client():
    handler = RequestHandlerMock()
    logger = LogMock()
    conn = ConnectionMock(
        handler=handler,
        config=CONFIG.servers["NatNegotiation"], t_client=ClientMock,
        logger=logger)

    return conn._client
