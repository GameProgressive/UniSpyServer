
from typing import TYPE_CHECKING
from frontends.gamespy.library.abstractions.client import ClientBase
from frontends.gamespy.library.abstractions.handler import CmdHandlerBase
from frontends.gamespy.library.abstractions.switcher import SwitcherBase
from frontends.gamespy.protocols.chat.aggregates.enums import RequestType
from frontends.gamespy.protocols.chat.contracts.requests import (
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
    AtmRequest,
    NoticeRequest,
    PrivateRequest,
    UtmRequest,
)
from frontends.gamespy.protocols.chat.applications.handlers import (
    GetCKeyHandler,
    GetChannelKeyHandler,
    JoinHandler,
    KickHandler,
    ModeHandler,
    NamesHandler,
    PartHandler,
    SetCKeyHandler,
    SetChannelKeyHandler,
    TopicHandler,
    CdKeyHandler,
    CryptHandler,
    GetKeyHandler,
    ListHandler,
    LoginHandler,
    NickHandler,
    PingHandler,
    QuitHandler,
    SetKeyHandler,
    UserHandler,
    UserIPHandler,
    WhoHandler,
    WhoIsHandler,
    ATMHandler,
    NoticeHandler,
    PrivateHandler,
    UTMHandler,
)

from frontends.gamespy.protocols.chat.applications.client import Client


class Switcher(SwitcherBase):
    _raw_request: str
    _client: Client

    def __init__(self, client: ClientBase, raw_request: str) -> None:
        assert isinstance(raw_request, str)
        super().__init__(client, raw_request)

    def _process_raw_request(self) -> None:
        splited_raw_requests = [
            req for req in self._raw_request.replace("\r", "").split("\n") if req]
        for raw_request in splited_raw_requests:
            name = raw_request.strip(" ").split(" ")[0]
            if name not in RequestType:
                self._client.log_debug(
                    f"Request: {name} is not a valid request.")
                continue
            self._requests.append((RequestType(name), raw_request))

    def _create_cmd_handlers(self, name: RequestType, raw_request: str) -> CmdHandlerBase | None:
        assert isinstance(name, RequestType)
        r_type = RequestType(name)
        match r_type:
            # region General
            case RequestType.CRYPT:
                return CryptHandler(self._client, CryptRequest(raw_request))
            case RequestType.CDKEY:
                return CdKeyHandler(self._client, CdkeyRequest(raw_request))
            case RequestType.GETKEY:
                return GetKeyHandler(self._client, GetKeyRequest(raw_request))
            case RequestType.LIST:
                return ListHandler(self._client, ListRequest(raw_request))
            case RequestType.LOGIN:
                return LoginHandler(self._client, LoginRequest(raw_request))
            case RequestType.NICK:
                return NickHandler(self._client, NickRequest(raw_request))
            case RequestType.PING:
                return PingHandler(self._client, PingRequest(raw_request))
            case RequestType.QUIT:
                return QuitHandler(self._client, QuitRequest(raw_request))
            case RequestType.SETKEY:
                return SetKeyHandler(self._client, SetKeyRequest(raw_request))
            case RequestType.USER:
                return UserHandler(self._client, UserRequest(raw_request))
            case RequestType.USRIP:
                return UserIPHandler(self._client, UserIPRequest(raw_request))
            case RequestType.WHO:
                return WhoHandler(self._client, WhoRequest(raw_request))
            case RequestType.WHOIS:
                return WhoIsHandler(self._client, WhoIsRequest(raw_request))

            # Channel commands
            case RequestType.GETCHANKEY:
                return GetChannelKeyHandler(self._client, GetChannelKeyRequest(raw_request))
            case RequestType.GETCKEY:
                return GetCKeyHandler(self._client, GetCKeyRequest(raw_request))
            case RequestType.JOIN:
                return JoinHandler(self._client, JoinRequest(raw_request))
            case RequestType.KICK:
                return KickHandler(self._client, KickRequest(raw_request))
            case RequestType.MODE:
                return ModeHandler(self._client, ModeRequest(raw_request))
            case RequestType.NAMES:
                return NamesHandler(self._client, NamesRequest(raw_request))
            case RequestType.PART:
                return PartHandler(self._client, PartRequest(raw_request))
            case RequestType.SETCHANKEY:
                return SetChannelKeyHandler(self._client, SetChannelKeyRequest(raw_request))
            case RequestType.SETCKEY:
                return SetCKeyHandler(self._client, SetCKeyRequest(raw_request))
            case RequestType.TOPIC:
                return TopicHandler(self._client, TopicRequest(raw_request))

            # Message commands
            case RequestType.ATM:
                return ATMHandler(self._client, AtmRequest(raw_request))
            case RequestType.NOTICE:
                return NoticeHandler(self._client, NoticeRequest(raw_request))
            case RequestType.PRIVMSG:
                return PrivateHandler(self._client, PrivateRequest(raw_request))
            case RequestType.UTM:
                return UTMHandler(self._client, UtmRequest(raw_request))
            case _:
                return None
            # endregion
