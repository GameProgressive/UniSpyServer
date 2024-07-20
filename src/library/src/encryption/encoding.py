from typing import List


class Encoding:
    @staticmethod
    def get_string(data: bytes) -> str:
        assert isinstance(data, bytes)
        return data.decode("ascii")

    def get_bytes(data: str) -> List[int]:
        assert isinstance(data, str)
        return list(data.encode())
