import frontends.gamespy.protocols.web_services.abstractions.handler as lib
from frontends.gamespy.protocols.web_services.modules.auth.aggregates.exceptions import EXCEPTIONS


class CmdHandlerBase(lib.CmdHandlerBase):
    def __init__(self, client: lib.Client, request: lib.RequestBase) -> None:
        self._exceptions_mapping = EXCEPTIONS
        super().__init__(client, request)
