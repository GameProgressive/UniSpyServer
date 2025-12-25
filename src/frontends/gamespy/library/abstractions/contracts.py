import abc
from typing import TYPE_CHECKING

from pydantic import BaseModel
if TYPE_CHECKING:
    from frontends.gamespy.library.network.http_handler import HttpData


class RequestBase:
    command_name: object
    raw_request:  "bytes | str | HttpData"

    def __init__(self, raw_request: "bytes | str | HttpData") -> None:
        """
        raw_request is for gamespy protocol\n
        json_dict is for restapi deserialization
        """
        super().__init__()
        from frontends.gamespy.library.network.http_handler import HttpData

        if (not isinstance(raw_request, bytes)) \
            and (not isinstance(raw_request, str)
                 and (not isinstance(raw_request, HttpData))
                 ):
            raise Exception("Unsupported raw_request type")
        self.raw_request = raw_request

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

    def __init__(self, result: ResultBase) -> None:
        assert issubclass(type(result), ResultBase)
        self._result = result

    @abc.abstractmethod
    def build(self) -> None:
        pass
