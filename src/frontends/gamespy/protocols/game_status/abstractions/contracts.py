import frontends.gamespy.library.abstractions.contracts as lib
from frontends.gamespy.library.extentions.gamespy_utils import convert_to_key_value
from frontends.gamespy.protocols.game_status.aggregations.enums import CommandName
from frontends.gamespy.protocols.game_status.aggregations.exceptions import GSException


class RequestBase(lib.RequestBase):
    command_name: CommandName
    raw_request: str
    local_id: int | None
    request_dict: dict[str, str]

    @staticmethod
    def gs_parse(raw_request: str) -> "RequestBase":
        assert isinstance(raw_request, str)
        data = lib.RequestBase.gs_parse(raw_request)
        data_dict = data.model_dump()
        request_dict = convert_to_key_value(raw_request)
        data_dict["command_name"] = next(iter(request_dict))
        data_dict["request_dict"] = request_dict
        data_dict["local_id"] = request_dict.get("lid") or request_dict.get("id")
        data = RequestBase(**data_dict)
        return data

    @staticmethod
    def convert_game_data_to_key_values(game_data: str):
        assert isinstance(game_data, str)
        game_data = game_data.replace("\u0001", "\\")
        convert_to_key_value(game_data)


class ResultBase(lib.ResultBase):
    local_id: int
    pass


class ResponseBase(lib.ResponseBase):
    _request: RequestBase
    _result: ResultBase
    sending_buffer: str
