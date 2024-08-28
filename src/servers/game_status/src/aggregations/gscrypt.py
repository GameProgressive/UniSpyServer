

from library.src.abstractions.enctypt_base import EncryptBase
from library.src.encryption.xor_encryption import XorEncoding, XorType
from servers.game_status.src.exceptions.general import GSException


class GSCrypt(EncryptBase):
    def decrypt(self, data: bytes) -> bytes:
        if b"final" not in data:
            raise GSException("Ciphertext must contains delimeter \\final\\")
        cipher = data[:-7]
        plain = XorEncoding.encode(cipher, XorType.TYPE_1)
        return plain + b"\\final\\"

    def encrypt(self, data: bytes) -> bytes:
        if b"final" not in data:
            raise GSException("Ciphertext must contains delimeter \\final\\")
        cipher = data[:-7]
        plain = XorEncoding.encode(cipher, XorType.TYPE_1)
        return plain + b"\\final\\"