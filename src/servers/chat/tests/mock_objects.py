from typing import TYPE_CHECKING, cast
from library.src.configs import CONFIG
from library.tests.mock_objects import ConnectionMock, LogMock, RequestHandlerMock, create_mock_url
from servers.chat.src.applications.client import Client
from servers.chat.src.applications.handlers import *


class ClientMock(Client):
    pass


def create_client() -> Client:
    handler = RequestHandlerMock()
    logger = LogMock()
    config = CONFIG.servers["Chat"]
    conn = ConnectionMock(
        handler=handler,
        config=config, t_client=ClientMock,
        logger=logger)
    create_mock_url(config, UserIPHandler, {"message": "ok"})
    create_mock_url(config, JoinHandler, {"message": "ok"})
    create_mock_url(config, UserHandler, {"message": "ok"})
    create_mock_url(config, CdKeyHandler, {"message": "ok"})
    create_mock_url(config, GetCKeyHandler, GetCKeyResult.model_validate(
        {"channel_name": "test", "infos": [{"nick_name": "test_nick", "user_values": "/data/key/value/data"}]}).model_dump())
    create_mock_url(config, ModeHandler, ModeResult.model_validate(
        {"channel_name": "test", "channel_modes": "+n", "joiner_nick_name": "test_nick"}).model_dump())
    create_mock_url(config, SetCKeyHandler, {"message": "ok"})
    create_mock_url(config, TopicHandler, TopicResult(
        channel_name="test_chan", channel_topic="test").model_dump())
    create_mock_url(config, PartHandler, PartResult(
        leaver_irc_prefix="test_prefix", is_channel_creator=False, channel_name="test_chan").model_dump())
    create_mock_url(config, NickHandler, NickResult(
        nick_name="test").model_dump())
    if TYPE_CHECKING:
        conn._client = cast(Client, conn._client)
    return conn._client
