from typing import Optional, final
from backends.library.abstractions.contracts import (
    DataResponse,
    ErrorResponse,
    OKResponse,
    RequestBase,
    Response,
)
from frontends.gamespy.library.abstractions.contracts import ResultBase

import logging

from frontends.gamespy.library.exceptions.general import UniSpyException


class HandlerBase:
    """
    The ultimate handler base of backend service
    """

    _request: RequestBase
    _result: Optional[ResultBase]
    _response: Response
    """
    the response using to wrap data
    """
    # _data: object
    """
    the data get from database, can be any type
    """

    @property
    def response(self) -> dict:
        """
        the dict response which send to client
        """
        return self._response.to_json_dict()

    def __init__(self, request: RequestBase) -> None:
        assert issubclass(type(request), RequestBase)
        self._request = request
        # decoupling the logging in home.py
        self.logger = logging.getLogger("backend")
        self._result = None
        self._response = OKResponse()

    async def handle(self) -> None:
        try:
            await self._request_check()
            await self._data_operate()
            await self._result_construct()
            await self._response_construct()
        except UniSpyException as ex:
            self.logger.error(ex.message)
            self._response = ErrorResponse(message=ex.message)
        except Exception as ex:
            self.logger.error(ex)
            self._response = ErrorResponse(message=str(ex))

    async def _request_check(self) -> None:
        """virtual method"""

    async def _data_operate(self) -> None:
        """virtual method\n
        override by child class to perform database operations
        """

    async def _result_construct(self) -> None:
        """virtual method\n
        can override by child class to create self._result
        """

    @final
    async def _response_construct(self) -> None:
        """
        _response_construct can not be overrided
        """
        # if there are no result, we send ok response
        if self._result is None:
            self._response = OKResponse()
            self.logger.info(f"[{self.__class__.__name__}] use default OKResponse")
        else:
            self._response = DataResponse(result=self._result.model_dump(mode="json"))
            self.logger.info(f"[{self.__class__.__name__}] use default DataResponse")
