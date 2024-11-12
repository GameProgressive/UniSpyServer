from abc import abstractmethod, ABC
from typing import Optional, final

from pydantic import BaseModel
from backends.library.abstractions.contracts import ErrorResponse, RequestBase
from backends.library.database.pg_orm import PG_SESSION
from library.src.abstractions.contracts import ResultBase

import logging

from library.src.exceptions.general import UniSpyException


class OkResponse(BaseModel):
    message: str = "ok"


class HandlerBase:
    """
    The ultimate handler base of backend service
    """
    _request: RequestBase
    _result: Optional[ResultBase]
    response: Optional[dict]
    """
    the dict response which send to client
    """

    def __init__(self, request: RequestBase) -> None:
        assert issubclass(type(request), RequestBase)
        self._request = request
        # decoupling the logging in home.py
        self.logger = logging.getLogger("backend")
        self._result = None
        self.response = None

    async def handle(self) -> None:
        try:
            await self._request_check()
            await self._data_operate()
            await self._result_construct()
            await self._response_construct()
        except UniSpyException as ex:
            self.logger.error(ex.message)
            self.response = ErrorResponse(message=ex.message).model_dump()
        except Exception as ex:
            self.logger.error(ex)
            self.response = ErrorResponse(message=str(ex)).model_dump()

    async def _request_check(self) -> None:
        """virtual method"""

    async def _data_operate(self) -> None:
        """virtual method"""

    async def _result_construct(self) -> None:
        """virtual method"""
    @final
    async def _response_construct(self) -> None:
        # if there are no result, we send ok response
        if self._result is None:
            self.response = OkResponse().model_dump()
        else:
            self.response = self._result.model_dump()
