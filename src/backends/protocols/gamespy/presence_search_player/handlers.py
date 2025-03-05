from typing import TYPE_CHECKING, cast
from backends.library.abstractions.handler_base import HandlerBase
from backends.library.database.pg_orm import PG_SESSION, Users, Profiles, SubProfiles
import backends.protocols.gamespy.presence_search_player.data as data
from backends.protocols.gamespy.presence_search_player.requests import *
from frontends.gamespy.library.exceptions.general import UniSpyException
from frontends.gamespy.protocols.presence_search_player.aggregates.exceptions import CheckException
from frontends.gamespy.protocols.presence_search_player.contracts.results import CheckResult, NewUserResult, NickResultData, NicksResult, OthersListData, OthersListResult, OthersResult, OthersResultData, SearchResult, SearchResultData, SearchUniqueResult, UniqueSearchResult, ValidResult


class CheckHandler(HandlerBase):
    """
    todo: whether need check the partner id, which means whether we need to check subprofiles
    """
    _request: CheckRequest

    async def _data_operate(self) -> None:
        if data.verify_email(self._request.email):
            raise CheckException("The email is not existed")
        if data.verify_email_and_password(self._request.email, self._request.password):
            raise CheckException("The password is incorrect")
        self._profile_id = data.get_profile_id(
            self._request.email, self._request.password, self._request.nick, self._request.partner_id)
        if self._profile_id is None:
            raise CheckException(f"No pid found with email{
                                 self._request.email}")

    async def _result_construct(self) -> None:
        assert self._profile_id is not None
        self._result = CheckResult(profile_id=self._profile_id)


class NewUserHandler(HandlerBase):
    _request: NewUserRequest

    async def _data_operate(self) -> None:

        # check if user exist
        self.user = data.get_user(self._request.email)
        if self.user is None:
            self._create_user()

        assert self.user != None
        assert isinstance(self.user.userid, int)

        self.profile = data.get_profile(self.user.userid, self._request.nick)
        if self.profile is None:
            self._create_profile()
        assert self.profile is not None
        assert isinstance(self.profile.profileid, int)
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


class OthersHandler(HandlerBase):
    _request: OthersRequest

    async def _data_operate(self) -> None:
        self._data: list = data.get_friend_info_list(
            self._request.profile_id, self._request.namespace_id, self._request.game_name)

    async def _result_construct(self) -> None:
        temp_list = []
        for item in self._data:
            temp_list.append(OthersResultData(
                profile_id=item[0], nick=item[1], uniquenick=item[2], lastname=item[3], firstname=item[4], user_id=item[5], email=item[6]
            ))
        self._result = OthersResult(data=temp_list)


class OthersListHandler(HandlerBase):
    _request: OthersListRequest

    async def _data_operate(self) -> None:
        self._data: list = data.get_matched_profile_info_list(
            self._request.profile_ids, self._request.namespace_id)

    async def _result_construct(self) -> None:
        temp = []
        for profile_id, uniquenick in self._data:
            temp.append(OthersListData(
                profile_id=profile_id, unique_nick=uniquenick))
        self._result = OthersListResult(data=temp)


# class PlayerMatchHandler(HandlerBase):
#     _request: playermatchrequest

class SearchHandler(HandlerBase):
    _request: SearchRequest

    async def _data_operate(self) -> None:
        if self._request.request_type == SearchType.NICK_SEARCH:
            assert self._request.nick
            self._data = data.get_matched_info_by_nick(self._request.nick)
        elif self._request.request_type == SearchType.NICK_EMAIL_SEARCH:
            assert self._request.email
            assert self._request.nick
            self._data = data.get_matched_info_by_nick_and_email(
                self._request.nick, self._request.email)
        elif self._request.request_type == SearchType.UNIQUENICK_NAMESPACEID_SEARCH:
            assert self._request.uniquenick
            self._data = data.get_matched_info_by_uniquenick_and_namespaceid(
                self._request.uniquenick, self._request.namespace_id)
        elif self._request.request_type == SearchType.EMAIL_SEARCH:
            assert self._request.email
            self._data = data.get_matched_info_by_email(self._request.email)
        else:
            raise UniSpyException("search type invalid")

    async def _result_construct(self) -> None:
        data = []
        for d in self._data:
            dd = SearchResultData(**d)
        data.append(dd)
        self._result = SearchResult(data=data)


class SearchUniqueHandler(HandlerBase):
    _request: SearchUniqueRequest

    async def _data_operate(self) -> None:
        self._data = data.get_matched_info_by_uniquenick_and_namespaceids(
            self._request.uniquenick, self._request.namespace_ids)

    async def _result_construct(self) -> None:
        data = []
        for d in self._data:
            dd = SearchResultData(**d)
            data.append(dd)
        self._result = SearchUniqueResult(data=data)


class UniqueSearchHandler(HandlerBase):
    _request: UniqueSearchRequest

    async def _data_operate(self) -> None:
        self._is_exist = data.is_uniquenick_exist(
            self._request.preferred_nick, self._request.namespace_id, self._request.game_name)

    async def _result_construct(self) -> None:
        self._result = UniqueSearchResult(is_uniquenick_exist=self._is_exist)


class ValidHandler(HandlerBase):
    _request: ValidRequest

    async def _data_operate(self) -> None:
        self._is_exist = data.is_email_exist(self._request.email)

    async def _result_construct(self) -> None:
        self._result = ValidResult(is_account_valid=self._is_exist)
