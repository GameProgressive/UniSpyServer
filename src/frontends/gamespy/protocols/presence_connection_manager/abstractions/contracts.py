
from frontends.gamespy.library.extentions.gamespy_utils import convert_to_key_value
from frontends.gamespy.protocols.presence_search_player.aggregates.exceptions import (
    GPParseException,
)
from typing import Dict, Optional
import frontends.gamespy.library.abstractions.contracts as lib


def normalize_request(message: str):
    if "login" in message:
        message = message.replace("\\-", "\\")
        pos = message.index("\\", message.index("\\") + 1)
        if message[pos: pos + 2] != "\\\\":
            message = message[:pos] + "\\" + message[pos:]
    return message


class RequestBase(lib.RequestBase):
    command_name: str
    operation_id: int
    """
    todo some request do not have operation id we just set it to 0 as default
    todo check this in future
    """
    raw_request: str
    _request_dict: Dict[str, str]

    def __init__(self, raw_request: str) -> None:
        assert isinstance(raw_request, str)
        super().__init__(raw_request)

    def parse(self):
        super().parse()
        self._request_dict = convert_to_key_value(self.raw_request)
        self.command_name = list(self._request_dict.keys())[0]

        if "id" in self._request_dict:
            try:
                self.operation_id = int(self._request_dict["id"])
            except Exception:
                raise GPParseException("operation is invalid")
        else:
            self.operation_id = 0
            # raise GPParseException("operation id is missing")


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
