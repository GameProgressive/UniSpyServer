from frontends.gamespy.protocols.query_report.aggregates.exceptions import QRException
from frontends.gamespy.protocols.server_browser.v1.abstractions.contracts import (
    RequestBase,
)
from frontends.gamespy.protocols.server_browser.v1.aggregations.enums import Modifier


class ValidateRequest(RequestBase):
    game_name: str
    game_version: str
    validate: str
    enctype: str
    query_id: str
    version: str

    @staticmethod
    def gs_parse(raw_request: str) -> "ValidateRequest":
        data = RequestBase.gs_parse(raw_request)
        data_dict = data.model_dump()
        data_dict["game_name"] = data.request_dict["gamename"]
        data_dict["version"] = data.request_dict["gamever"]
        data_dict["validate"] = data.request_dict["validate"]
        data_dict["enctype"] = data.request_dict["enctype"]
        data_dict["query_id"] = data.request_dict["queryid"]
        data = ValidateRequest(**data_dict)
        return data


class ServerListRequest(RequestBase):
    modifier: Modifier
    filter: str | None
    game_name: str

    @staticmethod
    def gs_parse(raw_request: str) -> "ServerListRequest":
        data = RequestBase.gs_parse(raw_request)
        data_dict = data.model_dump()
        data_dict["modifier"] = Modifier(data.request_dict["list"])
        data_dict["filter"] = data.request_dict.get("where")
        data_dict["game_name"] = data.request_dict["gamename"]
        data = ServerListRequest(**data_dict)
        return data
