from datetime import datetime
from typing import TYPE_CHECKING, cast
from backends.library.abstractions.contracts import RequestBase
from backends.library.abstractions.handler_base import HandlerBase
from backends.library.database.pg_orm import (
    PG_SESSION,
    ChatChannelCaches,
    ChatUserCaches,
    ChatChannelUserCaches,
)
from backends.protocols.gamespy.chat.abstractions import (
    ChannelHandlerBase,
    MessageHandlerBase,
)
from backends.protocols.gamespy.chat.helper import ChannelHelper, ChannelUserHelper
import backends.protocols.gamespy.chat.data as data
from backends.protocols.gamespy.chat.requests import (
    AtmRequest,
    CdkeyRequest,
    CryptRequest,
    GetCKeyRequest,
    GetKeyRequest,
    GetUdpRelayRequest,
    InviteRequest,
    JoinRequest,
    KickRequest,
    ListRequest,
    LoginPreAuthRequest,
    ModeRequest,
    NamesRequest,
    NickRequest,
    NoticeRequest,
    PartRequest,
    PrivateRequest,
    QuitRequest,
    SetCKeyRequest,
    SetChannelKeyRequest,
    SetKeyRequest,
    TopicRequest,
    UserRequest,
    UtmRequest,
    WhoIsRequest,
    WhoRequest,
)
from frontends.gamespy.protocols.chat.aggregates.enums import (
    GetKeyRequestType,
    TopicRequestType,
    WhoRequestType,
)
from frontends.gamespy.protocols.chat.aggregates.exceptions import (
    BadChannelKeyException,
    ChatException,
    LoginFailedException,
    NickNameInUseException,
    NoSuchChannelException,
    NoSuchNickException,
)
from frontends.gamespy.protocols.chat.contracts.results import (
    CryptResult,
    GetCKeyResult,
    GetKeyResult,
    ListResult,
    NamesResult,
    NickResult,
    PartResult,
    SetChannelKeyResult,
    TopicResult,
    WhoIsResult,
    WhoResult,
)

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
        result = data.get_user_cache_by_ip_port(
            self._request.client_ip, self._request.client_port
        )
        if result is None:
            raise NoSuchNickException(f"No nick found for {self._request.client_ip}")
        result.game_name = self._request.gamename  # type: ignore
        PG_SESSION.commit()

    async def _result_construct(self) -> None:
        self._result = CryptResult()


class GetKeyHandler(HandlerBase):
    _request: GetKeyRequest

    async def _data_operate(self) -> None:
        caches = data.get_user_cache_by_nick_name(self._request.nick_name)

        if caches is None:
            raise NoSuchNickException("nick not found")
        if TYPE_CHECKING:
            self.caches = cast(list, caches)

    async def _result_construct(self) -> None:
        self._result = GetKeyResult(
            nick_name=self._request.nick_name, values=self.caches
        )


class GetUdpRelayHandler(HandlerBase):
    _request: GetUdpRelayRequest

    def __init__(self, request: RequestBase) -> None:
        super().__init__(request)
        raise NotImplementedError()


class InviteHandler(HandlerBase):
    _request: InviteRequest

    async def _data_operate(self) -> None:
        chann = data.get_channel_by_name_and_ip_port(
            self._request.channel_name,
            self._request.client_ip,
            self._request.client_port,
        )
        if chann is None:
            raise NoSuchChannelException(
                "you have to be in this channel to invite your friends"
            )

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
                old_nick=self._request.nick_name,
                new_nick="",
                message="nick name in use",
            )
        else:
            cache = ChatUserCaches(
                nick_name=self._request.nick_name,
                server_id=self._request.server_id,
                update_time=datetime.now(),
            )
            ChatUserCaches()
            data.add_nick_cache(cache)

    async def _result_construct(self) -> None:
        self._result = NickResult(nick_name=self._request.nick_name)


class QuitHandler(HandlerBase):
    _request: QuitRequest

    async def _data_operate(self) -> None:
        data.remove_user_caches_by_ip_port(
            self._request.client_ip, self._request.client_port
        )
        raise NotImplementedError()


class RegisterNickHandler(HandlerBase):
    async def _data_operate(self) -> None:
        raise NotImplementedError("we do not know which unique nick should be updated")


class SetKeyHandler(HandlerBase):
    _request: SetKeyRequest

    async def _data_operate(self) -> None:
        user = data.get_user_cache_by_ip_port(
            self._request.client_ip, self._request.client_port
        )
        if user is None:
            raise NoSuchNickException("The ip and port is not find in database")

        user.key_value = self._request.key_values  # type:ignore
        data.db_commit()


class UserHandler(HandlerBase):
    _request: UserRequest

    async def _data_operate(self) -> None:
        raise NotImplementedError("maybe update the user caches")


class WhoHandler(HandlerBase):
    _request: WhoRequest

    async def _data_operate(self) -> None:
        if self._request.request_type == WhoRequestType.GET_CHANNEL_USER_INFO:
            self._get_channel_user_info()
        else:
            self._get_user_info()

    def _get_channel_user_info(self) -> None:
        self._data = data.get_channel_user_caches(self._request.channel_name)

    def _get_user_info(self) -> None:
        self._data = data.get_channel_user_cache_by_ip(
            self._request.client_ip, self._request.client_port
        )

    async def _result_construct(self) -> None:
        infos = []
        for d in self._data:
            info = WhoResult.WhoInfo(**d)
            infos.append(info)
        self._result = WhoResult(infos=infos)


class WhoIsHandler(HandlerBase):
    _request: WhoIsRequest

    async def _data_operate(self) -> None:
        self._data: tuple = data.get_whois_result(self._request.nick_name)

    async def _result_construct(self) -> None:
        self._result = WhoIsResult(
            nick_name=self._data[0],
            user_name=self._data[1],
            name=self._data[2],
            public_ip_address=self._data[3],
            joined_channel_name=self._data[4],
        )


# region Channel
class GetChannelKeyHandler(ChannelHandlerBase):
    def _get_key_values(self):
        assert isinstance(self._channel, ChatChannelCaches)
        self._key_values = self._channel.key_values

    async def _request_check(self) -> None:
        await super()._request_check()
        self._get_key_values()


class GetCKeyHandler(ChannelHandlerBase):
    _request: GetCKeyRequest

    async def _data_operate(self) -> None:
        match self._request.request_type:
            case GetKeyRequestType.GET_CHANNEL_ALL_USER_KEY_VALUE:
                self.get_channel_all_user_key_value()
            case GetKeyRequestType.GET_CHANNEL_SPECIFIC_USER_KEY_VALUE:
                self.get_channel_specific_user_key_value()

    def get_channel_all_user_key_value(self):
        self._data = data.get_channel_user_caches_by_name(self._request.channel_name)

    def get_channel_specific_user_key_value(self):
        d = data.get_channel_user_cache_by_name(
            self._request.channel_name, self._request.nick_name
        )
        if d is not None:
            self._data = [d]

    async def _result_construct(self) -> None:
        if self._data is None:
            return
        infos = []
        for d in self._data:
            assert isinstance(d, ChatChannelUserCaches)
            assert isinstance(d.nick_name, str)
            assert isinstance(d.key_values, dict)
            info = GetCKeyResult.GetCKeyInfos(
                nick_name=d.nick_name, user_values=list(d.key_values.values())
            )
            infos.append(info)

        self._result = GetCKeyResult(
            infos=infos, channel_name=self._request.channel_name
        )


class JoinHandler(ChannelHandlerBase):
    _request: JoinRequest

    async def _request_check(self) -> None:
        self._get_user()
        self._check_user()

        self._get_channel()

    async def _data_operate(self) -> None:
        assert self._user is not None
        assert isinstance(self._user.nick_name, str)
        assert self._channel is not None
        assert isinstance(self._channel.channel_name, str)
        if self._channel is None:
            self._channel = ChannelHelper.create(
                server_id=self._request.server_id,
                channel_name=self._request.channel_name,
                password=self._request.password,
                game_name=self._request.game_name,
                room_name="",
                topic="",
                key_values={},
                update_time=datetime.now(),
                modes=[],
                creator=self._user.nick_name,
                group_id=0,
                max_num_user=100,
            )
        ChannelHelper.join(self._channel, self._user)


class KickHandler(ChannelHandlerBase):
    _kickee: ChatChannelUserCaches | None
    _request: KickRequest

    def __init__(self, request: RequestBase) -> None:
        super().__init__(request)
        self._kickee = None

    async def _request_check(self) -> None:
        await super()._request_check()
        assert isinstance(self._channel, ChatChannelCaches)
        assert isinstance(self._channel.channel_name, str)
        self._kickee = data.get_channel_user_cache_by_name(
            self._channel.channel_name, self._request.kickee_nick_name
        )
        if self._kickee is None:
            raise BadChannelKeyException(
                f"kickee is not a user of channel:{self._channel.channel_name}"
            )

    async def _data_operate(self) -> None:
        assert self._channel
        assert self._channel_user
        assert self._kickee
        ChannelHelper.kick(self._channel, self._channel_user, self._kickee)


class ModeHandler(ChannelHandlerBase):
    _request: ModeRequest

    async def _data_operate(self) -> None:
        assert self._channel
        assert self._channel_user
        ChannelHelper.change_modes(self._channel, self._channel_user, self._request)

    async def _result_construct(self) -> None:
        raise NotImplementedError()
        return await super()._result_construct()


class NamesHandler(ChannelHandlerBase):
    _request: NamesRequest

    async def _request_check(self) -> None:
        self._get_user()
        self._check_user()

        self._get_channel()
        self._check_channel()

    async def _data_operate(self) -> None:
        assert self._channel
        self._data = ChannelHelper.get_all_user_nick_string(self._channel)

    async def _result_construct(self) -> None:
        assert self._user
        assert isinstance(self._user.nick_name, str)
        self._result = NamesResult(
            all_channel_user_nicks=self._data,
            channel_name=self._request.channel_name,
            requester_nick_name=self._user.nick_name,
        )


class PartHandler(ChannelHandlerBase):
    _request: PartRequest

    async def _data_operate(self) -> None:
        assert self._channel
        assert self._channel_user
        ChannelHelper.quit(self._channel, self._channel_user)

    async def _result_construct(self) -> None:
        assert self._channel_user
        assert self._channel
        assert isinstance(self._channel_user.is_channel_creator, bool)
        assert isinstance(self._channel_user.is_channel_operator, bool)
        assert isinstance(self._channel.channel_name, str)

        irc = ChannelUserHelper.get_user_irc_prefix(self._channel_user)
        self._result = PartResult(
            leaver_irc_prefix=irc,
            is_channel_creator=self._channel_user.is_channel_creator,
            channel_name=self._channel.channel_name,
        )


class SetChannelKeyHandler(ChannelHandlerBase):
    _request: SetChannelKeyRequest

    async def _request_check(self) -> None:
        await super()._request_check()
        assert self._channel_user
        assert isinstance(self._channel_user.is_channel_operator, bool)
        if self._channel_user.is_channel_operator:
            self._channel.key_values = self._request.key_values  # type:ignore
        data.db_commit()

    async def _result_construct(self) -> None:
        assert self._channel_user
        irc = ChannelUserHelper.get_user_irc_prefix(self._channel_user)
        self._result = SetChannelKeyResult(
            channel_user_irc_prefix=irc, channel_name=self._request.channel_name
        )


class SetCkeyHandler(ChannelHandlerBase):
    """
    todo check if set channel_user or user keyvalue or set for other channeluser keyvalue
    """

    _request: SetCKeyRequest

    async def _data_operate(self) -> None:
        self._channel_user.key_values = self._request.key_values  # type:ignore
        data.db_commit()
        self._is_broadcast = True

    async def _result_construct(self) -> None:
        # todo think how to broadcast message
        raise NotImplementedError()


class TopicHandler(ChannelHandlerBase):
    _request: TopicRequest

    async def _data_operate(self) -> None:
        assert self._channel_user
        assert isinstance(self._channel_user.is_channel_operator, bool)
        if self._request.request_type is TopicRequestType.GET_CHANNEL_TOPIC:
            self._data: str = self._channel.topic  # type:ignore
        else:
            if not self._channel_user.is_channel_operator:
                raise NoSuchChannelException(
                    "inorder to set channel topic, you have to be channel operator"
                )
            self._channel.topic = self._request.channel_topic  # type:ignore
            self._data: str = self._request.channel_topic

    async def _result_construct(self) -> None:
        self._result = TopicResult(
            channel_name=self._request.channel_name, channel_topic=self._data
        )


# region Message


class AtmHandler(MessageHandlerBase):
    _request: AtmRequest


class UtmHandler(MessageHandlerBase):
    _request: UtmRequest


class NoticeHandler(MessageHandlerBase):
    _request: NoticeRequest


class PrivateHandler(MessageHandlerBase):
    _request: PrivateRequest
