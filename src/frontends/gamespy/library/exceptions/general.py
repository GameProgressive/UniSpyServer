from typing import TYPE_CHECKING, Optional

from frontends.gamespy.library.configs import CONFIG
from frontends.gamespy.library.log.log_manager import GLOBAL_LOGGER


if TYPE_CHECKING:
    from frontends.gamespy.library.abstractions.client import ClientBase


class UniSpyException(Exception):
    message: str
    """the error message"""

    def __init__(self, message: str) -> None:
        self.message = message

    @staticmethod
    # def handle_exception(e: Exception, client: ClientBase = None):
    def handle_exception(e: Exception, client: Optional["ClientBase"] = None):
        # first log the exception
        if client is None:
            GLOBAL_LOGGER.info(str(e))
        else:
            if issubclass(type(e), UniSpyException):
                ex: UniSpyException = e  # type:ignore
                client.log_error(ex.message)
            else:
                client.log_error(str(e))
        # if we are unittesting we raise the exception out
        if CONFIG.unittest.is_raise_except:
            raise e

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
