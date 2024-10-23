from dataclasses import dataclass
import datetime
from typing import Optional, overload
from uuid import UUID

from pydantic import BaseModel, field_validator
from library.src.network.brockers import WebsocketBrocker
from library.src.configs import CONFIG
from servers.chat.src.abstractions.contract import ResponseBase
from servers.chat.src.aggregates.channel_user import ChannelUser
from servers.chat.src.aggregates.key_value_manager import KeyValueManager
from servers.chat.src.aggregates.peer_room import PeerRoom
from servers.chat.src.applications.client import Client
from servers.chat.src.contracts.requests.channel import ModeRequest
from servers.chat.src.enums.peer_room import PeerRoomType
from servers.chat.src.exceptions.general import ChatException
from servers.server_browser.src.v2.aggregations.server_info_builder import PEER_GROUP_LIST

MIN_CHANNEL_NAME_LENGTH = 4


class Channel:
    """
    The channel class, every channel class manage a brocker
    """
    server_id: UUID
    game_name: str
    name: str
    max_num_user: int = 200
    create_time: datetime.datetime = datetime.datetime.now(
        datetime.timezone.utc)
    kv_manager: KeyValueManager = KeyValueManager()
    room_type: PeerRoomType
    password: Optional[str]
    topic: Optional[str] = None
    group_id: Optional[int] = None
    room_name: Optional[str] = None
    previously_join_channel: Optional[str] = None

    @property
    def is_valid_peer_room(self) -> bool:
        return self.group_id is not None and self.room_name is not None

    def __init__(self, name: str, client: Client, password: Optional[str] = None) -> None:
        self.server_id = client.server_config.server_id
        self.name = name
        self.password = password
        self.game_name = client.info.gamename
        self.previously_join_channel = client.info.previously_joined_channel
        self.room_type = PeerRoom.get_room_type(name)
        # setup the message broker
        self._broker = WebsocketBrocker(
            self.name, CONFIG.backend.url, self.get_message_from_brocker)
        self._broker.subscribe()

        match self.room_type:
            case PeerRoomType.Group:
                self.get_group_id()
                self.get_peer_room_name()
            case PeerRoomType.Staging:
                self.get_staging_room_name()
            case PeerRoomType.Title:
                self.get_title_room_name()

    def get_group_id(self):
        group_id_str = self.name.split("!")[1]
        try:
            group_id = int(group_id_str)
        except ValueError:
            raise Exception("Peer room group id is incorrect")
        self.group_id = group_id

    def get_peer_room_name(self):
        if self.game_name in PEER_GROUP_LIST:
            grouplist = PEER_GROUP_LIST[self.game_name]
            room = next(
                (g for g in grouplist if g["group_id"] == self.group_id), None)
            if room is None:
                raise Exception(f"Invalid peer room: {self.name}")
            self.room_name = room["room_name"]

    def get_staging_room_name(self):
        self.room_name = self.name.split("!")[-1]

    def get_title_room_name(self):
        self.get_staging_room_name()

    ban_list: dict[str, ChannelUser] = {}
    users: dict[str, ChannelUser] = {}
    _creator_nick_name: str

    @property
    def creator(self) -> Optional[ChannelUser]:
        if self._creator_nick_name in self.users:
            return self.users[self._creator_nick_name]
        else:
            return None

    def _add_ban_user(self, request: ModeRequest):
        assert isinstance(request, ModeRequest)
        if request.nick_name not in self.users:
            raise ChatException(
                f"user:{request.nick_name} did not connected to this server"
            )
        user = self.users[request.nick_name]

        self.ban_list[request.nick_name] = user

    def _remove_ban_user(self, nick_name: str):
        if nick_name in self.ban_list:
            del self.ban_list[nick_name]

    def _add_channel_operator(self, nick_name: str):
        if nick_name not in self.users:
            return

        user = self.users[nick_name]
        if not user.is_channel_creator:
            user.is_channel_creator = True

    def _remove_channel_operator(self, nick_name: str):
        if nick_name not in self.users:
            return

        user = self.users[nick_name]
        user.is_channel_creator = False

    def _user_voice_permission(self, nick_name: str, enable: bool = True):
        if nick_name not in self.users:
            return
        user = self.users[nick_name]
        user.is_voiceable = enable

    def get_user_by_nick(self, nick_name: str) -> Optional[ChannelUser]:
        if nick_name in self.users:
            return self.users[nick_name]
        return None

    def get_user_by_client(self, client: Client) -> Optional[ChannelUser]:
        for user in self.users.values():
            if (
                client.connection.remote_ip == user.remote_ip
                and client.connection.remote_port == user.remote_port
            ):
                return user
        return None

    def add_bind_on_user_and_channel(self, joiner: ChannelUser):
        joiner.channel.users[joiner.client.info.nick_name]
        joiner.client.info.joined_channels[joiner.channel.name] = joiner.channel

    def remove_bind_on_user_and_channel(self, leaver: ChannelUser):
        del leaver.channel.users[leaver.client.info.nick_name]
        del leaver.client.info.joined_channels[leaver.channel.name]

    def multicast(self, sender: Client, message: ResponseBase, is_skip_sender=False):
        for nick, user in self.users.items():
            if is_skip_sender:
                if sender.info.nick_name == nick:
                    continue
                else:
                    user.client.send(message)

    def get_message_from_brocker(self, message: str):
        """
        we directly send the message from brocker to all channel local user
        """
        for nick, user in self.users.items():
            user.client.connection.send(message.encode())

    def send_message_to_brocker(self, message: str):
        data = {"channel_name": self.name, "message": message}
        import json
        data_str = json.dumps(data)
        self._broker.publish_message(data_str)

    def remove_user(self, user: ChannelUser):
        user.client.info.previously_joined_channel


class ChannelManager:
    local_channels: dict = {}
    """The code blow is for channel manage"""

    @staticmethod
    def get_channel(name: str) -> Optional[Channel]:
        if name in ChannelManager.local_channels:
            return ChannelManager.local_channels[name]
        return None

    @staticmethod
    def add_channel(channel: Channel):
        if channel.name not in ChannelManager.local_channels:
            ChannelManager.local_channels[channel.name] = channel

    @staticmethod
    def remove_channel(name: str) -> None:
        if name in ChannelManager.local_channels:
            del ChannelManager.local_channels[name]


class BrockerMessage(BaseModel):
    channel_name: str
    message: str

    @field_validator("channel_name")
    def validate_channel_name(cls, value):
        if value is None or len(value) < 3:
            raise ValueError("channel name is not valid")
        return value

    @field_validator("message")
    def validate_message(cls, value):
        if value is None or len(value) < 3:
            raise ValueError("message length is not valid")
