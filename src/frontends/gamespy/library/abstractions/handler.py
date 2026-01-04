import json
from frontends.gamespy.library.abstractions.client import ClientBase
from frontends.gamespy.library.exceptions.general import UniSpyException
from typing import final
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
    _result: "ResultBase | None"
    """
    the result instance, create by annotation in child class
    """
    _response: "ResponseBase | None"
    """
    the response instance, create by annotation in child class
    """
    _result_cls: type["ResultBase"] | None
    """
    the result class type, create by annotation in child class
    """
    _response_cls: type["ResponseBase"] | None
    """
    the response class type, create by annotation in child class
    """
    _debug: bool = False
    """
    whether is in debug mode, if in debug mode exception will raise from handler
    """
    _is_uploading: bool
    _is_fetching: bool
    """
    this is auto assigned variable
    """
    _exceptions_mapping: dict[str, type[UniSpyException]]

    def __init__(self, client: "ClientBase", request: "RequestBase") -> None:
        assert issubclass(type(client), ClientBase)
        assert issubclass(type(request), RequestBase)
        self._result = None
        self._response = None
        self._client = client
        self._request = request
        # create class type by annotation
        self._get_property_types()
        # get the exception dict
        self._get_exception_mappings()
        # set whether need backend featching
        self._set_fetching()

    def _set_fetching(self):
        self._is_uploading = True
        if self._result_cls is None:
            self._is_fetching = False
        else:
            self._is_fetching = True

    def _get_exception_mappings(self):
        if "_exceptions_mapping" in self.__class__.__annotations__:
            if len(self._exceptions_mapping) == 0:
                raise UniSpyException(
                    "init _exceptions_mapping in child class")

    def _get_property_types(self):
        """
        get result and response type by annotations
        """
        if "_result" in self.__class__.__annotations__:
            self._result_cls = self.__class__.__annotations__['_result']
            if self._result_cls == ResultBase:
                self._result_cls = None
        else:
            self._result_cls = None
        if "_response" in self.__class__.__annotations__:
            self._response_cls = self.__class__.__annotations__['_response']
            if self._response_cls == ResponseBase:
                self._response_cls = None
        else:
            self._response_cls = None

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
        raw request is nessesary param
        """
        # if there is gamespy raw request we convert it to unispy request
        self._request.parse()

    def _data_operate(self) -> None:
        """
        virtual function, can be override
        """
        if self._is_uploading:
            self._prepare_data()
            self._upload_data()
            if self._is_fetching:
                self._fetch_data()

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

    @final
    def _upload_data(self):
        """
        whether need send data to backend
        if child class do not require fetch, overide this function to do nothing
        """
        self._url = self._get_url()
        json_str = json.dumps(
            self._temp_data, cls=UniSpyJsonEncoder, ensure_ascii=False
        )
        self._client.log_network_upload(f"[{self._url}] {json_str}")
        try:
            response = requests.post(
                self._url, data=json_str, headers={
                    "Content-Type": "application/json"}
            )
        except requests.exceptions.ConnectionError:
            if CONFIG.unittest.is_raise_except:
                raise UniSpyException(
                    f"backends api for {[self.__class__.__name__]} is not mocked"
                )
            else:
                raise UniSpyException("backends is not avaliable")

        if response.status_code in [200, 450, 500]:
            self._http_result = response.json()
            # immidiatly show the http response
            self._client.log_network_fetch(
                f"[{self._url}] {self._http_result}")

        else:
            raise UniSpyException(
                f"failed to upload data to backends. reason: {response.text}"
            )

        match response.status_code:
            case 200:
                pass
            case 450:
                self._handle_unispy_error()
            case 500:
                self._handle_general_error()
            case _:
                raise UniSpyException(
                    f"failed to upload data to backends. reason: {response.text}"
                )

    def _handle_unispy_error(self):
        """
        handle the error message response from backend
        """
        # we raise the error as UniSpyException
        exp_name = self._http_result["exception_name"]
        if exp_name in self._exceptions_mapping:
            exp_cls = self._exceptions_mapping[exp_name]
            init_params = exp_cls.get_init_params(
                exp_cls, self._http_result["exception_data"])
            exp_instance = exp_cls(**init_params)
            raise exp_instance
        else:
            self._client.log_warn(
                "no exception class found, use default unispy exception")
            raise UniSpyException(self._http_result["message"])

    def _handle_general_error(self):
        raise UniSpyException(self._http_result["message"])

    @final
    def _fetch_data(self):
        """
        whether need get data from backend.
        if child class do not require feach, overide this function to do nothing
        """
        if self._result_cls is None:
            raise UniSpyException(
                "_result_cls can not be null when feach data.")

        assert issubclass(self._result_cls, ResultBase)
        if "result" not in self._http_result:
            raise UniSpyException("result can not be empty when feach data")
        self._result = self._result_cls(**self._http_result["result"])

    def _response_construct(self) -> None:
        """construct response here in specific child class"""
        if self._response_cls is not None:
            if self._result is None:
                raise UniSpyException(
                    "result instance is required for response construction")
            self._response = self._response_cls(result=self._result)

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

    @final
    def _log_current_class(self) -> None:
        if self._client is None:
            # todo
            GLOBAL_LOGGER.debug(f"=> <{self.__class__.__name__}>")
        else:
            self._client.log_current_class(self)
