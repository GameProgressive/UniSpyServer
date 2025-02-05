from datetime import datetime
from typing import TYPE_CHECKING, cast
from backends.library.abstractions.contracts import RequestBase
from backends.library.abstractions.handler_base import HandlerBase
from backends.library.database.pg_orm import PG_SESSION, ChatChannelCaches, ChatUserCaches, ChatChannelUserCaches
import backends.protocols.gamespy.chat.data as data
from backends.protocols.gamespy.chat.requests import *
from frontends.gamespy.protocols.chat.aggregates.exceptions import ChatException, LoginFailedException, NickNameInUseException, NoSuchChannelException, NoSuchNickException
from frontends.gamespy.protocols.chat.contracts.results import CryptResult, GetKeyResult, ListResult, NickResult, WhoIsResult

# region General


class CdKeyHandler(HandlerBase):
    _request: CdkeyRequest

    async def _data_operate(self) -> None:
        is_valid = data.is_cdkey_valid(self._request.cdkey)
        if not is_valid:
            raise LoginFailedException("cdkey not matched")


class CryptHandler(HandlerBase):
    _request: CryptRequest

    async def _data_operate(self) -> None:
        result = PG_SESSION.query(ChatUserCaches).where(ChatUserCaches.remote_ip_address ==
                                                        self._request.client_ip, ChatUserCaches.remote_port == self._request.client_port).first()
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
        caches = PG_SESSION.query(ChatUserCaches.key_value).where(
            ChatUserCaches.nick_name == self._request.nick_name).first()

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
        chann = data.get_channel_by_name_and_ip_port(
            self._request.channel_name, self._request.client_ip, self._request.client_port)
        if chann is None:
            raise NoSuchChannelException(
                "you have to be in this channel to invite your friends")

        assert isinstance(chann.invited_nicks, list)
        chann.invited_nicks.append(self._request.nick_name)
        data.db_commit()


class ListHandler(HandlerBase):
    _request: ListRequest

    async def _request_check(self) -> None:
        if self._request.is_searching_channel:
            pass
        elif self._request.is_searching_user:
            pass
        else:
            raise ChatException("request is invalid")

    async def _data_operate(self) -> None:
        if self._request.is_searching_channel:
            # get the channel names with the substring
            self._data = data.find_channel_by_substring(self._request.filter)
            return
        if self._request.is_searching_user:
            # get the user names with the substring
            self._data = data.find_user_by_substring(self._request.filter)

    async def _result_construct(self) -> None:
        data = []
        for d in self._data:
            dd = ListResult.ListInfo(**d)
            data.append(dd)
        self._result = ListResult(user_irc_prefix="", channel_info_list=data)


class LoginPreAuthHandler(HandlerBase):
    _request: LoginPreAuthRequest

    async def _data_operate(self) -> None:
        raise NotImplementedError("should this access to PCM's api?")


class LoginHandler(HandlerBase):
    async def _data_operate(self) -> None:
        raise NotImplementedError("should this access to PCM's api?")


class NickHandler(HandlerBase):
    _request: NickRequest

    async def _data_operate(self) -> None:
        is_nick = data.is_nick_exist(self._request.nick_name)
        if is_nick:
            raise NickNameInUseException(
                old_nick=self._request.nick_name, new_nick="", message="nick name in use")
        else:
            cache = ChatUserCaches(nick_name=self._request.nick_name,
                                   server_id=self._request.server_id,
                                   update_time=datetime.now())
            data.add_nick_cache(cache)

    async def _result_construct(self) -> None:
        self._result = NickResult(nick_name=self._request.nick_name)


class QuitHandler(HandlerBase):
    _request: QuitRequest

    async def _data_operate(self) -> None:
        data.remove_user_caches_by_ip_port(
            self._request.client_ip, self._request.client_port)
        raise NotImplementedError()


class RegisterNickHandler(HandlerBase):
    async def _data_operate(self) -> None:
        raise NotImplementedError(
            "we do not know which unique nick should be updated")


class SetKeyHandler(HandlerBase):
    _request: SetKeyRequest

    async def _data_operate(self) -> None:
        user = data.get_user_cache_by_ip_port(
            self._request.client_ip, self._request.client_port)
        if user is None:
            raise NoSuchNickException(
                "The ip and port is not find in database")

        user.key_value = self._request.key_values  # type:ignore
        data.db_commit()


class UserHandler(HandlerBase):
    _request: UserRequest

    async def _data_operate(self) -> None:
        raise NotImplementedError("maybe update the user caches")


class WhoIsHandler(HandlerBase):
    _request: WhoIsRequest

    async def _data_operate(self) -> None:
        self._data: tuple = data.get_whois_result(self._request.nick_name)

    async def _result_construct(self) -> None:
        self._result = WhoIsResult(nick_name=self._data[0],
                                   user_name=self._data[1],
                                   name=self._data[2],
                                   public_ip_address=self._data[3],
                                   joined_channel_name=self._data[4])


class WhoHandler(HandlerBase):
    _request: WhoRequest

    async def _data_operate(self) -> None:
        if self._request.request_type == WhoRequestType.GET_CHANNEL_USER_INFO:
            self._get_channel_user_info()
        else:
            self._get_user_info()

    def _get_channel_user_info(self) -> None:
        pass

    def _get_user_info(self) -> None:
        pass
    
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


# region Channel

# region Message
