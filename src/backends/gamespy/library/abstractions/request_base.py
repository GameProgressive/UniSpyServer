import abc
from dataclasses import dataclass
from uuid import UUID

import jsonschema


@dataclass
class RequestBase(abc.ABC):
    """
    The ultimate request base class of all gamespy requests
    """

    server_id: UUID
    raw_request: object
    json_schema = {
        "type": "object",
        "properties": {
            "server_id": {
                "type": "string",
                "pattern": "^[0-9a-f]{8}-[0-9a-f]{4}-[1-5][0-9a-f]{3}-[89ab][0-9a-f]{3}-[0-9a-f]{12}$",
            },
        },
    }
    pass

    def validate(self) -> None:
        jsonschema.validate(self.__dict__, self.json_schema)
