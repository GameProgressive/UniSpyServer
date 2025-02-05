import abc

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
    raw_request: str
    request_dict: Dict[str, str]

    def __init__(self, raw_request: str) -> None:
        assert isinstance(raw_request, str)
        super().__init__(raw_request)

    def parse(self):
        super().parse()
        self.request_dict = convert_to_key_value(self.raw_request)
        self.command_name = list(self.request_dict.keys())[0]

        if "id" in self.request_dict:
            try:
                self.operation_id = int(self.request_dict["id"])
            except:
                raise GPParseException("namespaceid is invalid.")
    
class ResultBase(lib.ResultBase):
    pass


class ResponseBase(lib.ResponseBase):
    _request: RequestBase
    _result: ResultBase
    sending_buffer: str

    def __init__(self, request: RequestBase, result: Optional[ResultBase]) -> None:
        assert issubclass(type(request), RequestBase)
        assert issubclass(type(result), ResultBase)
        super().__init__(request, result)
