from backends.library.abstractions.handler_base import HandlerBase
import backends.protocols.gamespy.game_status.data as data
from backends.protocols.gamespy.game_status.requests import (
    AuthGameRequest,
    AuthPlayerRequest,
    GetPlayerDataRequest,
    GetProfileIdRequest,
    NewGameRequest,
    SetPlayerDataRequest,
)
from frontends.gamespy.protocols.game_status.aggregations.enums import AuthMethod
from frontends.gamespy.protocols.game_status.aggregations.exceptions import GSException
from frontends.gamespy.protocols.game_status.contracts.results import (
    AuthGameResult,
    AuthPlayerResult,
    GetPlayerDataResult,
    GetProfileIdResult,
)


class AuthGameHandler(HandlerBase):
    _request: AuthGameRequest

    def _data_operate(self) -> None:
        # generate session key
        # todo: check whether need to store on database
        self.session_key = "11111"

    def _result_construct(self) -> None:
        self._result = AuthGameResult(
            session_key=self.session_key, local_id=self._request.local_id, game_name=self._request.game_name)


class AuthPlayerHandler(HandlerBase):
    _request: AuthPlayerRequest

    def _data_operate(self):
        match self._request.auth_type:
            case AuthMethod.PARTNER_ID_AUTH:
                self.data = data.get_profile_id_by_token(
                    token=self._request.auth_token)
            case AuthMethod.PROFILE_ID_AUTH:
                self.data = data.get_profile_id_by_profile_id(
                    profile_id=self._request.profile_id
                )
            case AuthMethod.CDKEY_AUTH:
                self.data = data.get_profile_id_by_cdkey(
                    cdkey=self._request.cdkey_hash, nick_name=self._request.nick
                )
            case _:
                raise GSException("Invalid auth type")

    def _result_construct(self):
        self._result = AuthPlayerResult(
            profile_id=self.data, local_id=self._request.local_id)


class GetPlayerDataHandler(HandlerBase):
    _request: GetPlayerDataRequest

    def _data_operate(self):
        self.data = data.get_player_data(
            self._request.profile_id,
            self._request.storage_type,
            self._request.data_index,
        )

    def _result_construct(self):
        self._result = GetPlayerDataResult(
            keyvalues=self.data,
            local_id=self._request.local_id,
            profile_id=self._request.profile_id)


class GetProfileIdHandler(HandlerBase):
    _request: GetProfileIdRequest

    def _data_operate(self):
        self.data = data.get_profile_id_by_cdkey(
            cdkey=self._request.cdkey, nick_name=self._request.nick
        )

    def _result_construct(self):
        self._result = GetProfileIdResult(
            profile_id=self.data,
            local_id=self._request.local_id)


class NewGameHandler(HandlerBase):
    _request: NewGameRequest
    """
    find game based on the session key, and create a space for the game data
    """

    def _data_operate(self):
        self.data = data.create_new_game_data()


class SetPlayerDataHandler(HandlerBase):
    _request: SetPlayerDataRequest

    def _data_operate(self):
        raise NotImplementedError()


class UpdateGameHandler(HandlerBase):
    _request: SetPlayerDataRequest

    def _data_operate(self):
        raise NotImplementedError()
