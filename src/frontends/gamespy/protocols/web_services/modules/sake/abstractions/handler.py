from typing import cast
from http.server import BaseHTTPRequestHandler
import frontends.gamespy.protocols.web_services.abstractions.handler as h
from frontends.gamespy.protocols.web_services.modules.sake.abstractions.contracts import RequestBase, ResultBase
from frontends.gamespy.protocols.web_services.modules.sake.aggregates.enums import SakePlatform


class CmdHandlerBase(h.CmdHandlerBase):
    _request: RequestBase
    _result: ResultBase
    _http_handler: BaseHTTPRequestHandler

    def __init__(self, client: h.Client, request: h.RequestBase) -> None:
        super().__init__(client, request)
        self._http_handler = cast(
            BaseHTTPRequestHandler, self._client.connection.handler)

    def _request_check(self) -> None:
        super()._request_check()
        if self._request.login_ticket is None:
            self._request.parse_headers(dict(self._http_handler.headers))

    def _response_construct(self) -> None:
        super()._response_construct()
        # add extra headers here
        if self._request.platform == SakePlatform.Unity:
            # we send header data back as same as http request
            headers = dict(self._http_handler.headers)
            if "GameID" in headers:
                self._http_handler.send_header("GameID", headers["GameID"])

            if "SessionToken" in headers:
                self._http_handler.send_header(
                    "SessionToken", headers["SessionToken"])

            if "ProfileID" in headers:
                self._http_handler.send_header(
                    "ProfileID", headers["ProfileID"])

    def _handle_unispy_error(self):
        super()._handle_unispy_error()
        if self._request.platform == SakePlatform.Unity:
            self._http_handler.send_header(
                "Error", self._http_result["message"])

    def _handle_general_error(self):
        super()._handle_general_error()
        if self._request.platform == SakePlatform.Unity:
            self._http_handler.send_header(
                "Error", self._http_result["message"])
