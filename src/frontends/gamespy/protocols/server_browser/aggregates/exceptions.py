from frontends.gamespy.library.exceptions.general import UniSpyException


class ServerBrowserException(UniSpyException):
    def __init__(self, message: str) -> None:
        super().__init__(message)
