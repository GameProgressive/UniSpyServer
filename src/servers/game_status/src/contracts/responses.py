
from typing import final
from library.src.abstractions.contracts import ResponseBase
from servers.game_status.src.contracts.requests import AuthGameRequest, AuthPlayerRequest, GetPlayerDataRequest, GetProfileIdRequest, SetPlayerDataRequest
from servers.game_status.src.contracts.results import AuthGameResult, AuthPlayerResult, GetPlayerDataResult, GetProfileIdResult


@final
class AuthGameResponse(ResponseBase):
    _request: AuthGameRequest
    _result: AuthGameResult

    def build(self) -> None:
        # fmt: off 
        self.sending_buffer = f"\\sesskey\\{self._result.session_key}\\lid\\{self._request.local_id}\\final\\"
        # fmt: on


@final
class AuthPlayerResponse(ResponseBase):
    _request: AuthPlayerRequest
    _result: AuthPlayerResult

    def build(self) -> None:
        # fmt: off 
        self.sending_buffer = f"\\pauthr\\{self._result.profile_id}\\lid\\{self._request.local_id}\\final\\"
        # fmt: on


@final
class GetPlayerDataResponse(ResponseBase):
    _request: GetPlayerDataRequest
    _result: GetPlayerDataResult

    def build(self) -> None:
        # fmt: off 
        self.sending_buffer = f"\\getpdr\\1\\pid\\{self._request.profile_id}\\lid\\{self._request.local_id}\\mod\\1234\\length\\5\\data\\mydata\\final\\"
        # fmt: on


@final
class GetProfileIdResponse(ResponseBase):
    _request: GetProfileIdRequest
    _result: GetProfileIdResult

    def build(self) -> None:
        # fmt: off 
        self.sending_buffer = f"\\getpidr\\{self._result.profile_id}\\lid\\{self._request.local_id}\\final\\"
        # fmt: on


@final
class SetPlayerDataResponse(ResponseBase):
    _request: SetPlayerDataRequest
    _result: GetPlayerDataResult

    def build(self) -> None:
        raise NotImplementedError()
