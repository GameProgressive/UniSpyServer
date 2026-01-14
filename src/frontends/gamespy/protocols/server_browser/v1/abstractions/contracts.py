import frontends.gamespy.library.abstractions.contracts as lib
from frontends.gamespy.library.extentions.gamespy_utils import convert_to_key_value


class RequestBase(lib.RequestBase):
    _request_dict: dict[str, str]
    raw_request: str

    def __init__(self, raw_request: str) -> None:
        assert isinstance(raw_request, str)
        super().__init__(raw_request)

    def parse(self) -> None:
        self._request_dict = convert_to_key_value(self.raw_request)


class ResultBase(lib.ResultBase):
    pass


class ResponseBase(lib.ResponseBase):
    pass
