from library.src.database.pg_orm import Profiles, SubProfiles, Users
from servers.presence_connection_manager.src.abstractions.contracts import ResultBase


class CheckResult(ResultBase):
    profile_id: int


class NewUserResult(ResultBase):
    user: Users
    profile: Profiles
    subprofiles: SubProfiles


class NickResultModel:
    nick: str
    uniquenick: str


class NicksResult(ResultBase):
    data: list[NickResultModel] = []
    """ [
            (nick1, uniquenick1),
            (nick2, uniquenick2),
            (nick3, uniquenick3),
            ...
        ]
    """
    is_require_uniquenicks: bool = False


class OthersListModel:
    profile_id: int
    unique_nick: str


class OthersListResult(ResultBase):
    data: list[OthersListModel] = []
    """
    [
        (prifileid1,uniquenick1),
        (prifileid2,uniquenick2),
        (prifileid3,uniquenick3),
        ...
    ]
    """


class OthersResultModel:
    profile_id: int
    nick: str
    uniquenick: str
    lastname: str
    firstname: str
    user_id: int
    email: str


class OthersResult(ResultBase):
    data: list[OthersResultModel] = []


class SearchResultDataModel:
    profile_id: int
    nick: str
    uniquenick: str
    email: str
    firstname: str
    lastname: str
    namespace_id: int


class SearchResult(ResultBase):
    result: list[SearchResultDataModel] = []


class SearchUniqueResultModel:
    profile_id: int
    nick: str
    uniquenick: str
    email: str
    firstname: str
    lastname: str
    namespace_id: int


class SearchUniqueResult(ResultBase):
    data: list[SearchUniqueResultModel] = []


class UniqueSearchResult(ResultBase):
    is_uniquenick_exist: bool


class ValidResult(ResultBase):
    is_account_valid: bool = False
