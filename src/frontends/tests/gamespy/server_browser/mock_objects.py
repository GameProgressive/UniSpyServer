from typing import cast
from frontends.gamespy.library.configs import CONFIG
from frontends.tests.gamespy.library.mock_objects import (
    ConnectionMock,
    LogMock,
    RequestHandlerMock,
    create_mock_url,
)
from frontends.gamespy.protocols.server_browser.v2.applications.client import Client
from frontends.gamespy.protocols.server_browser.v2.applications.handlers import (
    UpdateServerInfoHandler,
    ServerMainListHandler,
)
from frontends.gamespy.protocols.server_browser.v2.contracts.results import (
    UpdateServerInfoResult,
    ServerMainListResult,
)


class ClientMock(Client):
    pass


def create_v2_client() -> Client:
    handler = RequestHandlerMock()
    logger = LogMock()
    conn = ConnectionMock(
        handler=handler,
        config=CONFIG.servers["ServerBrowserV2"],
        t_client=ClientMock,
        logger=logger,
    )
    config = CONFIG.servers["ServerBrowserV2"]

    create_mock_url(
        config,
        ServerMainListHandler,
        ServerMainListResult.model_validate(
            {
                "client_remote_ip": "127.0.0.1",
                "flag": 64,
                "game_secret_key": "123567",
                "servers_info": [],
                "keys": []
            }
        ).model_dump(mode="json"),
    )
    create_mock_url(
        config,
        UpdateServerInfoHandler,
        UpdateServerInfoResult.model_validate(
            {
                "game_server_info": {
                    "server_id": "550e8400-e29b-41d4-a716-446655440000",
                    "host_ip_address": "192.168.1.1",
                    "instant_key": "123456",
                    "game_name": "Example Game",
                    "query_report_port": 8080,
                    "update_time": "2023-10-01T12:00:00Z",
                    "status": 3,
                    "data": {
                        "max_players": "100",
                        "current_players": "50",
                        "region": "US-East",
                    },
                }
            }
        ).model_dump(mode="json"),
    )

    return cast(Client, conn._client)


def create_v1_client() -> Client:
    handler = RequestHandlerMock()
    logger = LogMock()
    conn = ConnectionMock(
        handler=handler,
        config=CONFIG.servers["ServerBrowserV1"],
        t_client=ClientMock,
        logger=logger,
    )
    config = CONFIG.servers["ServerBrowserV1"]
    create_mock_url(
        config,
        ServerMainListHandler,
        ServerMainListResult.model_validate(
            {"remote_ip": conn.remote_ip, "remote_port": conn.remote_port}
        ).model_dump(),
    )

    return cast(Client, conn._client)
