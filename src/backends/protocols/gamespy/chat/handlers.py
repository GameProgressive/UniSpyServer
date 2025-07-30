from datetime import datetime
from typing import TYPE_CHECKING, cast
from backends.library.abstractions.contracts import RequestBase
import backends.library.abstractions.handler_base as hb


from backends.library.database.pg_orm import (
    ChatChannelCaches,
    ChatUserCaches,
    ChatChannelUserCaches,
)
from backends.protocols.gamespy.chat.helper import ChannelHelper, ChannelUserHelper
import backends.protocols.gamespy.chat.data as data
from backends.protocols.gamespy.chat.requests import (
    AtmRequest,
    CdkeyRequest,
    ChannelRequestBase,
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
from frontends.gamespy.library.exceptions.general import UniSpyException
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
    JoinResult,
    ListResult,
    NamesResult,
    NamesResultData,
    NickResult,
    PartResult,
    SetChannelKeyResult,
    TopicResult,
    WhoIsResult,
    WhoResult,
)
from sqlalchemy.orm import Session

# abstraction


class HandlerBase(hb.HandlerBase):
    _user: ChatUserCaches | None
    _session: Session

    def __init__(self, request: RequestBase) -> None:
        super().__init__(request)
        self._user = None

    def _request_check(self) -> None:
        self._get_user()
        self._check_user()

    def _get_user(self):
        self._user = data.get_user_cache_by_ip_port(
            self._request.client_ip, self._request.client_port, self._session
        )
        if self._user is None:
            self._user = ChatUserCaches(
                server_id=self._request.server_id,
                remote_ip_address=self._request.client_ip,
                remote_port=self._request.client_port,
                update_time=datetime.now(),
                nick_name=f"{self._request.client_ip}:{self._request.client_port}",
            )
            self._session.add(self._user)
        else:
            self._user.update_time = datetime.now()  # type: ignore

    def _check_user(self):
        if self._user is None:
            raise NoSuchNickException(
                f"Can not find user with ip address: {self._request.client_ip}:{self._request.client_port}"
            )


class ChannelHandlerBase(HandlerBase):
    _request: ChannelRequestBase
    _channel: ChatChannelCaches | None
    _channel_user: ChatChannelUserCaches | None
    _is_broadcast: bool

    def __init__(self, request: RequestBase) -> None:
        super().__init__(request)
        self._channel = None
        self._channel_user = None
        self._is_broadcast = False

    def _request_check(self) -> None:
        super()._request_check()

        self._get_channel()
        self._check_channel()

        self._get_channel_user()
        self._check_channel_user()

    def _get_channel(self):
        self._channel = data.get_channel_by_name(
            self._request.channel_name, self._session
        )

    def _get_channel_user(self):
        self._channel_user = data.get_channel_user_cache_by_name_and_ip_port(
            self._request.channel_name,
            self._request.client_ip,
            self._request.client_port,
            self._session,
        )

    def _check_channel(self):
        if self._channel is None:
            raise NoSuchChannelException(
                f"Can not find channel with name: {self._request.channel_name}"
            )
        self._channel.update_time = datetime.now()  # type: ignore

    def _check_channel_user(self):
        if self._channel_user is None:
            raise NoSuchNickException(
                f"Can not find channel user with channel name: {self._request.channel_name}, ip address: {self._request.client_ip}:{self._request.client_port}"
            )
        self._channel_user.update_time = datetime.now()  # type: ignore

    def _boradcast(self) -> None:
        # todo boradcast message here
        raise NotImplementedError()

    def handle(self) -> None:
        super().handle()
        if self._is_broadcast:
            self._boradcast()


class MessageHandlerBase(ChannelHandlerBase):
    pass


# region General


class CdKeyHandler(HandlerBase):
    _request: CdkeyRequest

    def _data_operate(self) -> None:
        is_valid = data.is_cdkey_valid(self._request.cdkey, self._session)
        if not is_valid:
            raise LoginFailedException("cdkey not matched")


class CryptHandler(HandlerBase):
    _request: CryptRequest

    def _data_operate(self) -> None:
        assert self._user is not None
        self._user.game_name = self._request.gamename  # type: ignore
        self._secret_key = data.get_secret_key_by_game_name(
            self._request.gamename, self._session
        )
        if self._secret_key is None:
            raise UniSpyException("game secret key not found in database.")

    def _result_construct(self) -> None:
        assert isinstance(self._secret_key, str)
        self._result = CryptResult(secret_key=self._secret_key)


class GetKeyHandler(HandlerBase):
    _request: GetKeyRequest

    def _data_operate(self) -> None:
        caches = data.get_user_cache_by_nick_name(
            self._request.nick_name, self._session
        )

        if caches is None:
            raise NoSuchNickException("nick not found")
        if TYPE_CHECKING:
            kv = cast(dict, caches.key_value)
            self._values = cast(list, kv.keys())

    def _result_construct(self) -> None:
        self._result = GetKeyResult(
            nick_name=self._request.nick_name, values=self._values
        )


class GetUdpRelayHandler(HandlerBase):
    _request: GetUdpRelayRequest

    def __init__(self, request: RequestBase) -> None:
        super().__init__(request)
        raise NotImplementedError()


class InviteHandler(HandlerBase):
    _request: InviteRequest

    def _data_operate(self) -> None:
        chann = data.get_channel_by_name_and_ip_port(
            self._request.channel_name,
            self._request.client_ip,
            self._request.client_port,
            self._session,
        )
        if chann is None:
            raise NoSuchChannelException(
                "you have to be in this channel to invite your friends"
            )

        assert isinstance(chann.invited_nicks, list)
        chann.invited_nicks.append(self._request.nick_name)
        data.db_commit(self._session)


class ListHandler(HandlerBase):
    _request: ListRequest

    def _request_check(self) -> None:
        if self._request.is_searching_channel:
            pass
        elif self._request.is_searching_user:
            pass
        else:
            raise ChatException("request is invalid")

    def _data_operate(self) -> None:
        if self._request.is_searching_channel:
            # get the channel names with the substring
            self._data = data.find_channel_by_substring(
                self._request.filter, self._session
            )
            return
        if self._request.is_searching_user:
            # get the user names with the substring
            self._data = data.find_user_by_substring(
                self._request.filter, self._session
            )

    def _result_construct(self) -> None:
        data = []
        for d in self._data:
            dd = ListResult.ListInfo(**d)
            data.append(dd)
        self._result = ListResult(user_irc_prefix="", channel_info_list=data)


class LoginPreAuthHandler(HandlerBase):
    _request: LoginPreAuthRequest

    def _data_operate(self) -> None:
        raise NotImplementedError("should this access to PCM's api?")


class LoginHandler(HandlerBase):
    def _data_operate(self) -> None:
        raise NotImplementedError("should this access to PCM's api?")


class NickHandler(HandlerBase):
    _request: NickRequest

    def _data_operate(self) -> None:
        # cache = data.get_user_cache_by_nick_name("172.19.0.5:52986")
        self._user.nick_name = self._request.nick_name  # type: ignore
        self._session.commit()
        cache = None
        # some game do not use CRYPT
        # todo check game with no encryption send nick or user request first
        if cache is None:
            # assign nick_name to current user
            self._user.nick_name = self._request.nick_name  # type: ignore
        else:
            assert isinstance(cache.update_time, datetime)
            if (datetime.now() - cache.update_time).seconds > 120:
                # old profile delete it
                self._session.delete(cache)
                # data.remove_user_cache(cache)
                self._user.nick_name = self._request.nick_name  # type: ignore
                return

            if (
                cache.remote_ip_address != self._request.client_ip
                and cache.remote_port != self._request.client_port
            ):  # type: ignore
                raise NickNameInUseException(
                    old_nick=self._request.nick_name,
                    new_nick="",
                    message="nick name in use",
                )
            else:
                # update user cache
                self._user.nick_name = self._request.nick_name  # type: ignore
        self._session.commit()

    def _result_construct(self) -> None:
        self._result = NickResult(nick_name=self._request.nick_name)


class QuitHandler(HandlerBase):
    _request: QuitRequest

    def _data_operate(self) -> None:
        data.remove_user_caches_by_ip_port(
            self._request.client_ip, self._request.client_port, self._session
        )
        raise NotImplementedError()


class RegisterNickHandler(HandlerBase):
    def _data_operate(self) -> None:
        raise NotImplementedError("we do not know which unique nick should be updated")


class SetKeyHandler(HandlerBase):
    _request: SetKeyRequest

    def _data_operate(self) -> None:
        user = data.get_user_cache_by_ip_port(
            self._request.client_ip, self._request.client_port, self._session
        )
        if user is None:
            raise NoSuchNickException("The ip and port is not find in database")

        user.key_value = self._request.key_values  # type:ignore
        super()._data_operate()


class UserHandler(HandlerBase):
    _request: UserRequest

    def _data_operate(self) -> None:
        self._user.user_name = self._request.user_name  # type: ignore
        self._session.commit()


class WhoHandler(HandlerBase):
    _request: WhoRequest

    def _data_operate(self) -> None:
        if self._request.request_type == WhoRequestType.GET_CHANNEL_USER_INFO:
            self._get_channel_user_info()
        else:
            self._get_user_info()

    def _get_channel_user_info(self) -> None:
        self._data = data.get_channel_user_caches(
            self._request.channel_name, self._session
        )

    def _get_user_info(self) -> None:
        self._data = data.get_channel_user_cache_by_ip(
            self._request.client_ip, self._request.client_port, self._session
        )

    def _result_construct(self) -> None:
        infos = []
        for d in self._data:
            info = WhoResult.WhoInfo(**d)
            infos.append(info)
        self._result = WhoResult(infos=infos)


class WhoIsHandler(HandlerBase):
    _request: WhoIsRequest

    def _data_operate(self) -> None:
        self._data: tuple = data.get_whois_result(
            self._request.nick_name, self._session
        )

    def _result_construct(self) -> None:
        self._result = WhoIsResult(
            nick_name=self._data[0],
            user_name=self._data[1],
            name=self._data[2],
            public_ip_address=self._data[3],
            joined_channel_name=self._data[4],
        )


# region Channel


class JoinHandler(ChannelHandlerBase):
    _request: JoinRequest

    def _request_check(self) -> None:
        self._get_user()
        self._check_user()

        self._get_channel()

    def _data_operate(self) -> None:
        assert self._user is not None

        if self._channel is None:
            self._channel = ChannelHelper.create(
                server_id=self._request.server_id,
                channel_name=self._request.channel_name,
                password=self._request.password,
                game_name=self._user.game_name,  # type: ignore
                room_name="",
                topic="",
                key_values={},
                update_time=datetime.now(),
                creator=self._user.nick_name,  # type: ignore
                group_id=0,
                max_num_user=100,
                session=self._session,
            )
        ChannelHelper.join(self._channel, self._user, self._session)
        self._channel_users_info = ChannelHelper.get_channel_all_nicks(
            self._channel, self._session
        )

    def _result_construct(self) -> None:
        assert self._user is not None
        assert isinstance(self._user.user_name, str)
        assert isinstance(self._user.nick_name, str)
        assert self._channel is not None
        assert isinstance(self._channel.modes, list)
        
        self._result = JoinResult(
            joiner_user_name=self._user.user_name,
            joiner_nick_name=self._user.nick_name,
        )


class GetChannelKeyHandler(ChannelHandlerBase):
    def _get_key_values(self):
        assert isinstance(self._channel, ChatChannelCaches)
        self._key_values = self._channel.key_values

    def _request_check(self) -> None:
        super()._request_check()
        self._get_key_values()


class GetCKeyHandler(ChannelHandlerBase):
    _request: GetCKeyRequest

    def _data_operate(self) -> None:
        match self._request.request_type:
            case GetKeyRequestType.GET_CHANNEL_ALL_USER_KEY_VALUE:
                self.get_channel_all_user_key_value()
            case GetKeyRequestType.GET_CHANNEL_SPECIFIC_USER_KEY_VALUE:
                self.get_channel_specific_user_key_value()

    def get_channel_all_user_key_value(self):
        self._data = data.get_channel_user_caches_by_name(
            self._request.channel_name, self._session
        )

    def get_channel_specific_user_key_value(self):
        d = data.get_channel_user_cache_by_name(
            self._request.channel_name, self._request.nick_name, self._session
        )
        if d is not None:
            self._data = [d]

    def _result_construct(self) -> None:
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


class KickHandler(ChannelHandlerBase):
    _kickee: ChatChannelUserCaches | None
    _request: KickRequest

    def __init__(self, request: RequestBase) -> None:
        super().__init__(request)
        self._kickee = None

    def _request_check(self) -> None:
        super()._request_check()
        assert isinstance(self._channel, ChatChannelCaches)
        assert isinstance(self._channel.channel_name, str)
        self._kickee = data.get_channel_user_cache_by_name(
            self._channel.channel_name, self._request.kickee_nick_name, self._session
        )
        if self._kickee is None:
            raise BadChannelKeyException(
                f"kickee is not a user of channel:{self._channel.channel_name}"
            )

    def _data_operate(self) -> None:
        assert self._channel
        assert self._channel_user
        assert self._kickee
        ChannelHelper.kick(self._channel, self._channel_user, self._kickee)


class ModeHandler(ChannelHandlerBase):
    _request: ModeRequest

    def _data_operate(self) -> None:
        assert self._channel
        assert self._channel_user
        ChannelHelper.change_modes(
            self._channel, self._channel_user, self._request, self._session
        )

    def _result_construct(self) -> None:
        raise NotImplementedError()
        return super()._result_construct()


class NamesHandler(ChannelHandlerBase):
    _request: NamesRequest

    def _request_check(self) -> None:
        self._get_user()
        self._check_user()

        self._get_channel()
        self._check_channel()

    def _data_operate(self) -> None:
        assert self._channel
        self._channel_users_info = ChannelHelper.get_channel_all_nicks(
            self._channel, self._session
        )

    def _result_construct(self) -> None:
        assert self._user
        assert isinstance(self._user.nick_name, str)
        nicks = []
        for nick in self._channel_users_info:
            nick.channel_name # yield nick
            data = NamesResultData(**nick.__dict__)
            nicks.append(data)

        self._result = NamesResult(
            channel_nicks=nicks,
            channel_name=self._request.channel_name,
            requester_nick_name=self._user.nick_name,
        )


class PartHandler(ChannelHandlerBase):
    _request: PartRequest

    def _data_operate(self) -> None:
        assert self._channel
        assert self._channel_user
        ChannelHelper.quit(self._channel, self._channel_user)

    def _result_construct(self) -> None:
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

    def _request_check(self) -> None:
        super()._request_check()
        assert self._channel_user
        assert isinstance(self._channel_user.is_channel_operator, bool)
        if self._channel_user.is_channel_operator:
            self._channel.key_values = self._request.key_values  # type:ignore
        data.db_commit(self._session)

    def _result_construct(self) -> None:
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

    def _data_operate(self) -> None:
        self._channel_user.key_values = self._request.key_values  # type:ignore
        data.db_commit(self._session)
        self._is_broadcast = True

    def _result_construct(self) -> None:
        # todo think how to broadcast message
        raise NotImplementedError()


class TopicHandler(ChannelHandlerBase):
    _request: TopicRequest

    def _data_operate(self) -> None:
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

    def _result_construct(self) -> None:
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
