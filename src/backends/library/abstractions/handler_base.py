from typing import final
from backends.library.abstractions.contracts import (
    DataResponse,
    OKResponse,
    RequestBase,
    Response,
)
from backends.library.database.pg_orm import ENGINE
from frontends.gamespy.library.abstractions.contracts import ResultBase

import logging
from sqlalchemy.orm import Session

from frontends.gamespy.library.exceptions.general import UniSpyException


class HandlerBase:
    """
    The ultimate handler base of backend service
    """

    _request: RequestBase
    _result: ResultBase | None
    response: Response
    """
    response is created by child class annotation
    the response using to wrap data
    """
    # _data: object
    """
    the data get from database, can be any type
    """

    def __init__(self, request: RequestBase) -> None:
        assert issubclass(type(request), RequestBase)
        self._request = request
        # decoupling the logging in home.py
        self.logger = logging.getLogger("backend")
        self._result = None
        self.response = OKResponse()

    def handle(self) -> None:
        with Session(ENGINE) as session:
            self._session = session
            self._request_check()
            self._data_operate()
            self._result_construct()
            self._response_construct()

    def _request_check(self) -> None:
        """virtual method"""

    def _data_operate(self) -> None:
        """virtual method\n
        override by child class to perform database operations
        """

    def _result_construct(self) -> None:
        """virtual method\n
        can override by child class to create self._result
        """

    @final
    def _response_construct(self) -> None:
        """
        _response_construct can not be overrided
        """
        # if there are no result, we send ok response
        if self._result is None:
            return
        if "response" not in self.__class__.__annotations__:
            raise UniSpyException(
                "write response type annotation in child class to create response instance.")
        response_cls = self.__class__.__annotations__['response']
        if not issubclass(response_cls, DataResponse):
            raise UniSpyException(
                "response type annotation must be a subclass of DataResponse")
        self.response = response_cls(result=self._result)
        # self.response = DataResponse(result=self._result)
