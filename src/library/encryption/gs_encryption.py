from library.abstractions.enctypt_base import EncryptBase
import hashlib

DIGITS_HEX = "0123456789abcdef"
DIGITS_CRYPT = "aFl4uOD9sfWq1vGp"
NEW_DIGITS_CRYPT = "qJ1h4N9cP3lzD0Ka"
IP_XOR_MASK = 0xC3801DC7
CLIENT_KEY = "0000000000000000"
SERVER_KEY = "0000000000000000"


class PeerChatCtx:
    def __init__(self):
        self.buffer1 = 0
        self.buffer2 = 0
        self.sbox = [0] * 256


def init(ctx: PeerChatCtx, challenge_key: str, secret_key: str):
    assert isinstance(ctx, PeerChatCtx)
    assert isinstance(challenge_key, str)
    assert isinstance(secret_key, str)

    challenge_bytes = list(challenge_key.encode("ascii"))
    secret_key_bytes = list(secret_key.encode("ascii"))

    ctx.buffer1 = 0
    ctx.buffer2 = 0

    secret_key_index = 0
    for i in range(len(challenge_bytes)):
        if secret_key_index >= len(secret_key_bytes):
            secret_key_index = 0

        challenge_bytes[i] ^= secret_key_bytes[secret_key_index]
        secret_key_index += 1

    index1 = 255
    for i in range(256):
        ctx.sbox[i] = index1
        index1 -= 1

    index1 = 0
    index2 = 0
    for i in range(len(ctx.sbox)):
        if index1 >= len(challenge_bytes):
            index1 = 0

        index2 = (challenge_bytes[index1] + ctx.sbox[i] + index2) % 256
        t = ctx.sbox[i]
        ctx.sbox[i] = ctx.sbox[index2]
        ctx.sbox[index2] = t
        index1 += 1


def handle(ctx: PeerChatCtx, data):
    num1 = ctx.buffer1
    num2 = ctx.buffer2
    buffer = []
    size = len(data)
    datapos = 0

    while size > 0:
        num1 = (num1 + 1) % 256
        num2 = (ctx.sbox[num1] + num2) % 256
        t = ctx.sbox[num1]
        ctx.sbox[num1] = ctx.sbox[num2]
        ctx.sbox[num2] = t
        t = (ctx.sbox[num2] + ctx.sbox[num1]) % 256
        temp = data[datapos] ^ ctx.sbox[t]
        buffer.append(temp)
        datapos += 1
        size -= 1

    ctx.buffer1 = num1
    ctx.buffer2 = num2
    return bytes(buffer)


class ChatCrypt(EncryptBase):
    def __init__(self, game_secret_key):
        self.client_ctx = PeerChatCtx()
        self.server_ctx = PeerChatCtx()
        init(self.client_ctx, CLIENT_KEY, game_secret_key)
        init(self.server_ctx, SERVER_KEY, game_secret_key)

    def encrypt(self, data: bytes) -> bytes:
        super().encrypt(data)
        return self.handle(self.server_ctx, data)

    def decrypt(self, data: bytes) -> bytes:
        super().decrypt(data)
        return self.handle(self.client_ctx, data)


if __name__ == "__main__":
    enc = ChatCrypt("123456")
    cipher = enc.encrypt("hello".encode())
    pass
