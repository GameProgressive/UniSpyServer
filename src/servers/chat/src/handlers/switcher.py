from library.src.abstractions.client import ClientBase
from library.src.abstractions.handler import CmdHandlerBase
from library.src.abstractions.switcher import SwitcherBase
from servers.chat.src.contracts.requests.channel import (
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
)
from servers.chat.src.contracts.requests.general import (
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
)
from servers.chat.src.contracts.requests.message import (
    ATMRequest,
    NoticeRequest,
    PrivateRequest,
    UTMRequest,
)
from servers.chat.src.handlers.channel import (
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
)
from servers.chat.src.handlers.general import (
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
)
from servers.chat.src.handlers.message import (
    ATMHandler,
    NoticeHandler,
    PrivateHandler,
    UTMHandler,
)


class Switcher(SwitcherBase):
    _raw_request: str
    _client: ClientBase

    def __init__(self, client: ClientBase, raw_request: str) -> None:
        assert isinstance(raw_request, str)
        super().__init__(client, raw_request)

    def _process_raw_request(self) -> None:
        splited_raw_requests = self._raw_request.replace("\r", "").split("\n")
        for raw_request in splited_raw_requests:
            name = raw_request.strip(" ").split(" ")[0]
            self._requests.append((name, raw_request))

    def _create_cmd_handlers(self, name: str, rawRequest: str) -> CmdHandlerBase:
        request = rawRequest
        assert isinstance(name, str)
        match name:
            # region General
            case "CRYPT":
                return CryptHandler(self._client, CryptRequest(request))
            case "CDKEY":
                return CdKeyHandler(self._client, CdkeyRequest(request))
            case "GETKEY":
                return GetKeyHandler(self._client, GetKeyRequest(request))
            case "LIST":
                return ListHandler(self._client, ListRequest(request))
            case "LOGIN":
                return LoginHandler(self._client, LoginRequest(request))
            case "NICK":
                return NickHandler(self._client, NickRequest(request))
            case "PING":
                return PingHandler(self._client, PingRequest(request))
            case "QUIT":
                return QuitHandler(self._client, QuitRequest(request))
            case "SETKEY":
                return SetKeyHandler(self._client, SetKeyRequest(request))
            case "USER":
                return UserHandler(self._client, UserRequest(request))
            case "USRIP":
                return UserIPHandler(self._client, UserIPRequest(request))
            case "WHO":
                return WhoHandler(self._client, WhoRequest(request))
            case "WHOIS":
                return WhoIsHandler(self._client, WhoIsRequest(request))
            # endregion

            # region Channel
            case "GETCHANKEY":
                return GetChannelKeyHandler(self._client, GetChannelKeyRequest(request))
            case "GETCKEY":
                return GetCKeyHandler(self._client, GetCKeyRequest(request))
            case "JOIN":
                return JoinHandler(self._client, JoinRequest(request))
            case "KICK":
                return KickHandler(self._client, KickRequest(request))
            case "MODE":
                return ModeHandler(self._client, ModeRequest(request))
            case "NAMES":
                return NamesHandler(self._client, NamesRequest(request))
            case "PART":
                return PartHandler(self._client, PartRequest(request))
            case "SETCHANKEY":
                return SetChannelKeyHandler(self._client, SetChannelKeyRequest(request))
            case "SETCKEY":
                return SetCKeyHandler(self._client, SetCKeyRequest(request))
            case "TOPIC":
                return TopicHandler(self._client, TopicRequest(request))
            # endregion
            # region  Message
            case "ATM":
                return ATMHandler(self._client, ATMRequest(request))
            case "NOTICE":
                return NoticeHandler(self._client, NoticeRequest(request))
            case "PRIVMSG":
                return PrivateHandler(self._client, PrivateRequest(request))
            case "UTM":
                return UTMHandler(self._client, UTMRequest(request))
            case _:
                return None
                # endregion
