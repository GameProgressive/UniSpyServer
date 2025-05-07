from typing import cast
from frontends.gamespy.library.configs import CONFIG
from frontends.gamespy.protocols.game_status.applications.client import Client
from frontends.gamespy.protocols.game_status.applications.handlers import AuthGameHandler, AuthPlayerHandler, GetPlayerDataHandler, GetProfileIdHandler, NewGameHandler, SetPlayerDataHandler, UpdateGameHandler
from frontends.gamespy.protocols.game_status.contracts.results import AuthGameResult, AuthPlayerResult, GetPlayerDataResult, GetProfileIdResult
from frontends.tests.gamespy.library.mock_objects import ConnectionMock, LogMock, RequestHandlerMock, create_mock_url

from frontends.gamespy.library.exceptions.general import UniSpyException

class ClientMock(Client):
    pass


def create_client() -> Client:
    UniSpyException._is_unittesting = True
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
