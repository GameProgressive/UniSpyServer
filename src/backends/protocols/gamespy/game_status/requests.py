import backends.library.abstractions.contracts as lib
import frontends.gamespy.protocols.game_status.contracts.requests as fnt


class RequestBase(lib.RequestBase):
    data: fnt.RequestBase


class AuthGameRequest(RequestBase):
    data: fnt.AuthGameRequest


class AuthPlayerRequest(RequestBase):
    data: fnt.AuthPlayerRequest


class GetPlayerDataRequest(RequestBase):
    data: fnt.GetPlayerDataRequest


class GetProfileIdRequest(RequestBase):
    data: fnt.GetProfileIdRequest


class NewGameRequest(RequestBase):
    data: fnt.NewGameRequest


class SetPlayerDataRequest(RequestBase):
    data: fnt.SetPlayerDataRequest


class UpdateGameRequest(RequestBase):
    data: fnt.UpdateGameRequest
