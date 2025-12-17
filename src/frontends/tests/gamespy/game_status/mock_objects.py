from datetime import datetime
from typing import cast
from frontends.gamespy.library.configs import CONFIG
from frontends.gamespy.protocols.game_status.applications.client import Client
from frontends.gamespy.protocols.game_status.applications.handlers import (
    AuthGameHandler,
    AuthPlayerHandler,
    GetPlayerDataHandler,
    GetProfileIdHandler,
    NewGameHandler,
    SetPlayerDataHandler,
    UpdateGameHandler,
)
from frontends.gamespy.protocols.game_status.contracts.results import (
    AuthGameResult,
    AuthPlayerResult,
    GetPlayerDataResult,
    GetProfileIdResult,
)
from frontends.tests.gamespy.library.mock_objects import (
    ConnectionMock,
    LogMock,
    RequestHandlerMock,
    create_mock_url,
)


class ClientMock(Client):
    pass


def create_client() -> Client:
    CONFIG.unittest.is_raise_except = True
    handler = RequestHandlerMock()
    logger = LogMock()
    conn = ConnectionMock(
        handler=handler,
        config=CONFIG.servers["GameStatus"],
        t_client=ClientMock,
        logger=logger,
    )
    config = CONFIG.servers["GameStatus"]
    create_mock_url(config, SetPlayerDataHandler, {"message": "ok"})
    create_mock_url(
        config,
        GetPlayerDataHandler,
        GetPlayerDataResult(
            data="\\key1\\value1\\key2\\value2\\key3\\value3",
            local_id=0,
            profile_id=0,
            modified=datetime.now()
        ).model_dump(mode="json"),
    )
    create_mock_url(
        config,
        GetProfileIdHandler,
        GetProfileIdResult.model_validate(
            {"profile_id": 1, "local_id": 0}).model_dump(),
    )
    create_mock_url(config, UpdateGameHandler, {"message": "ok"})
    create_mock_url(
        config, AuthPlayerHandler, AuthPlayerResult.model_validate(
            {"profile_id": 1, "local_id": 0}).model_dump(mode="json")
    )
    create_mock_url(config, NewGameHandler, {"message": "ok"})
    create_mock_url(
        config,
        AuthGameHandler,
        AuthGameResult(session_key="123456",
                       local_id=0,
                       game_name="gmtest").model_dump(),
    )

    return cast(Client, conn._client)
