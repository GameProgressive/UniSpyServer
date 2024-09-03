from dataclasses import dataclass
from typing import Optional

from library.src.exceptions.general import UniSpyException


class RedisChannelBase:
    pass


# class RedisKey:
#     is_required = False
#     value: str | int

#     def __init__(self, value: str | int) -> None:
#         if (not isinstance(value, str)) or (not isinstance(value, int)):
#             raise UniSpyException("Redis key must be int or str")
#         self.value = value


DELIMETER = ":"


class RedisKeyValueObject:
    """
    The base class of redis keyvalue object

    -------
        create the redis key with RedisKey to identity that is a redis key
    """
    pass
    _keys: list[str]

    def _define_keys(self):
        """
        Override this function to define keys
        """
        if self._keys is None:
            raise UniSpyException(
                "You must set keys before creating class instance")
        if not all(isinstance(key, str) for key in self._keys):
            raise UniSpyException("all key must be str")

    def __init__(self):
        self._define_keys()

    def to_json(self) -> dict:
        """
        convert object to json serializable dict
        """
        import json
        try:
            output_dict = json.dumps(self)
        except:
            raise UniSpyException("all value must be python basic type")
        return output_dict

    # @staticmethod
    # def _check_is_basic_type(value):
    #     if not isinstance(value, str) or not isinstance(value, int) or not isinstance(value, float) or not isinstance(value, list) or isinstance():

    def get_full_key(self) -> str:
        """
        key format:
            key1 = value1; key2 = value2; key3=value3
        every property must have 
        """
        full_key = ""
        for key in self._keys:
            if key not in self.__dict__:
                # fmt: off
                raise UniSpyException(f"key: {key} do not have value, in order to build full key every key must have value")
                # fmt: on
            if self.__dict__[key] is None:
                # fmt: off
                raise UniSpyException(f"key: {key} can not be none, in order to build full key every key must have value")
                # fmt: on
            value = self.__dict__[key]
            full_key += f"{key}={value}"
            if key != self._keys[-1]:
                full_key += DELIMETER
            return full_key

    def get_search_key(self) -> str:
        """
        get keys using to search
        key format:
            key1=value1;key2=*;key3=value3
        """
        search_key = ""
        for key in self._keys:
            if key not in self.__dict__:
                search_key += f"{key}=*"

            if self.__dict__[key] is None:
                search_key += f"{key}=*"
            
            value = self.__dict__[key]
            search_key += f"{key}={value}"
            if key != self._keys[-1]:
                search_key += DELIMETER
            return search_key
