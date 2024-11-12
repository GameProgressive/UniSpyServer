from typing import TYPE_CHECKING, cast
from backends.library.abstractions.handler_base import HandlerBase
from backends.library.database.pg_orm import PG_SESSION, Users, Profiles, SubProfiles
import backends.protocols.gamespy.presence_search_player.data as data
from backends.protocols.gamespy.presence_search_player.requests import *
from servers.presence_search_player.src.aggregates.exceptions import CheckException
from servers.presence_search_player.src.contracts.results import CheckResult, NewUserResult, NickResultData, NicksResult


class CheckHandler(HandlerBase):
    _request: CheckRequest

    async def _data_operate(self) -> None:
        if data.verify_email(self._request.email):
            raise CheckException("The email is not existed")
        if data.verify_email_and_password(self._request.email, self._request.password):
            raise CheckException("The password is incorrect")
        profile_id = data.get_profile_id(
            self._request.email, self._request.password, self._request.nick, self._request.partner_id)

        self._result = CheckResult(profile_id=profile_id)


class NewUserHandler(HandlerBase):
    _request: NewUserRequest

    async def _data_operate(self) -> None:

        # check if user exist
        self.user = data.get_user(self._request.email)
        if self.user is None:
            self._create_user()

        if TYPE_CHECKING:
            assert self.user
            self.user.userid = cast(int, self.user.userid)

        self.profile = data.get_profile(self.user.userid, self._request.nick)
        if self.profile is None:
            self._create_profile()

        if TYPE_CHECKING:
            self.profile.profileid = cast(int, self.profile.profileid)

        self.subprofile = data.get_sub_profile(
            profile_id=self.profile.profileid, namespace_id=self._request.namespace_id, product_id=self._request.product_id)
        if self.subprofile is None:
            self._create_subprofile()

    async def _result_construct(self) -> None:
        assert self.user is not None
        assert isinstance(self.user.userid, int)
        assert self.profile is not None
        assert isinstance(self.profile.profileid, int)
        self._result = NewUserResult(
            user_id=self.user.userid, profile_id=self.profile.profileid)

    def _create_user(self) -> None:
        user_dict = {}
        for key, value in self._request.__dict__.items():
            if key in Users.__dict__:
                user_dict[key] = value
        self.user = Users(**user_dict)
        PG_SESSION.commit()

    def _create_profile(self) -> None:

        profile_dict = {}
        for key, value in self._request.__dict__.items():
            if key in Profiles.__dict__:
                profile_dict[key] = value
        self.profile = Profiles(**profile_dict)
        PG_SESSION.commit()

    def _create_subprofile(self) -> None:
        subprofile_dict = {}
        for key, value in self._request.__dict__.items():
            if key in SubProfiles.__dict__:
                subprofile_dict[key] = value
        self.subprofile = SubProfiles(**subprofile_dict)
        PG_SESSION.commit()


class NicksHandler(HandlerBase):
    _request: NicksRequest

    async def _data_operate(self) -> None:
        self.temp_list = data.get_nick_and_unique_nick_list(
            self._request.email, self._request.password, self._request.namespace_id)
        self.result_data = []
        for nick, unique in self.temp_list:
            self.result_data.append(
                NickResultData(nick=nick, uniquenick=unique))

    async def _result_construct(self) -> None:
        self._result = NicksResult(data=self.result_data)
