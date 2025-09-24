from typing import cast
from frontends.gamespy.library.configs import CONFIG
from frontends.gamespy.protocols.query_report.applications.client import Client
from frontends.gamespy.protocols.query_report.v2.applications.handlers import AvailableHandler, HeartBeatHandler, KeepAliveHandler
from frontends.gamespy.protocols.query_report.v2.contracts.results import HeartBeatResult
from frontends.tests.gamespy.library.mock_objects import ConnectionMock, LogMock, RequestHandlerMock, create_mock_url


class ClientMock(Client):
    pass


def create_client() -> Client:
    CONFIG.unittest.is_raise_except = True
    handler = RequestHandlerMock()
    logger = LogMock()
    conn = ConnectionMock(
        handler=handler,
        config=CONFIG.servers["QueryReport"], t_client=ClientMock,
        logger=logger)
    config = CONFIG.servers["QueryReport"]
    create_mock_url(config, HeartBeatHandler, HeartBeatResult.model_validate(
        {"remote_ip": conn.remote_ip, "remote_port": conn.remote_port, "instant_key": "123", "command_name": 3}).model_dump(mode='json'))
    create_mock_url(config, AvailableHandler, {"message": "ok"})
    create_mock_url(config, KeepAliveHandler, {"message": "ok"})
    return cast(Client, conn._client)
