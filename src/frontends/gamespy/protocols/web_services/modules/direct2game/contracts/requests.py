from frontends.gamespy.library.network.http_data import HttpData
from frontends.gamespy.protocols.web_services.abstractions.contracts import RequestBase
from frontends.gamespy.protocols.web_services.aggregations.exceptions import (
    WebException,
)
from frontends.gamespy.protocols.web_services.modules.direct2game.abstractions.contracts import (
    RequestBase,
)


class GetPurchaseHistoryRequest(RequestBase):
    game_id: int
    access_token: str
    proof: str
    certificate: str

    @staticmethod
    def gs_parse(raw_request: HttpData) -> "GetPurchaseHistoryRequest":
        data = RequestBase.gs_parse(raw_request)
        data_dict = data.model_dump()
        data_dict["game_id"] = data.int_mapping["gameid"]
        data_dict["proof"] = data.str_mapping["proof"]
        data_dict["access_token"] = data.str_mapping["access_token"]
        data_dict["certificate"] = data.str_mapping["certificate"]

        data = GetPurchaseHistoryRequest(**data_dict)
        return data


class GetStoreAvailabilityRequest(RequestBase):
    game_id: int
    version: int
    region: str
    access_token: str

    @staticmethod
    def gs_parse(raw_request: HttpData) -> "GetStoreAvailabilityRequest":
        data = RequestBase.gs_parse(raw_request)
        data_dict = data.model_dump()
        data_dict["game_id"] = data.int_mapping["gameid"]
        data_dict["version"] = data.int_mapping["version"]
        data_dict["region"] = data.str_mapping["region"]
        data_dict["access_token"] = data.str_mapping["accesstoken"]
        data = GetStoreAvailabilityRequest(**data_dict)
        return data
