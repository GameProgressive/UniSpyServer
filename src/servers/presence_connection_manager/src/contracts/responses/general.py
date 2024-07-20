from servers.chat.src.contracts.requests.general import LoginRequest
from servers.chat.src.contracts.results.general import LoginResult
from servers.presence_connection_manager.src.abstractions.contracts import ResponseBase
from servers.presence_connection_manager.src.applications.client import (
    LOGIN_TICKET,
    SESSION_KEY,
)
from servers.presence_connection_manager.src.contracts.requests.general import (
    KeepAliveRequest,
)
from servers.presence_search_player.src.contracts.requests import (
    NewUserRequest,
)
from servers.presence_search_player.src.contracts.responses import (
    NewUserResponse as NUR,
)
from servers.presence_search_player.src.contracts.results import NewUserResult


class KeepAliveResponse(ResponseBase):
    def __init__(self, request: KeepAliveRequest) -> None:
        super().__init__(request, None)

    def build(self) -> None:
        self.sending_buffer = "\\ka\\final\\"


class LoginResponse(ResponseBase):
    _result: LoginResult

    def __init__(self, request: LoginRequest, result: LoginResult):
        super().__init__(request, result)

    def build(self):
        # string checkSumStr = _result.DatabaseResults.Nick + _result.DatabaseResults.UniqueNick + _result.DatabaseResults.NamespaceID;
        # _connection.UserData.SessionKey = _crc.ComputeChecksum(checkSumStr);

        self.sending_buffer = (
            "\\lc\\2\\sesskey\\"
            + SESSION_KEY
            + "\\proof\\"
            + self._result.response_proof
            + "\\userid\\"
            + self._result.data.user_id
            + "\\profileid\\"
            + self._result.data.profile_id
        )

        if self._result.data.unique_nick is not None:
            self.sending_buffer += "\\uniquenick\\" + self._result.data.unique_nick

        self.sending_buffer += f"\\lt\\{LOGIN_TICKET}"
        self.sending_buffer += f"\\id\\{self._request.operation_id}\\final\\"


class NewUserResponse(NUR):
    _result: NewUserResult
    _request: NewUserRequest

    def build(self):
        self.sending_buffer = f"\\nur\\userid\\{self._result.user.userid}\\profileid\\{self._result.sub_profile.profileid}\\id\\{self._request.operation_id}\\final\\"
