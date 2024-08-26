from pydantic import BaseModel

from servers.presence_search_player.src.abstractions.contracts import ResultBase


class CheckResult(ResultBase):
    profile_id: int


class NewUserResult(ResultBase):
    user_id: int
    profile_id: int


class NickResultData(BaseModel):
    nick: str
    uniquenick: str


class NicksResult(ResultBase):
    data: list[NickResultData]
    """ [
            (nick1, uniquenick1),
            (nick2, uniquenick2),
            (nick3, uniquenick3),
            ...
        ]
    """
    is_require_uniquenicks: bool = False


class OthersListData(BaseModel):
    profile_id: int
    unique_nick: str


class OthersListResult(ResultBase):
    data: list[OthersListData] = []
    """
    [
        (prifileid1,uniquenick1),
        (prifileid2,uniquenick2),
        (prifileid3,uniquenick3),
        ...
    ]
    """


class OthersResultData(BaseModel):
    profile_id: int
    nick: str
    uniquenick: str
    lastname: str
    firstname: str
    user_id: int
    email: str


class OthersResult(ResultBase):
    data: list[OthersResultData] = []


class SearchResultData(BaseModel):
    profile_id: int
    nick: str
    uniquenick: str
    email: str
    firstname: str
    lastname: str
    namespace_id: int


class SearchResult(ResultBase):
    result: list[SearchResultData]


class SearchUniqueResultData(BaseModel):
    profile_id: int
    nick: str
    uniquenick: str
    email: str
    firstname: str
    lastname: str
    namespace_id: int


class SearchUniqueResult(ResultBase):
    data: list[SearchUniqueResultData]


class UniqueSearchResult(ResultBase):
    is_uniquenick_exist: bool


class ValidResult(ResultBase):
    is_account_valid: bool = False
