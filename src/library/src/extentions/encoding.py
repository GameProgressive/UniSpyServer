def get_string(data: bytes) -> str:
    return data.decode("ascii")


def get_bytes(data: str) -> bytes:
    return data.encode("ascii")



