

from backends.library.abstractions.handler_base import HandlerBase
from backends.protocols.gamespy.game_status.requests import *
import backends.protocols.gamespy.game_status.data as data
from servers.game_status.src.aggregations.exceptions import GSException
from servers.game_status.src.contracts.results import AuthPlayerResult, GetPlayerDataResult, GetProfileIdResult


class AuthGameHandler(HandlerBase):
    _request: AuthGameRequest


class AuthPlayerHandler(HandlerBase):
    _request: AuthPlayerRequest

    async def _data_operate(self):
        match self._request.auth_type:
            case AuthMethod.PARTNER_ID_AUTH:
                self.data = data.get_profile_id_by_token(token=self._request.auth_token)
            case AuthMethod.PROFILE_ID_AUTH:
                self.data = data.get_profile_id_by_profile_id(
                    profile_id=self._request.profile_id)
            case AuthMethod.CDKEY_AUTH:
                self.data = data.get_profile_id_by_cdkey(
                    cdkey=self._request.cdkey_hash, nick_name=self._request.nick)
            case _:
                raise GSException("Invalid auth type")

    async def _result_construct(self):
        self._result = AuthPlayerResult(profile_id=self.data)


class GetPlayerDataHandler(HandlerBase):
    _request: GetPlayerDataRequest

    async def _data_operate(self):
        self.data = data.get_player_data(
            self._request.profile_id,
            self._request.storage_type,
            self._request.data_index)

    async def _result_construct(self):
        self._result = GetPlayerDataResult(keyvalues=self.data)


class GetProfileIdHandler(HandlerBase):
    _request: GetProfileIdRequest

    async def _data_operate(self):
        self.data = data.get_profile_id_by_cdkey(
            cdkey=self._request.cdkey, nick_name=self._request.nick)

    async def _result_construct(self):
        self._result = GetProfileIdResult(profile_id=self.data)


class NewGameHandler(HandlerBase):
    _request: NewGameRequest
    """
    find game based on the session key, and create a space for the game data
    """

    async def _data_operate(self):
        self.data = data.create_new_game_data()


class SetPlayerDataHandler(HandlerBase):
    _request: SetPlayerDataRequest

    async def _data_operate(self):
        raise NotImplementedError()


class UpdateGameHandler(HandlerBase):
    _request: SetPlayerDataRequest

    async def _data_operate(self):
        raise NotImplementedError()
