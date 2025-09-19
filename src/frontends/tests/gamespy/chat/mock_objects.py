from typing import TYPE_CHECKING, cast
from frontends.gamespy.library.abstractions.brocker import BrockerBase
from frontends.gamespy.library.configs import CONFIG
from frontends.gamespy.protocols.chat.applications.handlers import (
    CdKeyHandler,
    GetCKeyHandler,
    GetKeyHandler,
    JoinHandler,
    ModeHandler,
    NamesHandler,
    NickHandler,
    PartHandler,
    QuitHandler,
    SetCKeyHandler,
    SetChannelKeyHandler,
    TopicHandler,
    UTMHandler,
    UserHandler,
    UserIPHandler,
    WhoHandler,
    LoginHandler,
    CryptHandler,
)
from frontends.gamespy.protocols.chat.contracts.results import (
    CryptResult,
    GetCKeyResult,
    GetKeyResult,
    JoinResult,
    LoginResult,
    ModeResult,
    NamesResult,
    NamesResultData,
    NickResult,
    PartResult,
    SetChannelKeyResult,
    TopicResult,
    UtmResult,
    WhoResult,
)
from frontends.tests.gamespy.library.mock_objects import (
    ConnectionMock,
    LogMock,
    RequestHandlerMock,
    create_mock_url,
)
from frontends.gamespy.protocols.chat.applications.client import Client


class WebSocketBrockerMock(BrockerBase):
    def __init__(self) -> None:
        super().__init__("test", "ws://test.com", print)

    def subscribe(self):
        pass

    def publish_message(self, message):
        pass

    def unsubscribe(self):
        pass

    @property
    def ip_port(self):
        return "127.0.0.1:10086"


class ClientMock(Client):
    def start_brocker(self):
        self.brocker = WebSocketBrockerMock()  # type:ignore
        self.brocker.subscribe()  # type:ignore


def create_client() -> Client:
    CONFIG.unittest.is_raise_except = True
    handler = RequestHandlerMock()
    logger = LogMock()
    config = CONFIG.servers["Chat"]
    conn = ConnectionMock(
        handler=handler, config=config, t_client=ClientMock, logger=logger
    )
    create_mock_url(config, CryptHandler, CryptResult(secret_key="test").model_dump())
    create_mock_url(
        config, LoginHandler, LoginResult(profile_id=1, user_id=1).model_dump()
    )
    create_mock_url(
        config,
        NamesHandler,
        NamesResult(
            channel_nicks=[NamesResultData(nick_name="test")],
            channel_name="test",
            requester_nick_name="test1",
        ).model_dump(),
    )
    create_mock_url(config, QuitHandler, {"message": "ok"})
    create_mock_url(config, UserIPHandler, {"message": "ok"})
    create_mock_url(
        config,
        JoinHandler,
        JoinResult(
            joiner_nick_name="nickname", joiner_user_name="username"
        ).model_dump(),
    )
    create_mock_url(config, UserHandler, {"message": "ok"})
    create_mock_url(config, CdKeyHandler, {"message": "ok"})
    create_mock_url(
        config,
        GetCKeyHandler,
        GetCKeyResult.model_validate(
            {
                "channel_name": "test",
                "infos": [
                    {
                        "nick_name": "test_nick",
                        "user_values": ["data", "key", "value", "data"],
                    }
                ],
            }
        ).model_dump(),
    )
    create_mock_url(
        config,
        ModeHandler,
        ModeResult.model_validate(
            {
                "channel_name": "test",
                "channel_modes": ["n", "m"],
                "joiner_nick_name": "test_nick",
            }
        ).model_dump(),
    )
    create_mock_url(config, SetCKeyHandler, {"message": "ok"})
    create_mock_url(
        config,
        TopicHandler,
        TopicResult(channel_name="test_chan", channel_topic="test").model_dump(),
    )
    create_mock_url(
        config,
        PartHandler,
        PartResult(
            leaver_nick_name="nickname",
            leaver_user_name="username",
            is_channel_creator=False,
            channel_name="test_chan",
        ).model_dump(),
    )
    create_mock_url(config, NickHandler, NickResult(nick_name="test").model_dump())
    create_mock_url(config, WhoHandler, WhoResult(infos=[]).model_dump())
    create_mock_url(
        config,
        SetChannelKeyHandler,
        SetChannelKeyResult(
            setter_nick_name="nickname",
            setter_user_name="username",
            channel_name="test",
        ).model_dump(),
    )
    create_mock_url(
        config, GetKeyHandler, GetKeyResult(nick_name="unispy", values=[]).model_dump()
    )
    create_mock_url(
        config,
        UTMHandler,
        UtmResult(nick_name="unispy", user_name="unispy").model_dump(),
    )

    if TYPE_CHECKING:
        conn._client = cast(Client, conn._client)
    conn._client.on_connected()
    return conn._client


if __name__ == "__main__":
    create_client()
