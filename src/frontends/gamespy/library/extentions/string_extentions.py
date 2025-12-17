from typing import Literal


def convert_nonprintable_bytes_to_hex_string(buffer: bytes) -> str:
    assert isinstance(buffer, bytes)
    temp = ""
    for byte in buffer:
        if byte < 0x1F or byte > 0x7E:
            temp += f"[{byte:02X}]"
        else:
            temp += chr(byte)
    return temp


def convert_printable_bytes_to_string(buffer: bytes) -> str:
    assert isinstance(buffer, bytes)
    delimiter = " "
    temp = ""
    for byte in buffer:
        if byte < 0x1F or byte > 0x7E:
            if temp and temp[-1] != delimiter:
                temp += delimiter
        else:
            temp += chr(byte)
    return temp


def convert_kvstring_to_dictionary(kv_str: str):
    assert isinstance(kv_str, str)
    dic = {}
    key_value_list = kv_str.split("\\")

    for i in range(0, len(key_value_list), 2):
        if len(key_value_list) < i + 2:
            dic[key_value_list[i]] = ""
        else:
            dic[key_value_list[i]] = key_value_list[i + 1].replace("\x00", "")

    return dic


def convert_keystr_to_list(key_str: str):
    assert isinstance(key_str, str)

    data = key_str.split("\\")
    # Remove empty strings from the list
    data = [item for item in data if item]

    return data


def convert_byte_to_hex_string(buffer: bytes):
    assert isinstance(buffer, bytes)
    hex_string = ", ".join(["0x" + format(byte, "02X") for byte in buffer])
    return hex_string


def from_hex_string_to_bytes(hex_str: str):
    """
    use in webservice auth public key convertion
    """
    assert isinstance(hex_str, str)
    data = [int(hex_str[i: i + 2], 16) for i in range(0, len(hex_str), 2)]
    return bytes(data)


if __name__ == "__main__":
    sig = "0001FFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFF003020300C06082A864886F70D020505000410"

    data = from_hex_string_to_bytes(sig)
    pass


def get_first_capitalized_char(name: str):
    assert isinstance(name, str)
    result = ""
    for char in name:
        if char.isupper():
            result += char
    return result


def get_server_short_name(name: str):
    if len(name) >= 4:
        short_name = get_first_capitalized_char(name)
    else:
        short_name = name
    return short_name


def format_network_message(
    type: Literal["recv", "send"], message: bytes, is_log_raw: bool = False
):
    assert type in ["recv", "send"]
    if is_log_raw:
        tempLog = f"{convert_printable_bytes_to_string(
            message)} [{convert_byte_to_hex_string(message)}]"
    else:
        tempLog = convert_nonprintable_bytes_to_hex_string(message)

    return f"[{type}] {tempLog}"
