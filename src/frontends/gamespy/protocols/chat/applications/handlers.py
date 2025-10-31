from frontends.gamespy.library.encryption.gs_encryption import ChatCrypt
from frontends.gamespy.protocols.chat.applications.client import Client
from frontends.gamespy.protocols.chat.contracts.results import (
    AtmResult,
    NoticeResult,
    PrivateResult,
    SetCKeyResult,
    UtmResult,
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
from frontends.gamespy.protocols.chat.contracts.responses import (
    AtmResponse,
    GetCKeyResponse,
    GetChannelKeyResponse,
    JoinResponse,
    KickResponse,
    ModeResponse,
    NamesResponse,
    NoticeResponse,
    PartResponse,
    PrivateResponse,
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
    UtmResponse,
    UserIPResponse,
    WhoIsResponse,
    WhoResponse,
)
from frontends.gamespy.protocols.chat.contracts.requests import (
    AtmRequest,
    NoticeRequest,
    PrivateRequest,
    PublishMessageRequest,
    UtmRequest,
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
    GetUdpRelayRequest,
)
from frontends.gamespy.protocols.chat.aggregates.enums import ModeRequestType
from frontends.gamespy.protocols.chat.abstractions.contract import RequestBase
from frontends.gamespy.protocols.chat.abstractions.handler import (
    ChannelHandlerBase,
    CmdHandlerBase,
    MessageHandlerBase,
    PostLoginHandlerBase,
)
# region General


class CdKeyHandler(CmdHandlerBase):
    _request: CdkeyRequest

    def __init__(self, client: Client, request: CdkeyRequest):
        assert isinstance(request, CdkeyRequest)
        super().__init__(client, request)

    def _response_construct(self) -> None:
        self._response = CdKeyResponse()


class CryptHandler(CmdHandlerBase):
    _request: CryptRequest
    _result: CryptResult

    def __init__(self, client: Client, request: RequestBase):
        assert isinstance(request, CryptRequest)
        super().__init__(client, request)
        self._result_cls = CryptResult

    def _data_operate(self) -> None:
        super()._data_operate()
        self._client.info.gamename = self._request.gamename

    def _response_construct(self) -> None:
        self._response = CryptResponse()

    def _response_send(self) -> None:
        super()._response_send()
        self._client.crypto = ChatCrypt(self._result.secret_key)


class GetKeyHandler(CmdHandlerBase):
    _request: GetKeyRequest
    _result: GetKeyResult
    _response: GetKeyResponse

    def __init__(self, client: Client, request: GetKeyRequest):
        assert isinstance(request, GetKeyRequest)
        super().__init__(client, request)


class GetUdpRelayHandler(CmdHandlerBase):
    _request: GetUdpRelayRequest

    def __init__(self, client: Client, request: GetUdpRelayRequest):
        assert isinstance(request, GetUdpRelayRequest)
        super().__init__(client, request)


class InviteHandler(CmdHandlerBase):
    _request: InviteRequest

    def __init__(self, client: Client, request: InviteRequest):
        assert isinstance(request, InviteRequest)
        super().__init__(client, request)


class ListHandler(PostLoginHandlerBase):
    _request: ListRequest
    _result: ListResult
    _response: ListResponse

    def __init__(self, client: Client, request: ListRequest):
        assert isinstance(request, ListRequest)
        super().__init__(client, request)


class LoginHandler(CmdHandlerBase):
    _request: LoginRequest
    _result: LoginResult
    _response: LoginResponse

    def __init__(self, client: Client, request: LoginRequest):
        assert isinstance(request, LoginRequest)
        super().__init__(client, request)


class NickHandler(CmdHandlerBase):
    _request: NickRequest
    _result: NickResult
    _response: NickResponse

    def __init__(self, client: Client, request: NickRequest):
        assert isinstance(request, NickRequest)
        super().__init__(client, request)

    def _data_operate(self) -> None:
        super()._data_operate()
        self._client.info.nick_name = self._request.nick_name


class PingHandler(CmdHandlerBase):
    _request: PingRequest
    _result: PingResult
    _response: PingResponse

    def __init__(self, client: Client, request: PingRequest):
        assert isinstance(request, PingRequest)
        super().__init__(client, request)


class QuitHandler(CmdHandlerBase):
    _request: QuitRequest

    def __init__(self, client: Client, request: QuitRequest):
        assert isinstance(request, QuitRequest)
        super().__init__(client, request)


class SetKeyHandler(PostLoginHandlerBase):
    _request: SetKeyRequest

    def __init__(self, client: Client, request: SetKeyRequest):
        assert isinstance(request, SetKeyRequest)
        super().__init__(client, request)


class UserHandler(CmdHandlerBase):
    _request: UserRequest

    def __init__(self, client: Client, request: UserRequest):
        assert isinstance(request, UserRequest)
        super().__init__(client, request)

    def _data_operate(self) -> None:
        super()._data_operate()
        self._client.info.user_name = self._request.user_name


class UserIPHandler(CmdHandlerBase):
    _request: UserIPRequest
    _response: UserIPResponse

    def __init__(self, client: Client, request: UserIPRequest):
        assert isinstance(request, UserIPRequest)
        super().__init__(client, request)

    def _request_check(self) -> None:
        super()._request_check()
        self._request.remote_ip = self._client.connection.remote_ip

    def _data_operate(self) -> None:
        super()._data_operate()
        self._result = UserIPResult(
            remote_ip=self._client.connection.remote_ip)


class WhoHandler(CmdHandlerBase):
    _request: WhoRequest
    _result: WhoResult
    _response: WhoResponse

    def __init__(self, client: Client, request: WhoRequest):
        assert isinstance(request, WhoRequest)
        super().__init__(client, request)


class WhoIsHandler(CmdHandlerBase):
    _request: WhoIsRequest
    _result: WhoIsResult
    _response: WhoIsResponse

    def __init__(self, client: Client, request: WhoIsRequest):
        assert isinstance(request, WhoIsRequest)
        super().__init__(client, request)


# region channel


class GetChannelKeyHandler(ChannelHandlerBase):
    _request: GetChannelKeyRequest
    _result: GetChannelKeyResult
    _response: GetChannelKeyResponse

    def __init__(self, client: Client, request: GetChannelKeyRequest):
        assert isinstance(request, GetChannelKeyRequest)
        super().__init__(client, request)

    def _publish_message(self):
        pass

    def _update_channel_cache(self):
        pass


class GetCKeyHandler(ChannelHandlerBase):
    _request: GetCKeyRequest
    _result: GetCKeyResult
    _response: GetCKeyResponse

    def __init__(self, client: Client, request: GetCKeyRequest):
        assert isinstance(request, GetCKeyRequest)
        super().__init__(client, request)


class JoinHandler(ChannelHandlerBase):
    _request: JoinRequest
    _result: JoinResult
    _response: JoinResponse

    def __init__(self, client: Client, request: JoinRequest):
        assert isinstance(request, JoinRequest)
        super().__init__(client, request)
        self._is_broadcast = True

    def _response_send(self) -> None:
        super()._response_send()
        # gamespy sdk join require ciNameReplyHandler
        names_raw = NamesRequest.build(self._request.channel_name)
        names_req = NamesRequest(names_raw)
        names_handler = NamesHandler(self._client, names_req)
        names_handler.handle()

        mode_raw = ModeRequest.build(self._request.channel_name)
        mode_req = ModeRequest(mode_raw)
        mode_handler = ModeHandler(self._client, mode_req)
        mode_handler.handle()


class KickHandler(ChannelHandlerBase):
    _request: KickRequest
    _result: KickResult
    _response: KickResponse

    def __init__(self, client: Client, request: KickRequest):
        assert isinstance(request, KickRequest)
        super().__init__(client, request)


class ModeHandler(ChannelHandlerBase):
    _request: ModeRequest
    _result: ModeResult
    _response: ModeResponse

    def __init__(self, client: Client, request: ModeRequest):
        assert isinstance(request, ModeRequest)
        super().__init__(client, request)

    def _request_check(self) -> None:
        super()._request_check()
        # if self._request.request_type == ModeRequestType.SET_CHANNEL_MODES:
        #     self._is_fetching = False
        if self._request.request_type == [
            ModeRequestType.SET_CHANNEL_MODES,
            ModeRequestType.SET_CHANNEL_USER_MODES,
        ]:
            self._is_broadcast = True

    def _response_construct(self):
        if self._request.request_type == ModeRequestType.GET_CHANNEL_MODES:
            super()._response_construct()


class NamesHandler(ChannelHandlerBase):
    _request: NamesRequest
    _result: NamesResult
    _response: NamesResponse

    def __init__(self, client: Client, request: NamesRequest):
        assert isinstance(request, NamesRequest)
        super().__init__(client, request)


class PartHandler(ChannelHandlerBase):
    _request: PartRequest
    _result: PartResult
    _response: PartResponse

    def __init__(self, client: Client, request: PartRequest):
        assert isinstance(request, PartRequest)
        super().__init__(client, request)


class SetChannelKeyHandler(ChannelHandlerBase):
    _request: SetChannelKeyRequest
    _result: SetChannelKeyResult
    _response: SetChannelKeyResponse

    def __init__(self, client: Client, request: SetChannelKeyRequest):
        assert isinstance(request, SetChannelKeyRequest)
        super().__init__(client, request)
        self._is_broadcast = True


class SetCKeyHandler(ChannelHandlerBase):
    _request: SetCKeyRequest
    _result: SetCKeyResult
    _response: SetCKeyResponse

    def __init__(self, client: Client, request: SetCKeyRequest):
        assert isinstance(request, SetCKeyRequest)
        super().__init__(client, request)


class TopicHandler(ChannelHandlerBase):
    _request: TopicRequest
    _result: TopicResult
    _response: TopicResponse

    def __init__(self, client: Client, request: TopicRequest):
        assert isinstance(request, TopicRequest)
        super().__init__(client, request)
        self._is_broadcast = True


# region Message


class ATMHandler(MessageHandlerBase):
    _request: AtmRequest
    _result: AtmResult
    _response: AtmResponse

    def __init__(self, client: Client, request: AtmRequest):
        assert isinstance(request, AtmRequest)
        super().__init__(client, request)


class UTMHandler(MessageHandlerBase):
    _request: UtmRequest
    _result: UtmResult
    _response: UtmResponse

    def __init__(self, client: Client, request: UtmRequest):
        assert isinstance(request, UtmRequest)
        super().__init__(client, request)


class NoticeHandler(MessageHandlerBase):
    _request: NoticeRequest
    _result: NoticeResult
    _response: NoticeResponse

    def __init__(self, client: Client, request: NoticeRequest):
        assert isinstance(request, NoticeRequest)
        super().__init__(client, request)


class PrivateHandler(MessageHandlerBase):
    _request: PrivateRequest
    _result: PrivateResult
    _response: PrivateResponse

    def __init__(self, client: Client, request: PrivateRequest):
        assert isinstance(request, PrivateRequest)
        super().__init__(client, request)


# region publish message handler
class PublishMessageHandler(CmdHandlerBase):
    _request: PublishMessageRequest

    def __init__(self, client: Client, request: PublishMessageRequest):
        assert isinstance(request, PublishMessageRequest)
        super().__init__(client, request)
