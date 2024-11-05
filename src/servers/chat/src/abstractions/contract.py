from typing import Optional
from uuid import UUID

from pydantic import BaseModel
import library.src.abstractions.contracts


class RequestBase(library.src.abstractions.contracts.RequestBase):
    raw_request: str
    command_name: str
    _prefix: Optional[str]
    _cmd_params: list[str]
    _long_param: Optional[str]

    def __init__(self, raw_request: str) -> None:
        assert isinstance(raw_request, str)
        super().__init__(raw_request)
        self._prefix = None
        self._cmd_params = []
        self._long_param = None

    def parse(self) -> None:
        # at most 2 colon character
        # we do not sure about all command
        # so i block this code here
        self.raw_request = self.raw_request.replace("\r", "").replace("\n", "")
        dataFrag = []

        if self.raw_request.count(":") > 2:
            raise Exception(f"IRC request is invalid {self.raw_request}")
        if ":" not in self.raw_request:
            index_of_colon = -1
        else:
            index_of_colon = self.raw_request.index(":")

        raw = self.raw_request
        if index_of_colon == 0 and index_of_colon != -1:
            if " " not in raw:
                prefixIndex = -1
            else:
                prefixIndex = raw.index(" ")
            self._prefix = raw[index_of_colon:prefixIndex]
            raw = raw[prefixIndex:]

        if ":" not in raw:
            index_of_colon = 0
        else:
            index_of_colon = raw.index(":")
        if index_of_colon != 0 and index_of_colon != -1:
            self._long_param = raw[index_of_colon + 1:]
            # reset the request string
            raw = raw[:index_of_colon]

        dataFrag = raw.strip(" ").split(" ")

        self.command_name = dataFrag[0]

        if len(dataFrag) > 1:
            self._cmd_params = dataFrag[1:]


class ResultBase(library.src.abstractions.contracts.ResultBase):
    pass


SERVER_DOMAIN = "unispy.net"


class ResponseBase(library.src.abstractions.contracts.ResponseBase):
    sending_buffer: str
    _result: ResultBase
    _request: RequestBase

    def __init__(self, request: RequestBase, result: Optional[ResultBase]) -> None:
        super().__init__(request, result)
        if result is not None:
            assert issubclass(type(result), ResultBase)
        assert issubclass(type(request), RequestBase)

# region Brocker


class BrockerMessage(BaseModel):
    server_id: UUID
    channel_name: str
    sender_ip_end_point: str
    message: str


if __name__ == "__main__":
    # Example usage:
    request = RequestBase("")
    request.raw_request = "your_raw_request_here"
    request.parse()
    print(request.command_name)
