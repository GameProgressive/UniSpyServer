from abc import abstractmethod
from backends.library.abstractions.contracts import RequestBase
from library.src.abstractions.contracts import ResultBase

import logging

from library.src.exceptions.general import UniSpyException


class HandlerBase:
    """
    The ultimate handler base of backend service
    """
    _request: RequestBase
    _result: ResultBase
    response: dict
    """
    the dict response which send to client
    """

    def __init__(self, request: RequestBase) -> None:
        assert issubclass(type(request), RequestBase)
        self._request = request
        # decoupling the logging in home.py
        self.logger = logging.getLogger("backend")

    async def handle(self) -> None:
        try:
            await self.request_check()
            await self.data_fetch()
            await self.result_construct()
        except UniSpyException as ex:
            self.logger.error(ex.message)
            self.response = {"message": ex.message}
        except Exception as ex:
            self.logger.error(ex)
            self.response = {"message": str(ex)}

    @abstractmethod
    async def request_check(self) -> None:
        pass

    @abstractmethod
    async def data_fetch(self) -> None:
        pass

    @abstractmethod
    async def result_construct(self) -> None:
        pass
