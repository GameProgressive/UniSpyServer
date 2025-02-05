from frontends.gamespy.protocols.presence_connection_manager.abstractions.contracts import ResponseBase
from frontends.gamespy.protocols.presence_connection_manager.aggregates.login_challenge import SERVER_CHALLENGE, LoginChallengeProof
from frontends.gamespy.protocols.presence_connection_manager.applications.client import (
    LOGIN_TICKET,
    SESSION_KEY,
)
from frontends.gamespy.protocols.presence_connection_manager.contracts.requests import (
    KeepAliveRequest,
    LoginRequest,
    NewUserRequest,
)
from frontends.gamespy.protocols.presence_connection_manager.contracts.results import (
    LoginResult,
    NewUserResult,
)

# region General


class KeepAliveResponse(ResponseBase):
    def __init__(self, request: KeepAliveRequest) -> None:
        super().__init__(request, None)

    def build(self) -> None:
        self.sending_buffer = "\\ka\\final\\"


class LoginResponse(ResponseBase):
    _result: LoginResult
    _request: LoginRequest

    def __init__(self, request: LoginRequest, result: LoginResult):
        super().__init__(request, result)
        assert isinstance(request, LoginRequest)
        assert isinstance(result, LoginResult)

    def build(self):

        response_proof = LoginChallengeProof(self._request.user_data,
                                             self._request.type,
                                             self._request.partner_id,
                                             self._request.user_challenge,
                                             SERVER_CHALLENGE,
                                             self._result.data.password_hash).generate_proof()
        self.sending_buffer = f"\\lc\\2\\sesskey\\{SESSION_KEY}\\proof\\{response_proof}\\userid\\{
            self._result.data.user_id}\\profileid\\{self._result.data.profile_id}"

        if self._result.data.unique_nick is not None:
            self.sending_buffer += "\\uniquenick\\" + self._result.data.unique_nick

        self.sending_buffer += f"\\lt\\{LOGIN_TICKET}"
        self.sending_buffer += f"\\id\\{self._request.operation_id}\\final\\"


class NewUserResponse(ResponseBase):
    _request: NewUserRequest
    _result: NewUserResult

    def __init__(self, request: NewUserRequest, result: NewUserResult) -> None:
        super().__init__(request, result)
        assert isinstance(request, NewUserRequest)
        assert isinstance(result, NewUserResult)

    def build(self):
        # fmt: on
        self.sending_buffer = f"\\nur\\userid\\{self._result.user_id}\\profileid\\{self._result.profile_id}\\id\\{self._request.operation_id}\\final\\" # fmt: off


# region Buddy

from frontends.gamespy.protocols.presence_connection_manager.abstractions.contracts import (
    RequestBase,
    ResponseBase,
)
from frontends.gamespy.protocols.presence_connection_manager.contracts.requests import AddBuddyRequest, StatusInfoRequest
from frontends.gamespy.protocols.presence_connection_manager.contracts.results import (
    AddBuddyResult,
    BlockListResult,
    BuddyListResult,
    StatusInfoResult,
)

class AddBuddyResponse(ResponseBase):
    def __init__(self, request: AddBuddyRequest, result: AddBuddyResult) -> None:
        assert issubclass(type(request), AddBuddyRequest)
        assert issubclass(type(result), AddBuddyResult)
        super().__init__(request, result)

    def build(self) -> None:
        # return super().build()
        raise NotImplementedError()
        # \bm\<buddy message type>\f\<profile id>\date\<date>
        # GPI_BM_MESSAGE: \msg\<msg>\
        # GPI_BM_UTM:\msg\<msg>\
        # GPI_BM_REQUEST:\msg\|signed|<signed data>\
        # GPI_BM_AUTH:
        # GPI_BM_REVOKE:
        # GPI_BM_STATUS:\msg\|s|<status code>\ or \msg\|ss|<status info status string>|ls|<location string>|ip|<ip>|p|<product id>|qm|<quiet mode flag>
        # GPI_BM_INVITE:\msg\|p|<product id>|l|<location string>
        # GPI_BM_PING:\msg\\


class BlockListResponse(ResponseBase):
    _result: BlockListResult

    def __init__(self, result: BlockListResult):
        assert isinstance(result, BlockListResult)
        self._result = result

    def build(self):
        # \blk\< num in list >\list\< profileid list - comma delimited >\final\
        self.sending_buffer = f"\\blk\\{len(self._result.profile_ids)}\\list\\"
        self.sending_buffer += ",".join(str(pid) for pid in self._result.profile_ids)
        self.sending_buffer += "\\final\\"


class BuddyListResponse(ResponseBase):
    _result: BuddyListResult

    def __init__(self, request: RequestBase, result: BuddyListResult):
        super().__init__(request, result)

    def build(self):
        # \bdy\< num in list >\list\< profileid list - comma delimited >\final\
        self.sending_buffer = f"\\bdy\\{len(self._result.profile_ids)}\\list\\"
        self.sending_buffer += ",".join(str(pid) for pid in self._result.profile_ids)
        self.sending_buffer += "\\final\\"


class StatusInfoResponse(ResponseBase):
    _result: StatusInfoResult

    def __init__(self, request: StatusInfoRequest, result: StatusInfoResult):
        assert isinstance(request, StatusInfoRequest)
        assert isinstance(result, StatusInfoResult)
        super().__init__(request, result)

    def build(self):
        # \bsi\\state\\profile\\bip\\bport\\hostip\\hprivip\\qport\\hport\\sessflags\\rstatus\\gameType\\gameVnt\\gameMn\\product\\qmodeflags\
        self.sending_buffer = (
            f"\\bsi\\state\\{self._result.status_info.status_state}\\"
            f"profile\\{self._result.profile_id}\\bip\\{self._result.status_info.buddy_ip}\\"
            f"hostIp\\{self._result.status_info.host_ip}\\hprivIp\\{self._result.status_info.host_private_ip}\\"
            f"qport\\{self._result.status_info.query_report_port}\\hport\\{self._result.status_info.host_port}\\"
            f"sessflags\\{self._result.status_info.session_flags}\\rstatus\\{self._result.status_info.rich_status}\\"
            f"gameType\\{self._result.status_info.game_type}\\gameVnt\\{self._result.status_info.game_variant}\\"
            f"gameMn\\{self._result.status_info.game_map_name}\\product\\{self._result.product_id}\\"
            f"qmodeflags\\{self._result.status_info.quiet_mode_flags}\\final\\"
        )

# region Profile
from frontends.gamespy.library.extentions.gamespy_ramdoms import StringType, generate_random_string
from frontends.gamespy.protocols.presence_connection_manager.abstractions.contracts import ResponseBase
from frontends.gamespy.protocols.presence_connection_manager.contracts.requests import (
    GetProfileRequest,
    NewProfileRequest,
    RegisterNickRequest,
)
from frontends.gamespy.protocols.presence_connection_manager.contracts.results import (
    GetProfileResult,
    NewProfileResult,
)

class GetProfileResponse(ResponseBase):
    _result: GetProfileResult
    _request: GetProfileRequest

    def __init__(self, request: GetProfileRequest, result: GetProfileResult):
        assert isinstance(request, GetProfileRequest)
        assert isinstance(result, GetProfileResult)
        super().__init__(request, result)

    def build(self):
        self.sending_buffer = (
            f"\\pi\\profileid\\{self._result.user_profile.profile_id}"
            + f"\\nick\\{self._result.user_profile.nick}"
            + f"\\uniquenick\\{self._result.user_profile.unique_nick}"
            + f"\\email\\{self._result.user_profile.email}"
        )
        for key,value in self._result.user_profile.extra_infos.items():
            self.sending_buffer += f"\\{key}\\{value}"

        self.sending_buffer += (
            f"\\sig\\+{generate_random_string(10, StringType.HEX)}"
            + f"\\id\\{self._request.operation_id}\\final\\"
        )


class NewProfileResponse(ResponseBase):
    _request: NewProfileRequest
    _result: NewProfileResult

    def __init__(self, request: NewProfileRequest, result: NewProfileResult):
        assert isinstance(request, NewProfileRequest)
        assert isinstance(result, NewProfileResult)
        super().__init__(request, result)

    def build(self):
        # fmt: off
        self.sending_buffer = f"\\npr\\\\profileid\\{self.sending_buffer}\\id\\{self._request.operation_id}\\final\\"
        # fmt: on


class RegisterCDKeyResposne(ResponseBase):
    def __init__(self) -> None:
        pass

    def build(self):
        self.sending_buffer = "\\rc\\\\final\\"


class RegisterNickResponse(ResponseBase):
    _request: RegisterNickRequest

    def __init__(self, request: RegisterNickRequest) -> None:
        assert isinstance(request, RegisterNickRequest)
        self._request = request

    def build(self) -> None:
        # fmt: off
        self.sending_buffer = f"\\rn\\\\id\\{self._request.operation_id}\\final\\" 
        # fmt: on
