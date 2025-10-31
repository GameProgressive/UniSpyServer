from pydantic import BaseModel


class GetMyIPResponse(BaseModel):
    ip: str
