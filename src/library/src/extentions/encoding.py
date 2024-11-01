import enum
import json
from uuid import UUID


def get_string(data: bytes) -> str:
    return data.decode("ascii")


def get_bytes(data: str) -> bytes:
    return data.encode("ascii")


class UniSpyJsonEncoder(json.JSONEncoder):
    def default(self, obj):
        # Handle bytes
        if isinstance(obj, bytes):
            return obj.decode("ascii", "backslashreplace")
        # Handle enum and IntEnum
        elif isinstance(obj, enum.Enum):
            return obj.value
        elif isinstance(obj, enum.IntEnum):
            return obj.value
        elif isinstance(obj, UUID):
            return str(obj)
        # Fallback to the default method for other types
        return super().default(obj)
