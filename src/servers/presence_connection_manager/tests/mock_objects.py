
from library.src.unispy_server_config import CONFIG
from library.tests.mock_objects.general import ConnectionMock, LogMock, RequestHandlerMock
from servers.natneg.tests.mock_objects import ClientMock


def create_client():
    handler = RequestHandlerMock()
    logger = LogMock()
    conn = ConnectionMock(
        handler=handler,
        config=CONFIG.servers["PresenceConnectionManager"], t_client=ClientMock,
        logger=logger)

    return conn._client
