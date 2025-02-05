import asyncio
import threading
from time import sleep
from typing import TYPE_CHECKING, cast
from frontends.gamespy.library.configs import CONFIG
from frontends.tests.gamespy.library.mock_objects import BrokerMock, ConnectionMock, LogMock, RequestHandlerMock, create_mock_url
from frontends.gamespy.protocols.chat.applications.client import Client
from frontends.gamespy.protocols.chat.applications.handlers import *


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
    create_mock_url(config, JoinHandler, JoinResult(
        joiner_nick_name="unispy", joiner_prefix="xiaojiuwo!unispy@UNISPYSERVER", all_channel_user_nicks="test", channel_modes="+q").model_dump())
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
    create_mock_url(config, WhoHandler, WhoResult(infos=[]).model_dump())
    create_mock_url(config, SetChannelKeyHandler, SetChannelKeyResult(
        channel_user_irc_prefix="unispy!unispy@unispyserver", channel_name="test").model_dump())
    create_mock_url(config, GetKeyHandler, GetKeyResult(
        nick_name="unispy", values=[]).model_dump())
    create_mock_url(config, UTMHandler, UTMResult(
        user_irc_prefix="unispy!unispy@unispy", target_name="spyguy").model_dump())

    ChannelHandlerBase._brocker = BrokerMock()
    if TYPE_CHECKING:
        conn._client = cast(Client, conn._client)

    return conn._client


if __name__ == "__main__":
    create_client()
