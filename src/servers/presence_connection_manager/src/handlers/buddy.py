from multiprocessing.pool import Pool
from servers.presence_connection_manager.src.abstractions.contracts import RequestBase
from servers.presence_connection_manager.src.abstractions.handlers import (
    CmdHandlerBase,
    LoginedHandlerBase,
)
from servers.presence_connection_manager.src.applications.client import Client
from servers.presence_connection_manager.src.contracts.requests.buddy import (
    DelBuddyRequest,
    StatusInfoRequest,
    StatusRequest,
)
from servers.presence_connection_manager.src.contracts.responses.buddy import (
    BlockListResponse,
    BuddyListResponse,
    StatusInfoResponse,
)
from servers.presence_connection_manager.src.contracts.results.buddy import (
    BlockListResult,
    BuddyListResult,
    StatusInfoResult,
    StatusResult,
)


class AddBuddyHandler(CmdHandlerBase):
    def __init__(self, client: Client, request: RequestBase) -> None:
        raise NotImplementedError()
        super().__init__(client, request)


class BlockListHandler(CmdHandlerBase):
    _result: BlockListResult

    def __init__(self, client: Client) -> None:
        assert isinstance(client, Client)

    def _response_construct(self) -> None:
        self._response = BlockListResponse(self._result)


class BuddyListHandler(LoginedHandlerBase):
    _result: BuddyListResult
    _result_cls = BuddyListResult

    def __init__(self, client: Client):
        assert isinstance(client, Client)
        self._client = client

    def response_construct(self):
        self._response = BuddyListResponse(self._request, self._result)

    def handle_status_info(self, profile_id):
        request = StatusInfoRequest()
        request.profile_id = profile_id
        request.namespace_id = int(self._client.info.namespace_id)
        request.is_get_status_info = True

        StatusInfoHandler(self._client, request).handle()

    def _response_send(self) -> None:
        super()._response_send()

        if not self._client.info.sdk_revision.is_support_gpi_new_status_notification:
            return

        with Pool() as pool:
            pool.map(self.handle_status_info, self._result.profile_id_list)


class BuddyStatusInfoHandler(CmdHandlerBase):
    """
    This is what the message should look like.  Its broken up for easy viewing.

    \bsi\\state\\profile\\bip\\bport\\hostip\\hprivip\\qport\\hport\\sessflags\\rstatus\\gameType\\gameVnt\\gameMn\\product\\qmodeflags\
    """

    def __init__(self, client: Client, request: RequestBase) -> None:
        raise NotImplementedError()
        super().__init__(client, request)


class DelBuddyHandler(LoginedHandlerBase):
    _request: DelBuddyRequest

    def __init__(self, client: Client, request: DelBuddyRequest) -> None:
        assert isinstance(request, DelBuddyRequest)
        super().__init__(client, request)


class InviteToHandler(LoginedHandlerBase):
    def __init__(self, client: Client, request: RequestBase) -> None:
        raise NotImplementedError()
        super().__init__(client, request)

    pass


class StatusHandler(CmdHandlerBase):
    _request: StatusRequest
    _result: StatusResult = StatusResult()

    def __init__(self, client: Client, request: StatusRequest) -> None:
        assert isinstance(request, StatusRequest)
        super().__init__(client, request)

    def _response_send(self) -> None:
        # TODO check if statushandler need send response
        raise NotImplementedError()


class StatusInfoHandler(LoginedHandlerBase):
    _request: StatusInfoRequest
    _result: StatusInfoResult = StatusInfoResult()

    def __init__(self, client: Client, request: StatusInfoRequest) -> None:
        assert isinstance(request, StatusInfoRequest)
        super().__init__(client, request)

    def _response_send(self) -> None:
        if self._request.is_get_status_info:
            self._response = StatusInfoResponse(self._request, self._result)
        super()._response_send()
