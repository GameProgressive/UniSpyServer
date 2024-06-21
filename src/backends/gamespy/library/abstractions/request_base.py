from dataclasses import dataclass
from pydantic import BaseModel, Field,UUID4
@dataclass
class RequestBase(BaseModel):
    """
    The ultimate request base class of all gamespy requests
    """

    server_id: UUID4
    raw_request: object
