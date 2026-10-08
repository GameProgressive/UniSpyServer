from datetime import datetime, timezone

from backends.library.abstractions.handler_base import HandlerBase
from backends.library.database.pg_orm import Profiles, SubProfiles, Users
from backends.protocols.gamespy.presence_search_player import data
from backends.protocols.gamespy.presence_search_player.requests import (
    CheckRequest,
    NewUserRequest,
    NicksRequest,
    OthersListRequest,
    OthersRequest,
    SearchRequest,
    SearchUniqueRequest,
    UniqueSearchRequest,
    ValidRequest,
)
from backends.protocols.gamespy.presence_search_player.responses import (
    CheckResponse,
    NewUserResponse,
    NicksResponse,
    OthersListResponse,
    OthersResponse,
    SearchResponse,
    SearchUniqueResponse,
    UniqueSearchResponse,
    ValidResponse,
)
from frontends.gamespy.protocols.presence_search_player.aggregates.enums import (
    SearchType,
)
from frontends.gamespy.protocols.presence_search_player.aggregates.exceptions import (
    CheckException,
    GPException,
)
from frontends.gamespy.protocols.presence_search_player.contracts.results import (
    CheckResult,
    NewUserResult,
    NickResultData,
    NicksResult,
    OthersListData,
    OthersListResult,
    OthersResult,
    OthersResultData,
    SearchResult,
    SearchResultData,
    SearchUniqueResult,
    UniqueSearchResult,
    ValidResult,
)


class CheckHandler(HandlerBase):
    """
    todo: whether need check the partner id, which means whether we need to check subprofiles
    """

    _request: CheckRequest
    _result: CheckResult
    response: CheckResponse

    def _data_operate(self) -> None:
        if not data.verify_email(self._request.data.email, self._session):
            raise CheckException("The email is not existed")
        if not data.verify_email_and_password(
            self._request.data.email, self._request.data.password, self._session
        ):
            raise CheckException("The password is incorrect")
        self._profile_id = data.get_profile_id(
            self._request.data.email,
            self._request.data.password,
            self._request.data.nick,
            self._request.data.partner_id,
            self._session,
        )
        if self._profile_id is None:
            raise CheckException(f"No pid found with email{self._request.data.email}")

    def _result_construct(self) -> None:
        assert self._profile_id is not None
        self._result = CheckResult(profile_id=self._profile_id)


class NewUserHandler(HandlerBase):
    _request: NewUserRequest
    _result: NewUserResult
    response: NewUserResponse

    def _data_operate(self) -> None:
        # check if user exist
        self.user = data.get_user(self._request.data.email, self._session)
        if self.user is None:
            self._create_user()

        assert self.user
        assert isinstance(self.user.id, int)

        self.profile = data.get_profile(
            self.user.id, self._request.data.nick, self._session
        )
        if self.profile is None:
            self._create_profile()
        assert self.profile is not None
        assert isinstance(self.profile.id, int)
        self.subprofile = data.get_sub_profile(
            profile_id=self.profile.id,
            namespace_id=self._request.data.namespace_id,
            product_id=self._request.data.product_id,
            session=self._session,
        )
        if self.subprofile is None:
            self._create_subprofile()

    def _result_construct(self) -> None:
        assert self.user is not None
        assert isinstance(self.user.id, int)
        assert self.profile is not None
        assert isinstance(self.profile.id, int)
        self._result = NewUserResult(
            user_id=self.user.id, profile_id=self.profile.id
        )

    def _create_user(self) -> None:
        self.user = Users(
            email=self._request.data.email,
            password=self._request.data.password,
            lastip=self._request.info.client_ip,
            lastonline=datetime.now(),
            createddate=datetime.now(),
        )
        self._session.add(self.user)
        self._session.commit()

    def _create_profile(self) -> None:
        profile_dict = {}
        for key, value in self._request.data.__dict__.items():
            if key in Profiles.__dict__:
                profile_dict[key] = value

        assert self.user is not None
        assert isinstance(self.user.id, int)
        profile_dict["userid"] = self.user.id
        self.profile = Profiles(
            userid=self.user.id,
            nick=self._request.data.nick,
        )
        self._session.add(self.profile)
        self._session.commit()

    def _create_subprofile(self) -> None:
        assert self.profile
        self.subprofile = SubProfiles(
            profileid=self.profile.id,
            uniquenick=self._request.data.uniquenick,
            namespaceid=self._request.data.namespace_id,
            partnerid=self._request.data.partner_id,
            productid=self._request.data.product_id,
            gamename=self._request.data.game_name,
            cdkeyenc=self._request.data.cd_key,
        )
        self._session.add(self.subprofile)
        self._session.commit()


class NicksHandler(HandlerBase):
    _request: NicksRequest
    _result: NicksResult
    response: NicksResponse

    def _data_operate(self) -> None:
        self.temp_list = data.get_nick_and_unique_nick_list(
            self._request.data.email,
            self._request.data.password,
            self._request.data.namespace_id,
            self._session,
        )
        self.result_data = []
        for nick, unique in self.temp_list:
            self.result_data.append(NickResultData(nick=nick, uniquenick=unique))

    def _result_construct(self) -> None:
        self._result = NicksResult(data=self.result_data)


class OthersHandler(HandlerBase):
    _request: OthersRequest
    _result: OthersResult
    response: OthersResponse

    def _data_operate(self) -> None:
        self._data: list = data.get_friend_info_list(
            self._request.data.profile_id,
            self._request.data.namespace_id,
            self._request.data.game_name,
            self._session,
        )

    def _result_construct(self) -> None:
        temp_list = []
        for item in self._data:
            temp_list.append(
                OthersResultData(
                    profile_id=item[0],
                    nick=item[1],
                    uniquenick=item[2],
                    lastname=item[3],
                    firstname=item[4],
                    user_id=item[5],
                    email=item[6],
                )
            )
        self._result = OthersResult(data=temp_list)


class OthersListHandler(HandlerBase):
    _request: OthersListRequest
    result: OthersListResult
    response: OthersListResponse

    def _data_operate(self) -> None:
        self._data: list = data.get_matched_profile_info_list(
            self._request.data.profile_ids, self._request.data.namespace_id, self._session
        )

    def _result_construct(self) -> None:
        temp = []
        for profile_id, uniquenick in self._data:
            temp.append(OthersListData(profile_id=profile_id, unique_nick=uniquenick))
        self._result = OthersListResult(data=temp)


# class PlayerMatchHandler(HandlerBase):
#     _request: playermatchrequest


class SearchHandler(HandlerBase):
    _request: SearchRequest
    _result: SearchResult
    response: SearchResponse

    def _data_operate(self) -> None:
        if self._request.data.request_type == SearchType.NICK_SEARCH:
            assert self._request.data.nick
            self._data = data.get_matched_info_by_nick(
                self._request.data.nick, self._session
            )
        elif self._request.data.request_type == SearchType.NICK_EMAIL_SEARCH:
            assert self._request.data.email
            assert self._request.data.nick
            self._data = data.get_matched_info_by_nick_and_email(
                self._request.data.nick, self._request.data.email, self._session
            )
        elif self._request.data.request_type == SearchType.UNIQUENICK_NAMESPACEID_SEARCH:
            assert self._request.data.uniquenick
            self._data = data.get_matched_info_by_uniquenick_and_namespaceid(
                self._request.data.uniquenick, self._request.data.namespace_id, self._session
            )
        elif self._request.data.request_type == SearchType.EMAIL_SEARCH:
            assert self._request.data.email
            self._data = data.get_matched_info_by_email(
                self._request.data.email, self._session
            )
        else:
            raise GPException("search type invalid")

    def _result_construct(self) -> None:
        data = []
        for d in self._data:
            dd = SearchResultData(**d)
            data.append(dd)
        self._result = SearchResult(data=data)


class SearchUniqueHandler(HandlerBase):
    _request: SearchUniqueRequest
    _result: SearchUniqueResult
    response: SearchUniqueResponse

    def _data_operate(self) -> None:
        self._data = data.get_matched_info_by_uniquenick_and_namespaceids(
            self._request.data.uniquenick, self._request.data.namespace_ids, self._session
        )

    def _result_construct(self) -> None:
        self._result = SearchUniqueResult(data=self._data)


class UniqueSearchHandler(HandlerBase):
    _request: UniqueSearchRequest
    _result: UniqueSearchResult
    response: UniqueSearchResponse

    def _data_operate(self) -> None:
        self._is_exist = data.is_uniquenick_exist(
            self._request.data.preferred_nick,
            self._request.data.namespace_id,
            self._request.data.game_name,
            self._session,
        )

    def _result_construct(self) -> None:
        self._result = UniqueSearchResult(
            is_uniquenick_exist=self._is_exist,
            preferred_nick=self._request.data.preferred_nick,
        )


class ValidHandler(HandlerBase):
    _request: ValidRequest
    _result: ValidResult
    response: ValidResponse

    def _data_operate(self) -> None:
        self._is_exist = data.is_email_exist(self._request.data.email, self._session)

    def _result_construct(self) -> None:
        self._result = ValidResult(is_account_valid=self._is_exist)
