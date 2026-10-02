from pydantic import Field
import xmltodict

import frontends.gamespy.library.abstractions.contracts as lib
from frontends.gamespy.protocols.web_services.aggregations.exceptions import (
    WebException,
)
from frontends.gamespy.protocols.web_services.aggregations.soap_envelop import (
    SoapEnvelop,
)
from frontends.gamespy.protocols.web_services.modules.sake.aggregates.enums import CommandName


def remove_namespace(data) -> list | dict:
    if isinstance(data, dict):
        return {
            key.split(":")[-1]: remove_namespace(value) for key, value in data.items()
        }
    elif isinstance(data, list):
        return [remove_namespace(item) for item in data]
    else:
        return data


def find_key_in_nested_dict(nested_dict, target_key):
    found_values = []

    # Base case: If the input is a dictionary
    if isinstance(nested_dict, dict):
        for key, value in nested_dict.items():
            if key == target_key:
                found_values.append(value)  # Add the value if the key is found
            found_values.extend(
                find_key_in_nested_dict(value, target_key)
            )  # Search recursively

    # If the input is a list, iterate through elements
    elif isinstance(nested_dict, list):
        for item in nested_dict:
            found_values.extend(find_key_in_nested_dict(item, target_key))

    return found_values


def find_first_key_in_nested_dict(nested_dict, target_key) -> object | None:
    # Base case: If the input is a dictionary
    if isinstance(nested_dict, dict):
        for key, value in nested_dict.items():
            if key == target_key:
                return value  # Return the value if the key is found
            # Recursively search in the value
            found_value = find_first_key_in_nested_dict(value, target_key)
            if found_value is not None:
                return found_value  # Return the found value if any

    # If the input is a list, iterate through elements
    elif isinstance(nested_dict, list):
        for item in nested_dict:
            found_value = find_first_key_in_nested_dict(item, target_key)
            if found_value is not None:
                return found_value  # Return found value if any

    return None  # Return None if the key is not found


class RequestBase(lib.RequestBase):
    raw_request: lib.HttpData = Field(repr=False)
    request_dict: dict[str, object] = Field(repr=False)
    command_name: CommandName

    @property
    def str_mapping(self) -> dict[str, str]:
        """
        type checking trick
        """
        return self.request_dict  # type: ignore

    @property
    def int_mapping(self) -> dict[str, int]:
        """
        type checking trick
        """
        return self.request_dict  # type: ignore

    @property
    def list_mapping(self) -> dict[str, list]:
        """
        type checking trick
        """
        return self.request_dict  # type: ignore

    @property
    def dict_mapping(self) -> dict[str, dict]:
        """
        type checking trick
        """
        return self.request_dict  # type: ignore

    @staticmethod
    def gs_parse(raw_request: lib.HttpData) -> "RequestBase":
        data = lib.RequestBase.gs_parse(raw_request)
        parsed_data = xmltodict.parse(raw_request.body)
        processed_data = remove_namespace(parsed_data)
        assert isinstance(processed_data, dict)
        body = processed_data["Envelope"]["Body"]
        data.command_name = next(iter(body))
        request_dict = body[data.command_name]
        data = RequestBase(**data.model_dump(), request_dict=request_dict)
        return data


class ResultBase(lib.ResultBase):
    headers: dict[str, str]
    command_name: CommandName

    pass


class ResponseBase(lib.ResponseBase):
    _content: SoapEnvelop
    """
    Soap envelope content, should be initialized in response sub class
    """
    _result: ResultBase
    sending_buffer: lib.HttpData

    def __init__(self, result: ResultBase) -> None:
        assert issubclass(type(result), ResultBase)
        super().__init__(result)

    def build(self) -> None:
        self.sending_buffer = lib.HttpData(
            path=None, headers=self._result.headers, body=str(self._content)
        )
