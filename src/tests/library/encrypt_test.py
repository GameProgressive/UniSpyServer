import unittest
from library.encryption.gs_encryption import ChatCrypt


class GSEncryptionTest(unittest.TestCase):
    def test_encryption(self):
        enc = ChatCrypt("123345")
        result = enc.encrypt("hello")
        self.assertEqual(result, b"\xe9D\x91Q\xb9")

    def test_decryption(self):
        enc = ChatCrypt("123345")
        result = enc.decrypt(b"\xe9D\x91Q\xb9")
        self.assertEqual(result, "hello")



if __name__ ==  '__main__':
    unittest.main()