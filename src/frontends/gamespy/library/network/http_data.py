from pydantic import BaseModel


class HttpData(BaseModel):
    path: str | None
    headers: dict[str, object] | None
    body: str
