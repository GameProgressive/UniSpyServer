import abc
from typing import Dict
import library
from library.src.extentions.gamespy_utils import convert_to_key_value
from servers.presence_search_player.src.exceptions.general import (
    GPParseException,
)

from servers.presence_search_player.src.abstractions.contracts import (
    RequestBase,
    ResultBase,
)


class RequestBase(library.src.abstractions.contracts.RequestBase):
    request_dict: Dict[str, str]
    raw_request: str
    command_name: str
    operation_id: int
    namespace_id: int

    def __init__(self, raw_request: str) -> None:
        assert isinstance(raw_request, str)
        super().__init__(raw_request)

    def parse(self) -> None:
        self.request_dict = convert_to_key_value(self.raw_request)
        self.command_name = self.request_dict.keys()[0]
        if "id" in self.request_dict.keys():
            try:
                self.operation_id = int(self.request_dict["id"])
            except ValueError:
                raise GPParseException("operation id is invalid.")

        if "namespaceid" in self.request_dict:
            try:
                self.namespace_id = int(self.request_dict["namespaceid"])
            except ValueError:
                raise GPParseException("namespaceid is incorrect.")


class ResultBase(library.src.abstractions.contracts.ResultBase):
    pass


class ResponseBase(library.src.abstractions.contracts.ResponseBase):
    _result: ResultBase
    _request: RequestBase
