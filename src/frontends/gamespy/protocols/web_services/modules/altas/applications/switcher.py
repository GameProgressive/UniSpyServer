from typing import TYPE_CHECKING, cast
from frontends.gamespy.protocols.web_services.applications.client import Client
import frontends.gamespy.protocols.web_services.applications.switcher as web
from frontends.gamespy.protocols.web_services.modules.sake.abstractions.handler import CmdHandlerBase


class Switcher(web.Switcher):

    def _create_cmd_handlers(self, name: str, raw_request: str) -> CmdHandlerBase | None:
        match name:
            # Altas services
            case "CreateMatchlessSession":
                raise NotImplementedError()
            case "CreateSession":
                raise NotImplementedError()
            case "SetReportIntention":
                raise NotImplementedError()
            case "SubmitReport":
                raise NotImplementedError()
