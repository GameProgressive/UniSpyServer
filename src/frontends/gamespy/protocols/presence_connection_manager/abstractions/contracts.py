import frontends.gamespy.library.abstractions.contracts as lib
from frontends.gamespy.library.extentions.gamespy_utils import convert_to_key_value
from frontends.gamespy.protocols.presence_connection_manager.aggregates.enums import (
    CommandName,
)
from frontends.gamespy.protocols.presence_search_player.aggregates.exceptions import (
    GPParseException,
)


def normalize_request(message: str):
    if "login" in message:
        message = message.replace("\\-", "\\")
        pos = message.index("\\", message.index("\\") + 1)
        if message[pos : pos + 2] != "\\\\":
            message = message[:pos] + "\\" + message[pos:]
    return message


class RequestBase(lib.RequestBase):
    command_name: CommandName
    operation_id: int
    """
    todo some request do not have operation id we just set it to 0 as default
    todo check this in future
    """
    raw_request: str
    request_dict: dict[str, str]

    @staticmethod
    def gs_parse(raw_request: str) -> "RequestBase":
        data = lib.RequestBase.gs_parse(raw_request)
        data_dict = data.model_dump()
        data_dict["request_dict"] = convert_to_key_value(raw_request)
        data_dict["command_name"] = next(iter(data_dict["request_dict"]))
        data_dict["operation_id"] = data_dict["request_dict"].get("id", 0)
        data = RequestBase(**data_dict)
        return data


class ResultBase(lib.ResultBase):
    operation_id: int
    pass


class ResponseBase(lib.ResponseBase):
    _request: RequestBase
    _result: ResultBase
    sending_buffer: str

    def __init__(self, result: ResultBase) -> None:
        assert issubclass(type(result), ResultBase)
        super().__init__(result)
