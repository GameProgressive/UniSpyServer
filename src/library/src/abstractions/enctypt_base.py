from abc import abstractmethod


class EncryptBase:
    @abstractmethod
    def encrypt(self, data: bytes) -> bytes:
        assert isinstance(data, bytes)

    @abstractmethod
    def decrypt(self, data: bytes) -> bytes:
        assert isinstance(data, bytes)
