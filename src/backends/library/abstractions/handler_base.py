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


class HandlerBase:
    """
    The ultimate handler base of backend service
    """

    _request: RequestBase
    _result: ResultBase | None
    _response: Response | None
    """
    the response using to wrap data
    """
    # _data: object
    """
    the data get from database, can be any type
    """

    @property
    def response(self) -> dict | None:
        """
        the dict response which send to client
        """
        if self._response is None:
            return None
        return self._response.to_json_dict()

    def __init__(self, request: RequestBase) -> None:
        assert issubclass(type(request), RequestBase)
        self._request = request
        # decoupling the logging in home.py
        self.logger = logging.getLogger("backend")
        self._result = None
        self._response = None

    def handle(self) -> None:
        with Session(ENGINE) as session:
            self._session = session
            self._request_check()
            self._data_operate()
            self._session.commit()
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
            self._response = OKResponse()
        else:
            self._response = DataResponse(result=self._result.model_dump(mode="json"))
