from typing import cast
from http.server import BaseHTTPRequestHandler
import frontends.gamespy.protocols.web_services.abstractions.handler as h
from frontends.gamespy.protocols.web_services.modules.sake.abstractions.contracts import RequestBase, ResultBase


class CmdHandlerBase(h.CmdHandlerBase):
    _request: RequestBase
    _result: ResultBase
    _http_handler: BaseHTTPRequestHandler

    def __init__(self, client: h.Client, request: h.RequestBase) -> None:
        super().__init__(client, request)
        self._http_handler = cast(
            BaseHTTPRequestHandler, self._client.connection.handler)
            