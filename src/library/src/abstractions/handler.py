from library.src.abstractions.client import ClientBase
from library.src.exceptions.general import UniSpyException
from typing import Type
import requests

from library.src.configs import CONFIG

# if TYPE_CHECKING:
from library.src.abstractions.contracts import RequestBase, ResultBase, ResponseBase


class CmdHandlerBase:
    _client: "ClientBase"
    _request: "RequestBase"
    _result: "ResultBase"
    _response: "ResponseBase"
    _result_cls: "Type[ResultBase]"
    """
    the result type class, use to deserialize json data from backend
    """
    _is_uploading: bool
    """
    whether need send data to backend
    """
    _is_feaching: bool
    """
    whether need get data from backend
    """
    _debug: bool = False
    """
    whether is in debug mode, if in debug mode exception will raise from handler
    """

    def __init__(self, client: "ClientBase", request: "RequestBase") -> None:

        assert issubclass(type(client), ClientBase)
        assert issubclass(type(request), RequestBase)
        # if some subclass do not need result, override the __init__() in that subclass
        if not hasattr(self, "_is_feaching"):
            self._is_feaching = True
        if not hasattr(self, "_is_uploading"):
            self._is_uploading = True
        if self._is_feaching:
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
        # we check whether we need fetch data
        if not self._is_uploading:
            return

        # default use restapi to access to our backend service
        # get the http response and create it with this type
        # http://127.0.0.1:8080/gamespy/pcm/login/

        # fmt: off

        url = f"{CONFIG.backend.url}/GameSpy/{self._client.server_config.server_name}/{self.__class__.__name__}/"

        # fmt: on
        data = self._request.to_json()
        data["server_id"] = str(self._client.server_config.server_id)

        response = requests.post(url, json=data)
        result = response.json()
        # if the result cls is not declared, we do not parse the response values

        if self._is_feaching:
            self._result = self._result_cls(**result)

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
        """
        override in child class if there are different exception handling behavior
        """
        UniSpyException.handle_exception(ex, self._client)
        # if we are debugging the app we re-raise the exception
        if CmdHandlerBase._debug:
            raise ex

    def _log_current_class(self) -> None:
        if self._client is None:
            # todo
            # self._client.log_current_class(self)
            print(self)
        else:
            self._client.log_current_class(self)
