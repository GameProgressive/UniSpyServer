

from frontends.gamespy.library.abstractions.enctypt_base import EncryptBase
from frontends.gamespy.library.encryption.xor_encryption import XorEncoding, XorType
from frontends.gamespy.protocols.game_status.aggregations.exceptions import GSException


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