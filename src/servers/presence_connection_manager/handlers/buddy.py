from multiprocessing.pool import Pool
from servers.presence_connection_manager.abstractions.contracts import RequestBase
from servers.presence_connection_manager.abstractions.handler import (
    CmdHandlerBase,
    LoginHandlerBase,
)
from servers.presence_connection_manager.applications.client import Client
from servers.presence_connection_manager.applications.data import (
    get_blocked_profile_id_list,
    get_friend_profile_id_list,
    delete_friend_by_profile_id,
)
from servers.presence_connection_manager.contracts.requests.buddy import (
    DelBuddyRequest,
    StatusInfoRequest,
    StatusRequest,
)
from servers.presence_connection_manager.contracts.responses.buddy import (
    BlockListResponse,
    BuddyListResponse,
    StatusInfoResponse,
)
from servers.presence_connection_manager.contracts.results.buddy import (
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

    def _data_operation(self) -> None:
        self._result.profile_ids = get_blocked_profile_id_list(self._client.info.profile_id, self._client.info.namespace_id)

    def _response_construct(self) -> None:
        self._response = BlockListResponse(self._result)


class BuddyListHandler(LoginHandlerBase):
    _result: BuddyListResult = BuddyListResult()

    def __init__(self, client: Client):
        assert isinstance(client, Client)
        self._client = client

    def _data_operation(self) -> None:
        friends_id = get_friend_profile_id_list(self._client.info.profile_id, self._client.info.namespace_id)
        self._result.profile_ids = friends_id

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

    def _data_operation(self) -> None:
        raise NotImplementedError()


class DelBuddyHandler(LoginHandlerBase):
    _request: DelBuddyRequest

    def __init__(self, client: Client, request: DelBuddyRequest) -> None:
        assert isinstance(request, DelBuddyRequest)
        super().__init__(client, request)

    def _data_operation(self) -> None:
        delete_friend_by_profile_id(self._client.info.profile_id, self._request.friend_profile_id, self._client.info.namespace_id)


class InviteToHandler(LoginHandlerBase):
    def __init__(self, client: Client, request: RequestBase) -> None:
        raise NotImplementedError()
        super().__init__(client, request)

    def _data_operation(self) -> None:
        if(self._client == None):
            return
        else:
            # TODO
            # Parse user to Buddy Message System
            raise NotImplementedError()


class StatusHandler(CmdHandlerBase):
    _request: StatusRequest
    _result: StatusResult = StatusResult()

    def __init__(self, client: Client, request: StatusRequest) -> None:
        assert isinstance(request, StatusRequest)
        super().__init__(client, request)

    def _response_send(self) -> None:
        # TODO check if statushandler need send response
        raise NotImplementedError()


class StatusInfoHandler(LoginHandlerBase):
    _request: StatusInfoRequest
    _result: StatusInfoResult = StatusInfoResult()

    def __init__(self, client: Client, request: StatusInfoRequest) -> None:
        assert isinstance(request, StatusInfoRequest)
        super().__init__(client, request)

    def _data_operation(self) -> None:
        if(self._request.is_get_status_info):
            if(self._client != None):
                # User is not online we do not need to send status info
                self._result.status_info = self._client.info.status_info
            else:
                self._result.status_info = self._request.status_info
                # TODO
                # Notify every online friend?

    def _response_send(self) -> None:
        if self._request.is_get_status_info:
            self._response = StatusInfoResponse(self._request, self._result)
        super()._response_send()
