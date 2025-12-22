from typing import TYPE_CHECKING, cast
from frontends.gamespy.library.abstractions.handler import CmdHandlerBase
from frontends.gamespy.protocols.web_services.applications.client import Client
import frontends.gamespy.protocols.web_services.applications.switcher as web
from frontends.gamespy.protocols.web_services.modules.direct2game.applications.handlers import GetPurchaseHistoryHandler, GetStoreAvailabilityHandler
from frontends.gamespy.protocols.web_services.modules.direct2game.contracts.requests import GetPurchaseHistoryRequest, GetStoreAvailabilityRequest


class Switcher(web.Switcher):

    def _create_cmd_handlers(self, name: str, raw_request: str) -> CmdHandlerBase | None:
        match name:
            # Altas services
            case "GetStoreAvailability":
                return GetStoreAvailabilityHandler(self._client, GetStoreAvailabilityRequest(raw_request))

            case "GetPurchaseHistory":
                return GetPurchaseHistoryHandler(self._client, GetPurchaseHistoryRequest(raw_request))

            case "GetTargettedAd":
                raise NotImplementedError()
            case _:
                self._client.log_error(f"Unknown {name} request received")
                return None
