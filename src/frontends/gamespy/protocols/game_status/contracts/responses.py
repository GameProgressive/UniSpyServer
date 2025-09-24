
from typing import final
from frontends.gamespy.library.abstractions.contracts import ResponseBase
from frontends.gamespy.protocols.game_status.contracts.results import AuthGameResult, AuthPlayerResult, GetPlayerDataResult, GetProfileIdResult


@final
class AuthGameResponse(ResponseBase):
    _result: AuthGameResult

    def build(self) -> None:
        # fmt: off 
        self.sending_buffer = f"\\sesskey\\{self._result.session_key}\\lid\\{self._result.local_id}\\final\\"
        # fmt: on


@final
class AuthPlayerResponse(ResponseBase):
    _result: AuthPlayerResult

    def build(self) -> None:
        # fmt: off 
        self.sending_buffer = f"\\pauthr\\{self._result.profile_id}\\lid\\{self._result.local_id}\\final\\"
        # fmt: on


@final
class GetPlayerDataResponse(ResponseBase):
    _result: GetPlayerDataResult

    def build(self) -> None:
        # fmt: off 
        self.sending_buffer = f"\\getpdr\\1\\pid\\{self._result.profile_id}\\lid\\{self._result.local_id}\\mod\\1234\\length\\5\\data\\mydata\\final\\"
        # fmt: on


@final
class GetProfileIdResponse(ResponseBase):
    _result: GetProfileIdResult

    def build(self) -> None:
        # fmt: off 
        self.sending_buffer = f"\\getpidr\\{self._result.profile_id}\\lid\\{self._result.local_id}\\final\\"
        # fmt: on


@final
class SetPlayerDataResponse(ResponseBase):
    _result: GetPlayerDataResult

    def build(self) -> None:
        raise NotImplementedError()
