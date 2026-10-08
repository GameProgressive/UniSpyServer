import backends.library.abstractions.contracts as lib
from frontends.gamespy.protocols.presence_search_player.aggregates.enums import (
    SearchType,
)
import frontends.gamespy.protocols.presence_search_player.contracts.requests as fnt


class RequestBase(lib.RequestBase):
    data: fnt.RequestBase


# general

# we just need to recreate the requests and just put the property inside it. The result we can use the results inside servers.


class CheckRequest(RequestBase):
    data: fnt.CheckRequest



class NewUserRequest(RequestBase):
    data: fnt.NewUserRequest



class NicksRequest(RequestBase):
    data: fnt.NicksRequest



class OthersListRequest(RequestBase):
    data: fnt.OthersListRequest



class OthersRequest(RequestBase):
    data: fnt.OthersRequest



class SearchRequest(RequestBase):
    data: fnt.SearchRequest



class SearchUniqueRequest(RequestBase):
    data: fnt.SearchUniqueRequest



class UniqueSearchRequest(RequestBase):
    data: fnt.UniqueSearchRequest



class ValidRequest(RequestBase):
    data: fnt.ValidRequest

