import unittest
from library.src.encryption.gs_encryption import ChatCrypt
from library.src.encryption.xor_encryption import XorEncoding, XorType
from servers.game_status.src.aggregations.gscrypt import GSCrypt


class EncryptionTest(unittest.TestCase):
    def test_chat_encryption(self):
        enc = ChatCrypt("123345")
        result = enc.encrypt("hello".encode("ascii"))
        self.assertEqual(result, b"\xda\xaek^d")

    def test_chat_decryption(self):
        enc = ChatCrypt("123345")
        result = enc.decrypt(b"\xda\xaek^d")
        self.assertEqual(result, b"hello")

    def test_xor_encoding(self):
        raw = b"abcdefghijklmnopqrstuvwxyz"
        plaintext = XorEncoding.encode(raw, XorType.TYPE_1)
        self.assertEqual(
            b"&\x03\x0e\x016\x16\x1e[--\n\x01\x08=\x1f\tB64\x15\x18\x13$\x08\x00I", plaintext)


if __name__ == "__main__":
    unittest.main()
