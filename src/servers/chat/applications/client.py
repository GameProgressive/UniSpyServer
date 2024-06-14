from library.abstractions.client import ClientBase
from typing import TYPE_CHECKING

if TYPE_CHECKING:
    from servers.chat.aggregates.channel import Channel


class ClientInfo:
    previously_joined_channel: "Channel"
    joined_channels: list["Channel"]
    nick_name: str
    gamename: str


class Client(ClientBase):
    info: ClientInfo = ClientInfo()
    pass
