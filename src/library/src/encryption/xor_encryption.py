from enum import IntEnum
import base64

from library.src.abstractions.enctypt_base import EncryptBase


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

        length = len(plaintext)
        index = 0
        temp = seed_0

        if enc_type == XorType.TYPE_0:
            temp = seed_0
        elif enc_type == XorType.TYPE_1:
            temp = seed_1
        elif enc_type == XorType.TYPE_2:
            temp = seed_2
        elif enc_type == XorType.TYPE_3:
            temp = seed_3

        temp_length = len(temp)

        for i in range(length):
            if i >= temp_length:
                i = 0

            plaintext[index] ^= temp[i]
            index += 1

        return plaintext

    def encrypt(self, data: bytes):
        super().encrypt(data)
        return XorEncoding.encode(data, self.encryption_type)

    def decrypt(self, data: bytes):
        super().decrypt(data)
        return XorEncoding.encode(data, self.encryption_type)
