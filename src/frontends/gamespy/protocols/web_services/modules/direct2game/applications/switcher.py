import frontends.gamespy.protocols.web_services.applications.switcher as web
from frontends.gamespy.library.abstractions.handler import CmdHandlerBase
from frontends.gamespy.library.network.http_data import HttpData
from frontends.gamespy.protocols.web_services.modules.direct2game.aggregation.enums import (
    CommandName,
)
from frontends.gamespy.protocols.web_services.modules.direct2game.applications.handlers import (
    GetPurchaseHistoryHandler,
    GetStoreAvailabilityHandler,
)
from frontends.gamespy.protocols.web_services.modules.direct2game.contracts.requests import (
    GetPurchaseHistoryRequest,
    GetStoreAvailabilityRequest,
)


class Switcher(web.Switcher):
    def _create_cmd_handlers(
        self, name: CommandName, raw_request: HttpData
    ) -> CmdHandlerBase | None:
        match name:
            # Altas services
            case CommandName.GET_STORE_AVAILABILITY:
                return GetStoreAvailabilityHandler(
                    self._client, GetStoreAvailabilityRequest.gs_parse(raw_request)
                )

            case CommandName.GET_PURCHACE_HISTORY:
                return GetPurchaseHistoryHandler(
                    self._client, GetPurchaseHistoryRequest.gs_parse(raw_request)
                )

            case CommandName.GET_TARGET_AD:
                raise NotImplementedError()
            case _:
                self._client.log_error(f"Unknown {name} request received")
                return None
