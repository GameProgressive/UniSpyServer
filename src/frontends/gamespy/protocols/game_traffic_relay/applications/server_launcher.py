from frontends.gamespy.library.abstractions.server_launcher import ServerLauncherBase
from frontends.gamespy.library.configs import CONFIG
from frontends.gamespy.library.network.udp_handler import UdpServer
from frontends.gamespy.protocols.game_traffic_relay.applications.client import Client
from frontends.gamespy.protocols.game_traffic_relay.applications.connection import ConnectionListener
from frontends.gamespy.protocols.game_traffic_relay.contracts.general import (
    GtrHeartbeat,
)


class ServerLauncher(ServerLauncherBase):
    _public_ip: str

    def __init__(self) -> None:
        super().__init__(
            config_name="GameTrafficRelay",
            client_cls=Client,
            server_cls=UdpServer,
        )
        self._get_public_ip()

    def _get_public_ip(self):
        url = f"{CONFIG.backend.url}/GameSpy/GameTrafficRelay/get_my_ip"
        data = self._get_data_from_backends(url=url, is_post=False)
        self._public_ip = data['ip']

    def _gtr_heartbeat(self):
        assert self.config
        req = GtrHeartbeat(
            server_id=self.config.server_id,
            public_ip_address=self.config.public_address,
            public_port=self.config.listening_port,
            client_count=len(ConnectionListener.client_pool),
        )
        req_str = req.model_dump_json()
        self._heartbeat_to_backend(
            f"{CONFIG.backend.url}/GameSpy/GameTrafficRelay/heartbeat", req_str
        )

    def _connect_to_backend(self):
        """
        check backend availability
        """
        assert self.logger is not None
        if CONFIG.unittest.is_collect_request:
            self.logger.debug(
                "CONFIG.unittest.is_collect_request is enabled ignore send heartbeat to backend"
            )
            return
        super()._connect_to_backend()
        self._gtr_heartbeat()


if __name__ == "__main__":
    s = ServerLauncher()
    s.start()
