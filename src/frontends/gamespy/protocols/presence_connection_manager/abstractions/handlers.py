
from frontends.gamespy.protocols.presence_connection_manager.applications.client import Client
from frontends.gamespy.protocols.presence_connection_manager.aggregates.enums import LoginStatus
from frontends.gamespy.protocols.presence_search_player.aggregates.exceptions import GPException

from frontends.gamespy.protocols.presence_connection_manager.abstractions.contracts import (
    RequestBase,
    ResultBase,
)
import frontends.gamespy.library.abstractions.handler as lib


class CmdHandlerBase(lib.CmdHandlerBase):
    _client: Client
    _request: RequestBase
    _result: ResultBase

    def __init__(self, client: Client, request: RequestBase) -> None:
        assert isinstance(client, Client)
        assert issubclass(type(request), RequestBase)
        super().__init__(client, request)

    def _handle_exception(self, ex) -> None:
        if ex is GPException:
            self._client.send(ex)
        super()._handle_exception(ex)


class LoginedHandlerBase(CmdHandlerBase):

    def _request_check(self) -> None:
        if self._client.info.login_status != LoginStatus.COMPLETED:
            raise GPException("please login first.")
        super()._request_check()
