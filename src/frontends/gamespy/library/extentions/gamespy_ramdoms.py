import datetime
import random
from enum import IntEnum


class StringType(IntEnum):
    ALPHANUMERIC = 0
    ALPHA = 1
    HEX = 2


def generate_random_string(count: int, type: StringType) -> str:
    random.seed(datetime.datetime.now(datetime.timezone.utc).timestamp())

    alpha_chars = "ABCDEFGHIJKLMNOPQRSTUVWXYZ"

    alpha_num_chars = "0123456789abcdefghijklmnopqrstuvwxyzABCDEFGHIJKLMNOPQRSTUVWXYZ"

    hex_chars = "0123456789abcdef"

    builder = []
    for _ in range(count):
        if type == StringType.ALPHANUMERIC:
            builder.append(random.choice(alpha_num_chars))
        elif type == StringType.HEX:
            builder.append(random.choice(hex_chars))
        else:
            builder.append(random.choice(alpha_chars))

    return "".join(builder)
