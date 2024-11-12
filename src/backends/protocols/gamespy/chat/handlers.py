from datetime import datetime
from typing import TYPE_CHECKING, cast
from backends.library.abstractions.contracts import RequestBase
from backends.library.abstractions.handler_base import HandlerBase
from backends.library.database.pg_orm import PG_SESSION, ChatChannelCaches, ChatNickCaches, ChatUserCaches
import backends.protocols.gamespy.chat.data as data
from backends.protocols.gamespy.chat.requests import *
from servers.chat.src.aggregates.exceptions import LoginFailedException, NickNameInUseException, NoSuchChannelException, NoSuchNickException
from servers.chat.src.contracts.results import CryptResult, GetKeyResult, NickResult


class CdKeyHandler(HandlerBase):
    _request: CdkeyRequest

    async def _data_operate(self) -> None:
        is_valid = data.is_cdkey_valid(self._request.cdkey)
        if not is_valid:
            raise LoginFailedException("cdkey not matched")


class CryptHandler(HandlerBase):
    _request: CryptRequest

    async def _data_operate(self) -> None:
        result = PG_SESSION.query(ChatNickCaches).where(ChatNickCaches.remote_ip_address ==
                                                        self._request.client_ip, ChatNickCaches.remote_port == self._request.client_port).first()
        if result is None:
            raise NoSuchNickException(
                f"No nick found for {self._request.client_ip}")
        result.game_name = self._request.gamename  # type: ignore
        PG_SESSION.commit()

    async def _result_construct(self) -> None:
        self._result = CryptResult()


class GetKeyHandler(HandlerBase):
    _request: GetKeyRequest

    async def _data_operate(self) -> None:
        caches = PG_SESSION.query(ChatNickCaches.key_value).where(
            ChatNickCaches.nick_name == self._request.nick_name).first()

        if caches is None:
            raise NoSuchNickException("nick not found")
        if TYPE_CHECKING:
            self.caches = cast(list, caches)

    async def _result_construct(self) -> None:
        self._result = GetKeyResult(
            nick_name=self._request.nick_name, values=self.caches)


class GetUdpRelayHandler(HandlerBase):
    _request: GetUdpRelayRequest

    def __init__(self, request: RequestBase) -> None:
        super().__init__(request)
        raise NotImplementedError()


class InviteHandler(HandlerBase):
    _request: InviteRequest

    async def _data_operate(self) -> None:
        chann = PG_SESSION.query(ChatChannelCaches).join(ChatUserCaches).where(
            ChatUserCaches.channel_name == self._request.channel_name, ChatUserCaches.remote_ip_address == self._request.client_ip, ChatUserCaches.remote_port == self._request.client_port).first()
        if chann is None:
            raise NoSuchChannelException(
                "you have to be in this channel to invite your friends")

        chann.invited_nicks.append(self._request.nick_name)


class NickHandler(HandlerBase):
    _request: NickRequest

    async def _data_operate(self) -> None:
        is_nick = data.is_nick_exist(self._request.nick_name)
        if is_nick:
            raise NickNameInUseException(
                old_nick=self._request.nick_name, new_nick="", message="nick name in use")
        else:
            cache = ChatNickCaches(nick_name=self._request.nick_name,
                                   server_id=self._request.server_id,
                                   update_time=datetime.now())
            data.add_nick_cache(cache)

    async def _result_construct(self) -> None:
        self._result = NickResult(nick_name=self._request.nick_name)

# class JoinHandler(HandlerBase):
#     _request: JoinRequest

#     async def _data_fetch(self) -> None:
#         is_chan_exist = data.is_channel_exist(self._request.channel_name,
#                                               self._request.game_name)
#         # group_id =
#         if is_chan_exist:
#             is_user_exist = data.is_user_exist(
#                 self._request.client_ip, self._request.client_port)
#         else:
#             # create channel
#             # create user
#             is_peer_room =
#             chan = ChatChannelCaches(channel_name=self._request.channel_name, server_id=self._request.server_id, game_name, room_name, topic,
#                                      password=self._request.password, group_id, max_num_user=200, key_values=None, update_time=datetime.now())
