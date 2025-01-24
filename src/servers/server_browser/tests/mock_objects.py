from typing import cast
from library.src.configs import CONFIG
from library.tests.mock_objects import ConnectionMock, LogMock, RequestHandlerMock, create_mock_url
from servers.server_browser.src.v2.applications.client import Client
from servers.server_browser.src.v2.applications.handlers import ServerInfoHandler, ServerListHandler
from servers.server_browser.src.v2.contracts.results import ServerInfoResult, ServerMainListResult
import json


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

    create_mock_url(config, ServerListHandler, 
        ServerMainListResult.model_validate({"client_remote_ip": "127.0.0.1", "flag": 64,
                                             "game_secret_key": "123567", "servers_info": []}).model_dump(mode="json"))
    create_mock_url(config, ServerInfoHandler, ServerInfoResult.model_validate({"game_server_info": {"server_id": "550e8400-e29b-41d4-a716-446655440000", "host_ip_address": "192.168.1.1", "instant_key": "123456", "game_name": "Example Game", "query_report_port": 8080, "last_heart_beat_received_time": "2023-10-01T12:00:00Z", "status": 3, "server_data": {
                    "max_players": "100", "current_players": "50", "region": "US-East"}, "player_data": [{"player_id": "player1", "player_name": "Player One"}, {"player_id": "player2", "player_name": "Player Two"}], "team_data": [{"team_id": "team1", "team_name": "Team Alpha"}, {"team_id": "team2", "team_name": "Team Beta"}]}}).model_dump(mode="json"))

    return cast(Client, conn._client)


def create_v1_client() -> Client:
    handler = RequestHandlerMock()
    logger = LogMock()
    conn = ConnectionMock(
        handler=handler,
        config=CONFIG.servers["ServerBrowserV1"], t_client=ClientMock,
        logger=logger)
    config = CONFIG.servers["ServerBrowserV1"]
    create_mock_url(config, ServerListHandler, ServerMainListResult.model_validate(
        {"remote_ip_address": conn.remote_ip, "remote_port": conn.remote_port}).model_dump())

    return cast(Client, conn._client)
