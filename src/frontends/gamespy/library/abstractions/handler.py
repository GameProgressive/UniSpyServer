import json
from frontends.gamespy.library.abstractions.client import ClientBase
from frontends.gamespy.library.exceptions.general import UniSpyException
from typing import Optional, Type
import requests

from frontends.gamespy.library.configs import CONFIG

# if TYPE_CHECKING:
from frontends.gamespy.library.abstractions.contracts import (
    RequestBase,
    ResultBase,
    ResponseBase,
)
from frontends.gamespy.library.extentions.encoding import UniSpyJsonEncoder
from frontends.gamespy.library.log.log_manager import GLOBAL_LOGGER


class CmdHandlerBase:
    _client: "ClientBase"
    _request: "RequestBase"
    _result: Optional["ResultBase"]
    _response: Optional["ResponseBase"]
    """
    the response instance, initialize as None in __init__
    """
    _result_cls: Optional["Type[ResultBase]"]
    """
    the result type class, use to deserialize json data from backend\n
    the initialization of _result_cls must after call super().__init__()
    """
    _debug: bool = False
    """
    whether is in debug mode, if in debug mode exception will raise from handler
    """
    _is_uploading: bool
    _is_fetching: bool

    def __init__(self, client: "ClientBase", request: "RequestBase") -> None:
        assert issubclass(type(client), ClientBase)
        assert issubclass(type(request), RequestBase)
        self._client = client
        self._request = request
        self._response = None
        self._result_cls = None
        self._result = None
        self._is_uploading = True
        self._is_fetching = True

    def handle(self) -> None:
        try:
            # we first log this class
            self._log_current_class()
            # then we handle it
            self._request_check()
            if CONFIG.unittest.is_collect_request:
                return
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
        self._prepare_data()
        if self._is_uploading:
            self._upload_data()
        if self._is_fetching:
            self._feach_data()

    def _prepare_data(self):
        self._temp_data = self._request.to_dict()
        if "server_id" in self._temp_data:
            raise UniSpyException("server_id name collision in dict")
        if "client_ip" in self._temp_data:
            raise UniSpyException("client_ip name collision in dict")
        if "client_port" in self._temp_data:
            raise UniSpyException("client_port name collision in dict")
        # add the server info to json request dict
        self._temp_data["client_ip"] = self._client.connection.remote_ip
        self._temp_data["server_id"] = self._client.server_config.server_id
        self._temp_data["client_port"] = self._client.connection.remote_port

    def _get_url(self) -> str:
        url = f"{CONFIG.backend.url}/GameSpy/{self._client.server_config.server_name}/{
            self.__class__.__name__
        }"
        return url

    def _upload_data(self):
        """
        whether need send data to backend
        if child class do not require feach, overide this function to do nothing
        """
        self._url = self._get_url()
        json_str = json.dumps(
            self._temp_data, cls=UniSpyJsonEncoder, ensure_ascii=False
        )
        self._client.log_network_upload(f"[{self._url}] {json_str}")
        try:
            response = requests.post(
                self._url, data=json_str, headers={"Content-Type": "application/json"}
            )
        except requests.exceptions.ConnectionError:
            if CONFIG.unittest.is_raise_except:
                raise UniSpyException(
                    f"backends api for {[self.__class__.__name__]} is not mocked"
                )
            else:
                raise UniSpyException("backends is not avaliable")

        if response.status_code != 200:
            raise UniSpyException(
                f"failed to upload data to backends. reason: {response.text}"
            )

        self._http_result = response.json()

        if "error" in self._http_result:
            self._handle_upload_error()

    def _handle_upload_error(self):
        """
        handle the error message response from backend
        """
        # we raise the error as UniSpyException
        raise UniSpyException(self._http_result["error"])

    def _feach_data(self):
        """
        whether need get data from backend.
        if child class do not require feach, overide this function to do nothing
        """
        if self._result_cls is None:
            raise UniSpyException("_result_cls can not be null when feach data.")
        assert issubclass(self._result_cls, ResultBase)
        self._client.log_network_fetch(f"[{self._url}] {self._http_result}")

        self._result = self._result_cls(**self._http_result)

    def _response_construct(self) -> None:
        """construct response here in specific child class"""
        pass

    def _response_send(self) -> None:
        """
        virtual function, can be override
        Send response back to client, this is a virtual function which can be override only by child class
        """
        assert isinstance(self._response, ResponseBase)
        self._client.send(self._response)

    def _handle_exception(self, ex: Exception) -> None:
        """
        override in child class if there are different exception handling behavior
        """
        UniSpyException.handle_exception(ex, self._client)

    def _log_current_class(self) -> None:
        if self._client is None:
            # todo
            GLOBAL_LOGGER.debug(f"=> <{self.__class__.__name__}>")
        else:
            self._client.log_current_class(self)
