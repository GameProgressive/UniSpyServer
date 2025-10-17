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

    def on_received(self, buffer: bytes) -> None:
        """
        when we receive udp message, we check whether the client pair is ready.
        if client is ready we send the following data to the another client
        """
        # get client from client pool
        saved_client = ConnectionListener.get_client_by_ip(
            self.connection.ip_endpoint)
        if saved_client is not None:
            Client.process_message(saved_client, buffer)
        else:
            Client.process_message(self, buffer)

    @staticmethod
    def process_message(client: "Client", buffer: bytes):
        client.info.last_receive_time = datetime.now()
        match client.info.status:
            case ConnectStatus.WAITING_FOR_ANOTHER | ConnectStatus.CONNECTING:
                client.log_network_receving(buffer)
                switcher = client._create_switcher(buffer)
                switcher.handle()
            case ConnectStatus.FINISHED:
                # !! manually stop ping message here, otherwise the ping message will continuely sending
                if client.info.ping_recv_times >= 7:
                    if len(buffer) >= 6 and buffer[:6] == MAGIC_DATA:
                        client.log_info(
                            "Negotiation is finished, ignore ping packet")
                        return

                assert client.info.cookie is not None
                another_client = ConnectionListener.get_another_client(
                    client.info.cookie, client)
                another_client.connection.send(buffer)
                client.log_network_sending(
                    f"=> [{another_client.connection.ip_endpoint}] {buffer}"
                )
