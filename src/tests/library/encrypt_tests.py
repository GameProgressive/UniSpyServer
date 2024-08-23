import unittest
from library.src.encryption.gs_encryption import ChatCrypt


class GSEncryptionTest(unittest.TestCase):
    def test_encryption(self):
        enc = ChatCrypt("123345")
        result = enc.encrypt("hello".encode("ascii"))
        self.assertEqual(result, b"\xda\xaek^d")

    def test_decryption(self):
        enc = ChatCrypt("123345")
        result = enc.decrypt(b"\xda\xaek^d")
        self.assertEqual(result, b"hello")


if __name__ == "__main__":
    unittest.main()
