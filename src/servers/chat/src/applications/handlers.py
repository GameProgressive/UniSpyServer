from servers.chat.src.contracts.results import (
    ATMResult,
    NoticeResult,
    PrivateResult,
    UTMResult,
    GetCKeyResult,
    GetChannelKeyResult,
    JoinResult,
    KickResult,
    ModeResult,
    NamesResult,
    PartResult,
    SetChannelKeyResult,
    TopicResult,
    CryptResult,
    GetKeyResult,
    ListResult,
    LoginResult,
    NickResult,
    PingResult,
    UserIPResult,
    WhoIsResult,
    WhoResult,
)
from servers.chat.src.contracts.responses import (
    ATMResponse,
    NoticeResponse,
    PrivateResponse,
    UTMResponse,
    GetCKeyResponse,
    JoinResponse,
    KickResponse,
    ModeResponse,
    NamesResponse,
    PartResponse,
    SetCKeyResponse,
    SetChannelKeyResponse,
    TopicResponse,
    CdKeyResponse,
    CryptResponse,
    GetKeyResponse,
    ListResponse,
    LoginResponse,
    NickResponse,
    PingResponse,
    UserIPResponse,
    WhoIsResponse,
    WhoResponse,

)
from servers.chat.src.contracts.requests import (
    ATMRequest,
    NoticeRequest,
    PrivateRequest,
    UTMRequest,
    GetCKeyRequest,
    GetChannelKeyRequest,
    JoinRequest,
    KickRequest,
    ModeRequest,
    NamesRequest,
    PartRequest,
    SetCKeyRequest,
    SetChannelKeyRequest,
    TopicRequest,
    CdkeyRequest,
    CryptRequest,
    GetKeyRequest,
    InviteRequest,
    ListRequest,
    LoginRequest,
    NickRequest,
    PingRequest,
    QuitRequest,
    SetKeyRequest,
    UserIPRequest,
    UserRequest,
    WhoIsRequest,
    WhoRequest,
    GetUdpRelayRequest
)
from servers.chat.src.aggregates.exceptions import ChatException
from servers.chat.src.aggregates.enums import ModeRequestType, TopicRequestType
from servers.chat.src.aggregates.response_name import *
from servers.chat.src.aggregates.managers import ChannelManager
from servers.chat.src.aggregates.channel_user import ChannelUser
from servers.chat.src.aggregates.channel import Channel
from typing import Type
from library.src.abstractions.client import ClientBase
from servers.chat.src.abstractions.contract import RequestBase
from servers.chat.src.abstractions.handler import ChannelHandlerBase, CmdHandlerBase, MessageHandlerBase, PostLoginHandlerBase
# region General


class CdKeyHandler(CmdHandlerBase):
    _request: CdkeyRequest

    def __init__(self, client: ClientBase, request: CdkeyRequest):
        assert isinstance(request, CdkeyRequest)
        super().__init__(client, request)

    def _response_construct(self) -> None:
        self._response = CdKeyResponse(self._request, self._result)


class CryptHandler(CmdHandlerBase):
    _request: CryptRequest
    _result: CryptResult

    def __init__(self, client: ClientBase, request: RequestBase):
        assert isinstance(request, CryptRequest)
        super().__init__(client, request)

    def _response_construct(self) -> None:
        self._response = CryptResponse()


class GetKeyHandler(CmdHandlerBase):
    _request: GetKeyRequest
    _result: GetKeyResult

    def __init__(self, client: ClientBase, request: GetKeyRequest):
        assert isinstance(request, GetKeyRequest)
        super().__init__(client, request)

    def _response_construct(self) -> None:
        self._response = GetKeyResponse(self._request, self._result)


class GetUdpRelayHandler(CmdHandlerBase):
    _request: GetUdpRelayRequest

    def __init__(self, client: ClientBase, request: GetUdpRelayRequest):
        assert isinstance(request, GetUdpRelayRequest)
        super().__init__(client, request)


class InviteHandler(CmdHandlerBase):
    _request: InviteRequest

    def __init__(self, client: ClientBase, request: InviteRequest):
        assert isinstance(request, InviteRequest)
        super().__init__(client, request)


class ListHandler(PostLoginHandlerBase):
    _request: ListRequest
    _result: ListResult

    def __init__(self, client: ClientBase, request: ListRequest):
        assert isinstance(request, ListRequest)
        super().__init__(client, request)

    def _response_construct(self) -> None:
        self._response = ListResponse(self._result)


class LoginHandler(CmdHandlerBase):
    _request: LoginRequest
    _result: LoginResult

    def __init__(self, client: ClientBase, request: LoginRequest):
        assert isinstance(request, LoginRequest)
        super().__init__(client, request)

    def _response_construct(self) -> None:
        self._response = LoginResponse(self._result)


class NickHandler(CmdHandlerBase):
    _request: NickRequest
    _result: NickResult

    def __init__(self, client: ClientBase, request: NickRequest):
        assert isinstance(request, NickRequest)
        super().__init__(client, request)
        self._result_cls = NickResult

    def _response_construct(self) -> None:
        self._response = NickResponse(self._result)


class PingHandler(CmdHandlerBase):
    _request: PingRequest
    _result: PingResult

    def __init__(self, client: ClientBase, request: PingRequest):
        assert isinstance(request, PingRequest)
        super().__init__(client, request)

    def _response_construct(self) -> None:
        self._response = PingResponse(self._result)


class QuitHandler(CmdHandlerBase):
    _request: QuitRequest

    def __init__(self, client: ClientBase, request: QuitRequest):
        assert isinstance(request, QuitHandler)
        super().__init__(client, request)


class SetKeyHandler(PostLoginHandlerBase):
    _request: SetKeyRequest

    def __init__(self, client: ClientBase, request: SetKeyRequest):
        assert isinstance(request, SetKeyRequest)
        super().__init__(client, request)


class UserHandler(CmdHandlerBase):
    _request: UserRequest

    def __init__(self, client: ClientBase, request: UserRequest):
        assert isinstance(request, UserRequest)
        super().__init__(client, request)


class UserIPHandler(CmdHandlerBase):
    _request: UserIPRequest
    _result: UserIPResult

    def __init__(self, client: ClientBase, request: UserIPRequest):
        assert isinstance(request, UserIPRequest)
        super().__init__(client, request)

    def _feach_data(self):
        self._result = UserIPResult(
            remote_ip_address=self._client.connection.remote_ip)

    def _response_construct(self) -> None:
        self._response = UserIPResponse(self._result)


class WhoHandler(CmdHandlerBase):
    _request: WhoRequest
    _result: WhoResult

    def __init__(self, client: ClientBase, request: WhoRequest):
        assert isinstance(request, WhoRequest)
        super().__init__(client, request)

    def _response_construct(self) -> None:
        self._response = WhoResponse(self._request, self._result)


class WhoIsHandler(CmdHandlerBase):
    _request: WhoIsRequest
    _result: WhoIsResult
    _result_type: Type = WhoIsResult

    def __init__(self, client: ClientBase, request: WhoIsRequest):
        assert isinstance(request, WhoIsRequest)
        super().__init__(client, request)

    def _response_construct(self) -> None:
        self._response = WhoIsResponse(self._result)

# region channel


class GetChannelKeyHandler(ChannelHandlerBase):
    _request: GetChannelKeyRequest
    _result: GetChannelKeyResult

    def __init__(self, client: ClientBase, request: GetChannelKeyRequest):
        assert isinstance(request, GetChannelKeyRequest)
        super().__init__(client, request)

    def _publish_message(self):
        pass

    def _update_channel_cache(self):
        pass


class GetCKeyHandler(ChannelHandlerBase):
    _request: GetCKeyRequest
    _result: GetCKeyResult

    def __init__(self, client: ClientBase, request: GetCKeyRequest):
        assert isinstance(request, GetCKeyRequest)
        super().__init__(client, request)

    def _publish_message(self):
        pass

    def _update_channel_cache(self):
        pass

    def _response_construct(self):
        self._response = GetCKeyResponse(self._request, self._result)


class JoinHandler(ChannelHandlerBase):
    _request: JoinRequest
    _result: JoinResult

    def __init__(self, client: ClientBase, request: JoinRequest):
        assert isinstance(request, JoinRequest)
        super().__init__(client, request)

    def _check_user_in_remote(self):
        """
        todo maybe do not need because there are nick handler?
        """
        pass

    def _check_user_in_local(self):
        channel = ChannelManager.get_channel(
            self._request.channel_name)
        if channel is None:
            self._channel = Channel(
                self._request.channel_name, self._client, self._request.password)
            ChannelManager.add_channel(self._channel)
        else:
            self._channel = channel
            if self._client.info.nick_name in self._channel.users:
                raise ChatException("user is already in channel")

    def _request_check(self) -> None:
        # todo check if user already in local channel
        # self._check_user_in_remote()
        self._check_user_in_local()
        self._user = ChannelUser(self._client, self._channel)
        self._channel.add_bind_on_user_and_channel(self._user)

        super()._request_check()

    def _response_construct(self):
        self._response = JoinResponse(self._request, self._result)

    def _response_send(self):
        # for join request we need to send to our self
        self._channel.multicast(self._user.client, self._response, False)


class KickHandler(ChannelHandlerBase):
    _request: KickRequest
    _result: KickResult

    def __init__(self, client: ClientBase, request: KickRequest):
        assert isinstance(request, KickRequest)
        super().__init__(client, request)

    def _response_construct(self):
        self._response = KickResponse(self._request, self._result)

    def _response_send(self):
        self._channel.multicast(self._user.client, self._response, True)


class ModeHandler(ChannelHandlerBase):
    _request: ModeRequest
    _result: ModeResult

    def __init__(self, client: ClientBase, request: ModeRequest):
        assert isinstance(request, ModeRequest)
        super().__init__(client, request)

    def _response_construct(self):
        match self._request.request_type:
            case (
                ModeRequestType.GET_CHANNEL_AND_USER_MODES,
                ModeRequestType.GET_CHANNEL_MODES,
            ):
                self._response = ModeResponse(self._request, self._result)
            case ModeRequestType.SET_CHANNEL_MODES:
                pass
            case _:
                raise ChatException("Unknown mode request type")

    # def _publish_message(self):
    #     if self._request.request_type == ModeRequestType.SET_CHANNEL_MODES:
    #         super()._publish_message()

    # def _update_channel_cache(self):
    #     if self._request.request_type == ModeRequestType.SET_CHANNEL_MODES:
    #         super()._update_channel_cache()

    def _response_send(self):
        self._channel.multicast(self._user.client, self._response, True)


class NamesHandler(ChannelHandlerBase):
    _request: NamesRequest
    _result: NamesResult

    def __init__(self, client: ClientBase, request: NamesRequest):
        assert isinstance(request, NamesRequest)
        super().__init__(client, request)

    def _response_construct(self):
        self._response = NamesResponse(self._request, self._result)


class PartHandler(ChannelHandlerBase):
    _request: PartRequest
    _result: PartResult

    def __init__(self, client: ClientBase, request: PartRequest):
        assert isinstance(request, PartRequest)
        super().__init__(client, request)

    def _response_construct(self):
        self._response = PartResponse(self._request, self._result)

    def _response_send(self):
        self._channel.multicast(self._user.client, self._response, True)


class SetChannelKeyHandler(ChannelHandlerBase):
    _request: SetChannelKeyRequest
    _result: SetChannelKeyResult

    def __init__(self, client: ClientBase, request: SetChannelKeyRequest):
        assert isinstance(self._request, SetChannelKeyRequest)
        super().__init__(client, request)

    def _response_construct(self):
        self._response = SetChannelKeyResponse(self._request, self._result)

    def _response_send(self):
        self._channel.multicast(self._user.client, self._response, True)


class SetCKeyHandler(ChannelHandlerBase):
    _request: SetCKeyRequest

    def __init__(self, client: ClientBase, request: SetCKeyRequest):
        assert isinstance(request, SetCKeyRequest)
        super().__init__(client, request)

    def _response_construct(self) -> None:
        self._response = SetCKeyResponse(self._request)

    def _response_send(self):
        self._channel.multicast(self._user.client, self._response, True)


class TopicHandler(ChannelHandlerBase):
    _request: TopicRequest
    _result: TopicResult

    def __init__(self, client: ClientBase, request: TopicRequest):
        assert isinstance(request, TopicRequest)
        super().__init__(client, request)

    def _response_construct(self) -> None:
        self._response = TopicResponse(self._request, self._result)

    def _response_send(self) -> None:
        match self._request.request_type:
            case TopicRequestType.GET_CHANNEL_TOPIC:
                self._client.send(self._response)
            case TopicRequestType.SET_CHANNEL_TOPIC:
                self._channel.multicast(self._client, self._response)
# region Message


class ATMHandler(MessageHandlerBase):
    _request: ATMRequest
    _result: ATMResult

    def __init__(self, client: ClientBase, request: ATMRequest):
        assert isinstance(request, ATMRequest)
        super().__init__(client, request)

    def _response_construct(self) -> None:
        self._response = ATMResponse(self._request, self._result)


class UTMHandler(MessageHandlerBase):
    _request: UTMRequest
    _result: UTMResult

    def __init__(self, client: ClientBase, request: UTMRequest):
        assert isinstance(request, UTMRequest)
        super().__init__(client, request)

    def _response_construct(self) -> None:
        self._response = UTMResponse(self._request, self._result)


class NoticeHandler(MessageHandlerBase):
    _request: NoticeRequest
    _result: NoticeResult

    def __init__(self, client: ClientBase, request: NoticeRequest):
        assert isinstance(request, NoticeRequest)
        super().__init__(client, request)

    def _response_construct(self) -> None:
        self._response = NoticeResponse(self._request, self._result)


class PrivateHandler(MessageHandlerBase):
    _request: PrivateRequest
    _result: PrivateResult

    def __init__(self, client: ClientBase, request: PrivateRequest):
        assert isinstance(request, PrivateRequest)
        super().__init__(client, request)

    def _response_construct(self) -> None:
        self._response = PrivateResponse(self._request, self._result)
