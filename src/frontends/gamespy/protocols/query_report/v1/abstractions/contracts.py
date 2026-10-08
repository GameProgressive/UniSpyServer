import frontends.gamespy.library.abstractions.contracts as lib
from frontends.gamespy.library.extentions.gamespy_utils import convert_to_key_value
from frontends.gamespy.protocols.query_report.v1.aggregates.enums import CommandName


class RequestBase(lib.RequestBase):
    raw_request: CommandName
    request_dict: dict[str, str]

    @staticmethod
    def gs_parse(raw_request: str) -> "RequestBase":
        data = lib.RequestBase.gs_parse(raw_request)
        data_dict = data.model_dump()
        data_dict["request_dict"] = convert_to_key_value(raw_request)
        data_dict["command_name"] = next(iter(data_dict["request_dict"]))
        data = RequestBase(**data_dict)
        return data


class ResultBase(lib.ResultBase):
    pass


class ResponseBase(lib.ResponseBase):
    sending_buffer: str

    def __init__(self, result: ResultBase) -> None:
        assert issubclass(type(result), ResultBase)
        super().__init__(result)
