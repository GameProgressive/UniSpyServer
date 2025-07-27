import json
from frontends.gamespy.library.abstractions.client import ClientBase

from frontends.gamespy.library.abstractions.switcher import SwitcherBase
from frontends.gamespy.library.log.log_manager import LogWriter
from frontends.gamespy.library.network.brockers import WebSocketBrocker
from frontends.gamespy.library.network.tcp_handler import TcpConnection
from frontends.gamespy.library.configs import CONFIG, ServerConfig



from frontends.gamespy.protocols.chat.abstractions.contract import BrockerMessage


class ClientInfo:
    previously_joined_channel: str | None
    joined_channels: list[str]
    nick_name: str | None
    gamename: str | None
    user_name: str | None

    def __init__(self) -> None:
        self.joined_channels = []
        self.nick_name = None
        self.gamename = None
        self.user_name = None
        self.previously_joined_channel = None


class Client(ClientBase):
    info: ClientInfo
    brocker: WebSocketBrocker | None

    def __init__(
        self, connection: TcpConnection, server_config: ServerConfig, logger: LogWriter
    ):
        super().__init__(connection, server_config, logger)
        self.info = ClientInfo()
        self.brocker = None

    def _create_switcher(self, buffer: bytes) -> SwitcherBase:
        from frontends.gamespy.protocols.chat.applications.switcher import Switcher

        switcher = Switcher(self, buffer.decode())
        return switcher

    def on_connected(self) -> None:
        self.start_brocker()
        super().on_connected()

    def on_disconnected(self) -> None:
        self.stop_brocker()
        super().on_disconnected()

    def start_brocker(self):
        self.brocker = WebSocketBrocker(
            name=self.server_config.server_name,
            url=f"{CONFIG.backend.url}/Chat/ws",
            call_back_func=self._process_brocker_message,
        )
        self.brocker.subscribe()

    def stop_brocker(self):
        assert self.brocker is not None
        self.brocker.unsubscribe()

    def _process_brocker_message(self, message: str):
        # responsible for receive message and send out
        # TODO: check whether exception here will cause brocker stop
        assert isinstance(message, str)
        j = json.loads(message)
        r = BrockerMessage(**j)
        self.connection.send(r.message.encode())
