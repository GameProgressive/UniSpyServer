import abc

from pydantic import UUID4, BaseModel, ConfigDict, Field, IPvAnyAddress

from frontends.gamespy.library.network.http_data import HttpData


class ClientInfo(BaseModel):
    server_id: UUID4
    client_ip: str
    client_port: int = Field(ge=1, le=65535)


class RequestBase(BaseModel):
    model_config = ConfigDict(
        json_encoders={bytes: lambda value: value.decode("ascii", "backslashreplace")}
    )

    command_name: object
    raw_request: bytes | str | HttpData

    @staticmethod
    def gs_parse(raw_request: bytes | str | HttpData) -> "RequestBase":
        data = RequestBase(raw_request=raw_request, command_name=None)
        return data


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
