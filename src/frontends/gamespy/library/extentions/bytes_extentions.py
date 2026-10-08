import socket


def bytes_to_int(input: bytes) -> int:
    assert isinstance(input, bytes)
    return int.from_bytes(input, "little")


def int_to_bytes(input: int) -> bytes:
    assert isinstance(input, int)
    return input.to_bytes(4, "little", signed=False)


def ip_to_4_bytes(ip: str) -> bytes:
    assert isinstance(ip, str)
    return socket.inet_aton(ip)


def port_to_2_bytes(port: int) -> bytes:
    """
    using for qr sb natneg to convert port to bytes
    """
    assert isinstance(port, int)
    return port.to_bytes(2, "little")
