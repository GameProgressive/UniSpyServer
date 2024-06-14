import abc

from servers.presence_connection_manager.applications.client import Client
from servers.presence_connection_manager.enums.general import LoginStatus
from servers.presence_search_player.exceptions.general import GPException

from servers.presence_connection_manager.abstractions.contracts import (
    RequestBase,
    ResultBase,
)
import library


class CmdHandlerBase(library.abstractions.handler.CmdHandlerBase):
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


class LoginHandlerBase(CmdHandlerBase, abc.ABC):

    def _request_check(self) -> None:
        if self._client.info.login_status != LoginStatus.COMPLETED:
            raise GPException("please login first.")
        super()._request_check()
