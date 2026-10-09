
from frontends.gamespy.library.abstractions.client import ClientBase
from frontends.gamespy.library.abstractions.handler import CmdHandlerBase
from frontends.gamespy.library.abstractions.switcher import SwitcherBase
from frontends.gamespy.protocols.chat.aggregates.enums import CommandName
from frontends.gamespy.protocols.chat.applications.client import Client
from frontends.gamespy.protocols.chat.applications.handlers import (
    ATMHandler,
    CdKeyHandler,
    CryptHandler,
    GetChannelKeyHandler,
    GetCKeyHandler,
    GetKeyHandler,
    JoinHandler,
    KickHandler,
    ListHandler,
    LoginHandler,
    ModeHandler,
    NamesHandler,
    NickHandler,
    NoticeHandler,
    PartHandler,
    PingHandler,
    PrivateHandler,
    QuitHandler,
    SetChannelKeyHandler,
    SetCKeyHandler,
    SetKeyHandler,
    TopicHandler,
    UserHandler,
    UserIPHandler,
    UTMHandler,
    WhoHandler,
    WhoIsHandler,
)
from frontends.gamespy.protocols.chat.contracts.requests import (
    AtmRequest,
    CdkeyRequest,
    CryptRequest,
    GetChannelKeyRequest,
    GetCKeyRequest,
    GetKeyRequest,
    JoinRequest,
    KickRequest,
    ListRequest,
    LoginRequest,
    ModeRequest,
    NamesRequest,
    NickRequest,
    NoticeRequest,
    PartRequest,
    PingRequest,
    PrivateRequest,
    QuitRequest,
    SetChannelKeyRequest,
    SetCKeyRequest,
    SetKeyRequest,
    TopicRequest,
    UserIPRequest,
    UserRequest,
    UtmRequest,
    WhoIsRequest,
    WhoRequest,
)


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
            if name not in CommandName:
                self._client.log_debug(
                    f"Request: {name} is not a valid request.")
                continue
            self._requests.append((CommandName(name), raw_request))

    def _create_cmd_handlers(self, name: CommandName, raw_request: str) -> CmdHandlerBase | None:
        match name:
            # region General
            case CommandName.CRYPT:
                return CryptHandler(self._client, CryptRequest.gs_parse(raw_request))
            case CommandName.CDKEY:
                return CdKeyHandler(self._client, CdkeyRequest.gs_parse(raw_request))
            case CommandName.GETKEY:
                return GetKeyHandler(self._client, GetKeyRequest.gs_parse(raw_request))
            case CommandName.LIST:
                return ListHandler(self._client, ListRequest.gs_parse(raw_request))
            case CommandName.LOGIN:
                return LoginHandler(self._client, LoginRequest.gs_parse(raw_request))
            case CommandName.NICK:
                return NickHandler(self._client, NickRequest.gs_parse(raw_request))
            case CommandName.PING:
                return PingHandler(self._client, PingRequest.gs_parse(raw_request))
            case CommandName.QUIT:
                return QuitHandler(self._client, QuitRequest.gs_parse(raw_request))
            case CommandName.SETKEY:
                return SetKeyHandler(self._client, SetKeyRequest.gs_parse(raw_request))
            case CommandName.USER:
                return UserHandler(self._client, UserRequest.gs_parse(raw_request))
            case CommandName.USRIP:
                return UserIPHandler(self._client, UserIPRequest.gs_parse(raw_request))
            case CommandName.WHO:
                return WhoHandler(self._client, WhoRequest.gs_parse(raw_request))
            case CommandName.WHOIS:
                return WhoIsHandler(self._client, WhoIsRequest.gs_parse(raw_request))

            # Channel commands
            case CommandName.GETCHANKEY:
                return GetChannelKeyHandler(self._client, GetChannelKeyRequest.gs_parse(raw_request))
            case CommandName.GETCKEY:
                return GetCKeyHandler(self._client, GetCKeyRequest.gs_parse(raw_request))
            case CommandName.JOIN:
                return JoinHandler(self._client, JoinRequest.gs_parse(raw_request))
            case CommandName.KICK:
                return KickHandler(self._client, KickRequest.gs_parse(raw_request))
            case CommandName.MODE:
                return ModeHandler(self._client, ModeRequest.gs_parse(raw_request))
            case CommandName.NAMES:
                return NamesHandler(self._client, NamesRequest.gs_parse(raw_request))
            case CommandName.PART:
                return PartHandler(self._client, PartRequest.gs_parse(raw_request))
            case CommandName.SETCHANKEY:
                return SetChannelKeyHandler(self._client, SetChannelKeyRequest.gs_parse(raw_request))
            case CommandName.SETCKEY:
                return SetCKeyHandler(self._client, SetCKeyRequest.gs_parse(raw_request))
            case CommandName.TOPIC:
                return TopicHandler(self._client, TopicRequest.gs_parse(raw_request))

            # Message commands
            case CommandName.ATM:
                return ATMHandler(self._client, AtmRequest.gs_parse(raw_request))
            case CommandName.NOTICE:
                return NoticeHandler(self._client, NoticeRequest.gs_parse(raw_request))
            case CommandName.PRIVMSG:
                return PrivateHandler(self._client, PrivateRequest.gs_parse(raw_request))
            case CommandName.UTM:
                return UTMHandler(self._client, UtmRequest.gs_parse(raw_request))
            case _:
                return None
            # endregion
