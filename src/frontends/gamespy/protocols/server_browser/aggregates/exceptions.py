from frontends.gamespy.library.exceptions.general import UniSpyException, get_exceptions_dict


class ServerBrowserException(UniSpyException):
    def __init__(self, message: str) -> None:
        super().__init__(message)


EXCEPTIONS = get_exceptions_dict(__name__)
