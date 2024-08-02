import abc


class EncryptBase(abc.ABC):
    @abc.abstractmethod
    def encrypt(self, data: bytes) -> bytes:
        assert isinstance(data, bytes)

    @abc.abstractmethod
    def decrypt(self, data: bytes) -> bytes:
        assert isinstance(data, bytes)
