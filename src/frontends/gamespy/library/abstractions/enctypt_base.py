from abc import abstractmethod

class Byte:
    value: int

    def __init__(self, value):
        if value > 255:
            raise ValueError("byte should be in 0 to 256")
        self.value = value

    def _clamp(self, value) -> int:
        """clamp value in 0 to 255"""
        return value % 256

    def __add__(self, other):
        if not isinstance(other, Byte):
            raise TypeError("operation must on Byte object")
        new = self._clamp(self.value + other.value)
        return Byte(new)

    def __sub__(self, other):
        if not isinstance(other, Byte):
            raise TypeError("operation must on Byte object")
        new = self._clamp(self.value - other.value)
        return Byte(new)

    def __mul__(self, other):
        if not isinstance(other, Byte):
            raise TypeError("operation must on Byte object")
        new = self._clamp(self.value * other.value)
        return Byte(new)

    def __truediv__(self, other):
        if not isinstance(other, Byte):
            raise TypeError("operation must on Byte object")
        if other.value == 0:
            raise ValueError("Cannot divide by zero")
        new = self._clamp(self.value // other.value)
        return Byte(new)

    def __xor__(self, other):
        if not isinstance(other, Byte):
            raise TypeError("operation must on Byte object")
        new = self._clamp(self.value ^ other.value)
        return Byte(new)

    def __lt__(self, other):
        if not isinstance(other, Byte):
            raise TypeError("operation must be on Byte object")
        return self.value < other.value

    def __le__(self, other):
        if not isinstance(other, Byte):
            raise TypeError("operation must be on Byte object")
        return self.value <= other.value

    def __gt__(self, other):
        if not isinstance(other, Byte):
            raise TypeError("operation must be on Byte object")
        return self.value > other.value

    def __ge__(self, other):
        if not isinstance(other, Byte):
            raise TypeError("operation must be on Byte object")
        return self.value >= other.value

    def __eq__(self, other):
        if not isinstance(other, Byte):
            raise TypeError("operation must be on Byte object")
        return self.value == other.value

    def __lshift__(self, other):
        """Perform in-place left shift operation."""
        if not isinstance(other, Byte):
            raise TypeError("operation must be on Byte object")
        if other < Byte(0):
            raise ValueError("Shift amount must be non-negative")
        return Byte(self.value << other.value)

    def __and__(self, other):
        if not isinstance(other, Byte):
            raise TypeError("operation must be on Byte object")
        return Byte(self.value & other.value)

    def __mod__(self, other):
        if not isinstance(other, Byte):
            raise TypeError("operation must be on Byte object")
        return Byte(self.value % other.value)

    def __repr__(self):
        return f"Byte({self.value})"

    @staticmethod
    def from_bytes(data: bytes | bytearray):
        temp = []
        for d in data:
            temp.append(Byte(d))
        return temp

class EncryptBase:
    @abstractmethod
    def encrypt(self, data: bytes) -> bytes:
        assert isinstance(data, bytes)

    @abstractmethod
    def decrypt(self, data: bytes) -> bytes:
        assert isinstance(data, bytes)
