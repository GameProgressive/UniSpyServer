from pydantic import BaseModel, UUID4


class RequestBase(BaseModel):
    """
    The ultimate request base class of all gamespy requests
    """

    server_id: UUID4
    raw_request: str
    """
    if the raw_request is bytes, we decode it to decode("ascii","backslashreplace") str
    """
    client_ip: str
    client_port: int


class Response(BaseModel):
    message: str

    def to_json_dict(self) -> dict[str, object]:
        return self.model_dump(mode="json")


class OKResponse(Response):
    message: str = "ok"


class DataResponse(OKResponse):
    result: dict
    pass


class ErrorResponse(Response):
    # code: int
    pass
