import datetime
from uuid import UUID
from servers.chat.src.abstractions.contract import ResponseBase
from servers.chat.src.aggregates.channel_user import ChannelUser
from servers.chat.src.aggregates.key_value_manager import KeyValueManager
from servers.chat.src.aggregates.peer_room import PeerRoom
from servers.chat.src.applications.client import Client
from servers.chat.src.contracts.requests.channel import ModeRequest
from servers.chat.src.enums.peer_room import PeerRoomType
from servers.chat.src.exceptions.general import ChatException
from servers.server_browser.src.v2.aggregations.server_info_builder import PEER_GROUP_LIST


class Channel:
    server_id: UUID
    game_name: str
    name: str
    max_num_user: int = 200
    create_time: datetime.datetime = datetime.datetime.now(datetime.timezone.utc)
    kv_manager: KeyValueManager = KeyValueManager()
    room_type: PeerRoomType
    password: str
    topic: str = None
    group_id: int = None
    room_name: str = None
    previously_join_channel: str = None

    @property
    def is_valid_peer_room(self) -> bool:
        return self.group_id is not None and self.room_name is not None

    def __init__(self, name: str, client: Client, password: str = None) -> None:
        self.server_id = client.server_config.server_id
        self.name = name
        self.password = password
        self.game_name = client.info.gamename
        self.previously_join_channel = client.info.previously_joined_channel
        self.room_type = PeerRoom.get_room_type(name)
        # setup the message broker
        self._broker = None

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
            room = next((g for g in grouplist if g["group_id"] == self.group_id), None)
            if room is None:
                raise Exception(f"Invalid peer room: {self.name}")
            self.room_name = room["room_name"]

    def get_staging_room_name(self):
        self.room_name = self.name.split("!")[-1]

    def get_title_room_name(self):
        self.get_staging_room_name()

    # from multiprocessing import Manager

    ban_list: dict[str, ChannelUser] = {}
    users: dict[str, ChannelUser] = {}
    _creator_nick_name: str

    @property
    def creator(self) -> ChannelUser:
        if self._creator_nick_name in self.users:
            return self.users[self._creator_nick_name]
        else:
            return None

    def __add_ban_user(self, request: ModeRequest):
        assert isinstance(request, ModeRequest)
        if request.nick_name not in self.users:
            raise ChatException(
                f"user:{request.nick_name} did not connected to this server"
            )
        user = self.users[request.nick_name]

        self.ban_list[request.nick_name] = user

    def __remove_ban_user(self, nick_name: str):
        if nick_name in self.ban_list:
            del self.ban_list[nick_name:str]

    def __add_channel_operator(self, nick_name: str):
        if nick_name not in self.users:
            return

        user = self.users[nick_name]
        if not user.is_channel_creator:
            user.is_channel_creator = True

    def __remove_channel_operator(self, nick_name: str):
        if nick_name not in self.users:
            return

        user = self.users[nick_name]
        user.is_channel_creator = False

    def __user_voice_permission(self, nick_name: str, enable: bool = True):
        if nick_name not in self.users:
            return
        user = self.users[nick_name]
        user.is_voiceable = enable

    def get_user(self, nick_name: str) -> ChannelUser:
        if nick_name in self.users:
            return self.users[nick_name]
        return None

    def get_user(self, client: Client) -> ChannelUser:
        for user in self.users.values():
            if (
                client.connection.remote_ip == user.remote_ip
                and client.connection.remote_port == user.remote_port
            ):
                return user
        return None

    def add_bind_on_user_and_channel(joiner: ChannelUser):
        joiner.channel.users[joiner.client.info.nick_name]
        joiner.client.info.joined_channels[joiner.channel.name] = joiner.channel

    def remove_bind_on_user_and_channel(leaver: ChannelUser):
        del leaver.channel.users[leaver.client.info.nick_name]
        del leaver.client.info.joined_channels[leaver.channel.name]

    def multicast(self, sender: Client, message: ResponseBase, is_skip_snder=False):
        for nick, user in self.users.items():
            if is_skip_snder:
                if sender.info.nick_name == nick:
                    continue
                else:
                    user.client.send(message)

    def remove_user(self, user: ChannelUser):
        user.client.info.previously_joined_channel = self.name


# channel_manager = Manager()
# brocker_manager = Manager()
# local_channels: dict = channel_manager.dict()
# message_brokers: dict = brocker_manager.dict()
local_channels: dict = {}
message_brokers: dict = {}


"""The code blow is for channel manage"""


def get_local_channel(name: str) -> Channel:
    if name in local_channels:
        return local_channels[name]


def remove_local_channel(name: str) -> None:
    if name in local_channels:
        del local_channels[name]


def add_message_broker(name: str) -> object:
    if name not in message_brokers:
        message_brokers[name] = broker
    return message_brokers[name]


def remove_message_brocker(name: str) -> None:
    if name in message_brokers:
        del message_brokers[name]
