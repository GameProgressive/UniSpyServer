from library.database.pg_orm import Profiles
from servers.chat.contracts.requests.general import RegisterNickRequest
from servers.presence_connection_manager.abstractions.contracts import RequestBase
from servers.presence_connection_manager.abstractions.handler import CmdHandlerBase
from servers.presence_connection_manager.applications.client import Client
from servers.presence_connection_manager.applications.data import add_nick_name, get_profile_info_list, update_block_info_list, update_profile_info, update_subprofile_info, update_unique_nick
from servers.presence_connection_manager.contracts.requests.profile import (
    AddBlockRequest,
    GetProfileRequest,
    NewProfileRequest,
    RegisterCDKeyRequest,
    UpdateProfileRequest,
)
from servers.presence_connection_manager.contracts.responses.profile import (
    GetProfileResponse,
    NewProfileResponse,
    RegisterNickResponse,
)
from servers.presence_connection_manager.contracts.results.profile import NewProfileResult


class AddBlockHandler(CmdHandlerBase):
    _request: AddBlockRequest

    def __init__(self, client: Client, request: AddBlockRequest) -> None:
        assert isinstance(request, AddBlockRequest)
        super().__init__(client, request)

    def _data_operation(self) -> None:
        update_block_info_list(self._request.taget_id, self._client.info.profile_id, self._client.info.namespace_id)


class GetProfileHandler(CmdHandlerBase):
    _request: GetProfileRequest
    _result: GetProfileResponse

    def __init__(self, client: Client, request: GetProfileRequest) -> None:
        assert isinstance(request, GetProfileRequest)
        super().__init__(client, request)

    def data_operation(self) -> None:
        self._result.user_profile = get_profile_info_list(self._request.profile_id, self._client.info.namespace_id)
        if(self._result.user_profile is None):
            # TODO
            # raise right exception
            raise NotImplementedError()

    def _response_construct(self) -> None:
        self._response = GetProfileResponse(self._request, self._result)


class NewProfileHandler(CmdHandlerBase):
    _request: NewProfileRequest
    _result: NewProfileResult

    def __init__(self, client: Client, request: NewProfileRequest) -> None:
        assert isinstance(request, NewProfileRequest)
        super().__init__(client, request)

    def _data_operation(self) -> None:
        if(self._request.is_replace_nick_name):
            update_unique_nick(self._client.info.profile_id, self._request.new_nick)
        else:
            add_nick_name(self._client.info.profile_id, self._request.new_nick, self._request.new_nick)

    def _response_construct(self) -> None:
        self._response = NewProfileResponse(self._request, self._result)


class RegisterCDKeyHandler(CmdHandlerBase):
    _request: RegisterCDKeyRequest

    def __init__(self, client: Client, request: RegisterCDKeyRequest) -> None:
        assert isinstance(request, RegisterCDKeyRequest)
        super().__init__(client, request)

    def _data_operation(self) -> None:
        self._request.cdkey_enc = self._request.cdkey_enc
        # TODO
        # update subprofile info
        update_subprofile_info(self._client.info.sub_profile_id)

class RegisterNickHandler(CmdHandlerBase):

    _request: RegisterNickRequest

    def __init__(self, client: Client, request: RegisterNickRequest) -> None:
        assert isinstance(request, RegisterNickRequest)
        super().__init__(client, request)

    def data_operation(self) -> None:
        update_unique_nick(self._client.info.profile_id, self._request.unique_nick)

    def _response_construct(self) -> None:
        self._response = RegisterNickResponse(self._request, self._result)


class RemoveBlockHandler(CmdHandlerBase):
    def __init__(self, client: Client, request: RequestBase) -> None:
        raise NotImplementedError()
        super().__init__(client, request)


class UpdateProfileHandler(CmdHandlerBase):
    _request: UpdateProfileRequest

    def __init__(self, client: Client, request: UpdateProfileRequest) -> None:
        assert isinstance(request, UpdateProfileRequest)
        super().__init__(client, request)

    def _data_operation(self) -> None:
        profile = self._client.info
        if(self._request.has_public_mask_flag):
            profile.public_mask = self._request.public_mask
        if(self._request.has_first_name_flag):
            profile.first_name = self._request.first_name
        if(self._request.has_last_name_flag):
            profile.last_name = self._request.last_name
        if(self._request.has_icq_flag):
            profile.icq = self._request.icq_uin
        if(self._request.has_home_page_flag):
            profile.home_page = self._request.home_page
        if(self._request.has_birthday_flag):
            profile.birth_day = self._request.birth_day
            profile.birth_month = self._request.birth_month
            profile.birth_year = self._request.birth_year
        if(self._request.has_sex_flag):
            profile.sex = self._request.sex
        if(self._request.has_zip_code):
            profile.zip_code = self._request.zip_code
        if(self._request.has_country_flag):
            profile.country_code = self._request.country_code

        update_profile_info(Profiles(profile))


class UpdateUserInfoHandler(CmdHandlerBase):
    raise NotImplementedError()
