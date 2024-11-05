# from typing import TYPE_CHECKING, Optional


# class KeyValueManager:
#     data: dict
#     """
#     store the key and values
#     """

#     def __init__(self):
#         self.data = {}

#     def update(self, data: dict):
#         for key, value in data.items():
#             self.data[key] = value

#     def build_key_value_string(self, key_values: dict):
#         flags = ""
#         for key, value in key_values.items():
#             flags += f"\\{key}\\{value}"
#         return flags

#     def get_value_string(self, keys: list[str]) -> str:
#         values = ""
#         for key in keys:
#             if key in self.data:
#                 values += f"\\{self.data[key]}"
#             else:
#                 values += "\\"
#                 # Uncomment the line below to raise an exception if key is not found
#                 # raise Exception(f"Can not find key: {key}")
#         return values

#     def is_contain_all_key(self, keys: list[str]):
#         return all(key in self.data for key in keys)


# if TYPE_CHECKING:
#     from servers.chat.src.aggregates.channel import Channel


# class ChannelManager:
#     local_channels: dict = {}
#     """The code blow is for channel manage"""

#     @staticmethod
#     def get_channel(name: str) -> Optional["Channel"]:
#         if name in ChannelManager.local_channels:
#             return ChannelManager.local_channels[name]
#         return None

#     @staticmethod
#     def add_channel(channel: "Channel"):
#         if channel.name not in ChannelManager.local_channels:
#             ChannelManager.local_channels[channel.name] = channel

#     @staticmethod
#     def remove_channel(name: str) -> None:
#         if name in ChannelManager.local_channels:
#             del ChannelManager.local_channels[name]
