from typing import cast
from library.src.configs import CONFIG
from library.tests.mock_objects import ConnectionMock, LogMock, RequestHandlerMock, create_mock_url
from servers.query_report.src.v2.applications.handlers import HeartBeatHandler
from servers.query_report.src.v2.contracts.results import HeartBeatResult
from servers.server_browser.src.v2.applications.client import Client


class ClientMock(Client):
    pass


def create_v2_client() -> Client:
    handler = RequestHandlerMock()
    logger = LogMock()
    conn = ConnectionMock(
        handler=handler,
        config=CONFIG.servers["ServerBrowserV2"], t_client=ClientMock,
        logger=logger)
    config = CONFIG.servers["ServerBrowserV2"]
    create_mock_url(config, HeartBeatHandler, HeartBeatResult.model_validate(
        {"remote_ip_address": conn.remote_ip, "remote_port": conn.remote_port}).model_dump())

    return cast(Client, conn._client)


def create_v1_client() -> Client:
    handler = RequestHandlerMock()
    logger = LogMock()
    conn = ConnectionMock(
        handler=handler,
        config=CONFIG.servers["ServerBrowserV1"], t_client=ClientMock,
        logger=logger)
    config = CONFIG.servers["ServerBrowserV1"]
    create_mock_url(config, HeartBeatHandler, HeartBeatResult.model_validate(
        {"remote_ip_address": conn.remote_ip, "remote_port": conn.remote_port}).model_dump())

    return cast(Client, conn._client)
