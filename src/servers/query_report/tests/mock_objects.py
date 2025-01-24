from typing import cast
from library.src.configs import CONFIG
from library.tests.mock_objects import ConnectionMock, LogMock, RequestHandlerMock, create_mock_url
from servers.query_report.src.applications.client import Client
from servers.query_report.src.v2.applications.handlers import AvailableHandler, HeartBeatHandler, KeepAliveHandler
from servers.query_report.src.v2.contracts.results import HeartBeatResult


class ClientMock(Client):
    pass


def create_client() -> Client:
    handler = RequestHandlerMock()
    logger = LogMock()
    conn = ConnectionMock(
        handler=handler,
        config=CONFIG.servers["QueryReport"], t_client=ClientMock,
        logger=logger)
    config = CONFIG.servers["QueryReport"]
    create_mock_url(config, HeartBeatHandler, HeartBeatResult.model_validate(
        {"remote_ip_address": conn.remote_ip, "remote_port": conn.remote_port}).model_dump(mode='json'))
    create_mock_url(config, AvailableHandler, {"message": "ok"})
    create_mock_url(config, KeepAliveHandler, {"message": "ok"})
    return cast(Client, conn._client)
