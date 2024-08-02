def bytes_to_int(input: bytes) -> int:
    assert isinstance(input, bytes)
    return int.from_bytes(input, "little")

def int_to_bytes(input: int) -> bytes:
    assert isinstance(input, int)
    return input.to_bytes(4, "little", signed=False)
