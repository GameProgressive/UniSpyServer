# from typing import TYPE_CHECKING, Optional


class KeyValueManager:
    """
    Handle key value string
    """
    @staticmethod
    def update(data: dict):
        for key, value in data.items():
            data[key] = value

    @staticmethod
    def build_key_value_string(key_values: dict):
        flags = ""
        for key, value in key_values.items():
            flags += f"\\{key}\\{value}"
        return flags

    @staticmethod
    def get_value_string(data: dict, keys: list[str]) -> str:
        values = ""
        for key in keys:
            if key in data:
                values += f"\\{data[key]}"
            else:
                values += "\\"
                # Uncomment the line below to raise an exception if key is not found
                # raise Exception(f"Can not find key: {key}")
        return values

    @staticmethod
    def is_contain_all_key(data: dict, keys: list[str]):
        return all(key in data for key in keys)


# if TYPE_CHECKING:
#     from frontends.gamespy.protocols.chat.aggregates.channel import Channel


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
