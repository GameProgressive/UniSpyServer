from frontends.gamespy.library.network.http_data import HttpData
import frontends.gamespy.protocols.web_services.applications.switcher as web
from frontends.gamespy.library.abstractions.handler import CmdHandlerBase
from frontends.gamespy.protocols.web_services.modules.auth.applications.handlers import (
    CreateUserAccountHandler,
    LoginProfileHandler,
    LoginRemoteAuthHandler,
    LoginUniqueNickHandler,
)
from frontends.gamespy.protocols.web_services.modules.auth.contracts.requests import (
    CreateUserAccountRequest,
    LoginProfileRequest,
    LoginRemoteAuthRequest,
    LoginUniqueNickRequest,
)


class Switcher(web.Switcher):

    def _create_cmd_handlers(
        self, name: str, raw_request: HttpData
    ) -> CmdHandlerBase | None:
        match name:
            # Auth services
            case "LoginProfile" | "LoginProfileWithGameId":
                return LoginProfileHandler(
                    self._client, LoginProfileRequest.gs_parse(raw_request)
                )
            case "LoginRemoteAuth" | "LoginRemoteAuthWithGameId":
                return LoginRemoteAuthHandler(
                    self._client, LoginRemoteAuthRequest.gs_parse(raw_request)
                )
            case "LoginUniqueNick" | "LoginUniqueNickWithGameId":
                return LoginUniqueNickHandler(
                    self._client, LoginUniqueNickRequest.gs_parse(raw_request)
                )
            case "CreateUserAccount":
                return CreateUserAccountHandler(
                    self._client, CreateUserAccountRequest.gs_parse(raw_request)
                )
