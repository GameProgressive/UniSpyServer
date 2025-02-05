from typing import Dict
import frontends.gamespy.library.abstractions.contracts as lib
from frontends.gamespy.library.extentions.gamespy_utils import convert_to_key_value
from frontends.gamespy.protocols.presence_search_player.aggregates.exceptions import (
    GPParseException,
)


class RequestBase(lib.RequestBase):
    request_dict: Dict[str, str]
    raw_request: str
    command_name: str
    operation_id: int
    namespace_id: int

    def __init__(self, raw_request: str) -> None:
        assert isinstance(raw_request, str)
        super().__init__(raw_request)
        self.operation_id = 0
        self.namespace_id = 0

    def parse(self) -> None:
        self.request_dict = convert_to_key_value(self.raw_request)
        self.command_name = list(self.request_dict.keys())[0]
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


class ResultBase(lib.ResultBase):
    pass


class ResponseBase(lib.ResponseBase):
    _result: ResultBase
    _request: RequestBase
