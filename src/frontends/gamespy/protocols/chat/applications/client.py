import threading
from frontends.gamespy.library.abstractions.client import ClientBase
from frontends.gamespy.library.abstractions.switcher import SwitcherBase
from frontends.gamespy.library.configs import CONFIG, ServerConfig
from frontends.gamespy.library.log.log_manager import LogWriter
from frontends.gamespy.library.network.tcp_handler import TcpConnection
from frontends.gamespy.library.network.websocket_brocker import WebSocketListener
from frontends.gamespy.protocols.chat.abstractions.contract import (
    SERVER_DOMAIN,
)
from frontends.gamespy.protocols.chat.applications.messages import WebsocketMessage


class ClientInfo:
    joined_channels: list[str]
    nick_name: str | None
    gamename: str | None
    user_name: str | None
    websocket_token: str

    def __init__(self) -> None:
        self.joined_channels = []
        self.nick_name = None
        self.gamename = None
        self.user_name = None
        self.uniquenick = None

    @property
    def irc_prefix(self) -> str:
        assert self.nick_name
        assert self.user_name
        return f"{self.nick_name}!{self.user_name}@{SERVER_DOMAIN}"


class Client(ClientBase):
    info: ClientInfo
    brocker: WebSocketListener
    pool: dict[str, "Client"] = {}
    channel_pool: dict[str, dict[str, "Client"]] = {}

    def __init__(
        self, connection: TcpConnection, server_config: ServerConfig, logger: LogWriter
    ):
        super().__init__(connection, server_config, logger)
        self.info = ClientInfo()

    def _create_switcher(self, buffer: bytes) -> SwitcherBase:
        from frontends.gamespy.protocols.chat.applications.switcher import Switcher

        switcher = Switcher(self, buffer.decode())
        return switcher

    def on_disconnected(self) -> None:
        with self._lock:
            Client.pool.pop(self.connection.ip_endpoint, None)
        from frontends.gamespy.protocols.chat.contracts.requests import QuitRequest
        from frontends.gamespy.protocols.chat.applications.handlers import QuitHandler
        req = QuitRequest.gs_parse("QUIT :disconnected\r\n")
        h = QuitHandler(self, req)
        h.handle()

    @staticmethod
    def websocket_message_callback(message) -> None:
        try:
            assert isinstance(message, str)
            r = WebsocketMessage.model_validate_json(message)
            clients = Client.channel_pool.get(r.message.target_name)
            if clients is None:
                return
            # todo process channel message
            for client in clients.values():
                client.log_network_broadcast(r.message.message)
                client.connection.send(r.message.message.encode())
        except Exception as e:
            # todo change to log and handle exception here
            print(e)

    def join_channel(self, channel_name: str) -> None:
        clients = self.channel_pool.setdefault(channel_name, {})
        clients[self.connection.ip_endpoint] = self

        if channel_name not in self.info.joined_channels:
            self.info.joined_channels.append(channel_name)

    def leave_channel(self, channel_name: str):
        if channel_name in self.info.joined_channels:
            self.info.joined_channels.remove(channel_name)

        clients = self.channel_pool.get(channel_name)
        if clients is None:
            return

        clients.pop(self.connection.ip_endpoint, None)

        if not clients:
            self.channel_pool.pop(channel_name, None)
