import abc
from library.src.abstractions.client import ClientBase
from library.src.exceptions.error import UniSpyException
from typing import TYPE_CHECKING, List
from typing import Type


if TYPE_CHECKING:
    from library.src.abstractions.contracts import RequestBase, ResultBase, ResponseBase


class CmdHandlerBase(abc.ABC):

    _client: "ClientBase"
    _request: "RequestBase"
    _result: "ResultBase" = None
    _response: "ResponseBase" = None
    _backend_url: str
    _result_type: Type
    """
    store the backend url
    """

    def __init__(self, client: "ClientBase", request: "RequestBase") -> None:
        assert issubclass(type(client), ClientBase)
        assert issubclass(type(request), RequestBase)
        self._client = client
        self._request = request

    def handle(self) -> None:
        try:
            # we first log this class
            self.__log_current_class()
            # then we handle it
            self._request_check()
            self._data_operate()
            self._response_construct()
            if self._response is None:
                return
        except Exception as ex:
            self._handle_exception(ex)

    @abc.abstractmethod
    def _request_check(self) -> None:
        # if there is gamespy raw request we convert it to unispy request
        if self._request.raw_request is not None:
            self._request._parse_raw()

    @abc.abstractmethod
    def _data_operate(self) -> None:
        "default use restapi to access to our backend service"
        if self._result is not None:
            result_type = type(self._result)
            # get the http response and create it with this type
        pass

    @abc.abstractmethod
    def _response_construct(self) -> None:
        """construct response here in specific child class"""
        pass

    @abc.abstractmethod
    def _response_send(self) -> None:
        """
        Send response back to client, this is a virtual function which can be override only by child class
        """
        self._client.send(self._response)

    def _handle_exception(self, ex) -> None:
        UniSpyException.handle_exception(ex, self._client)

    def _log_current_class(self) -> None:
        if self._client is None:
            self._client.log_current_class(self)
        else:
            self._client.log_current_class(self)
