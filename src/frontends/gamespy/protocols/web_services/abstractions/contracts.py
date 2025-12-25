import frontends.gamespy.library.abstractions.contracts as lib
from frontends.gamespy.library.network.http_handler import HttpData
from frontends.gamespy.protocols.web_services.aggregations.exceptions import WebException
from frontends.gamespy.protocols.web_services.aggregations.soap_envelop import SoapEnvelop
import xmltodict


def remove_namespace(data):
    if isinstance(data, dict):
        return {key.split(':')[-1]: remove_namespace(value) for key, value in data.items()}
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
            found_values.extend(find_key_in_nested_dict(
                value, target_key))  # Search recursively

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


# class HttpData:
#     path: str
#     headers: dict
#     body: str

#     def __init__(self, path: str, headers: dict, body: str) -> None:

#         self.path = path
#         self.headers = headers
#         self.body = body

#     def __str__(self) -> str:
#         json_str = json.dumps(self.__dict__)
#         return json_str

#     @staticmethod
#     def from_bytes(buffer: bytes) -> "HttpData":
#         assert isinstance(buffer, bytes)
#         json_dict = json.loads(buffer)
#         data = HttpData(**json_dict)
#         return data

#     @staticmethod
#     def from_str(buffer: str) -> "HttpData":
#         assert isinstance(buffer, str)
#         json_dict = json.loads(buffer)
#         data = HttpData(**json_dict)
#         return data

#     def to_bytes(self) -> bytes:
#         j_str = json.dumps(self.__dict__)
#         j_bytes = j_str.encode()
#         return j_bytes


class RequestBase(lib.RequestBase):
    raw_request: HttpData
    _request_dict: dict

    def __init__(self, raw_request: HttpData) -> None:
        assert isinstance(raw_request, HttpData)
        super().__init__(raw_request)

    def parse(self) -> None:
        parsed_data = xmltodict.parse(self.raw_request.body)
        processed_data = remove_namespace(parsed_data)
        assert isinstance(processed_data, dict)
        self._request_dict = processed_data["Envelope"]["Body"]
        self.command_name = list(self._request_dict.keys())[0]

    def _get_int(self, attr_name: str) -> int:
        result_str = RequestBase._get_str(self, attr_name)
        result = int(result_str)
        return result

    def _get_str(self, attr_name: str) -> str:
        assert isinstance(attr_name, str)
        value = self._get_value_by_key(attr_name)
        if value is None:
            raise WebException(f"{attr_name} is missing")
        assert isinstance(value, str)
        return value

    def _get_value_by_key(self, key: str) -> object | None:
        value = find_first_key_in_nested_dict(self._request_dict, key)
        return value

    def _get_value(self, attr_name: str) -> object:
        value = self._get_value_by_key(attr_name)
        if value is None:
            raise WebException(f"{attr_name} is missing")
        return value

    def _get_dict(self, attr_name: str) -> dict:
        value = self._get_value_by_key(attr_name)
        if value is None:
            raise WebException(f"{attr_name} is missing")
        if not isinstance(value, dict):
            raise WebException(f"{attr_name} is not a dict")
        return value


class ResultBase(lib.ResultBase):
    pass


class ResponseBase(lib.ResponseBase):
    _content: SoapEnvelop
    """
    Soap envelope content, should be initialized in response sub class
    """
    _result: ResultBase
    sending_buffer: HttpData

    def __init__(self, result: ResultBase) -> None:
        assert issubclass(type(result), ResultBase)
        super().__init__(result)

    def build(self) -> None:
        self.sending_buffer = HttpData(body=str(self._content))
