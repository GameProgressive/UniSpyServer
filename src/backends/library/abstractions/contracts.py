from typing import Dict
from pydantic import BaseModel, UUID4
import frontends.gamespy.library.abstractions.contracts as fnt


class RequestBase(BaseModel):
    info: fnt.ClientInfo
    data: fnt.RequestBase


# class RequestBase(BaseModel):
#     """
#     The ultimate request base class of all gamespy requests
#     """

#     server_id: UUID4
#     raw_request: str
#     """
#     if the raw_request is bytes, we decode it to decode("ascii","backslashreplace") str
#     """
#     client_ip: str
#     client_port: int


class Response(BaseModel):
    message: str


class OKResponse(Response):
    message: str = "ok"


class DataResponse(OKResponse):
    result: BaseModel
    pass


class ErrorResponse(Response):
    exception_name: str
    exception_data: dict | None = None
    pass


RESPONSES_DEF: Dict = {
    400: {"model": ErrorResponse},
    422: {"model": ErrorResponse},
    500: {"model": ErrorResponse},
    450: {"model": ErrorResponse},
}
"""Dict type is using to compatible with language checking"""
