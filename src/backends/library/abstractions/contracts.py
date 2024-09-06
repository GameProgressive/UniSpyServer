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


class GeneralResponse(BaseModel):
    message: str
