from backends.library.abstractions.contracts import DataResponse
from frontends.gamespy.protocols.game_status.contracts.results import AuthGameResult, AuthPlayerResult, GetPlayerDataResult, GetProfileIdResult


class AuthGameResponse(DataResponse):
    result: AuthGameResult


class AuthPlayerResponse(DataResponse):
    result: AuthPlayerResult


class GetPlayerDataResponse(DataResponse):
    result: GetPlayerDataResult


class GetProfileIdResponse(DataResponse):
    result: GetProfileIdResult
