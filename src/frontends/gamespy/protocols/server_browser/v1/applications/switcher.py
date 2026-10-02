from frontends.gamespy.library.abstractions.client import ClientBase
from frontends.gamespy.library.abstractions.handler import CmdHandlerBase
from frontends.gamespy.library.abstractions.switcher import SwitcherBase
from frontends.gamespy.library.extentions.gamespy_utils import convert_to_key_value
from frontends.gamespy.library.extentions.string_extentions import (
    get_kv_str_name,
    split_nested_kv_str,
)
from frontends.gamespy.protocols.server_browser.v1.aggregations.enums import (
    Modifier,
    CommandName,
)
from frontends.gamespy.protocols.server_browser.v1.applications.client import Client
from frontends.gamespy.protocols.server_browser.v1.applications.handlers import (
    GroupListHandler,
    ServerInfoHandler,
    ServerListCompressHandler,
)
from frontends.gamespy.protocols.server_browser.v1.contracts.requests import (
    ServerListRequest,
)
from frontends.gamespy.protocols.server_browser.v2.aggregations.exceptions import (
    SBException,
)


class Switcher(SwitcherBase):
    _raw_request: str
    _client: Client

    def __init__(self, client: ClientBase, raw_request: str) -> None:
        assert isinstance(raw_request, str)
        super().__init__(client, raw_request)

    def _process_raw_request(self) -> None:
        if self._raw_request[0] != "\\":
            self._client.log_info("Invalid request received!")
            return
        raw_requests = split_nested_kv_str(self._raw_request)
        for raw_request in raw_requests:
            name = get_kv_str_name(raw_request)
            if name not in CommandName:
                self._client.log_debug(f"Request: {name} is not a valid request.")
                continue
            self._requests.append((CommandName(name), raw_request))

    def _create_cmd_handlers(
        self, name: CommandName, raw_request: str
    ) -> CmdHandlerBase | None:

        match name:
            case CommandName.SERVER_LIST:
                return self.__create_server_list_handler(raw_request)
            case _:
                raise SBException(f"unkown request:{name}")

    def __create_server_list_handler(self, raw_request: str) -> CmdHandlerBase | None:
        request_dict = convert_to_key_value(raw_request)
        modifier = request_dict.get("list")
        if modifier is None:
            raise SBException("modifier is missing")
        modifier = Modifier(modifier)

        # raise NotImplementedError("Server list is not implemented")
        match modifier:
            case Modifier.COMPRESS:
                return ServerListCompressHandler(
                    self._client, ServerListRequest.gs_parse(self._raw_request)
                )
            case Modifier.INFO:
                return ServerInfoHandler(
                    self._client, ServerListRequest.gs_parse(self._raw_request)
                )
            case Modifier.GROUPS:
                return GroupListHandler(
                    self._client, ServerListRequest.gs_parse(self._raw_request)
                )
            case _:
                raise SBException(f"unknown modifier {modifier}")
