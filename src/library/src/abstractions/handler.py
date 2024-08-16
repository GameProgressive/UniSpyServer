import abc
from library.src.abstractions.client import ClientBase
from library.src.exceptions.error import UniSpyException
from typing import TYPE_CHECKING
from typing import Type
import requests

from library.src.unispy_server_config import CONFIG

# if TYPE_CHECKING:
from library.src.abstractions.contracts import RequestBase, ResultBase, ResponseBase


class CmdHandlerBase(abc.ABC):

    _client: "ClientBase"
    _request: "RequestBase"
    _result: "ResultBase"
    _response: "ResponseBase"
    _backend_url: "str"
    """
    store the backend url
    """
    _result_cls: "Type[ResultBase]"
    """
    the result class type
    """

    def __init__(self, client: "ClientBase", request: "RequestBase") -> None:

        if self._backend_url is None:
            raise UniSpyException(
                "The backend url and result_cls should not be None or not None at same time")

        assert issubclass(type(client), ClientBase)
        assert issubclass(type(request), RequestBase)
        # if some subclass do not need result, override the __init__() in that subclass
        if self._result_cls is not None:
            assert issubclass(self._result_cls, ResultBase)

        self._client = client
        self._request = request

    def handle(self) -> None:
        try:
            # we first log this class
            self._log_current_class()
            # then we handle it
            self._request_check()
            self._data_operate()
            self._response_construct()
            if self._response is None:
                return
            self._response_send()
        except Exception as ex:
            self._handle_exception(ex)

    def _request_check(self) -> None:
        """
        virtual function, can be override
        """
        # if there is gamespy raw request we convert it to unispy request
        if self._request.raw_request is not None:
            self._request.parse()

    def _data_operate(self) -> None:
        """
        virtual function, can be override
        """

        # default use restapi to access to our backend service

        # get the http response and create it with this type
        url = f"{
            CONFIG.backend.url}/{self._client.server_config.server_name}/{self.__class__.__name__}/"
        data = self._request.to_serializable_dict()
        data["server_id"] = str(self._client.server_config.server_id)

        response = requests.post(url, json=data)
        result = response.json()
        # if the result cls is not declared, we do not parse the response values

        if self._result_cls is None:
            return
        if self._result is not None:
            return
        self._result = self._result_cls(**result)
        pass

    def _response_construct(self) -> None:
        """construct response here in specific child class"""
        pass

    def _response_send(self) -> None:
        """
        virtual function, can be override
        Send response back to client, this is a virtual function which can be override only by child class
        """
        self._client.send(self._response)

    def _handle_exception(self, ex) -> None:
        UniSpyException.handle_exception(ex, self._client)

    def _log_current_class(self) -> None:
        if self._client is None:
            # todo
            # self._client.log_current_class(self)
            print(self)
        else:
            self._client.log_current_class(self)
