from typing import cast
from library.src.configs import CONFIG
from library.tests.mock_objects import ConnectionMock, LogMock, RequestHandlerMock, create_mock_url
from servers.game_status.src.applications.client import Client
from servers.game_status.src.applications.handlers import AuthGameHandler, AuthPlayerHandler, GetPlayerDataHandler, GetProfileIdHandler, NewGameHandler, SetPlayerDataHandler, UpdateGameHandler
from servers.game_status.src.contracts.results import AuthGameResult, AuthPlayerResult, GetPlayerDataResult, GetProfileIdResult


class ClientMock(Client):
    pass


def create_client() -> Client:
    handler = RequestHandlerMock()
    logger = LogMock()
    conn = ConnectionMock(
        handler=handler,
        config=CONFIG.servers["GameStatus"], t_client=ClientMock,
        logger=logger)
    config = CONFIG.servers["GameStatus"]
    create_mock_url(config, SetPlayerDataHandler, {"message": "ok"})
    create_mock_url(config, GetPlayerDataHandler, GetPlayerDataResult(
        **{"keyvalues": {"hello": "hello_value", "hi": "hi_value"}}).model_dump())
    create_mock_url(config, GetProfileIdHandler,
                    GetProfileIdResult(**{"profile_id": 1}).model_dump())
    create_mock_url(config, UpdateGameHandler, {"message": "ok"})
    create_mock_url(config, AuthPlayerHandler,
                    AuthPlayerResult(**{"profile_id": 1}).model_dump())
    create_mock_url(config, NewGameHandler,  {"message": "ok"})
    create_mock_url(config, AuthGameHandler,  AuthGameResult(
        **{"session_key": "123456"}).model_dump())

    return cast(Client, conn._client)
