from typing import final

from frontends.gamespy.library.extentions.gamespy_ramdoms import (
    StringType,
    generate_random_string,
)
from frontends.gamespy.protocols.presence_connection_manager.abstractions.contracts import (
    ResponseBase,
    ResultBase,
)
from frontends.gamespy.protocols.presence_connection_manager.aggregates.enums import (
    BuddyMessageType,
)
from frontends.gamespy.protocols.presence_connection_manager.aggregates.login_challenge import (
    SERVER_CHALLENGE,
    generate_proof,
)
from frontends.gamespy.protocols.presence_connection_manager.applications.client import (
    LOGIN_TICKET,
    SESSION_KEY,
)
from frontends.gamespy.protocols.presence_connection_manager.contracts.requests import (
    GetProfileRequest,
    LoginRequest,
    NewProfileRequest,
    NewUserRequest,
)
from frontends.gamespy.protocols.presence_connection_manager.contracts.results import (
    AddBuddyResult,
    BlockListResult,
    BuddyListResult,
    BuddyMessageAuthResult,
    BuddyMessageDataBase,
    BuddyMessageFriendAddResult,
    BuddyMessageResult,
    BuddyMessageResultBase,
    BuddyMessageRevokResult,
    BuddyMessageUTMResult,
    GetProfileResult,
    LoginResult,
    NewProfileResult,
    NewUserResult,
    RegisterNickResult,
    StatusInfoResult,
)

# region General


class KeepAliveResponse(ResponseBase):
    def __init__(self) -> None:
        pass

    def build(self) -> None:
        self.sending_buffer = "\\ka\\final\\"


class LoginResponse(ResponseBase):
    _request: LoginRequest
    _result: LoginResult

    def __init__(self, result: LoginResult):
        super().__init__(result)

        assert isinstance(result, LoginResult)

    def build(self):
        response_proof = generate_proof(
            self._result.user_data,
            self._result.type,
            self._result.partner_id,
            SERVER_CHALLENGE,
            self._result.user_challenge,
            self._result.data.password_hash,
        )
        self.sending_buffer = f"\\lc\\2\\sesskey\\{SESSION_KEY}\\proof\\{response_proof}\\userid\\{self._result.data.user_id}\\profileid\\{self._result.data.profile_id}"

        if self._result.data.unique_nick is not None:
            self.sending_buffer += "\\uniquenick\\" + self._result.data.unique_nick

        self.sending_buffer += f"\\lt\\{LOGIN_TICKET}"
        self.sending_buffer += f"\\id\\{self._result.operation_id}\\final\\"


class NewUserResponse(ResponseBase):
    _request: NewUserRequest
    _result: NewUserResult

    def __init__(self, result: NewUserResult) -> None:
        super().__init__(result)

        assert isinstance(result, NewUserResult)

    def build(self):
        # fmt: on
        self.sending_buffer = f"\\nur\\userid\\{self._result.user_id}\\profileid\\{self._result.profile_id}\\id\\{self._result.operation_id}\\final\\"  # fmt: off


# region Buddy


class AddBuddyResponse(ResponseBase):
    def __init__(self, result: AddBuddyResult) -> None:
        assert issubclass(type(result), AddBuddyResult)
        super().__init__(result)

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

    def __init__(self, result: BuddyListResult):
        super().__init__(result)

    def build(self):
        # \bdy\< num in list >\list\< profileid list - comma delimited >\final\
        self.sending_buffer = f"\\bdy\\{len(self._result.profile_ids)}\\list\\"
        self.sending_buffer += ",".join(str(pid) for pid in self._result.profile_ids)
        self.sending_buffer += "\\final\\"


# region Buddy Message
class BuddyMessageResponseBase(ResponseBase):
    _result: BuddyMessageResultBase
    _type: BuddyMessageType

    def build(self) -> None:
        self.sending_buffer = ""
        for data in self._result.data:
            self.__build_message(data)

    def __build_message(self, data: BuddyMessageDataBase) -> None:
        self.sending_buffer += f"\\bm\\{BuddyMessageType.BM_REQUEST}"
        self.sending_buffer += f"\\f\\{data.from_profile_id}"
        date = int(data.date.timestamp())
        self.sending_buffer += f"\\date\\{date}"
        self._build_extra(data)
        self.sending_buffer += "\\final\\"

    def _build_extra(self, data: BuddyMessageDataBase) -> None:
        """
        build extra data here
        """
        pass


@final
class BuddyMessageFriendAddResponse(BuddyMessageResponseBase):
    _result: BuddyMessageFriendAddResult

    def __init__(self, result: BuddyMessageFriendAddResult) -> None:
        super().__init__(result)

    def _build_extra(
        self, data: BuddyMessageFriendAddResult.BuddyMessageFriendAddData
    ) -> None:
        self.sending_buffer += f"\\msg\\{data.message}|signed|{data.signature}"


@final
class BuddyMessageResponse(BuddyMessageResponseBase):
    _result: BuddyMessageResult

    def __init__(self, result: BuddyMessageResult) -> None:
        super().__init__(result)

    def _build_extra(self, data: BuddyMessageResult.BuddyMessageData) -> None:
        self.sending_buffer += f"\\message\\{data.message}"


@final
class BuddyMessageUTMResponse(BuddyMessageResponseBase):
    _result: BuddyMessageUTMResult

    def __init__(self, result: BuddyMessageUTMResult) -> None:
        super().__init__(result)

    def _build_extra(self, data: BuddyMessageResult.BuddyMessageData) -> None:
        self.sending_buffer += f"\\message\\{data.message}"


@final
class BuddyMessageRevokeResponse(BuddyMessageResponseBase):
    _result: BuddyMessageRevokResult


@final
class BuddyMessageAuthResponse(BuddyMessageResponseBase):
    _result: BuddyMessageAuthResult


class StatusInfoResponse(ResponseBase):
    _result: StatusInfoResult

    def __init__(self, result: StatusInfoResult):

        assert isinstance(result, StatusInfoResult)
        super().__init__(result)

    def build(self):
        # \bsi\\state\\profile\\bip\\bport\\hostip\\hprivip\\qport\\hport\\sessflags\\rstatus\\gameType\\gameVnt\\gameMn\\product\\qmodeflags\
        self.sending_buffer = (
            f"\\bsi\\state\\{self._result.status_state}\\"
            f"profile\\{self._result.profile_id}\\bip\\{self._result.buddy_ip}\\"
            f"hostIp\\{self._result.host_ip}\\hprivIp\\{self._result.host_private_ip}\\"
            f"qport\\{self._result.query_report_port}\\hport\\{self._result.host_port}\\"
            f"sessflags\\{self._result.session_flags}\\rstatus\\{self._result.rich_status}\\"
            f"gameType\\{self._result.game_type}\\gameVnt\\{self._result.game_variant}\\"
            f"gameMn\\{self._result.game_map_name}\\product\\{self._result.product_id}\\"
            f"qmodeflags\\{self._result.quiet_mode_flags}\\final\\"
        )


# region Profile


class GetProfileResponse(ResponseBase):
    _result: GetProfileResult
    _request: GetProfileRequest

    def __init__(self, result: GetProfileResult):

        assert isinstance(result, GetProfileResult)
        super().__init__(result)

    def build(self):
        self.sending_buffer = (
            f"\\pi\\profileid\\{self._result.user_profile.profile_id}"
            + f"\\nick\\{self._result.user_profile.nick}"
            + f"\\uniquenick\\{self._result.user_profile.unique_nick}"
            + f"\\email\\{self._result.user_profile.email}"
        )
        for key, value in self._result.user_profile.extra_infos.items():
            self.sending_buffer += f"\\{key}\\{value}"

        self.sending_buffer += (
            f"\\sig\\+{generate_random_string(10, StringType.HEX)}"
            + f"\\id\\{self._result.operation_id}\\final\\"
        )


class NewProfileResponse(ResponseBase):
    _request: NewProfileRequest
    _result: NewProfileResult

    def __init__(self, result: NewProfileResult):

        assert isinstance(result, NewProfileResult)
        super().__init__(result)

    def build(self):
        # fmt: off
        self.sending_buffer = f"\\npr\\\\profileid\\{self.sending_buffer}\\id\\{self._result.operation_id}\\final\\"
        # fmt: on


class RegisterCDKeyResposne(ResponseBase):
    def __init__(self) -> None:
        pass

    def build(self):
        self.sending_buffer = "\\rc\\\\final\\"


class RegisterNickResponse(ResponseBase):
    _result: RegisterNickResult

    def build(self) -> None:
        # fmt: off
        self.sending_buffer = f"\\rn\\\\id\\{self._result.operation_id}\\final\\" 
        # fmt: on
