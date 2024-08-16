import hashlib
import base64

from library.src.exceptions.error import UniSpyException


def process_password(request: dict):
    """process password in standard format and return the password"""
    assert isinstance(request, dict)
    if "passwordenc" in request:
        md5_password = get_md5_hash(decode(request["passwordenc"]))
    elif "passenc" in request:
        md5_password = get_md5_hash(decode(request["passenc"]))
    elif "pass" in request:
        md5_password = get_md5_hash(request["pass"])
    elif "password" in request:
        md5_password = get_md5_hash(request["password"])
    else:
        raise UniSpyException("Can not find password field in request")
    return md5_password


def encode(password: str):
    assert isinstance(password, str)
    password_bytes = password.encode("utf-8")
    pass_encoded = base64.b64encode(game_spy_encode_method(password_bytes))
    pass_encoded = pass_encoded.decode("utf-8")
    pass_encoded = pass_encoded.replace("=", "_").replace("+", "[").replace("/", "]")
    return pass_encoded


def decode(password: str):
    assert isinstance(password, str)
    password = password.replace("_", "=").replace("[", "+").replace("]", "/")
    password_bytes = base64.b64decode(password)
    return game_spy_encode_method(password_bytes).decode("utf-8")


def game_spy_encode_method(password_bytes: bytes):
    assert isinstance(password_bytes, bytes)
    a = 0
    num = 0x79707367  # gamespy
    temp_data = list(password_bytes)
    for i in range(len(password_bytes)):
        num = game_spy_byte_shift(num)
        a = num % 0xFF
        temp_data[i] ^= a
    return bytes(temp_data)


def game_spy_byte_shift(num):
    assert isinstance(num, int)
    c = (num >> 16) & 0xFFFF
    a = num & 0xFFFF

    c *= 0x41A7
    a *= 0x41A7
    a += (c & 0x7FFF) << 16

    if a < 0:
        a &= 0x7FFFFFFF
        a += 1

    a += c >> 15

    if a < 0:
        a &= 0x7FFFFFFF
        a += 1

    return a


def get_md5_hash(data):
    isinstance(data, str)
    md5_hash = hashlib.md5()
    md5_hash.update(data.encode("utf-8"))
    return md5_hash.hexdigest()
