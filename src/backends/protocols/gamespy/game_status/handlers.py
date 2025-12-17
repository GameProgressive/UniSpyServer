from datetime import datetime
from backends.library.abstractions.contracts import OKResponse
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
from backends.protocols.gamespy.game_status.response import AuthGameResponse, AuthPlayerResponse, GetPlayerDataResponse, GetProfileIdResponse, SetPlayerDataResponse
from frontends.gamespy.protocols.game_status.aggregations.enums import AuthMethod
from frontends.gamespy.protocols.game_status.aggregations.exceptions import GSException
from frontends.gamespy.protocols.game_status.contracts.results import (
    AuthGameResult,
    AuthPlayerResult,
    GetPlayerDataResult,
    GetProfileIdResult,
    SetPlayerDataResult,

)
import base64


class AuthGameHandler(HandlerBase):
    _request: AuthGameRequest
    response: AuthGameResponse

    def _data_operate(self) -> None:
        # generate session key
        # todo: check whether need to store on database
        self.session_key = "11111"

    def _result_construct(self) -> None:
        self._result = AuthGameResult(
            session_key=self.session_key,
            local_id=self._request.local_id,
            game_name=self._request.game_name)


class AuthPlayerHandler(HandlerBase):
    _request: AuthPlayerRequest
    response: AuthPlayerResponse

    def _data_operate(self):
        match self._request.auth_type:
            case AuthMethod.PARTNER_ID_AUTH:
                if self._request.auth_token is None:
                    raise GSException("auth tocken is missing")
                self.data = data.get_profile_id_by_token(
                    token=self._request.auth_token,
                    session=self._session)
            case AuthMethod.PROFILE_ID_AUTH:
                if self._request.profile_id is None:
                    raise GSException("profileid is missing")
                self.data = data.get_profile_id_by_profile_id(
                    profile_id=self._request.profile_id,
                    session=self._session
                )
            case AuthMethod.CDKEY_AUTH:
                if self._request.cdkey_hash is None:
                    raise GSException("cdkey hash is required")
                if self._request.nick is None:
                    raise GSException("nick is missing")
                self.data = data.get_profile_id_by_cdkey(
                    cdkey=self._request.cdkey_hash,
                    nick_name=self._request.nick,
                    session=self._session
                )
            case _:
                raise GSException("Invalid auth type")

    def _result_construct(self):
        self._result = AuthPlayerResult(
            profile_id=self.data, local_id=self._request.local_id)


class GetPlayerDataHandler(HandlerBase):
    _request: GetPlayerDataRequest
    response: GetPlayerDataResponse

    def _data_operate(self):
        self.pd = data.get_player_data(
            self._request.profile_id,
            self._request.storage_type,
            self._request.data_index,
            self._session
        )
        if self.pd is None:
            raise GSException("No records found in database")

    def _result_construct(self):
        assert self.pd is not None
        assert isinstance(self.pd.data, str)
        assert isinstance(self.pd.update_time, datetime)

        if str(self.pd.data).endswith("=="):
            data_formated = base64.b64decode(self.pd.data).decode()
        else:
            data_formated = self.pd.data

        self._result = GetPlayerDataResult(
            local_id=self._request.local_id,
            profile_id=self._request.profile_id,
            data=data_formated,
            modified=self.pd.update_time)


class GetProfileIdHandler(HandlerBase):
    _request: GetProfileIdRequest
    response: GetProfileIdResponse

    def _data_operate(self):
        self.data = data.get_profile_id_by_cdkey(
            cdkey=self._request.key_hash,
            nick_name=self._request.nick,
            session=self._session
        )

    def _result_construct(self):
        self._result = GetProfileIdResult(
            profile_id=self.data,
            local_id=self._request.local_id)


class NewGameHandler(HandlerBase):
    """
    find game based on the session key, and create a space for the game data
    """
    _request: NewGameRequest
    response: OKResponse

    def _data_operate(self):
        self.data = data.create_new_game_data()


class SetPlayerDataHandler(HandlerBase):
    _request: SetPlayerDataRequest
    _result: SetPlayerDataResult
    response: SetPlayerDataResponse

    def _data_operate(self):
        if not self._request.is_key_value:
            data_formated = base64.b64encode(
                self._request.data.encode()).decode()
        else:
            data_formated = self._request.data

        pd = data.get_player_data(self._request.profile_id,
                                  self._request.storage_type,
                                  self._request.data_index,
                                  self._session)
        if pd is not None:
            data.update_player_data(pd,
                                    data_formated,
                                    self._session)
        else:
            data.create_player_data(
                self._request.profile_id,
                self._request.storage_type,
                self._request.data_index,
                self._request.data,
                self._session
            )

    def _result_construct(self) -> None:
        self._result = SetPlayerDataResult(
            local_id=self._request.local_id,
            profile_id=self._request.profile_id,
            modified=datetime.now()
        )


class UpdateGameHandler(HandlerBase):
    _request: SetPlayerDataRequest
    response: OKResponse

    def _data_operate(self):
        raise NotImplementedError()
