from datetime import datetime
from frontends.gamespy.library.abstractions.client import ClientBase
from frontends.gamespy.library.abstractions.switcher import SwitcherBase
from frontends.gamespy.protocols.game_traffic_relay.applications.connection import ConnectStatus, ConnectionListener
from frontends.gamespy.protocols.natneg.abstractions.contracts import MAGIC_DATA
import frontends.gamespy.protocols.natneg.applications.client as natneg


class ClientInfo:
    cookie: int | None
    last_receive_time: datetime
    status: ConnectStatus
    ping_recv_times: int

    def __init__(self) -> None:
        self.cookie = None
        self.last_receive_time = datetime.now()
        self.status = ConnectStatus.WAITING_FOR_ANOTHER
        self.ping_recv_times = 0


class Client(ClientBase):
    info: ClientInfo

    def __init__(
        self,
        connection: natneg.UdpConnection,
        server_config: natneg.ServerConfig,
        logger: natneg.LogWriter,
    ):
        super().__init__(connection, server_config, logger)
        self.info = ClientInfo()

    def _create_switcher(self, buffer: bytes) -> SwitcherBase:
        from frontends.gamespy.protocols.game_traffic_relay.applications.switcher import Switcher
        return Switcher(self, buffer)