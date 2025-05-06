import abc
from copy import deepcopy
from typing import Optional
from pydantic import BaseModel


class RequestBase:
    command_name: object
    raw_request: object

    def __init__(self, raw_request: object) -> None:
        """
        raw_request is for gamespy protocol\n
        json_dict is for restapi deserialization
        """
        super().__init__()
        if raw_request is None:
            raise Exception("raw_request should not be None")

        if raw_request is not None:
            if (not isinstance(raw_request, bytes)) and (
                not isinstance(raw_request, str)
            ):
                raise Exception("Unsupported raw_request type")
            self.raw_request = raw_request
            return
        # self.command_name = None
        # self.raw_request = None

    def parse(self) -> None:
        pass

    def to_dict(self) -> dict:
        """
        create a json serializable dict of this class
        """
        result = {}
        for key, value in self.__dict__.items():
            if key[0] != "_":
                result[key] = value
        return result


class ResultBase(BaseModel):
    pass


class ResponseBase:
    sending_buffer: object
    _result: ResultBase
    _request: RequestBase

    def __init__(self, request: RequestBase, result: Optional[ResultBase]) -> None:
        super().__init__()
        if request is not None:
            assert issubclass(type(request), RequestBase)
            self._request = request

        if result is not None:
            assert issubclass(type(result), ResultBase)
            self._result = result

    @abc.abstractmethod
    def build(self) -> None:
        pass
