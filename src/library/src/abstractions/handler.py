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
    """
    the response instance, initialize as None in __init__
    """
    _result_cls: "Type[ResultBase]"
    """
    the result type class, use to deserialize json data from backend\n
    the initialization of _result_cls must before call super().__init__()
    """
    _is_uploading: bool

    _is_feaching: bool

    _debug: bool = False
    """
    whether is in debug mode, if in debug mode exception will raise from handler
    """

    def __init__(self, client: "ClientBase", request: "RequestBase") -> None:

        assert issubclass(type(client), ClientBase)
        assert issubclass(type(request), RequestBase)
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
            if not hasattr(self, "_response"):
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
        self._prepare_data()
        self._upload_data()
        self._feach_data()

    def _prepare_data(self):
        self._temp_data = self._request.to_json()
        self._temp_data["server_id"] = str(
            self._client.server_config.server_id)
        self._temp_data["client_ip_endpoint"] = self._client.connection.ip_endpoint

    def _upload_data(self):
        """
        whether need send data to backend
        if child class do not require feach, overide this function to do nothing
        """
        url = f"{CONFIG.backend.url}/GameSpy/{
            self._client.server_config.server_name}/{self.__class__.__name__}/"

        response = requests.post(url, json=self._temp_data)
        if response.status_code != 200:
            raise UniSpyException("Upload data to background failed.")
        self._http_result = response.json()

    def _feach_data(self):
        """
        whether need get data from backend.
        if child class do not require feach, overide this function to do nothing
        """
        if not hasattr(self, "_result_cls"):
            raise UniSpyException(
                "_result should be initialized when feach data")

        if self._result_cls is None:
            raise UniSpyException("_result should not be null when feach data")

        assert issubclass(self._result_cls, ResultBase)
        self._result = self._result_cls(**self._http_result)

    def _response_construct(self) -> None:
        """construct response here in specific child class"""
        pass

    def _response_send(self) -> None:
        """
        virtual function, can be override
        Send response back to client, this is a virtual function which can be override only by child class
        """
        self._client.send(self._response)

    def _handle_exception(self, ex: Exception) -> None:
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
            print(self)
        else:
            self._client.log_current_class(self)
