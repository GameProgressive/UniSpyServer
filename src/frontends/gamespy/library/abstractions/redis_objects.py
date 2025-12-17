# from dataclasses import dataclass, field
# from datetime import datetime
# # import threading
# import asyncio
# 

# from redis import Redis


# from frontends.gamespy.library.exceptions.general import UniSpyException

# import json


# class RedisJsonEncoder(json.JSONEncoder):
#     def default(self, obj):
#         if isinstance(obj, datetime):
#             return str(obj)


# class RedisJsonDecoder(json.JSONDecoder):
#     pass


# @dataclass
# class RedisQuery:
#     key: str
#     value: object


# @dataclass
# class RedisKey:
#     type: "type"
#     name: str

#     def __eq__(self, value: object) -> RedisQuery:
#         if self.type is not type(value):
#             raise ValueError("The value type is not equal to redis key type")
#         # return (self.name, value)
#         return RedisQuery(self.name, value)

#     def __ne__(self, value: object) -> bool:
#         raise UniSpyException(
#             "Redis key attribute do not have this method")

#     def __add__(self, other):
#         raise UniSpyException(
#             "Redis key attribute do not have this method")

#     def __lt__(self, other):
#         raise UniSpyException(
#             "Redis key attribute do not have this method")

#     def __gt__(self, other):
#         raise UniSpyException(
#             "Redis key attribute do not have this method")

#     def __ge__(self, other):
#         raise UniSpyException(
#             "Redis key attribute do not have this method")

#     def __le__(self, other):
#         raise UniSpyException(
#             "Redis key attribute do not have this method")


# class RedisKeyValueObject:
#     _DELIMETER = ":"
#     _VALID_TYPE = [int, float, str, datetime]
#     """
#     The base class of redis keyvalue object

#     -------
#         create the redis key with RedisKey to identity that is a redis key
#     """
#     _database: int
#     """
#     Init _database in child class __post_init__(self)
#     """
#     _redis_client: Redis
#     _expire_time: int
#     """
#     expire time seconds
#     """

#     def __post_init__(self):
#         if not hasattr(self, "_database"):
#             raise UniSpyException(
#                 "You have to initailize _database in child class")
#         self._get_all_redis_key()

#     @property
#     def _keys(self):
#         keys = []
#         for k, v in type(self).__dict__.items():
#             if type(v) is RedisKey:
#                 keys.append(k)
#         return keys

#     def _get_all_redis_key(self):
#         for k, v in type(self).__dict__["__annotations__"].items():
#             if v not in RedisKeyValueObject._VALID_TYPE:
#                 raise UniSpyException(f"type: {v} is not allowed")
#             setattr(type(self), k, RedisKey(type=v, name=k))

#     def to_json_str(self) -> str:
#         """
#         convert object to RedisKeyValueObjectjson serializable dict
#         """
#         import json
#         try:
#             output_dict = json.dumps(self)
#         except:
#             raise UniSpyException("all value must be python basic type")
#         return output_dict

#     def build_full_key(self) -> str:
#         """
#         key format:
#             key1 = value1; key2 = value2; key3=value3
#         every property mus__init__t have
#         """
#         full_key = f"db={self._database}:"

#         for index in range(len(self._keys)):
#             key = self._keys[index]
#             if key not in self.__dict__:
#                 raise ValueError(
#                     f"key: {key} is not initialized, in order to build full key every key must have value")
#             value = self.__dict__[key]
#             if value is None:
#                 # fmt: r.keys('*')off
#                 raise UniSpyException(
#                     f"key: {key} can not be none, in order to build full key every key must have value")
#                 # fmt: on
#             full_key += f"{key}={value}"
#             if index != len(self._keys)-1:
#                 full_key += RedisKeyValueObject._DELIMETER
#         return full_key

#     def build_search_key(self) -> str:
#         """
#         get keys using to search
#         key format:
#             key1=value1;key2=*;key3=value3
#         """
#         search_key = f"db={self._database}:"
#         for index in range(len(self._keys)):
#             key = self._keys[index]
#             if key not in self.__dict__:
#                 search_key += f"{key}=*"

#             if self.__dict__[key] is None:
#                 search_key += f"{key}=*"
#             else:
#                 value = self.__dict__[key]
#                 search_key += f"{key}={value}"

#             if index != len(self._keys)-1:
#                 search_key += RedisKeyValueObject._DELIMETER
#         return search_key

#     def _build_search_object(self, query_dict: dict[str, object]) -> "RedisKeyValueObject":
#         param_dict = {}
#         for k in self._keys:
#             if k not in query_dict:
#                 param_dict[k] = None
#             else:
#                 param_dict[k] = query_dict[k]
#         q_obj = type(self)(**param_dict)
#         return q_obj

#     def is_all_instance(self, data: list):
#         for d in data:
#             if not isinstance(d, RedisQuery):
#                 raise UniSpyException(
#                     "The queries list contain non RedisQuery object")

#         self._database = 0

#     def convert_query_to_dict(self, queries: list[RedisQuery]) -> dict[str, object]:
#         query_dict = {}
#         for q in queries:
#             if q.key not in query_dict.keys():
#                 query_dict[q.key] = q.value
#             else:
#                 raise UniSpyException(
#                     f"The query: {q.key} is duplicated, check your query syntax")

#         return query_dict

#     def query(self, queries: list[RedisQuery]) -> list["RedisKeyValueObject"]:
#         self.is_all_instance(queries)
#         q_dict = self.convert_query_to_dict(queries)
#         obj = self._build_search_object(q_dict)
#         search_key = obj.build_search_key()
#         db_keys = self._redis_client.keys(search_key)
#         results: list = self._redis_client.mget(db_keys)  # type: ignore

#         objs: list["RedisKeyValueObject"] = []
#         if results is not None:
#             for r in results:
#                 r_dict = json.loads(r)
#                 objs.append(type(self)(**r_dict))
#         return objs

#     def count(self,  queries: list[RedisQuery]) -> int:
#         self.is_all_instance(queries)
#         param_dict = {}
#         for q in queries:
#             param_dict[q.key] = q.value
#         obj = type(self)(**param_dict)
#         search_key = obj.build_search_key()
#         db_keys: list = self._redis_client.keys(search_key)  # type: ignore
#         return len(db_keys)

#     def first(self,  queries: list[RedisQuery]) -> Optional["RedisKeyValueObject"]:
#         self.is_all_instance(queries)
#         param_dict = {}
#         for q in queries:
#             param_dict[q.key] = q.value
#         obj = type(self)(**param_dict)
#         search_key = obj.build_search_key()
#         db_keys = self._redis_client.keys(search_key)
#         if len(db_keys) == 0:
#             result = None
#         else:
#             result = self._redis_client.get(db_keys[0])
#         return result

#     def async_count(self,  queries: list[RedisQuery]) -> int:
#         loop = asyncio.get_event_loop()
#         result = loop.run_in_executor(None, self.count, queries)
#         return result

#     def async_query(self, queries: list[RedisQuery]) -> list["RedisKeyValueObject"]:
#         loop = asyncio.get_event_loop()
#         result = loop.run_in_executor(None, self.query, queries)
#         return result

#     def async_first(self, queries: list[RedisQuery]) -> Optional["RedisKeyValueObject"]:
#         loop = asyncio.get_event_loop()
#         result = loop.run_in_executor(None, self.first, queries)
#         return result


# @dataclass
# class TestKvObject(RedisKeyValueObject):
#     param1: int
#     param2: int

#     def __post_init__(self):
#         self._database = 0
#         return super().__post_init__()


# t = TestKvObject(param1=1, param2=2)
# t.query([TestKvObject.param1 == 1])
# t.build_full_key()
# t.build_search_key()
# t2 = TestKvObject(param1=1, param2=None)
# t2.build_search_key()
