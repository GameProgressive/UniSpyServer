import frontends.gamespy.library.abstractions.contracts as lib
import abc

from frontends.gamespy.library.extentions.gamespy_utils import convert_to_key_value


class RequestBase(lib.RequestBase):
    request_dict: dict[str, str]
    raw_request: str

    def __init__(self, raw_request: str) -> None:
        assert isinstance(raw_request, str)
        super().__init__(raw_request)
        self.request_dict = {}

    def parse(self) -> None:
        self.request_dict = convert_to_key_value(self.raw_request)
        self.command_name = list(self.request_dict.keys())[0]


class ResultBase(lib.ResultBase):
    pass


class ResponseBase(lib.ResponseBase):
    sending_buffer: str

    def __init__(self, request: RequestBase, result: ResultBase) -> None:
        assert issubclass(type(request), RequestBase)
        assert issubclass(type(result), ResultBase)
        super().__init__(request, result)
