import abc

import library.src.abstractions
import library.src.abstractions.contracts
from library.src.extentions.gamespy_utils import convert_to_key_value
from servers.presence_search_player.src.exceptions.general import (
    GPParseException,
)
from typing import Dict, Optional

import library.src.abstractions.contracts


def normalize_request(message: str):
    if "login" in message:
        message = message.replace("\\-", "\\")
        pos = message.index("\\", message.index("\\") + 1)
        if message[pos: pos + 2] != "\\\\":
            message = message[:pos] + "\\" + message[pos:]
    return message


class RequestBase(library.src.abstractions.contracts.RequestBase):
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


class ResultBase(library.src.abstractions.contracts.ResultBase):
    pass


class ResponseBase(library.src.abstractions.contracts.ResponseBase):
    _request: RequestBase
    _result: ResultBase
    sending_buffer: str

    def __init__(self, request: RequestBase, result: Optional[ResultBase]) -> None:
        assert issubclass(type(request), RequestBase)
        assert issubclass(type(result), ResultBase)
        super().__init__(request, result)
