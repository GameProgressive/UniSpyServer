from abc import abstractmethod, ABC
from backends.library.abstractions.contracts import ErrorResponse, RequestBase
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
            await self._request_check()
            await self._data_fetch()
            await self._result_construct()
        except UniSpyException as ex:
            self.logger.error(ex.message)
            self.response = ErrorResponse(message=ex.message).model_dump()
        except Exception as ex:
            self.logger.error(ex)
            self.response = ErrorResponse(message=str(ex)).model_dump()

    @abstractmethod
    async def _request_check(self) -> None:
        pass

    @abstractmethod
    async def _data_fetch(self) -> None:
        pass

    @abstractmethod
    async def _result_construct(self) -> None:
        pass
