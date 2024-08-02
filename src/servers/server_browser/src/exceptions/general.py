from library.src.exceptions.error import UniSpyException


class ServerBrowserException(UniSpyException):
    def __init__(self, message: str) -> None:
        super().__init__(message)
