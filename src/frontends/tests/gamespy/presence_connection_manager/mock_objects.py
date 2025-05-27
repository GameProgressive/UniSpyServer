from typing import cast
from frontends.gamespy.library.configs import CONFIG
from frontends.tests.gamespy.library.mock_objects import (
    ConnectionMock,
    LogMock,
    RequestHandlerMock,
    create_mock_url,
)
from frontends.gamespy.protocols.presence_connection_manager.applications.client import (
    Client,
)
from frontends.gamespy.protocols.presence_connection_manager.applications.handlers import (
    LoginHandler,
    NewUserHandler,
)


class ClientMock(Client):
    pass


def create_client() -> Client:
    CONFIG.unittest.is_raise_except = True
    handler = RequestHandlerMock()
    logger = LogMock()
    conn = ConnectionMock(
        handler=handler,
        config=CONFIG.servers["PresenceConnectionManager"],
        t_client=ClientMock,
        logger=logger,
    )
    config = CONFIG.servers["PresenceConnectionManager"]
    create_mock_url(config, NewUserHandler, {"user_id": 0, "profile_id": 0})
    create_mock_url(
        config,
        LoginHandler,
        {
            "response_proof": "7f2c9c6685570ea18b7207d2cbd72452",
            "data": {
                "user_id": 0,
                "profile_id": 0,
                "nick": "test",
                "email": "test@gamespy.com",
                "unique_nick": "test_unique",
                "password_hash": "password",
                "email_verified_flag": True,
                "namespace_id": 0,
                "sub_profile_id": 0,
                "banned_flag": False,
            },
        },
    )

    return cast(Client, conn._client)
