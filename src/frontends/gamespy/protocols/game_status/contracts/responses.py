
from typing import final
from frontends.gamespy.library.abstractions.contracts import ResponseBase
from frontends.gamespy.protocols.game_status.contracts.results import AuthGameResult, AuthPlayerResult, GetPlayerDataResult, GetProfileIdResult, SetPlayerDataResult


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
        self.sending_buffer = f"\\pauthr\\{self._result.profile_id}\\lid\\{self._result.local_id}\\final\\"


@final
class GetPlayerDataResponse(ResponseBase):
    _result: GetPlayerDataResult

    def build(self) -> None:
        mod_time = int(self._result.modified.timestamp())
        self.sending_buffer = f"\\getpdr\\1\\pid\\{self._result.profile_id}\\lid\\{self._result.local_id}\\mod\\{mod_time}\\length\\{len(self._result.data)}\\data\\{self._result.data}\\final\\"


@final
class GetProfileIdResponse(ResponseBase):
    _result: GetProfileIdResult

    def build(self) -> None:
        # fmt: off 
        self.sending_buffer = f"\\getpidr\\{self._result.profile_id}\\lid\\{self._result.local_id}\\final\\"
        # fmt: on


@final
class SetPlayerDataResponse(ResponseBase):
    _result: SetPlayerDataResult

    def build(self) -> None:
        # \\setpdr\\1\\lid\\2\\pid\\100000\\mod\\12345
        mod_time = int(self._result.modified.timestamp())
        self.sending_buffer = f"\\setpdr\\1\\pid\\{self._result.profile_id}\\lid\\{self._result.local_id}\\mod\\{mod_time}\\final\\"
