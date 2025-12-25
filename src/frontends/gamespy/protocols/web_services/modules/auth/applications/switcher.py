from typing import TYPE_CHECKING, cast
from frontends.gamespy.library.abstractions.handler import CmdHandlerBase
from frontends.gamespy.library.network.http_handler import HttpData
import frontends.gamespy.protocols.web_services.applications.switcher as web
from frontends.gamespy.protocols.web_services.modules.auth.applications.handlers import CreateUserAccountHandler, LoginProfileHandler, LoginProfileWithGameIdHandler, LoginRemoteAuthHandler, LoginRemoteAuthWithGameIdHandler, LoginUniqueNickHandler, LoginUniqueNickWithGameIdHandler
from frontends.gamespy.protocols.web_services.modules.auth.contracts.requests import CreateUserAccountRequest, LoginProfileRequest, LoginProfileWithGameIdRequest, LoginRemoteAuthRequest, LoginRemoteAuthWithGameIdRequest, LoginUniqueNickRequest, LoginUniqueNickWithGameIdRequest


class Switcher(web.Switcher):

    def _create_cmd_handlers(self, name: str, raw_request: HttpData) -> CmdHandlerBase | None:
        match name:
            # Auth services
            case "LoginProfile":
                return LoginProfileHandler(
                    self._client, LoginProfileRequest(raw_request))
            case "LoginProfileWithGameId":
                return LoginProfileWithGameIdHandler(
                    self._client, LoginProfileWithGameIdRequest(raw_request))
            case "LoginRemoteAuth":
                return LoginRemoteAuthHandler(self._client, LoginRemoteAuthRequest(raw_request))

            case "LoginRemoteAuthWithGameId":
                return LoginRemoteAuthWithGameIdHandler(self._client, LoginRemoteAuthWithGameIdRequest(raw_request))

            case "LoginUniqueNick":
                return LoginUniqueNickHandler(self._client, LoginUniqueNickRequest(raw_request))
            case "LoginUniqueNickWithGameId":
                return LoginUniqueNickWithGameIdHandler(self._client, LoginUniqueNickWithGameIdRequest(raw_request))
            case "CreateUserAccount":
                return CreateUserAccountHandler(self._client, CreateUserAccountRequest(raw_request))
