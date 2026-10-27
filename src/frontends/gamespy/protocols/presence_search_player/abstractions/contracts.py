import frontends.gamespy.library.abstractions.contracts as lib
from frontends.gamespy.library.extentions.gamespy_utils import convert_to_key_value
from frontends.gamespy.protocols.presence_search_player.aggregates.enums import (
    CommandName,
)
from frontends.gamespy.protocols.presence_search_player.aggregates.exceptions import (
    GPParseException,
)


class RequestBase(lib.RequestBase):
    request_dict: dict[str, str]
    raw_request: str
    command_name: CommandName
    operation_id: int
    namespace_id: int

    @staticmethod
    def gs_parse(raw_request: str) -> "RequestBase":
        data = lib.RequestBase.gs_parse(raw_request)
        request_dict = convert_to_key_value(raw_request)
        data_dict = data.model_dump()
        data_dict["request_dict"] = request_dict
        data_dict["command_name"] = next(iter(request_dict))
        data_dict["operation_id"] = request_dict.get("id", 0)
        data_dict["namespace_id"] = request_dict.get("namespaceid", 0)
        data = RequestBase(**data_dict)
        return data


class ResultBase(lib.ResultBase):
    pass


class ResponseBase(lib.ResponseBase):
    _result: ResultBase
    sending_buffer: str
