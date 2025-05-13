from enum import IntEnum
import base64

from frontends.gamespy.library.abstractions.enctypt_base import EncryptBase


class XorType(IntEnum):
    TYPE_0 = 0
    TYPE_1 = 1
    TYPE_2 = 2
    TYPE_3 = 3


class XorEncoding(EncryptBase):
    def __init__(self, xor_type):
        self.encryption_type = xor_type

    @staticmethod
    def encode(plaintext: bytes, enc_type: XorType):
        assert isinstance(plaintext, bytes)
        assert isinstance(enc_type, XorType)
        seed_0 = b"gamespy"
        seed_1 = b"GameSpy3D"
        seed_2 = b"Industries"
        seed_3 = b"ProjectAphex"
        index = 0
        key = seed_0
        if enc_type == XorType.TYPE_0:
            key = seed_0
        elif enc_type == XorType.TYPE_1:
            key = seed_1
        elif enc_type == XorType.TYPE_2:
            key = seed_2
        elif enc_type == XorType.TYPE_3:
            key = seed_3
        result = []
        key_index = 0
        for index in range(len(plaintext)):
            key_index = index % len(key)
            print(key_index)
            enc_byte = (plaintext[index] ^ key[key_index]) % 255
            result.append(enc_byte)

        return bytes(result)

    def encrypt(self, data: bytes):
        super().encrypt(data)
        return XorEncoding.encode(data, self.encryption_type)

    def decrypt(self, data: bytes):
        super().decrypt(data)
        return XorEncoding.encode(data, self.encryption_type)
