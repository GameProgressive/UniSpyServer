
from typing import TYPE_CHECKING


if TYPE_CHECKING:
    from library.src.abstractions.client import ClientBase


class UniSpyException(Exception):
    message: str
    """the error message"""

    def __init__(self, message: str) -> None:
        self.message = message

    @staticmethod
    # def handle_exception(e: Exception, client: ClientBase = None):
    def handle_exception(e: Exception, client: "ClientBase" = None):
        if issubclass(type(e), UniSpyException):
            ex: UniSpyException = e
            if client is None:
                # LogWriter.LogError(ex.Message);
                pass
            else:
                client.log_error(ex.message)
                pass
        else:
            if client is None:
                # LogWriter.LogError(ex.ToString());
                pass
            else:
                # client.LogError(ex.ToString());
                pass

    def __repr__(self) -> str:
        # return super().__repr__()
        return f'Error message: "{self.message}"'


class DatabaseConnectionException(UniSpyException):
    def __init__(self, message: str = "Can not connect to database.") -> None:
        super().__init__(message)


class RedisConnectionException(UniSpyException):
    def __init__(self, message: str = "Can not connect to redis") -> None:
        super().__init__(message)


if __name__ == "__main__":
    err = UniSpyException("test")
    pass
