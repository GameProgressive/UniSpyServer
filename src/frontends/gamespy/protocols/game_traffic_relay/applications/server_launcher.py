from frontends.gamespy.library.abstractions.server_launcher import ServerLauncherBase
from frontends.gamespy.library.configs import CONFIG
from frontends.gamespy.library.network.udp_handler import UdpServer
from frontends.gamespy.protocols.game_traffic_relay.applications.client import Client
from frontends.gamespy.protocols.game_traffic_relay.contracts.general import (
    GtrHeartbeat,
)


class ServerLauncher(ServerLauncherBase):
    def __init__(self) -> None:
        super().__init__(
            config_name="GameTrafficRelay",
            client_cls=Client,
            server_cls=UdpServer,
        )
    def _gtr_heartbeat(self):
        assert self.config
        req = GtrHeartbeat(
            server_id=self.config.server_id,
            public_ip_address=self.config.public_address,
            public_port=self.config.listening_port,
            client_count=len(Client.client_pool),
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