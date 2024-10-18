from library.tests.mock_objects.general import ConnectionMock, LogMock, RequestHandlerMock
from library.src.configs import CONFIG
from servers.natneg.src.applications.client import Client


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
