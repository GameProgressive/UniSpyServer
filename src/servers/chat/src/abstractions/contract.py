import abc
import library.src.abstractions.contracts
from servers.chat.src.exceptions.general import ChatException


class RequestBase(library.src.abstractions.contracts.RequestBase):
    raw_request: str
    command_name: str
    _prefix: str
    _cmd_params: list
    _longParam: str

    def __init__(self, raw_request: str) -> None:
        assert isinstance(raw_request, str)
        super().__init__(raw_request)

    @abc.abstractmethod
    def parse(self) -> None:
        # at most 2 colon character
        # we do not sure about all command
        # so i block this code here
        self.raw_request = self.raw_request.replace("\r", "").replace("\n", "")
        dataFrag = []

        if self.raw_request.count(":") > 2:
            raise Exception(f"IRC request is invalid {self.raw_request}")

        indexOfColon = self.raw_request.index(":")

        rawRequest = self.raw_request
        if indexOfColon == 0 and indexOfColon != -1:
            prefixIndex = rawRequest.index(" ")
            self._prefix = rawRequest[indexOfColon:prefixIndex]
            rawRequest = rawRequest[prefixIndex:]

        indexOfColon = rawRequest.index(":")
        if indexOfColon != 0 and indexOfColon != -1:
            self._longParam = rawRequest[indexOfColon + 1:]
            # reset the request string
            rawRequest = rawRequest[:indexOfColon]

        dataFrag = rawRequest.strip(" ").split(" ")

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

    def __init__(self, request: RequestBase, result: ResultBase | None) -> None:
        super().__init__(request, result)
        if result is not None:
            assert issubclass(type(result), ResultBase)
        assert issubclass(request, RequestBase)



if __name__ == "__main__":
    # Example usage:
    request = RequestBase()
    request.raw_request = "your_raw_request_here"
    request.parse()
    print(request.command_name)
