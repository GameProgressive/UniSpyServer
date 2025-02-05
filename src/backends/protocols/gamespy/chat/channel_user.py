# from uuid import UUID

# from backends.protocols.gamespy.chat.managers import KeyValueManager
# from frontends.gamespy.protocols.chat.applications.client import Client
# from typing import TYPE_CHECKING

# if TYPE_CHECKING:
#     from frontends.gamespy.protocols.chat.aggregates.channel import Channel


# class ChannelUser:
#     server_id: UUID
#     is_voiceable: bool = True
#     is_channel_operator: bool = False
#     is_channel_creator: bool = False
#     remote_ip: str
#     remote_port: int
#     client: Client
#     kv_manager: KeyValueManager = KeyValueManager()
#     channel: "Channel"

#     @property
#     def modes(self):
#         buffer = ""
#         if self.is_channel_creator:
#             buffer += "@"
#         if self.is_voiceable:
#             buffer += "+"
#         return buffer

#     def __init__(self, client: Client, channel: "Channel") -> None:
#         self.client = client
#         self.channel = channel
#         self.server_id = client.server_config.server_id
#         self.remote_ip = client.connection.remote_ip
#         self.remote_port = client.connection.remote_port
